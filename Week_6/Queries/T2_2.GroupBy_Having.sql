--1.	�� ������� Orders ����� ���������� ������� � ������������ �� �����.
 --� ����������� ������� ���� ���������� ��� ������� c ���������� Year � Total.
 -- �������� ����������� ������, ������� ��������� ���������� ���� �������.

select count(OrderID) as 'Total Orders Amount' from Northwind.Northwind.Orders 

select YEAR(OrderDate) as 'Year', COUNT(OrderID) as 'Total' 
from Northwind.Northwind.Orders 
group by YEAR(OrderDate)


--2.	�� ������� Orders ����� ���������� �������, c�������� ������ ���������.
 --����� ��� ���������� �������� � ��� ����� ������ � ������� Orders, ��� � ������� EmployeeID ������ �������� ��� ������� ��������.
 -- � ����������� ������� ���� ���������� ������� � ������ �������� (������ ������������� ��� ���������� ������������� LastName & FirstName.
 --  ��� ������ LastName & FirstName ������ ���� �������� ��������� �������� � ������� ��������� �������.
 --   ����� �������� ������ ������ ������������ ����������� �� EmployeeID.)
	-- � ��������� ������� �Seller� � ������� c ����������� ������� ���������� � ��������� 'Amount'.
	--  ���������� ������� ������ ���� ����������� �� �������� ���������� �������. 

select emp.FirstName + emp.LastName as 'Seller', oa.Amount 
from Northwind.Northwind.Employees as emp 
inner join (select ord.EmployeeID, count(ord.EmployeeID) as 'Amount' 
	  from Northwind.Northwind.Orders as ord 
	  group by ord.EmployeeID) as oa on emp.EmployeeID = oa.EmployeeID 
order by oa.Amount desc



--3.	�� ������� Orders ����� ���������� �������, ��������� ������ ��������� � ��� ������� ����������.
 --���������� ���������� ��� ������ ��� �������, ��������� � 1998 ����. 

select EmployeeID as 'Seller', CustomerID as 'Customer', count(EmployeeID) as 'Orders Amount'
from Northwind.Northwind.Orders 
where OrderDate between '1998-01-01' and '1998-12-31' 
group by EmployeeID,CustomerID

--4.	����� ����������� � ���������, ������� ����� � ����� ������.
 --���� � ������ ����� ������ ���� ��� ��������� ���������, ��� ������ ���� ��� ��������� �����������,
 -- �� ���������� � ����� ���������� � ��������� �� ������ �������� � �������������� �����.
 -- �� ������������ ����������� JOIN. 

 --????
 select emp.FirstName + emp.LastName as 'Seller', cust.CustomerID as 'Customer', cust.City
 from Northwind.Northwind.Employees as emp, Northwind.Northwind.Customers as cust 
 where emp.City = cust.City


--5.	����� ���� �����������, ������� ����� � ����� ������.

--6.	�� ������� Employees ����� ��� ������� �������� ��� ������������.
select * from Northwind.Northwind.Employees
