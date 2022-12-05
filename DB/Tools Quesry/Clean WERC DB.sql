use werc

delete [dbo].[AspNetUsers] where UserName <> 'Admin'

delete [dbo].[TeamGradeDetail]
delete TeamSubmissionRule
delete TaskSubmissionRule
delete [dbo].[team] 
delete [dbo].[InvoiceDetail]
delete [dbo].[Invoice]

