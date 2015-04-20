insert into dbo.[Car] ([Type], [Brand], [Driver], [Colour], [Seats], [Number]) values 
(N'Седан', N'Опель', N'Вася', N'Розовый', 4, '123456'),
(N'Седан', N'Лада', N'Магомед', N'Черный', 4, '654321')

GO

insert into dbo.[Customer] ([Address], [Name], [Phone], [Status]) values
(N'Где-то в Купчино', N'Петя', N'+79810101010', N'Обычный')

GO

insert into dbo.[Order] ([CarId], [CustomerId], [Address], [Cost], [Datetime], [Payment], [Status]) values
(1, 1, N'Петергоф, Ботаническая 70, корп. 2', 800, '23 Mar 09:25', 'SMS', N'Исполнен')
