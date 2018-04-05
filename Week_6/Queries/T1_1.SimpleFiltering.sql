
/*
1.	Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года (колонка ShippedDate) 
включительно и которые доставлены с ShipVia >= 2. Запрос должен возвращать только колонки OrderID, ShippedDate и ShipVia. 
*/
select OrderID, ShippedDate, ShipVia 
from Northwind.Northwind.Orders 
where ShippedDate > '1998-05-06' and ShipVia >= 2

/*
2.	Написать запрос, который выводит только недоставленные заказы из таблицы Orders. 
В результатах запроса возвращать для колонки ShippedDate вместо значений NULL строку ‘Not Shipped’ (использовать системную функцию CASЕ).
 Запрос должен возвращать только колонки OrderID и ShippedDate.
*/

select OrderID, ShippedDate = 'Not shipped'
from Northwind.Northwind.Orders
where ShippedDate is null

/*
3.	Выбрать в таблице Orders заказы, которые были доставлены после 6 мая 1998 года (ShippedDate) 
не включая эту дату или которые еще не доставлены. В запросе должны возвращаться только колонки OrderID
 (переименовать в Order Number) и ShippedDate (переименовать в Shipped Date). 
 В результатах запроса возвращать для колонки ShippedDate вместо значений NULL строку ‘Not Shipped’,
  для остальных значений возвращать дату в формате по умолчанию.
*/

-- select OrderID as 'Order Number', ShippedDate
-- from Northwind.Northwind.Orders 
-- where ShippedDate > '1998-06-06'
-- union
-- select OrderID as 'Order Number', ShippedDate = 'Not Shipped'
-- from Northwind.Northwind.Orders 
-- where ShippedDate is null



