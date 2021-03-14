SELECT top(1) Id
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
  AND DateEvenementFin>=cast(getdate() as Date)
  ORDER BY DateEvenementDebut