using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using NDMOTC_Auction.WebPortal.Entities;
using System.Linq;

namespace NDMOTC_Auction.WebPortal.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private NDMOTCAuction_db_devEntities db = new NDMOTCAuction_db_devEntities();

        // GET: Items
        public async Task<ActionResult> Index()
        {
            var items = db.Items.Include(i => i.Guest);
            return View(await items.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<ActionResult> Details(int? id)
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

        // GET: Items/Create
        public ActionResult Create()
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

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,Value,BuyoutPrice,PurchaserId")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Items.Add(item);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            var guests = db.Guests.Distinct().ToList();
            var guestList = guests.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = string.Format("{0} {1}", g.FirstName, g.LastName)
            }).ToList();
            ViewBag.PurchaserId = new SelectList(guestList, "Value", "Text");
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            var guests = db.Guests.Distinct().ToList();
            var guestList = guests.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = string.Format("{0} - {1} {2}", g.Id.ToString(), g.FirstName, g.LastName)
            }).ToList();
            var selected = guestList.Where(gli => gli.Value.Equals(item.PurchaserId.ToString())).FirstOrDefault();
            if(selected != null)
            {
                selected.Selected = true;
            }
            ViewBag.GuestList = new SelectList(guestList, "Value", "Text");
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,Value,BuyoutPrice,PurchaserId")] Item item)
        {
            if (ModelState.IsValid)
            {
                db.Entry(item).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(item);
        }

        // GET: Items/Delete/5
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

        // POST: Items/Delete/5
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
