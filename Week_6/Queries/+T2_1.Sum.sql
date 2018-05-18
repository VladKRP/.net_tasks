--1.	Найти общую сумму всех заказов из таблицы Order Details с учетом количества закупленных товарови скидок по ним. 
-- Результатом запроса должна быть одна запись с одной колонкой с названием колонки 'Totals'.

select sum(UnitPrice * Quantity *(1 - Discount)) 'Totals'
from Northwind.Northwind.[Order Details]

--2.	По таблице Orders найти количество заказов, которые еще не были доставлены 
--(т.е. в колонке ShippedDate нет значения даты доставки). Использовать при этом запросе только оператор COUNT.
-- Не использовать предложения WHERE и GROUP.

select OrderID,ShippedDate from Northwind.Northwind.Orders where ShippedDate is null

select count(OrderID) - count(ShippedDate)  as 'Undelivered orders' from Northwind.Northwind.Orders


--3.	По таблице Orders найти количество различных покупателей (CustomerID), сделавших заказы.
-- Использовать функцию COUNT и не использовать предложения WHERE и GROUP.
select count(distinct CustomerID) as 'Unique Customers' from Northwind.Northwind.Customers