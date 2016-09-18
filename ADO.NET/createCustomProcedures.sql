USE [Northwind]
GO
if exists (select  * from sys.objects
		   where object_id = OBJECT_ID(N'SetInProgress')
                 AND type IN ( N'P', N'PC' ) )
begin
  drop procedure [dbo].[SetInProgress] 
end

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SetInProgress] 
@OrderID int,
@id int output
AS
BEGIN
	select @id=OrderID 
	from Orders
	where ShippedDate is null and 
		  OrderDate is null and 
		  OrderID = @OrderID

	update Orders set OrderDate = GETDATE()
	where OrderID = @id

	return
END
GO

if exists (select  * from sys.objects
		   where object_id = OBJECT_ID(N'SetDone')
                 AND type IN ( N'P', N'PC' ) )
begin
  drop procedure [dbo].[SetDone] 
end

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SetDone] 
@OrderID int,
@id int output
AS
BEGIN
	select @id=OrderID 
	from Orders
	where ShippedDate is null and 
		  OrderDate is not null and 
		  OrderID = @OrderID

	update Orders set ShippedDate = GETDATE()
	where OrderID = @id

	return
END
GO