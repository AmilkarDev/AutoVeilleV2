SELECT NoteAppel, DateResultat 
FROM [atv].[TbRelanceAutoveille] 
WHERE noclient=@noclient
AND id<>@id
AND isnull(NoteAppel,'')<>''
AND DateResultat is not null
union 
SELECT  note, dateTel1 
FROM  suly_gestion.vpa.TbTentative t INNER JOIN suly_gestion.vpa.tbrelance r
ON t.idrelance=r.idrelance
WHERE noclient=@noclient
AND isnull(note,'')<>''
AND dateTel1 is not null
UNION
SELECT t.Note, t.DateTel1
FROM [atv].[TbTentativeAutoveille]  t INNER JOIN 
	atv.TbRelanceAutoveille r ON t.idrelance=r.Id
where isnull(t.Note,'')<>''
AND t.DateTel1 is not null
AND  r.noclient=@noclient
AND r.id<>@id