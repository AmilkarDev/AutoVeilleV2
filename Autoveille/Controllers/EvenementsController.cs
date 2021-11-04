using Autoveille.Views;
using AutoveilleBL;
using AutoveilleBL.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;

namespace Autoveille.Controllers
{
    public class EvenementsController : Controller
    {

        List<Evenement> evenements = Evenements.GetAllEvenements(160);
            //new List<Evenement>
            //{
            //    new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2020,10,15).Date,
            //        DateEvenementDebut = new DateTime(2021,10,12).Date,
            //        DateEvenementFin = new DateTime(2021,05,10).Date,
            //        DateModification = new DateTime(2021,10,12).Date,
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=10,
            //        Utilisateur="Malek Ferhi",
            //        UtilisateurModification= "eskan Gued"
            //    },
            //     new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2020,10,15).Date  ,
            //        DateEvenementDebut = new DateTime(2021,10,12).Date  ,
            //        DateEvenementFin = new DateTime(2021,05,10).Date,
            //        DateModification = new DateTime(2021,10,12).Date,
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=11,
            //        Utilisateur="Amilkar",
            //        UtilisateurModification= "eskaa"
            //    },
            //      new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2020,10,15).Date,
            //        DateEvenementDebut = new DateTime(2021,10,12).Date,
            //        DateEvenementFin = new DateTime(2021,05,10).Date,
            //        DateModification = new DateTime(2021,10,12).Date,
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=22,
            //        Utilisateur="Alpha ",
            //        UtilisateurModification= "Alex"
            //    },
            //      new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2020,10,15).Date,
            //        DateEvenementDebut = new DateTime(2021,10,12).Date,
            //        DateEvenementFin = new DateTime(2021,05,10).Date,
            //        DateModification = new DateTime(2021,10,12).Date,
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=1,
            //        Utilisateur="Alpha ",
            //        UtilisateurModification= "Alex"
            //    },
            //      new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2020,10,15).Date,
            //        DateEvenementDebut = new DateTime(2021,10,12).Date,
            //        DateEvenementFin = new DateTime(2021,05,10).Date,
            //        DateModification = new DateTime(2021,10,12).Date,
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=1,
            //        Utilisateur="Alpha ",
            //        UtilisateurModification= "Alex"
            //    },
            //      new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2020,10,15).Date,
            //        DateEvenementDebut = new DateTime(2021,10,12).Date,
            //        DateEvenementFin = new DateTime(2021,05,10).Date,
            //        DateModification = new DateTime(2021,10,12).Date, 
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=1,
            //        Utilisateur="Alpha ",
            //        UtilisateurModification= "Alex"
            //    },
            //      new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2020,10,15).Date,
            //        DateEvenementDebut = new DateTime(2021,10,12).Date,
            //        DateEvenementFin = new DateTime(2021,05,10).Date,
            //        DateModification = new DateTime(2021,10,12).Date,
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=1,
            //        Utilisateur="Noura ",
            //        UtilisateurModification= "Abdelli"
            //    },
            //      new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2020,10,15).Date,
            //        DateEvenementDebut = new DateTime(2021,10,12).Date,
            //        DateEvenementFin = new DateTime(2021,05,10).Date,
            //        DateModification = new DateTime(2021,10,12).Date,
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=1,
            //        Utilisateur="Ons ",
            //        UtilisateurModification= "Jihene"
            //    },
            //      new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2020,10,15).Date,
            //        DateEvenementDebut = new DateTime(2021,10,12).Date,
            //        DateEvenementFin = new DateTime(2021,05,10).Date,
            //        DateModification = new DateTime(2021,10,12).Date,
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=1,
            //        Utilisateur="Nihel ",
            //        UtilisateurModification= "chaima"
            //    },
            //      new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2020,10,15).Date,
            //        DateEvenementDebut = new DateTime(2021,10,12).Date,
            //        DateEvenementFin = new DateTime(2021,05,10).Date,
            //        DateModification = new DateTime(2021,10,12).Date,
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=1,
            //        Utilisateur="Sandra ",
            //        UtilisateurModification= "Alba"
            //    },
            //      new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2020,10,15).Date,
            //        DateEvenementDebut = new DateTime(2021,10,12).Date,
            //        DateEvenementFin = new DateTime(2021,05,10).Date,
            //        DateModification = new DateTime(2021,10,12).Date,
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=1,
            //        Utilisateur="phoebe ",
            //        UtilisateurModification= "harvey"
            //    },
            //      new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2016,05,15).Date,
            //        DateEvenementDebut = new DateTime(2020,05,12).Date,
            //        DateEvenementFin = new DateTime(2021,02,10).Date,
            //        DateModification = new DateTime(2020,08,12).Date,
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=1,
            //        Utilisateur="aoila ",
            //        UtilisateurModification= "Mirel"
            //    },
            //      new Evenement {
            //        TotalEvenements = 15,
            //        AppelsPrevusCASuly = 15,
            //        AppelsPrevusDirEv=10,
            //        DateCreation  = new DateTime(2020,10,15).Date,
            //        DateEvenementDebut = new DateTime(2021,10,12).Date,
            //        DateEvenementFin = new DateTime(2021,05,10).Date,
            //        DateModification = new DateTime(2021,10,12).Date,
            //        DatesConfirmer = 12,
            //        IdEvenement=15,
            //        NoCommerce=1,
            //        Utilisateur="Rachel ",
            //        UtilisateurModification= "Joey"
            //    }
            //};
        // GET: Evenements
        public ActionResult Index()
        {
            //List<Evenement> events = Evenements.GetAllEvenements(160);
            return View();
        }

