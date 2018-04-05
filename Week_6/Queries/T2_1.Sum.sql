--1.	����� ����� ����� ���� ������� �� ������� Order Details � ������ ���������� ����������� �������� ������ �� ���. 
-- ����������� ������� ������ ���� ���� ������ � ����� �������� � ��������� ������� 'Totals'.

select sum(UnitPrice * Quantity - (UnitPrice * Quantity * Discount)) 'Totals'
from Northwind.Northwind.[Order Details]
--2.	�� ������� Orders ����� ���������� �������, ������� ��� �� ���� ���������� 
--(�.�. � ������� ShippedDate ��� �������� ���� ��������). ������������ ��� ���� ������� ������ �������� COUNT.
-- �� ������������ ����������� WHERE � GROUP.

select count(OrderID) as 'Undelivered orders' from Northwind.Northwind.Orders where ShippedDate is null


--3.	�� ������� Orders ����� ���������� ��������� ����������� (CustomerID), ��������� ������.
-- ������������ ������� COUNT � �� ������������ ����������� WHERE � GROUP.
select count(distinct CustomerID) as 'Unique Customers' from Northwind.Northwind.Customers