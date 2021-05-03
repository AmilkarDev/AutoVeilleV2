SELECT NoteAgentAppel
FROM suly_gestion.dbo.tbclients
where noclient=@noclient
AND isnull(NoteAgentAppel,'')<>''