/*
1.	Выбрать из таблицы Customers всех заказчиков, проживающих в USA и Canada. 
Запрос сделать с только помощью оператора IN. 
Возвращать колонки с именем пользователя и названием страны в результатах запроса.
 Упорядочить результаты запроса по имени заказчиков и по месту проживания.
*/

select ContactName, Country from Northwind.Northwind.Customers where Country in ('USA','Canada') order by ContactName, Country
/*
2.	Выбрать из таблицы Customers всех заказчиков, не проживающих в USA и Canada.
 Запрос сделать с помощью оператора IN. Возвращать колонки с именем пользователя и названием страны в результатах запроса.
  Упорядочить результаты запроса по имени заказчиков.
*/

select ContactName, Country from Northwind.Northwind.Customers where Country not in ('USA','Canada') order by ContactName
/*
3.	Выбрать из таблицы Customers все страны, в которых проживают заказчики.
 Страна должна быть упомянута только один раз и список отсортирован по убыванию.
  Не использовать предложение GROUP BY. Возвращать только одну колонку в результатах запроса.
*/

select distinct Country from Northwind.Northwind.Customers order by Country desc 
