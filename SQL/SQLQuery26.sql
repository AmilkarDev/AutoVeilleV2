begin transaction
update atv.TbRelanceAutoveille
set is
--select *
from autoveille.atv.TbRelanceAutoveille
where type in (1,2,3)
--group by NoClient
--where type in (4,5)