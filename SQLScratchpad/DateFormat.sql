
SELECT *
FROM [Northwind].[dbo].[Orders]
WHERE CONVERT(VARCHAR(10), OrderDate, 104) = '19.07.1996'
