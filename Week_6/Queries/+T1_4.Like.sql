--1.	В таблице Products найти все продукты (колонка ProductName), где встречается подстрока 'chocolade'. 
--Известно, что в подстроке 'chocolade' может быть изменена одна буква 'c'
-- в середине - найти все продукты, которые удовлетворяют этому условию. 

select ProductName from Northwind.Northwind.Products where ProductName like '%cho_olade%'