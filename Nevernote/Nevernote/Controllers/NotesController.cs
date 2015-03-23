using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.AspNet;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using Nevernote.Filters;
using Nevernote.Models;

namespace Nevernote.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class NotesController : Controller
    {
        UsersContext db = new UsersContext();

        //
        // GET: /Notes/

        private ActionResult Result()
        {
            ViewBag.Notes = db.Notes.Where(c => c.UserID == WebSecurity.CurrentUserId).OrderByDescending(c => c.NoteId);
            return View();
        }

        public ActionResult Index ()
        {
            return this.Result();
        }

        public ActionResult Delete (int id)
        {
            Note note = db.Notes.Find(id);

            if (note.UserID == WebSecurity.CurrentUserId)
            {
                db.Notes.Remove(db.Notes.Find(id));
                db.SaveChanges();
            } else {
                ViewBag.Error = "Хак и взлом не пройдет.";
            }

            return RedirectToAction("/");
        }

        //
        // POST: /Notes

        [HttpPost]
        public ActionResult Index (Note note)
        {
            if (ModelState.IsValid)
            {
                note.Date = DateTime.Now;
                note.UserID = WebSecurity.CurrentUserId;
                db.Notes.Add(note);
                db.SaveChanges();
            }
            
            return this.Result();
        }

    }
}
