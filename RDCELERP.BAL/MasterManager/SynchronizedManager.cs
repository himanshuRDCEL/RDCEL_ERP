using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RDCELERP.BAL.Interface;
using RDCELERP.Common.Constant;
using RDCELERP.Common.Helper;
using RDCELERP.DAL.Entities;
using RDCELERP.DAL.IRepository;
using RDCELERP.DAL.Repository;
using RDCELERP.Model.Base;
using RDCELERP.Model.BusinessCustomer;
using RDCELERP.Model.Company;
using RDCELERP.Model.SynchronizedModel;
using Newtonsoft.Json;

using static Org.BouncyCastle.Math.EC.ECCurve;
using static System.Net.WebRequestMethods;
public class SynchronizedManager :ISynchronizedManager
    {
    IOptions<ApplicationSettings> _baseConfig;
    IItemRepository _itemRepository;
    private readonly IMapper _mapper;
    ILogging _logging;
    DateTime _currentDatetime = DateTime.Now.TrimMilliseconds();
    private readonly IHttpClientFactory _httpClientFactory;

    public SynchronizedManager(IOptions<ApplicationSettings> baseConfig,IItemRepository itemRepository, IMapper mapper , ILogging logging ,IHttpClientFactory httpClientFactory)
        {
        _baseConfig = baseConfig;
        _itemRepository = itemRepository;
        _mapper = mapper;
        _logging = logging;
      
        _httpClientFactory = httpClientFactory;

    }
    /// <summary>
    /// method to get master item by item code
    /// </summary>
    /// <param name="itemCode"></param>
    /// <returns></returns>
    public async Task<LstItemDetail> GetItemDetailsByItemCodeFromApiAsync(string itemDesc, string projectCode, string itemCode)
    {
        try
        {
            string url = _baseConfig.Value.synchronizedBaseURL + SynchronizedConstant.ItemMaster;

            var requestPayload = new RequestSyncItemMasterViewModel
            {
                itemcode = itemCode,
                reporttype = "ItemMaster",
                itemdesc = "",
                projectcode = "ROCWHR2500000",
                whcode = "WHFBD04"
            };

            var responseString = await PostApiAsync(url, requestPayload);

            if (!string.IsNullOrWhiteSpace(responseString))
            {

                var data = JsonConvert.DeserializeObject<SyncItemMasterViewModel>(responseString);
                if (data?.lstItemDetails != null && data.lstItemDetails.Any())
                {
                    var item = data.lstItemDetails.FirstOrDefault(i => i.Itemcode == itemCode);
                    if (item != null)
                    {
                        _logging.WriteErrorToDB("itemDetail API Success", $"Found for itemCode: {itemCode}", null);
                        return item;
                    }
                    else
                    {
                        _logging.WriteErrorToDB("itemDetail Missing in list", itemCode, null);
                    }
                }
                else
                {
                    _logging.WriteErrorToDB("itemDetail List Empty", $"itemCode: {itemCode}", null);
                }
            }
            else
            {
                _logging.WriteErrorToDB("itemDetail API Empty Response", $"itemCode: {itemCode}", null);
            }
        }
        catch (Exception ex)
        {
            _logging.WriteErrorToDB("GetItemDetailsByFromApiAsync", "", ex);
        }

        return null;
    }


    /// <summary>
    /// method to get itemdetail stock list
    /// </summary>
    /// <returns></returns>
    public async Task<List<LstStockReport>> GetStockDataAsync(string itemcode, string ean)
    {
       string url = _baseConfig.Value.synchronizedBaseURL + SynchronizedConstant.GetItemList;

        var payload = new
        {
            Reporttype = "StockBalance",
            Itemcode = itemcode,
            EAN = ean,
            Locationcode = ""
        };

        try
        {
            string responseJson = await PostApiAsync(url, payload);

            if (string.IsNullOrEmpty(responseJson))
            {
                _logging.WriteErrorToDB("GetStockDataAsync", "Empty or failed response.", null);
                return new List<LstStockReport>(); // return empty list instead of null
            }


            var result = await Task.Run(() =>
                JsonConvert.DeserializeObject<ResponseStockReportDataViewModel>(responseJson)
            );

            _logging.WriteErrorToDB("result?.LstStockReportDetails", result?.LstStockReportDetails.Count.ToString(), null);

            return result?.LstStockReportDetails ?? new List<LstStockReport>();

        }
        catch (Exception ex)
        {
            _logging.WriteErrorToDB("GetStockDataAsync", "Deserialization or DB Save Failed", ex);
            return new List<LstStockReport>();
        }

    }

    /// <summary>
    /// methdo to call API
    /// </summary>
    /// <param name="url"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    //public async Task<string> PostApiAsync(string url, object payload)
    //{
    //    try
    //    {
    //        using var client = _httpClientFactory.CreateClient();
    //        var request = new HttpRequestMessage(HttpMethod.Post, url);

    //        string authToken = _baseConfig.Value.SynchronizedAPIKey;

    //        // ✅ DO NOT use AuthenticationHeaderValue here
    //        if (!string.IsNullOrWhiteSpace(authToken))
    //            request.Headers.Add("Authorization", authToken);

    //        var json = JsonConvert.SerializeObject(payload);
    //        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

    //        var response = await client.SendAsync(request);
    //        response.EnsureSuccessStatusCode();

    //        return await response.Content.ReadAsStringAsync();
    //    }
    //    catch (Exception ex)
    //    {
    //        _logging.WriteErrorToDB("PostApiAsync", $"Error: {ex.Message}", ex);
    //        return null;
    //    }
    //}

    public async Task<string> ImportOrdersToWmsAsync(List<ItemCartViewModel> itemlistVM, int userid, string orderNo)
    {
        try
        {
            var importModel = new ImportOrdersViewModel
            {
                lstOrderItemDetail = itemlistVM.Select(item =>
                {
                    var dbItem = _itemRepository.GetSingle(x => x.ItemId == item.ItemId);

                    return new LstOrderItemDetail
                    {
                        orderNo = orderNo,
                        deliveryNo = orderNo,
                        deliveryDate = DateTime.Now.ToString("yyyy-MM-dd"),
                        itemCode = dbItem.Itemcode,
                        eanNo = dbItem.Ean,
                        itemMRP = (int)(dbItem.Mrp ?? 0),
                        billingAmount = (int)(item.TotalPrice ?? 0),
                        qty = Convert.ToInt32(item.PurchaseQty),
                        consignee = "SARAI-WH",
                        consigneeName = "Rocking Deals , Sarai",
                        createdBy = userid.ToString(),
                        projectCode = "ROCWHR2500000",
                        whCode = "WHFBD04"
                    };
                }).ToList()
            };

            string url = _baseConfig.Value.synchronizedBaseURL + SynchronizedConstant.ImportData;

            string responseString = await PostApiAsync(url, importModel);

            if (!string.IsNullOrWhiteSpace(responseString))
            {
                var importResponse = JsonConvert.DeserializeObject<ResponseImportOrders>(responseString);

                if (importResponse != null && importResponse.status == "1")
                {
                    return importResponse.refno;
                }
                else
                {
                    _logging.WriteErrorToDB("Import Failed", $"OrderNo: {orderNo}, Msg: {importResponse?.msg}", null);
                }
            }
            else
            {
                _logging.WriteErrorToDB("API Empty Response", $"OrderNo: {orderNo}", null);
            }
        }
        catch (Exception ex)
        {
            _logging.WriteErrorToDB("ImportOrders Exception", $"OrderNo: {orderNo}", ex);
        }

        return null;
    }


    public async Task<string> PostApiAsync(string url, object payload)
    {
        try
        {
            string authToken = _baseConfig.Value.SynchronizedAPIKey;

            using var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Headers.TryAddWithoutValidation("Authorization", authToken); // raw token

            var json = JsonConvert.SerializeObject(payload);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.SendAsync(request);

            var rawResponse = await response.Content.ReadAsStringAsync();

            return rawResponse; // return even if failed
        }
      
        catch (Exception ex)
        {
            _logging.WriteErrorToDB("PostApiAsync","msg", ex);
       
            return null;
        }

    }


}
