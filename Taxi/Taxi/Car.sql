CREATE TABLE [dbo].[Car]
(
	[Id] INT NOT NULL IDENTITY (1,1) PRIMARY KEY, 
    [Type] NCHAR(50) NULL, 
    [Brand] NCHAR(50) NULL, 
    [Number] NCHAR(10) NULL, 
    [Driver] NCHAR(50) NULL, 
    [Colour] NCHAR(10) NULL, 
    [Seats] SMALLINT NULL
)
