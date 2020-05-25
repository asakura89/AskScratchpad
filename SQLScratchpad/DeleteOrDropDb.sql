USE [master]

SET NOCOUNT ON

DECLARE
    @name VARCHAR(200),
    @counter INT = 1,
    @sql NVARCHAR(MAX)

DECLARE db_cursor CURSOR READ_ONLY FOR
SELECT [name]
FROM master.dbo.sysdatabases
WHERE [name] NOT IN ('master','model','msdb','tempdb')

OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @name

WHILE @@FETCH_STATUS = 0
BEGIN
    PRINT 'Deleting ' + CAST(@counter AS VARCHAR) + '. ''' + @name + ''' ...'

    EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = @name

    SET @sql = N'ALTER DATABASE [' + @name + '] SET SINGLE_USER WITH ROLLBACK IMMEDIATE'
    EXEC sys.sp_executesql @sql

    SET @sql = N'DROP DATABASE [' + @name + ']'
    EXEC sys.sp_executesql @sql

    PRINT 'Done.'
    SET @counter = @counter +1

    FETCH NEXT FROM db_cursor INTO @name
END

CLOSE db_cursor
DEALLOCATE db_cursor

SET NOCOUNT OFF
