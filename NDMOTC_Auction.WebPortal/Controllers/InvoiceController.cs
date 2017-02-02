using NDMOTC_Auction.WebPortal.Entities;
using NDMOTC_Auction.WebPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NDMOTC_Auction.WebPortal.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        

        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetInvoice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View();
        }
    }
}