--select *
--from vp.Previsions
--order by id desc
DECLARE @nocommerce INT=160
INSERT autoveille.atv.TbRelanceAutoveille
(NoClient,NoSerie,Nom,Prenom,Compagnie,
Langue,TelephoneResidence,TelephoneTravail,Cellulaire,
ExtTravail, Marque, Modele, Annee,Email,NomSignataire,
DateAchat,NbreMois,FinDuTerme,AchatLocation,EtatVehicule,
IdEvenement, Resultat, DateResultat,Type,Provenance,
DateEquite, Date60Terme)
SELECT   top (111) v.noclient NoClient , v.NoSérie NoSerie, c.nomclient Nom, c.prénomclient Prenom, c.Compagnie Compagnie,
	c.foua Langue, c.Tél1client TelephoneResidence, c.Tél2client TelephoneTravail, c.cellulaire Cellulaire,
	 c.Extention ExtTravail,v.Marque Marque, v.Modèle Modele, v.Année Annee, c.Email Email,v.NomSignataire NomSignataire,
	 v.DateAchat DateAchat,v.NbreMois NbreMois,v.FinDuTerme FinDuTerme,v.AchatLocation AchatLocation,v.ÉtatVéhicule EtatVehicule,
	 1 IdEvenement, null Resultat, null DateResultat, 4  Type,
	 'Taktic' Provenance,
	null DateEquite, null Date60Terme
--select  StatsQualifiant, left( replace(StatsQualifiant, 'Vente  #',''),2),replace(StatsQualifiant, 'Vente  #',''), *
from vp.PrevisionsVehiculesQualifiant pv INNER JOIN TbVéhicule v ON pv.NoClient=v.NoClient AND pv.NoSerie=v.NoSérie
INNER JOIN TbClients c ON v.NoClient=c.noclient
where IDPrevisions=805
and left(  ltrim(replace(StatsQualifiant, 'Vente  #','')),2)='1;'
AND Categorie='F'
AND statut is null
order by DateCategorie


--DECLARE @nocommerce INT=160
INSERT autoveille.atv.TbRelanceAutoveille
(NoClient,NoSerie,Nom,Prenom,Compagnie,
Langue,TelephoneResidence,TelephoneTravail,Cellulaire,
ExtTravail, Marque, Modele, Annee,Email,NomSignataire,
DateAchat,NbreMois,FinDuTerme,AchatLocation,EtatVehicule,
IdEvenement, Resultat, DateResultat,Type,Provenance,
DateEquite, Date60Terme)
SELECT   top (112) v.noclient NoClient , v.NoSérie NoSerie, c.nomclient Nom, c.prénomclient Prenom, c.Compagnie Compagnie,
	c.foua Langue, c.Tél1client TelephoneResidence, c.Tél2client TelephoneTravail, c.cellulaire Cellulaire,
	 c.Extention ExtTravail,v.Marque Marque, v.Modèle Modele, v.Année Annee, c.Email Email,v.NomSignataire NomSignataire,
	 v.DateAchat DateAchat,v.NbreMois NbreMois,v.FinDuTerme FinDuTerme,v.AchatLocation AchatLocation,v.ÉtatVéhicule EtatVehicule,
	 1 IdEvenement, null Resultat, null DateResultat, 5  Type,
	 'Taktic' Provenance,
	null DateEquite, null Date60Terme
--select top (110) StatsQualifiant, left( replace(StatsQualifiant, 'Vente  #',''),2),replace(StatsQualifiant, 'Vente  #',''), *
FROM vp.PrevisionsVehiculesQualifiant pv INNER JOIN TbVéhicule v ON pv.NoClient=v.NoClient AND pv.NoSerie=v.NoSérie
INNER JOIN TbClients c ON v.NoClient=c.noclient
WHERE IDPrevisions=805
AND left(  ltrim(replace(StatsQualifiant, 'Vente  #','')),2)='1;'
AND Categorie='S'
AND statut is null
order by DateCategorie