        public ActionResult EventsList()
        {


            return PartialView(evenements);
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
                List<Evenement> data = Evenements.GetAllEvenements(160);
                int totalRecords = data.Count;
                var ss = evenements[0].DateCreation.ToString("dd-MM-yyyy");
                bool sr = ss.Contains("1");
                // Verification.
                if (!string.IsNullOrEmpty(search) &&
                        !string.IsNullOrWhiteSpace(search))
                {
                    // Apply search      
                    data = data.Where(p => p.IdEvenement.ToString().ToLower().Contains(search.ToLower()) ||
                                            (!p.DateEvenementDebut.HasValue ? false : (p.DateEvenementDebut.Value.ToString("dd-MM-yyyy").Contains(search.ToLower()))) ||
                                            (!p.DateEvenementFin.HasValue ? false : (p.DateEvenementFin.Value.ToString("dd-MM-yyyy").Contains(search.ToLower()))) ||
                                           p.TotalEvenements.ToString().Contains(search.ToLower()) ||
                                           p.DateCreation.ToString("dd-MM-yyyy").Contains(search.ToLower()) ||
                                           p.Utilisateur.ToString().ToLower().Contains(search.ToLower()) || 
                                           p.AppelsPrevusDirEv.ToString().Contains(search.ToLower()) ||
                                           p.AppelsPrevusCASuly.ToString().Contains(search.ToLower()) ||
                                           (!p.DateModification.HasValue ? false : (p.DateModification.Value.ToString("dd-MM-yyyy").Contains(search.ToLower()))) ||
                                           p.UtilisateurModification.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.DatesConfirmer.ToString().Contains(search.ToLower()) ||
                                           p.Utilisateur.ToString().ToLower().Contains(search.ToLower()) ||
                                           p.NoCommerce.ToString().Contains(search.ToLower())).ToList();
                }

                // Sorting.
                //data = this.SortByColumnWithOrder(order, orderDir, data);


                //Filtering 
                data = DataFilter(data, Request.Form);
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

        private List<Evenement> DataFilter(List<Evenement> data, NameValueCollection form)
        {
            string filterId = form.GetValues("columns[0][search][value]")[0];
            string filterDebutEvenement = form.GetValues("columns[1][search][value]")[0];
            string filterFinEvenement = form.GetValues("columns[2][search][value]")[0];
            string filterTotaleEvenement = form.GetValues("columns[3][search][value]")[0];
            string filterdateCreation = form.GetValues("columns[4][search][value]")[0];
            string filterUtilisateur = form.GetValues("columns[5][search][value]")[0];
            string filterNoCommerce = form.GetValues("columns[6][search][value]")[0];
            string filterAppelsPrevusDirEv = form.GetValues("columns[7][search][value]")[0];
            string filterAppelsPrevusCASuly = form.GetValues("columns[8][search][value]")[0];
            string filterDateModification = form.GetValues("columns[9][search][value]")[0];
            string filterUtilisateurModification = form.GetValues("columns[10][search][value]")[0];
            string filterDatesConfirmer = form.GetValues("columns[11][search][value]")[0];
            string filterActif = form.GetValues("columns[12][search][value]")[0];

            
            if (!string.IsNullOrEmpty(filterId) && !string.IsNullOrWhiteSpace(filterId))
            {
                data = data.Where(x => x.IdEvenement.ToString().Contains(filterId)).ToList();
            }

            if (!string.IsNullOrEmpty(filterDebutEvenement) && !string.IsNullOrWhiteSpace(filterDebutEvenement))
            {
                data = data.Where(x => x.DateEvenementDebut.Value.ToString("dd-MM-yyyy").Contains(filterDebutEvenement)).ToList();
            }

            if (!string.IsNullOrEmpty(filterFinEvenement) && !string.IsNullOrWhiteSpace(filterFinEvenement))
            {
                data = data.Where(x => x.DateEvenementFin.Value.ToString("dd-MM-yyyy").Contains(filterFinEvenement)).ToList();
            }

            if (!string.IsNullOrEmpty(filterTotaleEvenement) && !string.IsNullOrWhiteSpace(filterTotaleEvenement))
            {
                data = data.Where(x => x.TotalEvenements.ToString().Contains(filterTotaleEvenement)).ToList();
            }
            if (!string.IsNullOrEmpty(filterdateCreation) && !string.IsNullOrWhiteSpace(filterdateCreation))
            {
                data = data.Where(x => x.DateCreation.ToString("dd-MM-yyyy").Contains(filterdateCreation)).ToList();
            }

            if (!string.IsNullOrEmpty(filterUtilisateur) && !string.IsNullOrWhiteSpace(filterUtilisateur))
            {
                data = data.Where(x => x.Utilisateur.Contains(filterUtilisateur)).ToList();
            }

            if (!string.IsNullOrEmpty(filterNoCommerce) && !string.IsNullOrWhiteSpace(filterNoCommerce))
            {
                data = data.Where(x => x.NoCommerce.ToString().Contains(filterNoCommerce)).ToList();
            }
            
            if (!string.IsNullOrEmpty(filterAppelsPrevusDirEv) && !string.IsNullOrWhiteSpace(filterAppelsPrevusDirEv))
            {
                data = data.Where(x => x.AppelsPrevusDirEv.ToString().Contains(filterAppelsPrevusDirEv)).ToList();
            }

            if (!string.IsNullOrEmpty(filterAppelsPrevusCASuly) && !string.IsNullOrWhiteSpace(filterAppelsPrevusCASuly))
            {
                data = data.Where(x => x.AppelsPrevusCASuly.ToString().Contains(filterAppelsPrevusCASuly)).ToList();
            }

            if (!string.IsNullOrEmpty(filterDateModification) && !string.IsNullOrWhiteSpace(filterDateModification))
            {
                data = data.Where(x => x.DateModification.ToString().Contains(filterDateModification)).ToList();
            }

            if (!string.IsNullOrEmpty(filterDatesConfirmer) && !string.IsNullOrWhiteSpace(filterDatesConfirmer))
            {
                data = data.Where(x => x.DatesConfirmer.ToString().Contains(filterDatesConfirmer)).ToList();
            }

            if (!string.IsNullOrEmpty(filterActif) && !string.IsNullOrWhiteSpace(filterActif))
            {
                if (filterActif == "actif")
                    data = data.Where(x => x.Actif).ToList();
                else
                    data = data.Where(x => x.Actif == false).ToList(); 
            }
            return data;
        }



        [HttpGet]
        public ActionResult ModifierEvenement(int IdEvenement)
        {
            List<Evenement> data = evenements;
            Evenement eventt = evenements.Where(x => x.IdEvenement == IdEvenement).First();
            return PartialView(eventt);
        }

        [HttpPost]
        [ValidateAjax]
        public ActionResult ModifierEvenement(Evenement evenement)
        {
            if (ModelState.IsValid)
            {
                int s = Evenements.SaveEvenement(evenement);
                return Json( new { success = true, message = "Mise a jour d'evenements terminé avec succés !!" } , JsonRequestBehavior.AllowGet) ;
            }
            else
            {
                //return PartialView(evenement);
                return Json(new { success = false,
                    message = RazorViewToString.RenderRazorViewToString(this,"ModifierEvenement", evenement) ,
                errors = "ss"
                } , JsonRequestBehavior.AllowGet);
            }
            
        } 


        [HttpGet]
        public ActionResult AjouterEvenement()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAjax]
        public JsonResult AjouterEvenement(Evenement evenement)
        {
            if (ModelState.IsValid)
            {
                int s = Evenements.InsertEvenement(evenement); 
                return Json("Evenement ajoutée avec succés", JsonRequestBehavior.AllowGet);
            }
            else
            {
                //return PartialView(evenement);
                return Json(new
                {
                    success = false,
                    message = RazorViewToString.RenderRazorViewToString(this, "ModifierEvenement", evenement),
                    errors = "ss"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SupprimerEvenement(Evenement evenement)
        {
            bool result = Evenements.DeleteEvenement(evenement);
            return result? Json("Evenement supprimée avec succés"): Json("Problème du suppression d'evenements sélectionnée");
        }
    }
}