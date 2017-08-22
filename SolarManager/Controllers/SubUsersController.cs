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
using System.Diagnostics;
using Thinktecture.IdentityModel.Mvc;

namespace SolarManager.Controllers
{
    [ResourceAuthorize("read","SubUser")]
    //[HandleForbidden]
    public class SubUsersController : Controller
    {
        private Logger _log = LogManager.GetCurrentClassLogger();

        private FlashPortalDBEntities db = new FlashPortalDBEntities();

        // GET: SubUsers
        public async Task<ActionResult> Index()
        {
            //string userId = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();//.GetUserManager<AuthorisationManager>()
                                                                                                  //.FindByNameAsync(User.Identity.Name).Result.Id;

            
            string currentUserID = User.Identity.GetUserId();

            //Debug.WriteLine("currentUserID " + currentUserID);
            
            try
            {
                var subUsers = (from sb in db.SubUsers.Where(sb => sb.UserID == currentUserID)
                                    //&& sb.Status == true)
                                select sb);

                return View(await subUsers.ToListAsync());
            }
            catch(Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
            //return View(await db.SubUsers.ToListAsync());
        }

        // GET: SubUsers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubUser subUser = await db.SubUsers.FindAsync(id);
            if (subUser == null)
            {
                return HttpNotFound();
            }
            return View(subUser);
        }

        // GET: SubUsers/Create
        [ResourceAuthorize("create","SubUser")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<ActionResult> Create([Bind(Include = "SubUserID,SubUserName,SubUserPin,UserID,Admin,Status")] SubUser subUser)
        {
            subUser.UserID = User.Identity.GetUserId(); //use identity to get the UserID of curruntly logged user

            try
            {
                if (ModelState.IsValid)
                {
                    subUser.Status = true; //Default Status is set to be visible

                    //Check if user exist
                    bool userExist = db.SubUsers.Any(newUser => newUser.SubUserName.Equals(subUser.SubUserName) &&
                                                                newUser.UserID.Equals(subUser.UserID));

                    if (userExist)
                    {
                        ViewBag.Message = "SubUserName " + subUser.SubUserName + " Already Exist";
                        //ModelState.Clear();
                        return View();
                    }
                    else
                    {
                        //Check for duplicate PINS for subusers related to the same SuperUser
                        bool currentUserPin = db.SubUsers.Any(newSubUserID => newSubUserID.SubUserPin.Equals(subUser.SubUserPin) &&
                                                                              newSubUserID.UserID.Equals(subUser.UserID));
                        if (currentUserPin)
                        {
                            ViewBag.Message = "SubUser Pin Already exiiiiiiist";// "SubUserName " + subUser.SubUserName + " Already Exist";
                                                                                //ModelState.Clear();
                            return View();
                        }
                        else
                        {
                            db.SubUsers.Add(subUser);
                            await db.SaveChangesAsync();
                            ViewBag.Message = "Successfully Added New Sub User";
                            return RedirectToAction("Index");
                        }
                    }
                }
                return View(subUser);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: SubUsers/Edit/5
        [ResourceAuthorize("edit","SubUser")]
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SubUser subUser = await db.SubUsers.FindAsync(id);
                if (subUser == null)
                {
                    return HttpNotFound();
                }
                TempData["SubUserName"] = subUser.SubUserName;
                TempData["SubUserPin"] = subUser.SubUserPin;
                TempData["Admin"] = subUser.Admin;
                return View(subUser);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        public async Task<ActionResult> editFunction(SubUser subUser)
        {
            if (TempData["SubUserName"] == null || TempData["SubUserPin"] == null || TempData["Admin"] == null)
            {
                ViewBag.Message = "Session Expired..";// Todo Make a pop-up instead
                return RedirectToAction("index", "Account");
            }
            try
            {

                bool currentUserPin = TempData["SubUserPin"].Equals(subUser.SubUserPin);

                bool isCurrentAdminStatus = TempData["Admin"].Equals(subUser.Admin);

                bool isCurrentSubUser = TempData["SubUserName"].Equals(subUser.SubUserName);

                if (isCurrentSubUser && currentUserPin && isCurrentAdminStatus)
                {
                    ViewBag.Message = "No Changes";
                    return View();
                }

                //Check for duplicate PINS for subusers related to the same SuperUser
                bool pinExist = db.SubUsers.Any(newSubUserID => newSubUserID.SubUserPin.Equals(subUser.SubUserPin) &&
                                                                      newSubUserID.UserID.Equals(subUser.UserID));

                if (isCurrentSubUser)
                {
                    if (pinExist && isCurrentAdminStatus)
                    {
                        ViewBag.Message = "SubUser Pin Already Exiiiiiiist";// Todo Make a pop-up message instead.
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        db.Entry(subUser).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                }

                //Check if user exist
                bool userExist = db.SubUsers.Any(newUser => newUser.SubUserName.Equals(subUser.SubUserName) &&
                                                            newUser.UserID.Equals(subUser.UserID));
                if (userExist)
                {
                    ViewBag.Message = "SubUserName " + subUser.SubUserName + " Already Exist";
                    //ModelState.Clear();
                    return View();
                    //return RedirectToAction("Index");
                }
                else if (currentUserPin)
                {
                    db.Entry(subUser).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else if (pinExist)// && isCurrentAdminStatus)
                {
                    ViewBag.Message = "SubUser Pin Already Exiiiiiiist";
                    //ModelState.Clear();
                    return View();
                }
                else
                {
                    db.Entry(subUser).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
        
        // POST: SubUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SubUserID,SubUserName,SubUserPin,UserID,Admin,Status")] SubUser subUser)
        {
            subUser.UserID = User.Identity.GetUserId();

            //Count number of Admin
            int count = db.SubUsers.Where(numOfAdmin => numOfAdmin.Admin && numOfAdmin.UserID.Equals(subUser.UserID)).Count();

            try
            {
                if (ModelState.IsValid)
                {
                    bool isCurrentAdminStatus = TempData["Admin"].Equals(subUser.Admin);

                    if (subUser.Admin == true)
                    {
                        try
                        {
                            await editFunction(subUser);
                            return RedirectToAction("index");
                        }
                        catch (Exception ex)
                        {
                            _log.Error(ex);
                        }
                    }
                    else if (count > 1)
                    {
                        try
                        {
                            await editFunction(subUser);
                            return RedirectToAction("index");
                        }
                        catch (Exception ex)
                        {
                            _log.Error(ex);
                        }
                    }
                    else if (isCurrentAdminStatus) //Review this   ????
                    {
                        try
                        {
                            await editFunction(subUser);
                            return RedirectToAction("index");
                        }
                        catch (Exception ex)
                        {
                            _log.Error(ex);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "At least one Admin is required";//TODO make a pop-up instead
                        return RedirectToAction("Index");
                    }
                }
                return View(subUser);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // GET: SubUsers/Delete/5
        [ResourceAuthorize("delete","SubUser")]
        public async Task<ActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                SubUser subUser = await db.SubUsers.FindAsync(id);
                if (subUser == null)
                {
                    return HttpNotFound();
                }
                return View(subUser);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // POST: SubUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                SubUser subUser = await db.SubUsers.FindAsync(id);
                //subUser.Status = false;// set the Sub User to invisible
                db.SubUsers.Remove(subUser);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _log.Error(ex);
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
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
