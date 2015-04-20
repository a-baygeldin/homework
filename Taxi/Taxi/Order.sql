CREATE TABLE [dbo].[Order]
(
	[Id] INT NOT NULL IDENTITY (1,1) PRIMARY KEY, 
    [Datetime] DATETIME2 NULL, 
    [Address] NCHAR(100) NULL, 
    [Cost] INT NULL, 
    [Payment] NCHAR(50) NULL, 
    [CarId] INT NOT NULL, 
    [CustomerId] INT NOT NULL, 
    [Status] NCHAR(50) NULL, 
    CONSTRAINT [FK_Order_Car] FOREIGN KEY ([CarId]) REFERENCES [Car]([Id]), 
    CONSTRAINT [FK_Order_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [Customer]([Id]) ON DELETE CASCADE
)
