--1.	Ќайти общую сумму всех заказов из таблицы Order Details с учетом количества закупленных товарови скидок по ним. 
-- –езультатом запроса должна быть одна запись с одной колонкой с названием колонки 'Totals'.

select sum(UnitPrice * Quantity - (UnitPrice * Quantity * Discount)) 'Totals'
from Northwind.Northwind.[Order Details]
--2.	ѕо таблице Orders найти количество заказов, которые еще не были доставлены 
--(т.е. в колонке ShippedDate нет значени€ даты доставки). »спользовать при этом запросе только оператор COUNT.
-- Ќе использовать предложени€ WHERE и GROUP.

select count(OrderID) as 'Undelivered orders' from Northwind.Northwind.Orders where ShippedDate is null


--3.	ѕо таблице Orders найти количество различных покупателей (CustomerID), сделавших заказы.
-- »спользовать функцию COUNT и не использовать предложени€ WHERE и GROUP.
select count(distinct CustomerID) as 'Unique Customers' from Northwind.Northwind.Customers