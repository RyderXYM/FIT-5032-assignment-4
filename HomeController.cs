using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TEK.Models;

namespace TEK.Controllers
{
    public class HomeController : Controller
    {
        private TEK_models db = new TEK_models();

        public ActionResult Rent([Bind(Include = "PostCode, MaxBeds, MinBeds, MaxPrice, MinPrice")] TEKmodel Tekmodel)
        {
            var properties = db.Properties.Where(p => p.PostCode == Tekmodel.PostCode);

            List<Property> list = properties.ToList();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if ((Tekmodel.MaxBeds > list[i].Bathrooms) && (Tekmodel.MinBeds < list[i].Bathrooms) && (Tekmodel.MaxPrice > list[i].Price) && (Tekmodel.MinPrice < list[i].Price) && (list[i].Type == "Rent"))
                {

                }
                else
                {
                    list.RemoveAt(i);
                }
            }

            if (list.Count == 0)
            {
                ViewBag.error2 = "No property matched your search!";
                return View();
            }
            else
            {
                TempData["List"] = list;
                return Redirect("/Properties/Index?listtag=1");
            }

        }

        /* client part */
        [Authorize(Roles = "Client")]
        public ActionResult Sell()
        {
            return View();
        }
        [Authorize(Roles = "Client")]
        public ActionResult FindAgent()
        {
            return View();
        }
        [Authorize(Roles = "Client")]
        public ActionResult MyProperty()
        {
            return View();
        }
        [Authorize(Roles = "Client")]
        public ActionResult MyOrder()
        {
            return View();
        }
        /* agent part */

        [Authorize(Roles = "Agent")]
        public ActionResult MyAssignment()
        {

            var CurrentUser = User.Identity.GetUserId();
            var CurrentAgentId = db.Agents.Where(a => a.UserId == CurrentUser);


            if (CurrentAgentId == null)
            {
                return HttpNotFound();
            }//end of if
 

            var property = db.Properties.Where(p => p.AgentId == "Agent01").ToList();

            return View(property);
        }
        /* admin part */
        [Authorize(Roles = "Administrator")]
        public ActionResult ManageProperty()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult ManageClient()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult ManageAgent()
        {
            return View();
        }

        
        //([Bind(Include = "PId,AgentId,UserId,PostCode,Address,Type,Price,Beds,Bathrooms,Carpark,Area,Pic,Date")] Property property)
        public ActionResult Index([Bind(Include = "PostCode, MaxBeds, MinBeds, MaxPrice, MinPrice")] TEKmodel Tekmodel)
        {
            var properties = db.Properties.Where(p => p.PostCode == Tekmodel.PostCode);

            List<Property> list = properties.ToList();

            for (int i = list.Count - 1; i >= 0; i--)
            {
                if ((Tekmodel.MaxBeds > list[i].Bathrooms) && (Tekmodel.MinBeds < list[i].Bathrooms) && (Tekmodel.MaxPrice > list[i].Price) && (Tekmodel.MinPrice < list[i].Price)) //&& (list[i].Type == "Sell"))
                {
                    
                }
                else
                {
                    list.RemoveAt(i);
                }
            }

            if (list.Count == 0)
            {
                ViewBag.error1 = "No property matched your search!";
                return View();
            }
            else
            {
                TempData["List"] = list;
                //t = list;
                //return RedirectToAction("Index", "Properties", new { listtag = 1});
                return Redirect("/Properties/Index?listtag=1");
            }

        }
    }
}

