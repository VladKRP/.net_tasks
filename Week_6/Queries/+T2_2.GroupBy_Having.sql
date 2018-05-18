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
where YEAR(OrderDate) = 1998
group by EmployeeID,CustomerID

--4.	����� ����������� � ���������, ������� ����� � ����� ������.
 --���� � ������ ����� ������ ���� ��� ��������� ���������, ��� ������ ���� ��� ��������� �����������,
 -- �� ���������� � ����� ���������� � ��������� �� ������ �������� � �������������� �����.
 -- �� ������������ ����������� JOIN. 

-- ������� Join, �� ���� ����������� �� ������������

 select emp.EmployeeID, cust.CustomerID, emp.City, emp.Country
 from Northwind.Northwind.Employees as emp, Northwind.Northwind.Customers as cust
 where emp.City = cust.City and emp.Country = cust.Country 

--5.	����� ���� �����������, ������� ����� � ����� ������.

select CustomerID, ContactName, City, Country
from  Northwind.Northwind.Customers as cust
where exists (select City from Northwind.Northwind.Customers 
		      where cust.City = City and cust.Country = Country 
			  group by City, Country 
			  having count(CustomerID) > 1)

--6.	�� ������� Employees ����� ��� ������� �������� ��� ������������.

-- ������� �� �������

select empe.EmployeeID, empe.FirstName + ' ' + empe.LastName as 'Employee name',
emper.EmployeeID as 'EmployerID', emper.FirstName + emper.LastName as 'Employer name'
from Northwind.Northwind.Employees as empe 
left join Northwind.Northwind.Employees as emper on  empe.ReportsTo = emper.EmployeeID