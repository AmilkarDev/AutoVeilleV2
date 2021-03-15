select date, DebutMins, DebutMins / 60 , DebutMins%60, count(*)
from rh.TbHoraire h inner join rh.TbEmploye e on h.IdEmploye=e.Id
where date>'2021/01/06'
and date <'2021/01/10'
and statut=1
and nocommerce in (select nocommerce from TbCommerces where ActifSMSCovid=1)
group by date, DebutMins, DebutMins / 60 , DebutMins%60
order by  date, DebutMins, DebutMins / 60 , DebutMins%60