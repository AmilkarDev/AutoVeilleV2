SELECT top (1) ID,typeboni,  montant, priorite 
FROM TbValeurVehiculeAjustement 
WHERE nocommerce= @nocommerce 
	AND ISNULL(anneedebut, @annee)<=@annee 
	AND ISNULL(anneefin, @annee)>=@annee 
	AND ISNULL(marque, @marque) LIKE @marque + '%' 
	AND ISNULL(modele, @modele) LIKE @modele + '%' 
	AND ISNULL(datedebut, CAST(0 as datetime))<=GETDATE() 
	AND ISNULL(datefin, CAST(100000 as datetime))>=GETDATE() 
	AND typeBoni='C'
ORDER BY  priorite

SELECT top (1) ID,typeboni,  montant, priorite 
FROM TbValeurVehiculeAjustement 
WHERE nocommerce= @nocommerce 
	AND ISNULL(anneedebut, @annee)<=@annee 
	AND ISNULL(anneefin, @annee)>=@annee 
	AND ISNULL(marque, @marque) LIKE @marque + '%' 
	AND ISNULL(modele, @modele) LIKE @modele + '%' 
	AND ISNULL(datedebut, CAST(0 as datetime))<=GETDATE() 
	AND ISNULL(datefin, CAST(100000 as datetime))>=GETDATE() 
	AND typeBoni='M'
ORDER BY  priorite


select CAST(0 as datetime),*
from TbValeurVehiculeAjustement