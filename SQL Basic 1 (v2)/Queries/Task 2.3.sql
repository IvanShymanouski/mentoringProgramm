use Northwind

-- Task 2.3 1
declare @region varchar(7) = 'Western';
select e.LastName, e.FirstName, @region as 'region'
from Employees as e
join EmployeeTerritories as et
  on et.EmployeeID = e.EmployeeID
join Territories as t
  on t.TerritoryID = et.TerritoryID
join Region as r
  on r.RegionID = t.RegionID
where r.RegionDescription = @region
group by  e.LastName, e.FirstName
go

-- Task 2.3 2
SELECT c.ContactName, count(o.OrderID) as 'Count'
from Customers as c
left join Orders as o
  on o.CustomerID = c.CustomerID
group by c.CustomerID, c.ContactName
order by [Count]