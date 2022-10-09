using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RusadaAviation.Models;
using System.Configuration;
using System.Globalization;

namespace RusadaAviation.Controllers
{
    public class RusadaAviationController : Controller
    {
        string ErrorMessage = string.Empty;
        private RusadaAviationEntities db = new RusadaAviationEntities();

        [HttpGet]
        public ActionResult Index()
        {
            return View(db.AircraftDetails.ToList());
        }

        [HandleError]
        public ActionResult Details(int? id)
        {
            try
            {
                AircraftDetail aircraftdetail = db.AircraftDetails.Find(id);
                if (aircraftdetail == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    string imgBase64Data = Convert.ToBase64String(aircraftdetail.ModelImage);
                    string imgURL = string.Format("data:image/png;base64,{0}", imgBase64Data);
                    ViewBag.ImageData = imgURL;
                    ViewBag.PostedDate = aircraftdetail.DateTime.ToString("MM/dd/yyyy hh:mm");
                }
                return View(aircraftdetail);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HandleError]
        [HttpPost]
        public ActionResult Create(AircraftDetail aircraftDetails)
        {
            try
            {

                UploadImage(aircraftDetails);
                DateTime dt = DateTime.ParseExact(aircraftDetails.DateTime.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                aircraftDetails.DateTime = dt.Add(DateTime.Now.TimeOfDay);
                db.AircraftDetails.Add(aircraftDetails);
                db.SaveChanges();
                return RedirectToAction("Create");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        [HandleError]
        public ActionResult Edit(int? id)
        {
            try
            {
                AircraftDetail aircraftdetail = db.AircraftDetails.Find(id);
                if (aircraftdetail == null)
                {
                    return HttpNotFound();
                }
                return View(aircraftdetail);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HandleError]
        [HttpPost]
        public ActionResult Edit(AircraftDetail aircraftDetails)
        {
            try
            {
                db.Entry(aircraftDetails).State = EntityState.Modified;
                UploadImage(aircraftDetails);
                DateTime dt = DateTime.ParseExact(aircraftDetails.DateTime.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                aircraftDetails.DateTime = dt.Add(DateTime.Now.TimeOfDay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HandleError]
        public ActionResult Delete(int? id)
        {
            try
            {
                AircraftDetail aircraftdetail = db.AircraftDetails.Find(id);
                ViewBag.PostedDate = aircraftdetail.DateTime.ToString("MM/dd/yyyy hh:mm");
                if (aircraftdetail == null)
                {
                    return HttpNotFound();
                }
                return View(aircraftdetail);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HandleError]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                AircraftDetail aircraftdetail = db.AircraftDetails.Find(id);
                db.AircraftDetails.Remove(aircraftdetail);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HandleError]
        public void UploadImage(AircraftDetail aircraftDetails)
        {
            try
            {
                if (aircraftDetails.ModelImageFile != null && aircraftDetails.ModelImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(aircraftDetails.ModelImageFile.FileName);
                    string extension = Path.GetExtension(aircraftDetails.ModelImageFile.FileName);
                    aircraftDetails.ModelImage = new byte[aircraftDetails.ModelImageFile.ContentLength];
                    aircraftDetails.ModelImageFile.InputStream.Read(aircraftDetails.ModelImage, 0, aircraftDetails.ModelImageFile.ContentLength);
                    ViewBag.ErrorMessage = "";
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
        }
    }
}