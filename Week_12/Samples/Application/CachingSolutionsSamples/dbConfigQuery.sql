

alter database Northwind set trustworthy on with rollback immediate

alter database Northwind set enable_broker with rollback immediate

select * from sys.dm_qn_subscriptions

alter authorization on database :: Northwind to sa


select * from sys.transmission_queue