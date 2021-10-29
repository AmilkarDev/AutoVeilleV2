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
      ,Actif
  FROM atv.TbEvenement
  WHERE NoCommerce=@nocommerce
