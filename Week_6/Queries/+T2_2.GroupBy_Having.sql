--1.	По таблице Orders найти количество заказов с группировкой по годам.
 --В результатах запроса надо возвращать две колонки c названиями Year и Total.
 -- Написать проверочный запрос, который вычисляет количество всех заказов.

select count(OrderID) as 'Total Orders Amount' from Northwind.Northwind.Orders 

select YEAR(OrderDate) as 'Year', COUNT(OrderID) as 'Total' 
from Northwind.Northwind.Orders 
group by YEAR(OrderDate)


--2.	По таблице Orders найти количество заказов, cделанных каждым продавцом.
 --Заказ для указанного продавца – это любая запись в таблице Orders, где в колонке EmployeeID задано значение для данного продавца.
 -- В результатах запроса надо возвращать колонку с именем продавца (Должно высвечиваться имя полученное конкатенацией LastName & FirstName.
 --  Эта строка LastName & FirstName должна быть получена отдельным запросом в колонке основного запроса.
 --   Также основной запрос должен использовать группировку по EmployeeID.)
	-- с названием колонки ‘Seller’ и колонку c количеством заказов возвращать с названием 'Amount'.
	--  Результаты запроса должны быть упорядочены по убыванию количества заказов. 

select emp.FirstName + emp.LastName as 'Seller', oa.Amount 
from Northwind.Northwind.Employees as emp 
inner join (select ord.EmployeeID, count(ord.EmployeeID) as 'Amount' 
	  from Northwind.Northwind.Orders as ord 
	  group by ord.EmployeeID) as oa on emp.EmployeeID = oa.EmployeeID 
order by oa.Amount desc



--3.	По таблице Orders найти количество заказов, сделанных каждым продавцом и для каждого покупателя.
 --Необходимо определить это только для заказов, сделанных в 1998 году. 

select EmployeeID as 'Seller', CustomerID as 'Customer', count(EmployeeID) as 'Orders Amount'
from Northwind.Northwind.Orders 
where YEAR(OrderDate) = 1998
group by EmployeeID,CustomerID

--4.	Найти покупателей и продавцов, которые живут в одном городе.
 --Если в городе живут только один или несколько продавцов, или только один или несколько покупателей,
 -- то информация о таких покупателя и продавцах не должна попадать в результирующий набор.
 -- Не использовать конструкцию JOIN. 

-- Неявный Join, по сути конструкция не используется

 select emp.EmployeeID, cust.CustomerID, emp.City, emp.Country
 from Northwind.Northwind.Employees as emp, Northwind.Northwind.Customers as cust
 where emp.City = cust.City and emp.Country = cust.Country 

--5.	Найти всех покупателей, которые живут в одном городе.

select CustomerID, ContactName, City, Country
from  Northwind.Northwind.Customers as cust
where exists (select City from Northwind.Northwind.Customers 
		      where cust.City = City and cust.Country = Country 
			  group by City, Country 
			  having count(CustomerID) > 1)

--6.	По таблице Employees найти для каждого продавца его руководителя.

-- Условие не понятно

select empe.EmployeeID, empe.FirstName + ' ' + empe.LastName as 'Employee name',
emper.EmployeeID as 'EmployerID', emper.FirstName + emper.LastName as 'Employer name'
from Northwind.Northwind.Employees as empe 
left join Northwind.Northwind.Employees as emper on  empe.ReportsTo = emper.EmployeeID