CREATE TABLE [dbo].[Customer]
(
	[Id] INT NOT NULL IDENTITY (1,1) PRIMARY KEY, 
    [Name] NCHAR(50) NULL, 
    [Phone] NCHAR(14) NULL, 
    [Address] NCHAR(100) NULL, 
    [Status] NCHAR(50) NULL
)
