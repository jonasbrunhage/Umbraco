using CMS_Umbraco_Inlämning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace CMS_Umbraco_Inlämning.Controllers
{
    public class ContactController : SurfaceController
    {
        public ActionResult RenderContactForm()
        {
            return PartialView("_ContactForm", new ContactFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleContactForm(ContactFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            MailMessage email = new MailMessage("noreply@local.com", model.Email)
            {
                Subject = "Contact Request",
                Body = model.Message
            };

            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.Send(email);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            TempData["IsSuccesful"] = true;

            return CurrentUmbracoPage();
        }
    }
}