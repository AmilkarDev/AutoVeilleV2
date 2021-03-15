DECLARE @nocommerce INT=160
INSERT autoveille.atv.TbRelanceAutoveille
(NoClient,NoSerie,Nom,Prenom,Compagnie,
Langue,TelephoneResidence,TelephoneTravail,Cellulaire,
ExtTravail, Marque, Modele, Annee,Email,NomSignataire,
DateAchat,NbreMois,FinDuTerme,AchatLocation,EtatVehicule,
IdEvenement, Resultat, DateResultat,Type,Provenance,
DateEquite, Date60Terme)
SELECT v.noclient NoClient , v.NoSérie NoSerie, c.nomclient Nom, c.prénomclient Prenom, c.Compagnie Compagnie,
	c.foua Langue, c.Tél1client TelephoneResidence, c.Tél2client TelephoneTravail, c.cellulaire Cellulaire,
	 c.Extention ExtTravail,v.Marque Marque, v.Modèle Modele, v.Année Annee, c.Email Email,v.NomSignataire NomSignataire,
	 v.DateAchat DateAchat,v.NbreMois NbreMois,v.FinDuTerme FinDuTerme,v.AchatLocation AchatLocation,v.ÉtatVéhicule EtatVehicule,
	 1 IdEvenement, null Resultat, null DateResultat, 1  Type,
	 'Autoveille' Provenance,
	DateEquite DateEquite, Date60Terme Date60Terme
FROM tbclients c INNER JOIN TbVéhicule v ON c.Noclient=v.noclient
 INNER JOIN TbEquite e ON v.NoClient=e.noclient
AND v.NoSérie=e.noserie
LEFT JOIN  (SELECT noclient, noserie, MAX(datevp) maxdatevp FROM TbRelanceVP  GROUP BY noclient, noserie )
	vp ON e.noclient=vp.noclient AND e.noserie=vp.noserie
LEFT JOIN 
		(SELECT noclient, nosérie 
		 FROM TbHistoClient2
		 WHERE (Type_CIG='C' OR ISNULL(Type_CIG,'')='') AND Dernièrevisite2 >DATEADD(m, -14, GETDATE())
		 GROUP BY  noclient, nosérie
		) h ON v.noclient=h.Noclient AND v.nosérie=h.NoSérie
--LEFT JOIN 
--		(
--			SELECT top (1) ID,typeboni,  montant, priorite ,anneedebut,anneefin,marque,modele
--			FROM TbValeurVehiculeAjustement 
--			WHERE nocommerce= 371 
--				AND ISNULL(datedebut, CAST(0 as datetime))<=GETDATE() 
--				AND ISNULL(datefin, CAST(100000 as datetime))>=GETDATE() 
--				AND typeBoni='C'
--			ORDER BY  priorite
--		) boniC
--ON 	 ISNULL(boniC.anneedebut, v.Année)<=v.Année 
	--AND ISNULL(boniC.anneefin, v.Année)>=v.Année 
	--AND ISNULL(boniC.marque, v.Marque) LIKE v.Marque + '%' 
	--AND ISNULL(boniC.modele, v.Modèle) LIKE v.Modèle + '%' 
WHERE V.Nocommerce=@nocommerce
AND etat=11
AND actif=0
AND ( 
		(v.NbreMois =0 AND h.Noclient IS NOT NULL)
		OR
		v.NbreMois<>0
		)
AND AutoVeille='O'
AND e.prochaineRelance <= GETDATE()
AND (v.FinDuTerme>DATEADD(m, 6, GETDATE()) )
AND (vp.noclient is null or dateadd(m, 6, maxdatevp)<=getdate())	
ORDER BY dateetat desc
