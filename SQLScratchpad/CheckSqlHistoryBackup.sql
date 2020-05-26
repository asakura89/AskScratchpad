
USE msdb

SELECT
database_name [Database],
COUNT(backup_set_id) Orphans
FROM backupset
WHERE database_name NOT IN ('master', 'tempdb', 'model', 'msdb') --(SELECT name FROM master.dbo.sysdatabases)
GROUP BY database_name

