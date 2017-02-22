using System.Data.Entity;
using NDMOTC_Auction.WebPortal.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using NDMOTC_Auction.WebPortal.Helpers;
using System.Net;

namespace NDMOTC_Auction.WebPortal.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private NDMOTCAuction_db_devEntities db = new NDMOTCAuction_db_devEntities();
        
        // GET: Games
        public async Task<ActionResult> Index()
        {
            var items = db.Items.Include(i => i.Guest).Where(i => i.Value == Constants.RINGER_DINGER_VALUE);
            return View(await items.ToListAsync());
        }

        // GET: Games/CreateRingerDinger
        public ActionResult CreateRingerDinger()
        {
            var guests = db.Guests.Distinct().ToList();
            var guestList = guests.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = string.Format("{0} {1}", g.FirstName, g.LastName)
            }).ToList();
            ViewBag.PurchaserId = new SelectList(guestList, "Value", "Text");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateRingerDinger([Bind(Include = "Id,Title,Description,Value,BuyoutPrice,PurchaserId")] Item item)
        {
            if (ModelState.IsValid)
            {
                var guest = db.Guests.Where(g => g.Id == item.PurchaserId).FirstOrDefault();
                var totalBought = int.Parse(item.Title);
                if(totalBought >= 0)
                {
                    var packs = totalBought / Constants.RINGER_DINGER_PACK_SIZE;
                    var individualRings = totalBought % Constants.RINGER_DINGER_PACK_SIZE;
                    item.BuyoutPrice = (packs * Constants.RINGER_DINGER_PACK_VALUE) + (individualRings * Constants.RINGER_DINGER_RING_VALUE);
                    item.Description = string.Format("Rings bought: {0} | Packs of {1} (${2}): {3} | Additional Individual Rings (${4}): {5}", totalBought, Constants.RINGER_DINGER_PACK_SIZE, Constants.RINGER_DINGER_PACK_VALUE, packs, Constants.RINGER_DINGER_RING_VALUE, individualRings);
                } else
                {
                    item.Description = "None Bought";
                    item.BuyoutPrice = (decimal)0.00;
                }
                item.Title = string.Format("{0} - {1} {2} Ringer Dinger", guest.Id, guest.FirstName, guest.LastName);
                item.Value = Constants.RINGER_DINGER_VALUE;
                db.Items.Add(item);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Games/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = await db.Items.FindAsync(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Item item = await db.Items.FindAsync(id);
            db.Items.Remove(item);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}