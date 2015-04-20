--Примеры с простыми запросами:

--1. Выбрать всю информацию обо всех а/м.
select * from [Car]
--2. Выбрать легковые а/м.
select * from [Car] c where c.Type in (N'Седан', N'Унивесал', N'Хетчбэк')
--3. Выбрать номера а/м, в которых число мест >= 5 и упорядочить их по количеству мест.
select c.Id from [Car] c where c.Seats >=5 order by c.Seats
--4. Выбрать номера и фамилии водителей всех белых автомобилей  BMV, у которых в номере встречается цифра 2. Список упорядочить по фамилии водителя.
select c.Driver, c.Number from [Car] c where c.Colour=N'Белый' and c.Brand='BMV' and charindex('2', c.Number) != 0 order by c.Driver
--5. Выдать номера автомобилей, время заказа и фамилии водителей, принимавших заказы с 12:00 до 19:00 1 сентября 2005 года.
select c.Driver, c.Number, o.Datetime from [Car] c, [Order] o where o.Datetime between '2005/09/01 12:00' and '2005/09/01 19:00' and o.CarId = c.Id

--Примеры со сложными запросами:

--1. Посчитать общую сумму оплаты по всем выполненным заказам
select sum(o.Cost) from [Order] o where o.Status=N'Исполнен'
--2. Получить список водителей, отсортированный по количеству выполненных заказов
select c.Driver from [Car] c, [Order] o where c.Id=o.CarId and o.Status=N'Исполнен' group by c.Driver order by count(*) desc
--3. Выбрать постоянных клиентов (не менее 5 выполненных заказов)
select c.Id from [Customer] c, [Order] o where c.Id=o.CustomerId group by c.Id having count(*)>5

--Примеры на редактирование:
--1. Удалить клиента Сидорова.
delete c from [Customer] c where c.Name=N'Сидоров'
--2. Удалить клиента Сидорова и все его заказы.
delete c from [Customer] c where c.Name=N'Сидоров' --ON DELETE CASCADE в конструкторе dbo.[Order] сам все удалит
--3. Заменить номер автомобиля ‘C404HM78’ на ‘C405HM78’.
update [Car] set Number=N'C405HM78' where Number=N'C404HM78'