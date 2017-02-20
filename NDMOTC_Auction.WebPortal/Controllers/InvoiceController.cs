using NDMOTC_Auction.WebPortal.Entities;
using NDMOTC_Auction.WebPortal.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NDMOTC_Auction.WebPortal.Controllers
{
    [Authorize]
    public class InvoiceController : Controller
    {
        private NDMOTCAuction_db_devEntities db = new NDMOTCAuction_db_devEntities();

        // GET: Invoice
        public ActionResult Index()
        {
            return View();
        }

        // Get Guest Invoice
        public async Task<ActionResult> GetInvoice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = await db.Guests.FindAsync(id);
            if(guest == null)
            {
                return HttpNotFound();
            }
            if (guest.Items.Any())
            {
                ViewBag.Total = guest.Items.Sum(x => x.BuyoutPrice).Value;
            } else
            {
                ViewBag.Total = (decimal)0.00;
            }
            var paymentMethods = db.PaymentMethods.Distinct().ToList();
            var paymentMethodsList = paymentMethods.Select(pm => new SelectListItem
            {
                Value = pm.Id.ToString(),
                Text = pm.Title
            }).ToList();
            var selected = paymentMethodsList.Where(pm => pm.Value.Equals(guest.PaymentMethodId.ToString())).FirstOrDefault();
            if (selected != null)
            {
                selected.Selected = true;
            }
            ViewBag.PaymentMethods = new SelectList(paymentMethodsList, "Value", "Text");
            return View(guest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetInvoice([Bind(Include = "Id,FirstName,LastName,Phone,Email,AddressId,Misc,PaymentMethodId,HasPaid,TotalPaid")] Guest guest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(guest).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View();
            }
            catch (System.Exception e)
            {
                e.ToString();
                throw;
            }
            if (ModelState.IsValid)
            {
                db.Entry(guest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}