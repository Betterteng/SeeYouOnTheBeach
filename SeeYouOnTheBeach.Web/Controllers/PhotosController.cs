using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SeeYouOnTheBeach.Web.Models;
using System.IO;
using SeeYouOnTheBeach.Web.ViewModels;

namespace SeeYouOnTheBeach.Web.Controllers
{
    public class PhotosController : Controller
    {
        private PhotoDbContext db = new PhotoDbContext();

        // GET: Photos
        public async Task<ActionResult> Index()
        {
            var photos = await db.Photos.Include(p => p.Beach).ToListAsync();
            return View(new PhotoViewModel() {Photo = photos});
        }

        // GET: Photos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = await db.Photos.FindAsync(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: Photos/Create
        public ActionResult Create()
        {
            ViewBag.BeachId = new SelectList(db.Beaches, "BeachId", "Description");
            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PhotoId,BeachId,Description,Content")] Photo photo, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    var fileData = new MemoryStream();
                    upload.InputStream.CopyTo(fileData);
                    photo.Content = fileData.ToArray();
                }
                db.Photos.Add(photo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.BeachId = new SelectList(db.Beaches, "BeachId", "Description", photo.BeachId);
            return View(photo);
        }

        // GET: Photos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = await db.Photos.FindAsync(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            ViewBag.BeachId = new SelectList(db.Beaches, "BeachId", "Description", photo.BeachId);
            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PhotoId,BeachId,Description,Content")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.BeachId = new SelectList(db.Beaches, "BeachId", "Description", photo.BeachId);
            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = await db.Photos.FindAsync(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Photo photo = await db.Photos.FindAsync(id);
            db.Photos.Remove(photo);
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
