using SolarManager.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Mvc;

namespace SolarManager.Controllers
{
    [ResourceAuthorize("read", "TempModel")]
    //[HandleForbidden]

    public class TempController : Controller
    {
        private Logger _log;
        public TempController() : base()
        {
            _log = LogManager.GetCurrentClassLogger();
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            CultureInfo cinfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            cinfo.NumberFormat.CurrencyGroupSeparator = "";
            cinfo.NumberFormat.CurrencyDecimalSeparator = ".";
            cinfo.NumberFormat.NumberGroupSeparator = "";
            cinfo.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = cinfo;
        }
        // GET: Temp
        public ActionResult Index()
        {
            try
            {
                return View(Startup.ModelList);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: Temp/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    string id_string = id == null ? "null" : id.ToString();
                    ViewBag.ErrorMessage = $"Cannot find item with Id {id_string}";
                    return View("Error");
                }
                TempModel model = Startup.ModelList.FirstOrDefault(m => m.Id == id);
                return View(model);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: Temp/Create
        [ResourceAuthorize("create", "TempModel")]
        public ActionResult Create()
        {
            ViewBag.TypeId = new SelectList(TempType.GetList(), "Id", "Description");

            return View();
        }

        // POST: Temp/Create
        [HttpPost]
        public ActionResult Create(TempModel model)
        {
            try
            {
                model.Id = Startup.ModelList.Max(m => m.Id) + 1;
                Startup.ModelList.Add(model);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: Temp/Edit/5
        [ResourceAuthorize("edit", "TempModel")]
        public ActionResult Edit(int? id)
        {
            try
            {
                ViewBag.TypeId = new SelectList(TempType.GetList(), "Id", "Description");
                TempModel model = Startup.ModelList.FirstOrDefault(m => m.Id == id);
                if (model == null)
                {
                    string id_string = id == null ? "null" : id.ToString();
                    ViewBag.ErrorMessage = $"Cannot find item with Id {id_string}";
                    return View("Error");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // POST: Temp/Edit/5
        [HttpPost]
        public ActionResult Edit(TempModel model)
        {

            try
            {
                TempModel curr_model = Startup.ModelList.FirstOrDefault(m => m.Id == model.Id);
                curr_model.Name = model.Name;
                curr_model.TypeId = model.TypeId;

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: Temp/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    string id_string = id == null ? "null" : id.ToString();
                    ViewBag.ErrorMessage = $"Cannot find item with Id {id_string}";
                    return View("Error");
                }
                TempModel model = Startup.ModelList.FirstOrDefault(m => m.Id == id);
                return View(model);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // POST: Temp/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                TempModel model = Startup.ModelList.FirstOrDefault(m => m.Id == id);
                Startup.ModelList.Remove(model);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}
