USE [WERC]
GO

/****** Object:  Table [dbo].[Task]    Script Date: 12/28/2019 8:44:46 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Task] add
	
	[Preliminary] [bit] NULL,
	[OpenTaskTestPlan] [bit] NULL


	---------------------------------------------------------------------------------------

	
/****** Object:  View [dbo].[ViewPersonInRole]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewPersonInRole]
AS
SELECT       dbo.Person.FirstName + N' ' + dbo.Person.LastName AS Name, dbo.Person.Identifier, dbo.Person.Sex, dbo.Person.BirthDate, dbo.Person.UserId, dbo.Person.Id, dbo.AspNetUsers.UserName, dbo.AspNetUsers.Email, 
                         dbo.AspNetUsers.RegisterDate, dbo.AspNetRoles.Name AS RoleName, dbo.AspNetUserRoles.RoleId, dbo.AspNetUsers.UserDefiner, dbo.AspNetUsers.LastSignIn, dbo.Person.UniversityId, dbo.University.Name AS University, 
                         dbo.Person.StreetLine1, dbo.Person.StreetLine2, dbo.Person.City, dbo.Person.State, dbo.Person.ZipCode, dbo.Person.ShortBio, dbo.Person.ProfilePictureUrl, dbo.Person.ResumeUrl, dbo.Person.FirstName, dbo.Person.LastName, 
                         dbo.AspNetUsers.LockoutEnabled, dbo.AspNetUsers.EmailConfirmed, dbo.Person.SizeId, Size_1.Name AS T_Shirt_Size, dbo.Person.EmgyPersonFirstName, dbo.Person.EmgyPersonLastName, dbo.Person.EmgyPersonPhoneNumber, 
                         dbo.Person.EmgyPersonRelationship, dbo.AspNetUsers.PhoneNumber, dbo.Person.JacketSizeId, dbo.Size.Name AS JacketSize, dbo.Person.DietTypeId, dbo.DietType.Name AS DietType, dbo.University.UniversityPictureUrl, 
                         dbo.Person.WelcomeDinner, dbo.Person.LunchOnMonday, dbo.Person.LunchOnTuesday, dbo.Person.ReceptionNetworkOnTuesday, dbo.Person.AwardBanquet, dbo.Person.NoneOfTheAbove, dbo.Person.Allergies, 
                         dbo.Person.SecondaryEmail, dbo.Person.Agreement, dbo.Person.Abbreviation, dbo.Person.Affiliation, dbo.AspNetUsers.WorkPhoneNumber, dbo.Person.Minor, dbo.Person.Major, dbo.Person.YearClassificationId, 
                         dbo.Person.GoalsAfterGraduationIds, dbo.Person.EthnicityIds, dbo.YearClassification.Name AS YearClassification
FROM            dbo.Person INNER JOIN
                         dbo.AspNetUsers ON dbo.Person.UserId = dbo.AspNetUsers.Id INNER JOIN
                         dbo.AspNetUserRoles ON dbo.AspNetUsers.Id = dbo.AspNetUserRoles.UserId INNER JOIN
                         dbo.AspNetRoles ON dbo.AspNetUserRoles.RoleId = dbo.AspNetRoles.Id LEFT OUTER JOIN
                         dbo.YearClassification ON dbo.Person.YearClassificationId = dbo.YearClassification.Id LEFT OUTER JOIN
                         dbo.DietType ON dbo.Person.DietTypeId = dbo.DietType.Id LEFT OUTER JOIN
                         dbo.Size ON dbo.Person.JacketSizeId = dbo.Size.Id LEFT OUTER JOIN
                         dbo.Size AS Size_1 ON dbo.Person.SizeId = Size_1.Id LEFT OUTER JOIN
                         dbo.University ON dbo.Person.UniversityId = dbo.University.Id
GO

/****** Object:  View [dbo].[ViewTeamMember]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewTeamMember]
AS
SELECT       dbo.Team.Id AS TeamId, dbo.Team.TaskId, dbo.Team.Name AS TeamName, dbo.Task.Name AS Task, dbo.TeamMember.Survey, dbo.ViewPersonInRole.Name AS MemberName, dbo.ViewPersonInRole.Identifier, 
                         dbo.ViewPersonInRole.Sex, dbo.ViewPersonInRole.BirthDate, dbo.ViewPersonInRole.UserName, dbo.ViewPersonInRole.Email, dbo.ViewPersonInRole.RegisterDate, dbo.ViewPersonInRole.RoleName, dbo.ViewPersonInRole.RoleId, 
                         dbo.ViewPersonInRole.UserDefiner, dbo.ViewPersonInRole.LastSignIn, dbo.ViewPersonInRole.UniversityId, dbo.ViewPersonInRole.University, dbo.ViewPersonInRole.StreetLine1, dbo.ViewPersonInRole.StreetLine2, 
                         dbo.ViewPersonInRole.City, dbo.ViewPersonInRole.State, dbo.ViewPersonInRole.ZipCode, dbo.ViewPersonInRole.ShortBio, dbo.ViewPersonInRole.ProfilePictureUrl, dbo.ViewPersonInRole.ResumeUrl, 
                         dbo.TeamMember.MemberUserId, dbo.Team.Date, dbo.TeamMember.Id, dbo.Team.State AS TeamState, dbo.Team.ImageUrl AS TeamImageUrl, dbo.ViewPersonInRole.FirstName, dbo.ViewPersonInRole.LastName, 
                         dbo.ViewPersonInRole.EmailConfirmed, dbo.ViewPersonInRole.LockoutEnabled, dbo.TeamMember.RegistrationStatus, dbo.ViewPersonInRole.PhoneNumber, dbo.ViewPersonInRole.EmgyPersonFirstName, 
                         dbo.ViewPersonInRole.EmgyPersonLastName, dbo.ViewPersonInRole.EmgyPersonPhoneNumber, dbo.ViewPersonInRole.EmgyPersonRelationship, dbo.ViewPersonInRole.T_Shirt_Size, dbo.ViewPersonInRole.SizeId, 
                         dbo.Team.LabResultUrl, dbo.Team.WrittenReportUrl, dbo.Team.PreliminaryReportUrl, dbo.Team.TeamNumber, dbo.ViewPersonInRole.JacketSize, dbo.ViewPersonInRole.JacketSizeId, dbo.ViewPersonInRole.DietTypeId, 
                         dbo.ViewPersonInRole.DietType, dbo.ViewPersonInRole.UniversityPictureUrl, dbo.Team.PayStatus, dbo.ViewPersonInRole.WorkPhoneNumber, dbo.Team.OpenTaskTestPlanUrl, dbo.Task.Preliminary AS PreliminaryStatus, 
                         dbo.Task.OpenTaskTestPlan AS OpenTaskTestPlanStatus
FROM            dbo.Team INNER JOIN
                         dbo.TeamMember ON dbo.Team.Id = dbo.TeamMember.TeamId INNER JOIN
                         dbo.ViewPersonInRole ON dbo.TeamMember.MemberUserId = dbo.ViewPersonInRole.UserId INNER JOIN
                         dbo.Task ON dbo.Team.TaskId = dbo.Task.Id
WHERE        (dbo.Team.Deactivate = 0)
GO

/****** Object:  View [dbo].[ViewUserTask]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewUserTask]
AS
SELECT       dbo.UserTask.Id, dbo.UserTask.UserId, dbo.UserTask.TaskId, dbo.Task.Name AS TaskName, dbo.ViewPersonInRole.Name, dbo.ViewPersonInRole.UserName, dbo.ViewPersonInRole.Email, dbo.ViewPersonInRole.RoleName, 
                         dbo.ViewPersonInRole.RoleId, dbo.ViewPersonInRole.StreetLine1, dbo.ViewPersonInRole.StreetLine2, dbo.ViewPersonInRole.City, dbo.ViewPersonInRole.State, dbo.ViewPersonInRole.ZipCode, dbo.ViewPersonInRole.ShortBio, 
                         dbo.ViewPersonInRole.ProfilePictureUrl, dbo.ViewPersonInRole.FirstName, dbo.ViewPersonInRole.LastName, dbo.ViewPersonInRole.LockoutEnabled, dbo.ViewPersonInRole.EmailConfirmed, dbo.ViewPersonInRole.SizeId, 
                         dbo.ViewPersonInRole.T_Shirt_Size, dbo.ViewPersonInRole.PhoneNumber, dbo.Task.ImageUrl, dbo.ViewPersonInRole.JacketSizeId, dbo.ViewPersonInRole.JacketSize, dbo.ViewPersonInRole.DietTypeId, 
                         dbo.ViewPersonInRole.DietType, dbo.ViewPersonInRole.UniversityPictureUrl, dbo.Task.Description AS TaskDescription, dbo.ViewPersonInRole.WorkPhoneNumber
FROM            dbo.UserTask INNER JOIN
                         dbo.Task ON dbo.UserTask.TaskId = dbo.Task.Id INNER JOIN
                         dbo.ViewPersonInRole ON dbo.UserTask.UserId = dbo.ViewPersonInRole.UserId
GO

/****** Object:  View [dbo].[ViewTeamFullInfo]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewTeamFullInfo]
AS
SELECT       T .Id, T .TaskId, T .Name, dbo.ViewPersonInRole.Name AS Advisor,
                             (SELECT       vt.MemberName
                               FROM             dbo.ViewTeamMember vt
                               WHERE         vt.RoleId = N'291d6069-44a3-4960-86d3-b91bda430e71' AND vt.TeamId = T .Id) AS Leader, dbo.Task.Name AS TaskName, Judges = STUFF
                             ((SELECT       ', ' + vu.FirstName + ' ' + vu.LastName
                                 FROM            dbo.ViewUserTask vu
                                 WHERE        vu.RoleId = N'4d7951d8-8eda-4452-8ff1-dfc9076d8bbe' AND vu.TaskId = T .TaskId FOR XML path('')), 1, 1, ''), CASE WHEN EXISTS
                             (SELECT       tm.Survey
                               FROM             dbo.TeamMember tm
                               WHERE         tm.TeamId = T .Id AND (Survey = 0 OR
                                                         Survey IS NULL)) THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS Survey, CASE WHEN EXISTS
                             (SELECT       tm.RegistrationStatus
                               FROM             dbo.TeamMember tm
                               WHERE         tm.TeamId = T .Id AND (RegistrationStatus = 0 OR
                                                         RegistrationStatus IS NULL)) THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS RegistrationStatus, CASE WHEN (NOT EXISTS
                             (SELECT       Id
                               FROM             dbo.TeamSafetyItem
                               WHERE         TeamId = T .Id) OR
                         EXISTS
                             (SELECT       tsi.ItemStatus
                               FROM             dbo.TeamSafetyItem tsi
                               WHERE         tsi.TeamId = T .Id AND tsi.ItemStatus <> 3)) THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS Approved, CASE WHEN (NOT EXISTS
                             (SELECT       Id
                               FROM             dbo.TeamSafetyItem
                               WHERE         TeamId = T .Id) OR
                         NOT EXISTS
                             (SELECT       tsi.ItemStatus
                               FROM             dbo.TeamSafetyItem tsi
                               WHERE         tsi.TeamId = T .Id AND tsi.ItemStatus = 1)) THEN CAST(0 AS BIT) ELSE CAST(1 AS BIT) END AS Status, T .SubmitStatus, dbo.ViewPersonInRole.Identifier, dbo.ViewPersonInRole.Sex, dbo.ViewPersonInRole.BirthDate, 
                         dbo.ViewPersonInRole.UserName, dbo.ViewPersonInRole.Email, dbo.ViewPersonInRole.RegisterDate, dbo.ViewPersonInRole.RoleName, dbo.ViewPersonInRole.RoleId, dbo.ViewPersonInRole.UserDefiner, 
                         dbo.ViewPersonInRole.LastSignIn, dbo.ViewPersonInRole.UniversityId, dbo.ViewPersonInRole.University, dbo.ViewPersonInRole.StreetLine1, dbo.ViewPersonInRole.StreetLine2, dbo.ViewPersonInRole.City, 
                         dbo.ViewPersonInRole.State, dbo.ViewPersonInRole.ZipCode, dbo.ViewPersonInRole.ShortBio, dbo.ViewPersonInRole.ProfilePictureUrl, dbo.ViewPersonInRole.ResumeUrl, dbo.TeamMember.MemberUserId, T .Date, 
                         T .State AS TeamState, T .ImageUrl AS TeamImageUrl, dbo.ViewPersonInRole.EmailConfirmed, dbo.ViewPersonInRole.LockoutEnabled, dbo.ViewPersonInRole.LastName, dbo.ViewPersonInRole.FirstName, 
                         dbo.ViewPersonInRole.SizeId, dbo.ViewPersonInRole.T_Shirt_Size, dbo.ViewPersonInRole.EmgyPersonFirstName, dbo.ViewPersonInRole.EmgyPersonLastName, dbo.ViewPersonInRole.EmgyPersonPhoneNumber, 
                         dbo.ViewPersonInRole.EmgyPersonRelationship, dbo.ViewPersonInRole.PhoneNumber, dbo.ViewPersonInRole.WorkPhoneNumber, T .PayStatus, T .LabResultUrl, T .WrittenReportUrl, T .PreliminaryReportUrl, 
                         T .PreliminaryReportDate, T .WrittenReportDate, T .OpenTaskTestPlanDate, T .OpenTaskTestPlanUrl, T .TeamNumber, T .Deactivate, T .Payment, dbo.ViewPersonInRole.JacketSizeId, dbo.ViewPersonInRole.JacketSize, 
                         dbo.ViewPersonInRole.DietTypeId, dbo.ViewPersonInRole.DietType, dbo.ViewPersonInRole.UniversityPictureUrl, dbo.Task.Preliminary, dbo.Task.OpenTaskTestPlan
FROM            dbo.Team T INNER JOIN
                         dbo.Task ON T .TaskId = dbo.Task.Id INNER JOIN
                         dbo.TeamMember ON T .Id = dbo.TeamMember.TeamId INNER JOIN
                         dbo.ViewPersonInRole ON dbo.TeamMember.MemberUserId = dbo.ViewPersonInRole.UserId
WHERE        (dbo.ViewPersonInRole.RoleId = N'58c326dd-38ea-4d3c-92f9-3935e3763e68') AND T .Deactivate = 0
GO

/****** Object:  View [dbo].[ViewGradeDetail]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE view [dbo].[ViewGradeDetail]
AS
SELECT        dbo.Grade.Name, dbo.GradeDetail.EvaluationItem, dbo.GradeDetail.Point, dbo.GradeDetail.Coefficient, dbo.GradeDetail.Id, dbo.GradeDetail.GradeId
FROM            dbo.Grade INNER JOIN
                         dbo.GradeDetail ON dbo.Grade.Id = dbo.GradeDetail.GradeId
GO

/****** Object:  View [dbo].[ViewTeamGradeMetaData]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewTeamGradeMetaData]
AS
SELECT       dbo.Team.Id, dbo.Team.TaskId, dbo.ViewGradeDetail.EvaluationItem, dbo.ViewGradeDetail.Point, dbo.ViewGradeDetail.Coefficient, dbo.ViewGradeDetail.Id AS GradeDetailId, dbo.ViewGradeDetail.GradeId, 
                         dbo.Team.Name AS TeamName, dbo.Team.Date, dbo.Team.State, dbo.Team.ImageUrl, dbo.Team.LabResultUrl, dbo.Team.WrittenReportUrl, dbo.Team.PreliminaryReportUrl, dbo.ViewGradeDetail.Name AS Grade, 
                         dbo.Team.TeamNumber, dbo.Task.Description AS TaskDescription, dbo.Team.PayStatus, dbo.UserTask.UserId, dbo.Team.OpenTaskTestPlanUrl
FROM            dbo.Team INNER JOIN
                         dbo.Task ON dbo.Team.TaskId = dbo.Task.Id INNER JOIN
                         dbo.TaskGrade ON dbo.Task.Id = dbo.TaskGrade.TaskId INNER JOIN
                         dbo.ViewGradeDetail ON dbo.TaskGrade.GradeId = dbo.ViewGradeDetail.GradeId INNER JOIN
                         dbo.UserTask ON dbo.Task.Id = dbo.UserTask.TaskId
WHERE        (dbo.Team.Deactivate = 0)
GO

/****** Object:  View [dbo].[ViewTeamMemberCount]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewTeamMemberCount]
AS
SELECT       COUNT(*) AS MemberCount, TeamId
FROM            dbo.TeamMember
GROUP BY TeamId
GO

/****** Object:  View [dbo].[ViewTeamRegistrationCompletedMembersCount]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewTeamRegistrationCompletedMembersCount]
AS
SELECT       COUNT(*) AS RegisteredCount, TeamId
FROM            dbo.TeamMember
WHERE        (RegistrationStatus = 1)
GROUP BY TeamId
GO

/****** Object:  View [dbo].[ViewTeamCompleteRegistered]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewTeamCompleteRegistered]
AS
SELECT       dbo.ViewTeamMemberCount.TeamId
FROM            dbo.ViewTeamMemberCount INNER JOIN
                         dbo.ViewTeamRegistrationCompletedMembersCount ON dbo.ViewTeamMemberCount.MemberCount = dbo.ViewTeamRegistrationCompletedMembersCount.RegisteredCount AND 
                         dbo.ViewTeamMemberCount.TeamId = dbo.ViewTeamRegistrationCompletedMembersCount.TeamId
GO

/****** Object:  View [dbo].[ViewTeam]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewTeam]
AS
SELECT       dbo.Team.Id, dbo.Team.TaskId, dbo.Team.Name, dbo.Task.Name AS Task, dbo.TeamMember.Survey, dbo.ViewPersonInRole.Name AS MemberName, dbo.ViewPersonInRole.Identifier, dbo.ViewPersonInRole.Sex, 
                         dbo.ViewPersonInRole.BirthDate, dbo.ViewPersonInRole.UserName, dbo.ViewPersonInRole.Email, dbo.ViewPersonInRole.RegisterDate, dbo.ViewPersonInRole.RoleName, dbo.ViewPersonInRole.RoleId, 
                         dbo.ViewPersonInRole.UserDefiner, dbo.ViewPersonInRole.LastSignIn, dbo.ViewPersonInRole.UniversityId, dbo.ViewPersonInRole.University, dbo.ViewPersonInRole.StreetLine1, dbo.ViewPersonInRole.StreetLine2, 
                         dbo.ViewPersonInRole.City, dbo.ViewPersonInRole.State, dbo.ViewPersonInRole.ZipCode, dbo.ViewPersonInRole.ShortBio, dbo.ViewPersonInRole.ProfilePictureUrl, dbo.ViewPersonInRole.ResumeUrl, 
                         dbo.TeamMember.MemberUserId, dbo.Team.Date, dbo.Team.State AS TeamState, dbo.Team.ImageUrl AS TeamImageUrl, dbo.ViewPersonInRole.EmailConfirmed, dbo.ViewPersonInRole.LockoutEnabled, 
                         dbo.ViewPersonInRole.LastName, dbo.ViewPersonInRole.FirstName, dbo.TeamMember.RegistrationStatus, dbo.ViewPersonInRole.SizeId, dbo.ViewPersonInRole.T_Shirt_Size, dbo.ViewPersonInRole.EmgyPersonFirstName, 
                         dbo.ViewPersonInRole.EmgyPersonLastName, dbo.ViewPersonInRole.EmgyPersonPhoneNumber, dbo.ViewPersonInRole.EmgyPersonRelationship, dbo.ViewPersonInRole.PhoneNumber, dbo.Team.LabResultUrl, 
                         dbo.Team.WrittenReportUrl, dbo.Team.PreliminaryReportUrl, dbo.Team.TeamNumber, dbo.ViewPersonInRole.JacketSizeId, dbo.ViewPersonInRole.JacketSize, dbo.ViewPersonInRole.DietTypeId, dbo.ViewPersonInRole.DietType, 
                         dbo.ViewPersonInRole.UniversityPictureUrl, dbo.Task.Description AS TaskDescription, dbo.Team.PayStatus, dbo.Team.Deactivate, dbo.Team.SubmitStatus, dbo.Team.WrittenReportDate, dbo.Team.PreliminaryReportDate, 
                         dbo.Team.OpenTaskTestPlanDate, dbo.Team.SuppressScoring, dbo.ViewPersonInRole.WorkPhoneNumber, dbo.Team.OpenTaskTestPlanUrl, dbo.Team.Payment, dbo.Task.Preliminary, dbo.Task.OpenTaskTestPlan
FROM            dbo.Team INNER JOIN
                         dbo.Task ON dbo.Team.TaskId = dbo.Task.Id INNER JOIN
                         dbo.TeamMember ON dbo.Team.Id = dbo.TeamMember.TeamId INNER JOIN
                         dbo.ViewPersonInRole ON dbo.TeamMember.MemberUserId = dbo.ViewPersonInRole.UserId
WHERE        (dbo.ViewPersonInRole.RoleId = N'58c326dd-38ea-4d3c-92f9-3935e3763e68') AND (dbo.Team.Deactivate = 0)
GO

/****** Object:  View [dbo].[ViewInvoice]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewInvoice]
AS
SELECT       dbo.Invoice.Id AS InvoiceId, dbo.Invoice.Title, dbo.Invoice.DateOfIssue, dbo.Invoice.InvoiceNumber, dbo.Invoice.InvoiceTotal, dbo.InvoiceDetail.Id, dbo.InvoiceDetail.PaymentRuleId, dbo.InvoiceDetail.IsFirstTeam, 
                         dbo.InvoiceDetail.TeamUnitCost, dbo.InvoiceDetail.ExtraParticipantCount, dbo.InvoiceDetail.ExtraParticipantUnitCost, dbo.InvoiceDetail.ExtraParticipantAmount, dbo.InvoiceDetail.Amount, dbo.Invoice.Subtotal, dbo.Invoice.Tax, 
                         dbo.Invoice.Total, dbo.Invoice.AmountDue, dbo.PaymentRule.TypeOfRegistration, dbo.PaymentRule.FirstTeamFee, dbo.PaymentRule.DueDate, dbo.ViewTeam.Name AS TeamName, dbo.ViewTeam.RoleName, dbo.ViewTeam.RoleId, 
                         dbo.ViewTeam.UniversityId, dbo.ViewTeam.University, dbo.ViewTeam.StreetLine1, dbo.ViewTeam.StreetLine2, dbo.ViewTeam.City, dbo.ViewTeam.State, dbo.ViewTeam.ZipCode, dbo.ViewTeam.PayStatus, dbo.ViewTeam.FirstName, 
                         dbo.ViewTeam.LastName, dbo.ViewTeam.PhoneNumber, dbo.ViewTeam.WorkPhoneNumber, dbo.ViewTeam.Email, dbo.ViewTeam.Sex, dbo.ViewTeam.MemberUserId AS UserId, dbo.ViewTeam.Id AS TeamId, dbo.Invoice.Finished, 
                         dbo.InvoiceDetail.ExtraTeamDiscount, dbo.Invoice.ConventionalFee, dbo.InvoiceDetail.ConventionalFee AS DetailConventionalFee, dbo.ViewTeam.Task, dbo.ViewTeam.Payment
FROM            dbo.ViewTeamCompleteRegistered INNER JOIN
                         dbo.ViewTeam ON dbo.ViewTeamCompleteRegistered.TeamId = dbo.ViewTeam.Id LEFT OUTER JOIN
                         dbo.Invoice INNER JOIN
                         dbo.PaymentRule INNER JOIN
                         dbo.InvoiceDetail ON dbo.PaymentRule.Id = dbo.InvoiceDetail.PaymentRuleId ON dbo.Invoice.Id = dbo.InvoiceDetail.InvoiceId ON dbo.ViewTeam.Id = dbo.InvoiceDetail.TeamId
GO

/****** Object:  View [dbo].[ViewTaskTeam]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewTaskTeam]
AS
SELECT       dbo.UserTask.Id, dbo.UserTask.UserId, dbo.UserTask.TaskId, dbo.Task.Name AS TaskName, dbo.Task.ImageUrl AS TaskImageUrl, dbo.Team.ImageUrl AS TeamImageUrl, dbo.Team.State AS TeamState, dbo.Team.Date, 
                         dbo.Team.Name AS TeamName, dbo.Team.Id AS TeamId, dbo.ViewTeam.University, dbo.ViewTeam.Survey, dbo.ViewTeam.RegistrationStatus, dbo.Team.LabResultUrl, dbo.ViewTeam.MemberName, dbo.Team.WrittenReportUrl, 
                         dbo.Team.PreliminaryReportUrl, dbo.Team.TeamNumber, dbo.ViewTeam.UniversityPictureUrl, dbo.ViewTeam.PayStatus, dbo.Task.Description AS TaskDescription, dbo.Team.Deactivate, dbo.Team.WrittenReportDate, 
                         dbo.Team.PreliminaryReportDate, dbo.Team.OpenTaskTestPlanDate, dbo.Team.SuppressScoring, dbo.UserTask.Confirmed, dbo.ViewTeam.WorkPhoneNumber, dbo.Team.OpenTaskTestPlanUrl, dbo.ViewTeam.Payment, 
                         dbo.Task.Preliminary, dbo.Task.OpenTaskTestPlan
FROM            dbo.UserTask INNER JOIN
                         dbo.Task ON dbo.UserTask.TaskId = dbo.Task.Id INNER JOIN
                         dbo.Team ON dbo.Task.Id = dbo.Team.TaskId INNER JOIN
                         dbo.ViewTeam ON dbo.Team.Id = dbo.ViewTeam.Id
WHERE        (dbo.Team.Deactivate = 0)
GO

/****** Object:  View [dbo].[ViewJudgeFullInfo]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewJudgeFullInfo]
AS
SELECT       Id, UserId, RoleId, Sex, UserName, Email, RoleName, StreetLine1, StreetLine2, City, State, ZipCode, ShortBio, ProfilePictureUrl, ResumeUrl, FirstName, LastName, EmailConfirmed, SizeId, T_Shirt_Size, PhoneNumber,WorkPhoneNumber, DietTypeId, 
                         DietType, Agreement, Tasks = STUFF
                             ((SELECT       ', ' + vu.TaskName
                                 FROM            dbo.ViewUserTask vu
                                 WHERE        vu.UserId = P.UserId FOR XML path('')), 1, 1, ''), Teams = STUFF
                             ((SELECT       ', ' + vt.TeamName
                                 FROM            dbo.ViewTaskTeam vt
                                 WHERE        vt.UserId = P.UserId FOR XML path('')), 1, 1, '')
FROM            dbo.ViewPersonInRole AS P
WHERE        (RoleId = N'4d7951d8-8eda-4452-8ff1-dfc9076d8bbe')
GO

/****** Object:  View [dbo].[ViewInvoiceExtraMember]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewInvoiceExtraMember]
AS
SELECT       dbo.Invoice.Id AS InvoiceId, dbo.Invoice.Title, dbo.Invoice.DateOfIssue, dbo.Invoice.InvoiceNumber, dbo.Invoice.InvoiceTotal, dbo.InvoiceDetail.Id, dbo.InvoiceDetail.PaymentRuleId, dbo.InvoiceDetail.IsFirstTeam, 
                         dbo.InvoiceDetail.TeamUnitCost, dbo.InvoiceDetail.ExtraParticipantCount, dbo.InvoiceDetail.ExtraParticipantUnitCost, dbo.InvoiceDetail.ExtraParticipantAmount, dbo.InvoiceDetail.Amount, dbo.Invoice.Subtotal, dbo.Invoice.Tax, 
                         dbo.Invoice.Total, dbo.Invoice.AmountDue, dbo.PaymentRule.TypeOfRegistration, dbo.PaymentRule.FirstTeamFee, dbo.PaymentRule.DueDate, dbo.ViewTeam.Name AS TeamName, dbo.ViewTeam.RoleName, dbo.ViewTeam.RoleId, 
                         dbo.ViewTeam.UniversityId, dbo.ViewTeam.University, dbo.ViewTeam.StreetLine1, dbo.ViewTeam.StreetLine2, dbo.ViewTeam.City, dbo.ViewTeam.State, dbo.ViewTeam.ZipCode, dbo.ViewTeam.PayStatus, dbo.ViewTeam.FirstName, 
                         dbo.ViewTeam.LastName, dbo.ViewTeam.PhoneNumber, dbo.ViewTeam.WorkPhoneNumber, dbo.ViewTeam.Email, dbo.ViewTeam.Sex, dbo.ViewTeam.MemberUserId AS UserId, dbo.ViewTeam.Id AS TeamId, dbo.Invoice.Finished, 
                         dbo.InvoiceDetail.ExtraTeamDiscount, dbo.Invoice.ConventionalFee, dbo.InvoiceDetail.ConventionalFee AS DetailConventionalFee, dbo.ViewTeam.Task, dbo.ViewTeam.Payment
FROM            dbo.ViewTeam LEFT OUTER JOIN
                         dbo.Invoice INNER JOIN
                         dbo.PaymentRule INNER JOIN
                         dbo.InvoiceDetail ON dbo.PaymentRule.Id = dbo.InvoiceDetail.PaymentRuleId ON dbo.Invoice.Id = dbo.InvoiceDetail.InvoiceId ON dbo.ViewTeam.Id = dbo.InvoiceDetail.TeamId
GO

/****** Object:  View [dbo].[R1]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[R1]
AS
SELECT        dbo.ViewTeam.Task, dbo.ViewTeam.Name, dbo.ViewTeam.Email, dbo.ViewTeam.University, dbo.ViewTeam.State, dbo.ViewTeam.LastName, dbo.ViewTeam.FirstName, dbo.InvoiceDetail.InvoiceId, dbo.InvoiceDetail.TeamId, 
                         dbo.InvoiceDetail.IsFirstTeam, dbo.InvoiceDetail.TeamUnitCost, dbo.PaymentRule.TypeOfRegistration, dbo.PaymentRule.FirstTeamFee, dbo.PaymentRule.DueDate, dbo.PaymentRule.ExtraTeamDiscount, 
                         dbo.InvoiceDetail.ExtraParticipantCount, dbo.InvoiceDetail.ExtraParticipantUnitCost, dbo.InvoiceDetail.ExtraParticipantAmount, dbo.InvoiceDetail.ExtraTeamDiscount AS Expr1, dbo.InvoiceDetail.Amount, 
                         dbo.InvoiceDetail.ConventionalFee
FROM            dbo.ViewTeam INNER JOIN
                         dbo.InvoiceDetail ON dbo.ViewTeam.Id = dbo.InvoiceDetail.TeamId INNER JOIN
                         dbo.PaymentRule ON dbo.InvoiceDetail.PaymentRuleId = dbo.PaymentRule.Id
GO

/****** Object:  View [dbo].[ViewTask]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewTask]
AS
SELECT       dbo.Task.Id, dbo.Task.Name, dbo.Task.ImageUrl, dbo.Task.Description, dbo.TaskGrade.GradeId, dbo.Grade.Name AS Grade, dbo.Task.Preliminary, dbo.Task.OpenTaskTestPlan
FROM            dbo.Task INNER JOIN
                         dbo.TaskGrade ON dbo.Task.Id = dbo.TaskGrade.TaskId INNER JOIN
                         dbo.Grade ON dbo.TaskGrade.GradeId = dbo.Grade.Id
GO

/****** Object:  View [dbo].[ViewTaskFullInfo]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE view [dbo].[ViewTaskFullInfo]
AS
SELECT        t.Id, t.Name, t.ImageUrl, 
			  t.Description, 
			  GradeIds =  STUFF
                             ((SELECT        N'■ ' +  cast( vt.GradeId as nvarchar(max))
                                 FROM dbo.ViewTask vt
                                 WHERE  vt.Id = t.Id FOR XML path('')), 1, 1, '') 
			  , 
			  Grades =  STUFF
                             ((SELECT        N'■ ' + vt.Grade
                                 FROM dbo.ViewTask vt
                                 WHERE  vt.Id = t.Id FOR XML path('')), 1, 1, '') 
			  , Judges = STUFF
                             ((SELECT        ', ' + vu.FirstName + ' ' + vu.LastName
                                 FROM            dbo.ViewUserTask vu
                                 WHERE        vu.RoleId = N'4d7951d8-8eda-4452-8ff1-dfc9076d8bbe' 
								 AND 
								 vu.TaskId = t.Id FOR XML path('')), 1, 1, '')

FROM            dbo.Task  t


GO

/****** Object:  View [dbo].[View_Test]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[View_Test]
AS
SELECT       dbo.Team.Name AS [Team Name], dbo.SafetyItem.Name AS [Safety Item], dbo.TeamSafetyItem.LastComment
FROM            dbo.Team INNER JOIN
                         dbo.TeamSafetyItem ON dbo.Team.Id = dbo.TeamSafetyItem.TeamId INNER JOIN
                         dbo.SafetyItem ON dbo.TeamSafetyItem.SafetyItemId = dbo.SafetyItem.Id
GO

/****** Object:  View [dbo].[ViewOrder]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewOrder]
AS
SELECT       dbo.[Order].Id, dbo.[Order].ShopOrderId, dbo.[Order].UserId, dbo.[Order].OrderDate, dbo.[Order].InvoiceId, dbo.[Order].Complete, dbo.Invoice.Title, dbo.Invoice.DateOfIssue, dbo.Invoice.InvoiceNumber, dbo.Invoice.InvoiceTotal, 
                         dbo.Invoice.Subtotal, dbo.Invoice.Tax, dbo.Invoice.Total, dbo.Invoice.AmountDue, dbo.Invoice.Finished, dbo.Invoice.ConventionalFee
FROM            dbo.Invoice INNER JOIN
                         dbo.[Order] ON dbo.Invoice.Id = dbo.[Order].Id
GO

/****** Object:  View [dbo].[ViewSurvey]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewSurvey]
AS
SELECT       dbo.Question.Id, dbo.Question.Question, dbo.Question.Priority AS QuestionPriority, dbo.Question.Type AS QuestionType, dbo.QuestionAnswer.AnswerId, dbo.QuestionAnswer.Id AS QuestionAnswerId, 
                         dbo.QuestionAnswer.Type AS AnswerType, dbo.QuestionAnswer.Weight, dbo.QuestionAnswer.Priority AS AnswerPriority, dbo.Answer.Answer, dbo.Answer.TitleVisible, dbo.QuestionAnswer.Comment, 
                         dbo.Question.Comment AS QuestionComment, dbo.QuestionAnswer.ShowComment
FROM            dbo.Answer INNER JOIN
                         dbo.QuestionAnswer ON dbo.Answer.Id = dbo.QuestionAnswer.AnswerId INNER JOIN
                         dbo.Question ON dbo.QuestionAnswer.QuestionId = dbo.Question.Id
GO

/****** Object:  View [dbo].[ViewSurveyResult]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewSurveyResult]
AS
SELECT       dbo.AspNetUsers.Email, dbo.AspNetUsers.UserName, dbo.AspNetUsers.PhoneNumber, dbo.AspNetUsers.WorkPhoneNumber, dbo.Person.FirstName, dbo.Person.LastName, dbo.University.Name AS University, dbo.Team.Name AS TeamName, dbo.Team.TaskId, 
                         dbo.Task.Name AS TaskName, dbo.Person.Sex, dbo.SurveyResult.UserId, dbo.SurveyResultDetail.Value, dbo.SurveyResult.TeamId, dbo.SurveyResultDetail.QuestionAnswerId, dbo.SurveyResult.QuestionId AS Id, 
                         dbo.SurveyResultDetail.Comment
FROM            dbo.University RIGHT OUTER JOIN
                         dbo.AspNetUsers INNER JOIN
                         dbo.Person ON dbo.AspNetUsers.Id = dbo.Person.UserId INNER JOIN
                         dbo.SurveyResultDetail INNER JOIN
                         dbo.SurveyResult ON dbo.SurveyResultDetail.SurveyResultId = dbo.SurveyResult.Id ON dbo.AspNetUsers.Id = dbo.SurveyResult.UserId LEFT OUTER JOIN
                         dbo.Task INNER JOIN
                         dbo.Team ON dbo.Task.Id = dbo.Team.TaskId ON dbo.SurveyResult.TeamId = dbo.Team.Id ON dbo.University.Id = dbo.Person.UniversityId
GO

/****** Object:  View [dbo].[ViewTeamGradeDetail]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewTeamGradeDetail]
AS
SELECT       dbo.TeamGradeDetail.Id, dbo.TeamGradeDetail.TeamId, dbo.TeamGradeDetail.GradeDetailId, dbo.Team.TaskId, dbo.Team.Name AS TeamName, dbo.Team.Date, dbo.Team.State, dbo.Team.ImageUrl, dbo.Team.LabResultUrl, 
                         dbo.Team.WrittenReportUrl, dbo.GradeDetail.GradeId, dbo.GradeDetail.EvaluationItem, dbo.GradeDetail.Point AS MaxPoint, dbo.Grade.Name AS Grade, dbo.TeamGradeDetail.Point, dbo.Team.TeamNumber, 
                         dbo.TeamGradeDetail.Description, dbo.TeamGradeDetail.JudgeUserId, dbo.GradeDetail.Coefficient, dbo.TeamGradeDetail.Signature, dbo.Person.FirstName, dbo.Person.LastName, dbo.Team.PreliminaryReportUrl, 
                         dbo.Team.OpenTaskTestPlanUrl
FROM            dbo.TeamGradeDetail INNER JOIN
                         dbo.Team ON dbo.TeamGradeDetail.TeamId = dbo.Team.Id INNER JOIN
                         dbo.Grade INNER JOIN
                         dbo.GradeDetail ON dbo.Grade.Id = dbo.GradeDetail.GradeId ON dbo.TeamGradeDetail.GradeDetailId = dbo.GradeDetail.Id INNER JOIN
                         dbo.Person ON dbo.TeamGradeDetail.JudgeUserId = dbo.Person.UserId
WHERE        (dbo.Team.Deactivate = 0)
GO

/****** Object:  View [dbo].[ViewTeamSafetyItem]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewTeamSafetyItem]
AS
SELECT       dbo.TeamSafetyItem.Id, dbo.TeamSafetyItem.TeamId, dbo.TeamSafetyItem.LastContent, dbo.TeamSafetyItem.LastComment, dbo.TeamSafetyItem.ItemStatus, dbo.TeamSafetyItem.AttachedFileUrl, 
                         dbo.SafetyItem.Name AS SafetyItemName, dbo.SafetyItem.Instruction, dbo.SafetyItem.Priority, dbo.TeamSafetyItem.SafetyItemId, dbo.SafetyItem.AttachmentRequired, dbo.SafetyItem.TextRequired
FROM            dbo.SafetyItem INNER JOIN
                         dbo.TeamSafetyItem ON dbo.SafetyItem.Id = dbo.TeamSafetyItem.SafetyItemId
GO

/****** Object:  View [dbo].[ViewTeamSafetyItemLog]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewTeamSafetyItemLog]
AS
SELECT       dbo.TeamSafetyItemLog.Id, dbo.TeamSafetyItemLog.UserId, dbo.TeamSafetyItemLog.TeamSafetyItemId, dbo.TeamSafetyItemLog.[Content], dbo.TeamSafetyItemLog.AttachedFileUrl, dbo.TeamSafetyItemLog.DateTime, 
                         dbo.TeamSafetyItemLog.Type, dbo.TeamSafetyItem.SafetyItemId, dbo.SafetyItem.Name, dbo.TeamSafetyItem.TeamId, dbo.AspNetUsers.UserName, dbo.Person.FirstName, dbo.Person.LastName
FROM            dbo.TeamSafetyItemLog INNER JOIN
                         dbo.TeamSafetyItem ON dbo.TeamSafetyItemLog.TeamSafetyItemId = dbo.TeamSafetyItem.Id INNER JOIN
                         dbo.SafetyItem ON dbo.TeamSafetyItem.SafetyItemId = dbo.SafetyItem.Id INNER JOIN
                         dbo.AspNetUsers ON dbo.TeamSafetyItemLog.UserId = dbo.AspNetUsers.Id INNER JOIN
                         dbo.Person ON dbo.AspNetUsers.Id = dbo.Person.UserId
GO

/****** Object:  View [dbo].[ViewTeamTaskTest]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewTeamTaskTest]
AS
SELECT       dbo.Test.Id, dbo.UserTask.UserId, dbo.TaskTest.TaskId, dbo.Task.ImageUrl AS TaskImageUrl, dbo.Task.Description, dbo.Task.Name AS Task, dbo.Test.Name, dbo.Team.Id AS TeamId, dbo.Team.Name AS TeamName, dbo.Team.Date, 
                         dbo.Team.State, dbo.Team.ImageUrl AS TeamImageUrl, dbo.Team.LabResultUrl, dbo.Team.WrittenReportUrl, dbo.Team.PreliminaryReportUrl, dbo.Team.TeamNumber, dbo.Team.PayStatus, dbo.Team.Deactivate, 
                         dbo.Team.SubmitStatus, dbo.Team.WrittenReportDate, dbo.Team.PreliminaryReportDate, dbo.Team.OpenTaskTestPlanDate, dbo.Team.OpenTaskTestPlanUrl
FROM            dbo.Task INNER JOIN
                         dbo.TaskTest ON dbo.Task.Id = dbo.TaskTest.TaskId INNER JOIN
                         dbo.Test ON dbo.TaskTest.TestId = dbo.Test.Id INNER JOIN
                         dbo.Team ON dbo.Task.Id = dbo.Team.TaskId INNER JOIN
                         dbo.UserTask ON dbo.Task.Id = dbo.UserTask.TaskId
GO

/****** Object:  View [dbo].[ViewTeamTestResult]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewTeamTestResult]
AS
SELECT       dbo.TeamTestResult.Id, dbo.TeamTestResult.TeamId, dbo.TeamTestResult.TaskId, dbo.TeamTestResult.TestId, dbo.TeamTestResult.Score, dbo.Test.Name, dbo.Test.Description, dbo.Team.Name AS TeamName
FROM            dbo.TeamTestResult INNER JOIN
                         dbo.Test ON dbo.TeamTestResult.TestId = dbo.Test.Id INNER JOIN
                         dbo.Team ON dbo.TeamTestResult.TeamId = dbo.Team.Id
GO

/****** Object:  View [dbo].[ViewTest]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewTest]
AS
SELECT       dbo.Test.Id, dbo.Task.ImageUrl, dbo.Task.Description, dbo.TaskTest.TaskId, dbo.Task.Name AS Task, dbo.Test.Name
FROM            dbo.Task INNER JOIN
                         dbo.TaskTest ON dbo.Task.Id = dbo.TaskTest.TaskId INNER JOIN
                         dbo.Test ON dbo.TaskTest.TestId = dbo.Test.Id
GO

/****** Object:  View [dbo].[ViewUserRole]    Script Date: 12/28/2019 8:45:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE view [dbo].[ViewUserRole]
AS
SELECT       dbo.AspNetRoles.Id, dbo.AspNetUserRoles.UserId, dbo.AspNetRoles.Name AS RoleName, dbo.AspNetUsers.UserName, dbo.AspNetUsers.Email, dbo.AspNetUsers.WorkPhoneNumber
FROM            dbo.AspNetUserRoles INNER JOIN
                         dbo.AspNetRoles ON dbo.AspNetUserRoles.RoleId = dbo.AspNetRoles.Id INNER JOIN
                         dbo.AspNetUsers ON dbo.AspNetUserRoles.UserId = dbo.AspNetUsers.Id
GO

