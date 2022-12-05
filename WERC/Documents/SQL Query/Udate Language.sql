SELECT * 
FROM OPENROWSET('Microsoft.Jet.OLEDB.4.0',
    'Excel 8.0;HDR=YES;Database=D:\MyProject\Web\WERC\Documents\Dictionary\dictionary.xls;',
    'select * from [Dictionary$]');
 
EXEC sp_configure 'show advanced options', 1
RECONFIGURE
GO
EXEC sp_configure 'ad hoc distributed queries', 1
RECONFIGURE
GO
DROP TABLE [dbo].[ExcelTable]
delete [dbo].[Dictionary]
delete [dbo].[RefrenceWord]
insert [dbo].[RefrenceWord] (Id, Word) select  Id, en-US from [dbo].[ExcelTable]
insert [dbo].[Dictionary]  (CultureInfoCode, RefrenceWordId, Value) select  'en-US', Id, en-US  from [dbo].[ExcelTable]
insert [dbo].[Dictionary]  (CultureInfoCode, RefrenceWordId, Value) select  'da-DK', Id, da-DK from [dbo].[ExcelTable]

