use GarageDB
go

create procedure GetOrder
@ID_Order int,
@VIN VARCHAR(50)
AS
BEGIN
 SELECT o.IdOrder, o.DateBegin, o.DateEnd, c.fName, c.sName, o.Descript,
 h.idVin FROM Orders o  
 LEFT JOIN Clientes c ON o.IdClient = c.Id
 LEFT JOIN Employess e on o.VinNumber = e.Id   
 LEFT JOIN History h on o.id = h.idORDER  where  o.IdOrder = @ID_Order and o.VinNumber = @VIN

END


--select * from History