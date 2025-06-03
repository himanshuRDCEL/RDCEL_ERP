using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.Company;
using static Org.BouncyCastle.Math.EC.ECCurve;
public class SynchronizedManager :ISynchronizedManager
    {
    IOptions<ApplicationSettings> _baseConfig;
    IItemRepository _itemRepository;
    private readonly IMapper _mapper;
    ILogging _logging;
    DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();

    public SynchronizedManager(IOptions<ApplicationSettings> baseConfig,IItemRepository itemRepository, IMapper mapper , ILogging logging)
        {
        _baseConfig = baseConfig;
        _itemRepository = itemRepository;
        _mapper = mapper;
        _logging = logging;
        }

    public async Task<bool> UpsertItemsAsync(IEnumerable<ItemViewModel> items)
    {
        bool flag=false;
      
        try
        {
            foreach (var item in items)
            {
                var existing = _itemRepository.GetSingle(x => x.Itemcode == item.Itemcode && x.IsActive == true);

                if (existing != null)
                {
                    // Update existing item
                    existing.ItemDesc = item.ItemDesc; 
                    existing.Ean = item.Ean;
                    existing.Qty = item.Qty;
                    existing.ModifiedDate = _currentDatetime;
                }
                else
                {
                  TblItem TblItem = _mapper.Map<ItemViewModel, TblItem>(item);

                    if (TblItem != null)
                    {
                        TblItem.IsActive = true;
                        TblItem.CreatedDate= _currentDatetime;
                        await _itemRepository.CreateAsync(TblItem);
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }

        return flag;
    }
    public async Task FetchAndSaveStockDataAsync()
    {
        bool flag=false;
      //  string url = "http://sims.synchronized.in/wmsapi/api/RockingDeal/StockReportData";
        string url = _baseConfig.Value.synchronizedBaseURL + SynchronizedConstant.GetItemList;
        var payload = new
        {
            Reporttype = "StockBalance",
            Itemcode = "",
            EAN = "",
            Locationcode = ""
        }; _logging.WriteErrorToDB("responseJson", "start", null);

        string responseJson = await PostApiWithHttpWebRequest(url, payload);

        _logging.WriteErrorToDB("responseJson", responseJson, null);

        if (string.IsNullOrEmpty(responseJson))
        {
            Console.WriteLine("Empty or failed response.");
            return;
        }

        try
        {
            var items = JsonSerializer.Deserialize<List<ItemViewModel>>(responseJson);
            if (items != null && items.Any())
            {
               
              flag= await UpsertItemsAsync(items);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Deserialization or DB Save Failed: " + ex.Message);
        }
    }
    public async Task<string> PostApiAsync(string url, object payload)
    {
        string authToken = _baseConfig.Value.SynchronizedAPIKey;

        try
        {
            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            if (!string.IsNullOrEmpty(authToken))
                request.Headers.Add("Authorization", authToken);

            var json = JsonSerializer.Serialize(payload);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logging.WriteErrorToDB("SynchronizedManager", "PostApiAsync", ex);
        

        return null;
        }
    }

    public async Task<string> PostApiWithHttpWebRequest(string url, object payload)
    {
        try
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Timeout = 30000; // 30 seconds timeout
            request.ReadWriteTimeout = 30000;

            string authToken = _baseConfig.Value.SynchronizedAPIKey;
            request.Headers["Authorization"] = authToken;

            var json = JsonSerializer.Serialize(payload);
            using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
            {
                await streamWriter.WriteAsync(json);
            }

            _logging.WriteErrorToDB("HttpRequest", "Request sent", null);

            using (var response = (HttpWebResponse)await request.GetResponseAsync())
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                var result = await streamReader.ReadToEndAsync();
                _logging.WriteErrorToDB("HttpResponse", result, null);
                return result;
            }
        }
        catch (WebException webEx)
        {
            string errorResponse = "";

            if (webEx.Response != null)
            {
                using (var errorStream = new StreamReader(webEx.Response.GetResponseStream()))
                {
                    errorResponse = await errorStream.ReadToEndAsync();
                }
            }

            _logging.WriteErrorToDB("WebException", errorResponse, webEx);
            return null;
        }
        catch (Exception ex)
        {
            _logging.WriteErrorToDB("Exception", ex.Message, ex);
            return null;
        }
    }

}

