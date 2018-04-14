/*
1.	������� �� ������� Customers ���� ����������, ����������� � USA � Canada. 
������ ������� � ������ ������� ��������� IN. 
���������� ������� � ������ ������������ � ��������� ������ � ����������� �������.
 ����������� ���������� ������� �� ����� ���������� � �� ����� ����������.
*/

select ContactName, Country from Northwind.Northwind.Customers where Country in ('USA','Canada') order by ContactName, Country
/*
2.	������� �� ������� Customers ���� ����������, �� ����������� � USA � Canada.
 ������ ������� � ������� ��������� IN. ���������� ������� � ������ ������������ � ��������� ������ � ����������� �������.
  ����������� ���������� ������� �� ����� ����������.
*/

select ContactName, Country from Northwind.Northwind.Customers where Country not in ('USA','Canada') order by ContactName
/*
3.	������� �� ������� Customers ��� ������, � ������� ��������� ���������.
 ������ ������ ���� ��������� ������ ���� ��� � ������ ������������ �� ��������.
  �� ������������ ����������� GROUP BY. ���������� ������ ���� ������� � ����������� �������.
*/

select distinct Country from Northwind.Northwind.Customers order by Country desc 

--group by variant
select Country from Northwind.Northwind.Customers group by Country order by Country desc 