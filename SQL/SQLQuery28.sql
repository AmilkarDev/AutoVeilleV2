SELECT e.Id ,DateEvenementDebut,DateEvenementFin ,AppelsPrevusDirEv 
		,AppelsPrevusCASuly ,NoCommerce ,DateCreation ,Utilisateur,
		sum(case when ra.Type=1 then 1 else 0 end) NbreAppelsEquite, 
		sum(case when ra.Type=2 then 1 else 0 end) NbreAppelsLocationEquite,
		sum(case when ra.Type=3 then 1 else 0 end) NbreAppelsLocation,
		sum(case when ra.Type=4 then 1 else 0 end) NbreAppelsFinancementTaktic,
		sum(case when ra.Type=5 then 1 else 0 end) NbreAppelsServiceTaktic,
		sum(case when ra.Type=7 then 1 else 0 end) NbreAppelsWalkOut,
		sum(case when ra.Type=8 then 1 else 0 end) NbreAppelsLeads,
		sum(case when ra.Type=9 then 1 else 0 end) NbreAppelsRDVService,
		sum(case when ra.Type=10 then 1 else 0 end) NbreAppelsFinancementTakticLandingPage,
		sum(case when ra.Type=11 then 1 else 0 end) NbreAppelsFinancementTakticLandingPage
FROM atv.TbEvenement  e INNER JOIN 
	atv.TbRelanceAutoveille ra on e.Id=ra.IdEvenement
group by e.id,DateEvenementDebut,DateEvenementFin ,AppelsPrevusDirEv 
		,AppelsPrevusCASuly ,NoCommerce ,DateCreation ,Utilisateur 
                                  --WHERE nocommerce=@noCommmerce AND DateEvenementFin>=@dateFrom 