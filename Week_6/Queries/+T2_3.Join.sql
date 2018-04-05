
/*
1.	Определить продавцов, которые обслуживают регион 'Western' (таблица Region). 
*/

select distinct e.EmployeeID, e.FirstName + e.LastName as 'Seller', r.RegionDescription 
from Northwind.Northwind.Employees as e 
inner join Northwind.Northwind.EmployeeTerritories et on e.EmployeeID = et.EmployeeID
inner join Northwind.Northwind.Territories t on et.TerritoryID = t.TerritoryID
inner join Northwind.Northwind.Region r on t.RegionID = r.RegionID
where r.RegionDescription = 'Western'

/*
2.	
Выдать в результатах запроса имена всех заказчиков из таблицы Customers и суммарное количество их заказов из таблицы Orders.
Принять во внимание, что у некоторых заказчиков нет заказов, но они также должны быть выведены в результатах запроса.
Упорядочить результаты запроса по возрастанию количества заказов.
*/

select c.ContactName as 'Customer Name', count(o.OrderID) as 'Orders Amount' 
from Northwind.Northwind.Customers as c 
left join Northwind.Northwind.Orders as o on c.CustomerID = o.CustomerID
group by c.ContactName
order by 'Orders Amount'