/*
1.	Выдать всех поставщиков (колонка CompanyName в таблице Suppliers), у которых нет хотя бы одного продукта 
на складе (UnitsInStock в таблице Products равно 0). Использовать вложенный SELECT для этого запроса с использованием оператора IN. 
*/

select CompanyName from Northwind.Northwind.Suppliers 
where SupplierId in (select SupplierID from Northwind.Northwind.Products where UnitsInStock = 0)

/*
2.	Выдать всех продавцов, которые имеют более 150 заказов. Использовать вложенный SELECT.
*/
 
 select * from Northwind.Northwind.Employees 
 where EmployeeID in (select EmployeeID from Northwind.Northwind.Orders 
										group by EmployeeID 
										having count(EmployeeID) > 150)

/*
3.	Выдать всех заказчиков (таблица Customers), которые не имеют ни одного заказа (подзапрос по таблице Orders). Использовать оператор EXISTS.
*/
select * from Northwind.Northwind.Customers  
where CustomerID not in (select CustomerID from Northwind.Northwind.Orders group by CustomerID having count(CustomerID) > 0)