using Autoveille.Models;
using AutoveilleBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Autoveille.Controllers
{
    public class CompteController : BaseController
    {

        public ActionResult EnregistrerUtilisateur()
        {
            return View();
        }


        //
        // GET: /Compte/

        public ActionResult Connexion()
        {
            return View();
        }


        //public ActionResult Connexion(LoginModel model)
        //{
        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Connexion(LoginModel model, string returnUrl)
        {
            if (Authenticate(model.UserName, model.Password, false))
            {
                //return RedirectToAction("Index", "Home");

                return RedirectToLocal("");
            }

            return View(model);

        }


        public JsonResult sendPassword()
        {
            return Json("True", JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ModifierMotDePasse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModifierMotDePasse(string newPassword)
        {
            return View();
        }
        public ActionResult MotDePasseOubliee()
        {
            return View();
        }

        #region LogOff

        //
        // POST: /Compte/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            SignOut();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Private methods
        private void SetAuthenticationCookie(string aUser, int aUserId, bool isCookiePersistent)
        {
            //Login was successful, create FormsAuthentication ticket.
            System.Web.Security.FormsAuthenticationTicket authTicket = new System.Web.Security.FormsAuthenticationTicket(1, aUser, DateTime.Now, DateTime.Now.AddMinutes(30), isCookiePersistent, aUserId.ToString());

            //Encrypt the cookie
            HttpCookie cookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, System.Web.Security.FormsAuthentication.Encrypt(authTicket));

            //NOTE: You ABSOLUTELY MUST add() the cookie to the cookies collection in order for the User.Identity.IsAuthenticated to return TRUE.
            //      If you do not add() to the cookies collection, the User.Identity.IsAuthenticated will always return FALSE.
            Response.Cookies.Add(cookie);
        }

        private bool Authenticate(string username, string password, bool rememberMe)
        {
            //bool result = (username == "test" && password == "test");
            bool result = Utilisateurs.ValidateUtilisateurGroupe(username, password);
            if (result)
            {
                var utilisateur = Utilisateurs.GetUtilisateurFromAlias(username);
                var commerceSite = Utilisateurs.GetUtilisateurCommerceFromAlias(username);

                var nom = String.IsNullOrEmpty(utilisateur.FirstName) && String.IsNullOrEmpty(utilisateur.LastName) ?
                   username
                    : utilisateur.FirstName + " " + utilisateur.LastName;

                Session["Commerces"] = commerceSite;
                Session["Email"] = utilisateur.Email;
                Session["User"] = utilisateur.UserName;
                Session["UserId"] = utilisateur.UserID;
                Session["FirstName"] = utilisateur.FirstName;
                Session["LastName"] = utilisateur.LastName;
                Session["Nom"] = nom;
                Session["Role"] = utilisateur.Role.ToString();
                Session["Type"] = utilisateur.TypeUsager.ToString();
                //Session["Role"] = utilisateur.Role;

                //FormsAuthentication.SetAuthCookie(username, false);
                SetAuthenticationCookie(username, 1, rememberMe);
            }

            return result;
        }



        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                 return RedirectToAction("InfoConcession", "Home");              
            }
        }

        #endregion


    }
}
