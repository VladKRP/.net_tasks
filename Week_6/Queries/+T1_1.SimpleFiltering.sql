
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

-- with case
select OrderID,
case
  when ShippedDate is null then 'Not Shipped'
end ShippedDate
from Northwind.Northwind.Orders
where ShippedDate is null

-- without case
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

-- with case
select OrderID as 'Order Number',
case 
  when ShippedDate is null then 'Not Shipped'
  else convert(varchar, ShippedDate, 101)-- ����������� � ��������� �������������(������ ����), ���������� ���������� �.�. ������� � datetime �� ����� ������� ������
end 'Shipped Date'
from Northwind.Northwind.Orders
where ShippedDate > '1998-06-06' or ShippedDate is null

-- without case
select res.OrderID as 'Order Number', res.[Shipped Date] 
from 
(select OrderID, convert(varchar, ShippedDate, 101) as 'Shipped Date' 
  from Northwind.Northwind.Orders 
  where ShippedDate > '1998-06-06'
  union
  select OrderID, ShippedDate = 'Not Shipped'
  from Northwind.Northwind.Orders 
  where ShippedDate is null
) as res




