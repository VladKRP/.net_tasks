
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

-- with case
select OrderID,
case
  when ShippedDate is null then 'Not Shipped'
end ShippedDate
from Northwind.Northwind.Orders
where ShippedDate is null

-- without case
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

-- with case
select OrderID as 'Order Number',
case 
  when ShippedDate is null then 'Not Shipped'
  else convert(varchar, ShippedDate, 101)-- конвертация в строковое представление(только дата), необходимо приведение т.к. колонка с datetime не может хранить строки
end 'Shipped Date'
from Northwind.Northwind.Orders
where ShippedDate > '1998-06-06' or ShippedDate is null

-- without case
select res.OrderID as 'Order Number', res.[Shipped Date] 
from 
(select OrderID, convert(varchar, ShippedDate, 101) as 'Shipped Date' 
  from Northwind.Northwind.Orders 
  where ShippedDate > '1998-06-06'
  union
  select OrderID, ShippedDate = 'Not Shipped'
  from Northwind.Northwind.Orders 
  where ShippedDate is null
) as res




