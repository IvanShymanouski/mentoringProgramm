use Northwind

if not exists (select * from sys.objects 
	where object_id = object_id(N'dbo.CreditCard') and type in (N'U'))
begin
create table dbo.CreditCard(
	Id int identity(1,1) not null,
	CardNumber int not null,
	ExpirationDate date not null,
	CardHolder nvarchar (50) not null,
	EmployeeId int null,
	constraint PK_CreditCard unique (CardNumber ASC),
	constraint FK_CreditCard_Employee foreign key(EmployeeId) references dbo.Employees (EmployeeID)
)
end