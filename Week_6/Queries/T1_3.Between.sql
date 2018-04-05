--1.	¬ыбрать все заказы (OrderID) из таблицы Order Details (заказы не должны повтор€тьс€), 
--где встречаютс€ продукты с количеством от 3 до 10 включительно Ц это колонка Quantity в таблице Order Details.
-- »спользовать оператор BETWEEN. «апрос должен возвращать только колонку OrderID.

select distinct OrderID from Northwind.Northwind.[Order Details] where Quantity between 3 and 10

--2.	¬ыбрать всех заказчиков из таблицы Customers, у которых название страны начинаетс€ на буквы из диапазона b и g.
-- »спользовать оператор BETWEEN. ѕроверить, что в результаты запроса попадает Germany. 
-- «апрос должен возвращать только колонки CustomerID и Country и отсортирован по Country.
select CustomerID, Country 
from Northwind.Northwind.Customers 
where Country between 'b' and 'i' 
order by Country

--3.	¬ыбрать всех заказчиков из таблицы Customers, 
--у которых название страны начинаетс€ на буквы из диапазона b и g, не использу€ оператор BETWEEN. 

select CustomerID, Country from Northwind.Northwind.Customers
where Country like '^[b-g]'