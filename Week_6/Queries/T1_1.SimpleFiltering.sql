
/*
1.	������� � ������� Orders ������, ������� ���� ���������� ����� 6 ��� 1998 ���� (������� ShippedDate) 
������������ � ������� ���������� � ShipVia >= 2. ������ ������ ���������� ������ ������� OrderID, ShippedDate � ShipVia. 
*/
select OrderID, ShippedDate, ShipVia 
from Northwind.Northwind.Orders 
where ShippedDate > '1998-05-06' and ShipVia >= 2

/*
2.	�������� ������, ������� ������� ������ �������������� ������ �� ������� Orders. 
� ����������� ������� ���������� ��� ������� ShippedDate ������ �������� NULL ������ �Not Shipped� (������������ ��������� ������� CAS�).
 ������ ������ ���������� ������ ������� OrderID � ShippedDate.
*/

select OrderID, ShippedDate = 'Not shipped'
from Northwind.Northwind.Orders
where ShippedDate is null

/*
3.	������� � ������� Orders ������, ������� ���� ���������� ����� 6 ��� 1998 ���� (ShippedDate) 
�� ������� ��� ���� ��� ������� ��� �� ����������. � ������� ������ ������������ ������ ������� OrderID
 (������������� � Order Number) � ShippedDate (������������� � Shipped Date). 
 � ����������� ������� ���������� ��� ������� ShippedDate ������ �������� NULL ������ �Not Shipped�,
  ��� ��������� �������� ���������� ���� � ������� �� ���������.
*/

-- select OrderID as 'Order Number', ShippedDate
-- from Northwind.Northwind.Orders 
-- where ShippedDate > '1998-06-06'
-- union
-- select OrderID as 'Order Number', ShippedDate = 'Not Shipped'
-- from Northwind.Northwind.Orders 
-- where ShippedDate is null



