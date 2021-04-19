SELECT tr.id,ra.TypeRelanceAffichage, ra.typerelance, tr.TypeRelanceDesc,
 tr.OrdreAfichagePortail, SUM(case when ra.id is null then 0 else 1 end) nbreFiche
FROM   atv.TbRelanceAutoveille ra 
	INNER JOIN  atv.TbEvenement  e ON  e.Id=ra.IdEvenement AND e.id=@id 
	RIGHT JOIN atv.TbTypeRelance tr 	ON ra.TypeRelanceAffichage=tr.id
	and  tr.id in (1,2, 7,8,9,10) 
WHERE tr.id in (1,2, 7,8,9,10) 
  GROUP BY ra.TypeRelanceAffichage, ra.typerelance,tr.TypeRelanceDesc,
 tr.OrdreAfichagePortail, tr.id