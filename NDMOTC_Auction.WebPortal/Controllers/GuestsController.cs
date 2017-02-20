using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NDMOTC_Auction.WebPortal.Entities;

namespace NDMOTC_Auction.WebPortal.Controllers
{
    [Authorize]
    public class GuestsController : Controller
    {
        private NDMOTCAuction_db_devEntities db = new NDMOTCAuction_db_devEntities();

        // GET: Guests
        public async Task<ActionResult> Index()
        {
            var guests = db.Guests.Include(g => g.Address);
            return View(await guests.ToListAsync());
        }

        // GET: Guests/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = await db.Guests.FindAsync(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            if (guest.Items.Any())
            {
                ViewBag.Total = guest.Items.Sum(x => x.BuyoutPrice);
            } else
            {
                ViewBag.Total = 0.00;
            }
            return View(guest);
        }

        // GET: Guests/Create
        public ActionResult Create()
        {
            ViewBag.AddressId = new SelectList(db.Addresses, "Id", "Street1");
            return View();
        }

        // POST: Guests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,Phone,Email,AddressId,Misc")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                db.Guests.Add(guest);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AddressId = new SelectList(db.Addresses, "Id", "Street1", guest.AddressId);
            return View(guest);
        }

        // GET: Guests/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = await db.Guests.FindAsync(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            ViewBag.AddressId = new SelectList(db.Addresses, "Id", "Street1", guest.AddressId);
            return View(guest);
        }

        // POST: Guests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,Phone,Email,AddressId,Misc,HasPaid,PaymentMethodId,TotalPaid")] Guest guest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guest).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AddressId = new SelectList(db.Addresses, "Id", "Street1", guest.AddressId);
            return View(guest);
        }

        // GET: Guests/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = await db.Guests.FindAsync(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // POST: Guests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Guest guest = await db.Guests.FindAsync(id);
            db.Guests.Remove(guest);
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
