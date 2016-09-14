use Northwind

-- Task 2.4 1
select CompanyName
from Suppliers
where SupplierID in
	(
		select SupplierID
		from Products
		where UnitsInStock = 0
	)
go

-- Task 2.4 2
select *
from Employees
where EmployeeID in
	(
		select EmployeeID
		FROM Orders
		group by EmployeeID
		having COUNT(OrderID) > 150
	)
go

-- Task 2.4 3
select *
from Customers as c
where exists
	(
		select CustomerID
		from Orders as o
		where c.CustomerID = o.CustomerID
		group by o.CustomerID
		having COUNT(OrderID) = 0
	)