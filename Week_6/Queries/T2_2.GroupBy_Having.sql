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

--????
select EmployeeID, count(EmployeeID) as 'Amount' 
from Northwind.Northwind.Orders 
group by EmployeeID 
order by 'Amount' desc

select EmployeeID, FirstName + LastName as 'Seller' from Northwind.Northwind.Employees

--3.	По таблице Orders найти количество заказов, сделанных каждым продавцом и для каждого покупателя.
 --Необходимо определить это только для заказов, сделанных в 1998 году. 

select EmployeeID as 'Seller', CustomerID as 'Customer', count(EmployeeID) as 'Orders Amount'
from Northwind.Northwind.Orders 
where OrderDate between '1998-01-01' and '1998-12-31' 
group by EmployeeID,CustomerID

--4.	Найти покупателей и продавцов, которые живут в одном городе.
 --Если в городе живут только один или несколько продавцов, или только один или несколько покупателей,
 -- то информация о таких покупателя и продавцах не должна попадать в результирующий набор.
 -- Не использовать конструкцию JOIN. 

 --????
 select * from Northwind.Northwind.Employees where City in (select City from Northwind.Northwind.Customers)


--5.	Найти всех покупателей, которые живут в одном городе.

--6.	По таблице Employees найти для каждого продавца его руководителя.

