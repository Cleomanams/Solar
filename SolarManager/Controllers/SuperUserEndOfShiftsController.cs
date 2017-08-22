using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SolarManager.Models;
using Microsoft.AspNet.Identity;
using NLog;
using Thinktecture.IdentityModel.Mvc;

namespace SolarManager.Controllers
{
    [ResourceAuthorize("read", "SuperUserEndOfShift")]
    public class SuperUserEndOfShiftsController : Controller
    {
        private Logger _log = LogManager.GetCurrentClassLogger();

        private FlashPortalDBEntities db = new FlashPortalDBEntities();

        // GET: SuperUserEndOfShifts
        public async Task<ActionResult> Index()
        {
            var currentUserID = User.Identity.GetUserId();//UserID of Logged User
            try
            {
                var superUserEndOfShifts = (from spEos in db.SuperUserEndOfShifts.Include(s => s.SubUser)
                                                //.Where(spEos => spEos.superUserId == currentUserID)
                                            select spEos);

                //var superUserEndOfShifts = db.SuperUserEndOfShifts.Include(s => s.SubUser);
                return View(await superUserEndOfShifts.ToListAsync());
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: subUserTransaction/Transactions/1
        public async Task<ActionResult> Transactions(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //Logged User
                var currentUserID = User.Identity.GetUserId();

                var eosObj = db.SuperUserEndOfShifts.Where(eos => eos.id == id)
                    .Select(eos => new
                    {
                        EOSPrintNumber = eos.EOSPrintNumber,
                    //subUserID = eos.subUserID,
                }).Single();

                var transactions = (from trn in db.SubUserTransactions.Include(s => s.SubUser)
                                    .Where(trn => trn.txEoSNumber == eosObj.EOSPrintNumber
                                    //&& (trn.subUserId == eosObj.subUserID))
                                    && (db.SubUsers.Any(sb => sb.UserID == currentUserID &&
                                    sb.SubUserID == trn.subUserId)))
                                    select trn);

                return View(await transactions.ToListAsync());
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: SuperUserEndOfShifts/Details/5        
        public async Task<ActionResult> EOSPrint(int id)
        {
            SuperUserEndOfShift superUserEndOfShift = await db.SuperUserEndOfShifts.FindAsync(id);

            //SuperUserEndOfShift superUserEndOfShift = await db.SuperUserEndOfShifts.FindAsync(id);
            return View("EOSPrint", superUserEndOfShift);
        }

        public async Task<ActionResult> EOSPopup(int id)
        {
            SuperUserEndOfShift superUserEndOfShift = await db.SuperUserEndOfShifts.FindAsync(id);
            if (superUserEndOfShift != null)
            {
                if (Request.IsAjaxRequest())
                {
                    return View("_EOSPrint", superUserEndOfShift);
                }
                else
                {
                    return View("EOSPrint", superUserEndOfShift);
                }
            }
            return View("Index");
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
