  
  use WERC
declare @procName varchar(500)
declare cur cursor
    for select [name] from sys.objects where type = 'v'
open cur

fetch next from cur into @procName
      while @@fetch_status = 0
      begin
                  exec('drop view ' + @procName)
                  fetch next from cur into @procName
      end
close cur
deallocate cur

DECLARE @Sql NVARCHAR(500) DECLARE @Cursor CURSOR
SET @Cursor = CURSOR FAST_FORWARD FOR
SELECT DISTINCT sql = 'ALTER TABLE [' + tc2.TABLE_NAME + '] DROP [' + rc1.CONSTRAINT_NAME + ']'
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc1
LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2 ON tc2.CONSTRAINT_NAME =rc1.CONSTRAINT_NAME
OPEN @Cursor FETCH NEXT FROM @Cursor INTO @Sql
WHILE (@@FETCH_STATUS = 0)
BEGIN
Exec SP_EXECUTESQL @Sql
FETCH NEXT FROM @Cursor INTO @Sql
END
CLOSE @Cursor DEALLOCATE @Cursor
GO
EXEC sp_MSForEachTable 'DROP TABLE ?'
GO