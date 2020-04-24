SET NOCOUNT ON

DECLARE @name VARCHAR(50) -- database name. /** @var {out} */ don't fill this up. it'll automatically filled.
DECLARE @fileName VARCHAR(256) -- filename for backup. /** @var {out} */ don't fill this up. it'll automatically filled.
DECLARE @fileDate VARCHAR(20) = CONVERT(VARCHAR(20),GETDATE(),112) -- datetime format used for file name.
DECLARE @path VARCHAR(256) = (SELECT 'D:\Database\Backup\' + @fileDate + '\') -- path (directory) for backup files.

DECLARE db_cursor CURSOR READ_ONLY FOR
SELECT [name]
FROM master.dbo.sysdatabases
WHERE [name] NOT IN ('master','model','msdb','tempdb')  -- exclude these databases → predicate

OPEN db_cursor
FETCH NEXT FROM db_cursor INTO @name

WHILE @@FETCH_STATUS = 0
BEGIN
    -- SET @fileName = @path + @name + '_' + @fileDate + '.bak'
    SET @fileName = @path + @name + '.bak'
    PRINT '@name: ' + @name + ' → @fileName: ' + @fileName
    BACKUP DATABASE @name TO DISK = @fileName
    PRINT 'Done.'

    FETCH NEXT FROM db_cursor INTO @name
END

CLOSE db_cursor
DEALLOCATE db_cursor

SET NOCOUNT OFF