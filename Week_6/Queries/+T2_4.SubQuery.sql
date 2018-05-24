/*
1.	������ ���� ����������� (������� CompanyName � ������� Suppliers), � ������� ��� ���� �� ������ �������� 
�� ������ (UnitsInStock � ������� Products ����� 0). ������������ ��������� SELECT ��� ����� ������� � �������������� ��������� IN. 
*/

select CompanyName from Northwind.Northwind.Suppliers 
where SupplierId in (select SupplierID from Northwind.Northwind.Products where UnitsInStock = 0)

/*
2.	������ ���� ���������, ������� ����� ����� 150 �������. ������������ ��������� SELECT.
*/
 
 select * from Northwind.Northwind.Employees 
 where EmployeeID in (select EmployeeID from Northwind.Northwind.Orders 
										group by EmployeeID 
										having count(EmployeeID) > 150)

/*
3.	������ ���� ���������� (������� Customers), ������� �� ����� �� ������ ������ (��������� �� ������� Orders).
 ������������ �������� EXISTS.
*/

select cust.CustomerID from Northwind.Northwind.Customers  as cust
where not exists (select CustomerID 
						 from Northwind.Northwind.Orders 
						 where cust.CustomerID = CustomerID)