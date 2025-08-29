-- Insertar Owners
DECLARE @Owner1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Owner2 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Owners (Id, Name, Address, Photo, Birthday)
VALUES
(@Owner1, 'Carlos López', 'Cra 12 #8-20, Armenia', '', '1985-04-15'),
(@Owner2, 'Ana Torres', 'Av. Bolívar #123, Bogotá', '', '1990-07-20');

-- Insertar Properties
DECLARE @Property1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Property2 UNIQUEIDENTIFIER = NEWID();

INSERT INTO Properties (Id, Name, Address, Price, CodeInternational, Year, CreatedAt, UpdatedAt, OwnerId)
VALUES
(@Property1, 'Apartamento en el centro', 'Calle 10 #15-30, Armenia', 95000, 'INT-APT-001', 2018, SYSDATETIME(), NULL, @Owner1),
(@Property2, 'Casa Campestre', 'Km 5 vía Pereira, Armenia', 220000, 'INT-HSE-002', 2021, SYSDATETIME(), NULL, @Owner2);

-- Insertar PropertyImages
DECLARE @Image1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Image2 UNIQUEIDENTIFIER = NEWID();

INSERT INTO PropertyImages (Id, [File], Enabled, PropertyId)
VALUES
(@Image1, '', 1, @Property1),
(@Image2, '', 1, @Property2);

-- Insertar PropertyTraces (historial de ventas)
DECLARE @Trace1 UNIQUEIDENTIFIER = NEWID();
DECLARE @Trace2 UNIQUEIDENTIFIER = NEWID();

INSERT INTO PropertyTraces (Id, DateSale, Name, Value, Tax, PropertyId)
VALUES
(@Trace1, '2020-06-10', 'Venta inicial Apto Centro', 95000, 5000, @Property1),
(@Trace2, '2022-01-20', 'Venta inicial Casa Campestre', 220000, 12000, @Property2);
GO