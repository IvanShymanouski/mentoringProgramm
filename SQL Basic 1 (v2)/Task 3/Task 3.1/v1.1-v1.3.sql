use Northwind

if exists (select * from sys.objects 
	where object_id = object_id(N'dbo.Region') and type in (N'U'))
begin
execute sp_rename N'Region', N'Regions', 'OBJECT'
end

if not exists (select * from sys.columns 
	where Name = 'FoundationDate' and object_id = object_id(N'dbo.Customers'))
begin
alter table dbo.Customers 
	add FoundationDate date NULL
end