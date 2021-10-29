using AutoveilleBL.Models.Web;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoveilleBL;
using System;
using System.Linq;
using AutoveilleBL.Models;

namespace Autoveille.Controllers
{
    public class UtilisateursController : Controller
    {
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
    }
}