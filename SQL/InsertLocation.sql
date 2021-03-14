DECLARE @nocommerce INT=160
INSERT autoveille.atv.TbRelanceAutoveille
(NoClient,NoSerie,Nom,Prenom,Compagnie,
Langue,TelephoneResidence,TelephoneTravail,Cellulaire,
ExtTravail, Marque, Modele, Annee,Email,NomSignataire,
DateAchat,NbreMois,FinDuTerme,AchatLocation,EtatVehicule,
IdEvenement, Resultat, DateResultat,Type,Provenance,
DateEquite, Date60Terme)
SELECT v.noclient NoClient , v.NoS�rie NoSerie, c.nomclient Nom, c.pr�nomclient Prenom, c.Compagnie Compagnie,
	c.foua Langue, c.T�l1client TelephoneResidence, c.T�l2client TelephoneTravail, c.cellulaire Cellulaire,
	 c.Extention ExtTravail,v.Marque Marque, v.Mod�le Modele, v.Ann�e Annee, c.Email Email,v.NomSignataire NomSignataire,
	 v.DateAchat DateAchat,v.NbreMois NbreMois,v.FinDuTerme FinDuTerme,v.AchatLocation AchatLocation,v.�tatV�hicule EtatVehicule,
	 1 IdEvenement, null Resultat, null DateResultat, 
	 case when type=2 then 2 else  3 end  Type,
	 'Autoveille' Provenance,
	case when type=2 then DateDeRelance else null end DateEquite, null Date60Terme
FROM tbclients c INNER JOIN TbV�hicule v ON c.Noclient=v.noclient
 INNER JOIN tbRelance r ON v.NoClient=r.noclient
AND v.NoS�rie=r.noserie
WHERE V.Nocommerce=@nocommerce
AND resRelance IS NULL
AND type in (2,3)
AND (v.FinDuTerme>DATEADD(m, 1, GETDATE()) )
AND actif=0
