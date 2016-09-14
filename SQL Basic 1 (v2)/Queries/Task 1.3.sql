use Northwind

-- Task 1.3 1
select distinct OrderID, Quantity
from [Order Details]
where Quantity between 3 and 10
go

-- Task 1.3 2
select CustomerID, 
	   Country
from Customers
where left(Country, 1) between 'b' and 'g' 
-- between works in [,) range if it is used with whole word
order by Country
go

-- Task 1.3 3
select CustomerID, 
	   Country
from Customers
where Country like '[b-g]%'
/*--one more option
 where left(Country, 1) >= 'b'
 and left(Country, 1) <= 'g'
*/
order by Country