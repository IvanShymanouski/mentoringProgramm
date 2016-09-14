use Northwind

-- Task 2.1 1
select sum((1 - Discount)* UnitPrice * Quantity) as Totals
from [Order Details]
go

-- Task 2.1 2
select count(case when ShippedDate IS NULL then 1 end) as [Shipped count]
from Orders
go

-- Task 2.1 3
select count(distinct CustomerID)
from Orders

/*--first version in the mind
select count(CustomerID)
from (select distinct CustomerID 
	  from Orders) as t
*/