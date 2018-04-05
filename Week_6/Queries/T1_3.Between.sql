--1.	������� ��� ������ (OrderID) �� ������� Order Details (������ �� ������ �����������), 
--��� ����������� �������� � ����������� �� 3 �� 10 ������������ � ��� ������� Quantity � ������� Order Details.
-- ������������ �������� BETWEEN. ������ ������ ���������� ������ ������� OrderID.

select distinct OrderID from Northwind.Northwind.[Order Details] where Quantity between 3 and 10

--2.	������� ���� ���������� �� ������� Customers, � ������� �������� ������ ���������� �� ����� �� ��������� b � g.
-- ������������ �������� BETWEEN. ���������, ��� � ���������� ������� �������� Germany. 
-- ������ ������ ���������� ������ ������� CustomerID � Country � ������������ �� Country.
select CustomerID, Country 
from Northwind.Northwind.Customers 
where Country between 'b' and 'i' 
order by Country

--3.	������� ���� ���������� �� ������� Customers, 
--� ������� �������� ������ ���������� �� ����� �� ��������� b � g, �� ��������� �������� BETWEEN. 

select CustomerID, Country from Northwind.Northwind.Customers
where Country like '^[b-g]'