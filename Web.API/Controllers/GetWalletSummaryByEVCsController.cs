using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RDCELERP.DAL.Entities;


namespace Web.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GetWalletSummaryByEVCsController : ControllerBase
    {
        private readonly Digi2l_DevContext _context;

        public GetWalletSummaryByEVCsController(Digi2l_DevContext context)
        {
            _context = context;
        }

        //GET: api/GetWalletSummaryByEVCs


        //[HttpPost]
        //public async Task<ActionResult<IEnumerable<GetWalletSummaryByEVC>>> GetGetWalletSummariesByEVC(WalletViewModel EVC_WalletViewModel)
        //{
        //    string StoredProc = "exec GetWalletSummaryByEVC " +
        //         "@EVCID = " + EVC_WalletViewModel.EVCID + ",";
        //    return await _context.GetWalletSummaryByEVCs.FromSqlRaw(StoredProc).ToListAsync();


            //    //GET: api/GetWalletSummaryByEVCs/5
            //    //[HttpGet("{id}")]
            //    //public async Task<ActionResult<GetWalletSummaryByEVC>> GetGetWalletSummaryByEVC(int id)
            //    //{
            //    //    var getWalletSummaryByEVC = await _context.GetWalletSummariesByEVC.FindAsync(id);

            //    //    if (getWalletSummaryByEVC == null)
            //    //    {
            //    //        return NotFound();
            //    //    }

            //    //    return getWalletSummaryByEVC;
            //    //}

            //    //PUT: api/GetWalletSummaryByEVCs/5
            //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            //    //[HttpPut("{id}")]
            //    //public async Task<IActionResult> PutGetWalletSummaryByEVC(int id, GetWalletSummaryByEVC getWalletSummaryByEVC)
            //    //{
            //    //    if (id != getWalletSummaryByEVC.EVCID)
            //    //    {
            //    //        return BadRequest();
            //    //    }

            //    //    _context.Entry(getWalletSummaryByEVC).State = EntityState.Modified;

            //    //    try
            //    //    {
            //    //        await _context.SaveChangesAsync();
            //    //    }
            //    //    catch (DbUpdateConcurrencyException)
            //    //    {
            //    //        if (!GetWalletSummaryByEVCExists(id))
            //    //        {
            //    //            return NotFound();
            //    //        }
            //    //        else
            //    //        {
            //    //            throw;
            //    //        }
            //    //    }

            //    //    return NoContent();
            //    //}

            //    //POST: api/GetWalletSummaryByEVCs
            //    //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
            //    //[HttpPost]
            //    //public async Task<ActionResult<GetWalletSummaryByEVC>> PostGetWalletSummaryByEVC(GetWalletSummaryByEVC getWalletSummaryByEVC)
            //    //{
            //    //    _context.GetWalletSummariesByEVC.Add(getWalletSummaryByEVC);
            //    //    await _context.SaveChangesAsync();

            //    //    return CreatedAtAction("GetGetWalletSummaryByEVC", new { id = getWalletSummaryByEVC.EVCID }, getWalletSummaryByEVC);
            //    //}

            //    //DELETE: api/GetWalletSummaryByEVCs/5
            //    //[HttpDelete("{id}")]
            //    //public async Task<IActionResult> DeleteGetWalletSummaryByEVC(int id)
            //    //{
            //    //    var getWalletSummaryByEVC = await _context.GetWalletSummariesByEVC.FindAsync(id);
            //    //    if (getWalletSummaryByEVC == null)
            //    //    {
            //    //        return NotFound();
            //    //    }

            //    //    _context.GetWalletSummariesByEVC.Remove(getWalletSummaryByEVC);
            //    //    await _context.SaveChangesAsync();

            //    //    return NoContent();
            //    //}

            //    //private bool GetWalletSummaryByEVCExists(int id)
            //    //{
            //    //    return _context.GetWalletSummariesByEVC.Any(e => e.EVCID == id);
            //    //}
            //}
        }
    }

