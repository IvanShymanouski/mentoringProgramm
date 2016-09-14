use Northwind

-- Task 2.2 1
select datepart(yyyy, ShippedDate) as 'Year'
	,count(OrderID) as 'Total'
from Orders
group by datepart(yyyy, ShippedDate)
go

select count(OrderID) as 'Total'
from Orders
go

-- Task 2.2 2
select e.LastName + ' ' + e.FirstName as 'Seller' --Эта строка LastName & FirstName должна быть получена отдельным запросом в колонке основного запроса
                                                  --не совсем понял, но вроде по другому не сделаешь
	,count(o.OrderID) as 'Amount'
from Orders as o
join Employees as e
on e.EmployeeID = o.EmployeeID
group by o.EmployeeID, e.LastName, e.FirstName
order by Amount desc
go

-- Task 2.2 3
select e.LastName + ' ' + e.FirstName as 'Seller'
	,CustomerID
	,count(o.OrderID) as 'Amount'
from Orders as o
join Employees as e
on e.EmployeeID = o.EmployeeID
where datepart(yyyy, o.ShippedDate) = '1998'
group by o.EmployeeID, e.LastName, e.FirstName, o.CustomerID
go

-- Task 2.2 4
select e.LastName + ' ' + e.FirstName as 'Seller'
	,CustomerID
	,c.City
from Customers as c,
     Employees as e
where c.City = e.City
go

-- Task 2.2 5
	select c1.ContactName, c2.ContactName, c1.City --double records, but I don't know how to avoid it through SQL
from Customers as c1,
     Customers as c2
where c1.City = c2.City and 
      not c1.CustomerID = c2.CustomerID
go

-- Task 2.2 6
select e1.EmployeeID,
       e1.FirstName + ' ' + e1.LastName as 'Employee',
       case 
       when e2.FirstName + ' ' + e2.LastName is null 
         then 'don''t have boss'
       else e2.FirstName + ' ' + e2.LastName
	   end as 'Report to'
from Employees as e1
left join Employees as e2
on e2.EmployeeID = e1.ReportsTo
go
