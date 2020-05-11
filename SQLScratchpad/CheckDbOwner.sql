
--<< Check Db Owner >>--
SELECT d.name, d.owner_sid, sl.name
FROM sys.databases AS d
JOIN sys.sql_logins AS sl
ON d.owner_sid = sl.sid;


--<< Change Db Owner >>--
USE [YourDatabaseName] EXEC sp_changedbowner 'sa'

