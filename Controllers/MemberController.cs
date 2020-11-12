using CMS_Umbraco_Inlämning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Mvc;

namespace CMS_Umbraco_Inlämning.Controllers
{
    public class MemberController : SurfaceController
    {
        // GET: Member
        public ActionResult RenderLogin()
        {
            return PartialView("_Login", new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SubmitLogin(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Username, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Username, false);
                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("", "The username or password provided is incorrect");
                }
            }
            return CurrentUmbracoPage();
        }
        public ActionResult SubmitLogout()
        {
            TempData.Clear();
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToCurrentUmbracoPage();
        }
    }
}