use Northwind

-- Task 1.1 1
select OrderID
      ,ShippedDate
      ,ShipVia
from Orders
where ShippedDate >= '1998-05-06' 
and ShipVia >= 2
go

-- Task 1.1 2
select OrderID
      ,case 
       when ShippedDate IS NULL 
         then 'Not Shipped'
       end as ShippedDate
from Orders
where ShippedDate is null
go

-- Task 1.1 3
select OrderID
      ,case 
       when ShippedDate IS NULL 
         then 'Not Shipped'
       else convert(varchar(19), ShippedDate)
       end as ShippedDate
from Orders
where ShippedDate > '1998-05-06' 
or ShippedDate is null