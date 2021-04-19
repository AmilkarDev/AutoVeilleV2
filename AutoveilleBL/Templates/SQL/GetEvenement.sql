SELECT  Id
      ,DateEvenementDebut
      ,DateEvenementFin
      ,TotalEvenements
      ,DateCreation
      ,Utilisateur
      ,NoCommerce
      ,AppelsPrevusDirEv
      ,AppelsPrevusCASuly
      ,DateModification
      ,UtilisateurModification
      ,DatesConfirmer
  FROM atv.TbEvenement
  WHERE NoCommerce=@nocommerce
  AND id=@id