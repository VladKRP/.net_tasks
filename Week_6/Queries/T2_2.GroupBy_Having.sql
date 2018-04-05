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

--????
select EmployeeID, count(EmployeeID) as 'Amount' 
from Northwind.Northwind.Orders 
group by EmployeeID 
order by 'Amount' desc

select EmployeeID, FirstName + LastName as 'Seller' from Northwind.Northwind.Employees

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
 select * from Northwind.Northwind.Employees where City in (select City from Northwind.Northwind.Customers)


--5.	����� ���� �����������, ������� ����� � ����� ������.

--6.	�� ������� Employees ����� ��� ������� �������� ��� ������������.

