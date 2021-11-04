using AutoveilleBL.Models.Web;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoveilleBL;
using System;
using System.Linq;
using AutoveilleBL.Models;
using Autoveille.Views;

namespace Autoveille.Controllers
{
    public class UtilisateursController : Controller
    {
        private List<Concession> _commerces = Concessions.GetConcessionsActiveAutoveilleV2();

        List<UtilisateurSite> users = Utilisateurs.GetAllUtilisateurs();

        // GET: Utilisateurs
        public ActionResult Index()
        {
            List<UtilisateurSite> users = Utilisateurs.GetAllUtilisateurs();
            return View();
        }

        [HttpPost]
        public ActionResult GetData()
        {
            // Initialization.
            JsonResult result = new JsonResult();

            try
            {
                // Initialization.
                string search = Request.Form.GetValues("search[value]")[0];
                //string[] str = Request.Form.GetValues("columns[5][search][value]");
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                // Loading.
                List<UtilisateurSite> data = Utilisateurs.GetAllUtilisateurs();
                int totalRecords = data.Count;
                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                        !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search      
                    data = data.Where(p => p.UserName.Contains(search.ToLower()) ||
                                           p.FirstName.Contains(search.ToLower()) ||
                                           p.LastName.ToLower().Contains(search.ToLower()) ||
                                           p.Role.ToString().Contains(search.ToLower()) ||
                                           p.TypeUsager.ToString().Contains(search.ToLower()) ||
                                           p.NoCommerce.ToString().Contains(search.ToLower())).ToList();
                }

                //Sorting.
                //data = this.SortByColumnWithOrder(order, orderDir, data);


                //Filtering 
                //data = DataFilter(data, Request.Form);
                // Filter record count.
                int recFilter = data.Count;

                // Apply pagination.
                data = data.Skip(startRec).Take(pageSize).ToList();

                // Loading drop down lists.
                result = this.Json(new { draw = Convert.ToInt32(draw), recordsTotal = totalRecords, recordsFiltered = recFilter, data = data }, JsonRequestBehavior.AllowGet);
                //result = Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Return info.
            return result;
        }

        [HttpPost]
        public JsonResult SupprimerUtilisateur(UtilisateurSite user)
        {
            bool result = Utilisateurs.DeleteUtilisateur(user);
            return result ? Json("Utilisateur supprimée avec succés") : Json("Problème du suppression de l'utilisateur sélectionné");
        }

        [HttpGet]
        public ActionResult ModifierUtilisateur(int UserID)
        {
            ViewData["commerces"] = ToSelectList(_commerces);
            List<UtilisateurSite> data = users;
            UtilisateurSite user = users.Where(x => x.UserID == UserID).First();
            return PartialView(user);
        }
        [HttpPost]
        [ValidateAjax]
        public ActionResult ModifierUtilisateur(UtilisateurSite user)
        {
            if (ModelState.IsValid)
            {
                if (user.NoCommerce != 0)
                    user.NomCommerce = _commerces.Where(c => c.NoCommerce == user.NoCommerce).Select(c => c.NomCommerce).FirstOrDefault();

                int s = Utilisateurs.SaveUtilisateur(user);

                if (s > 0)
                    return Json(new { success = true, message = "Mise a jour de l'utilisateur terminé avec succés !!" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, message = "La mise a jour de l'utilisateur n'est pas validée !!" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = RazorViewToString.RenderRazorViewToString(this, "ModifierUtilisateur", user),
                    errors = "ss"
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult AjouterUtilisateur()
        {
            ViewData["commerces"] = ToSelectList(_commerces);
            return PartialView();
        }

        [NonAction]
        public SelectList ToSelectList(List<Concession> commerces)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (Concession commerce in commerces)
            {
                list.Add(new SelectListItem()
                {
                    Text = commerce.NomCommerce,
                    Value = commerce.NoCommerce.ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }

        [HttpPost]
        public JsonResult AjouterUtilisateur(UtilisateurSite user)
        {
            if (user.NoCommerce != 0)
                user.NomCommerce = _commerces.Where(c => c.NoCommerce == user.NoCommerce).Select(c => c.NomCommerce).FirstOrDefault();

            int s = Utilisateurs.InsertUtilisateur(user);

            if (s != 0)
                return Json(new { success = true, message = "Utilisateur ajouté avec succés" });
            else
                return Json(new { success = false, message = "Utilisateur non ajouté !!" });
        }
    }
}