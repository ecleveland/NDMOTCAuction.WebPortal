using NDMOTC_Auction.WebPortal.Entities;
using NDMOTC_Auction.WebPortal.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NDMOTC_Auction.WebPortal.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private NDMOTCAuction_db_devEntities db = new NDMOTCAuction_db_devEntities();

        // GET: Checkout
        public ActionResult Index()
        {
            var model = new CheckoutViewModel();
            var items = db.Items.Distinct();
            if (items.Any())
            {
                var soldItems = items.Distinct().Where(i => i.PurchaserId != null);
                var unsoldItems = items.Distinct().Where(i => i.PurchaserId == null);
                model.SoldItems = soldItems;
                model.UnsoldItems = unsoldItems;
                if (model.SoldItems.Any())
                {
                    model.TotalSold = soldItems.Sum(ts => ts.BuyoutPrice).Value;
                } else
                {
                    model.TotalSold = (decimal)0.00;
                }
            }
            else
            {
                model.SoldItems = new List<Item>();
                model.UnsoldItems = new List<Item>();
                model.TotalSold = (decimal)0.00;
            }
            var guestsWhoPaid = db.Guests.Where(tp => tp.TotalPaid.HasValue);
            if (guestsWhoPaid.Any())
            {
                model.TotalPaid = guestsWhoPaid.Sum(pg => pg.TotalPaid);
            }
            else
            {
                model.TotalPaid = (decimal)0.00;
            }
            return View(model);
        }

        // GET: Checkout/CheckoutGuests
        public async Task<ActionResult> CheckoutGuests()
        {
            var guests = db.Guests.Include(g => g.Address);
            return View(await guests.ToListAsync());
        }

        // GET: Checkout/CheckoutGuest
        public async Task<ActionResult> CheckoutGuest(int? guestId)
        {
            if (guestId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = await db.Guests.FindAsync(guestId);
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

        // POST: Checkout/CheckoutGuest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckoutGuest([Bind(Include = "Id,FirstName,LastName,Phone,Email,AddressId,Misc,PaymentMethodId,HasPaid,TotalPaid")] Guest guest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(guest).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("CheckoutGuests");
                }
                return View();
            }
            catch (System.Exception e)
            {
                e.ToString();
                throw;
            }
        }

        // GET: Collections
        public async Task<ActionResult> HasNotPaid()
        {
            var guests = db.Guests.Include(g => g.PaymentMethod).Where(g => g.HasPaid != true);
            return View(await guests.ToListAsync());
        }
    }
}