using Model.ViewModels.Person;

using System;
using System.Collections.Generic;

namespace Model.ApplicationDomainModels
{
    public class _ImageType
    {
        public string this[byte index]
        {
            get
            {
                if (index == FirstPageImage) return FirstPageImageDesc;
                else
                if (index == LeftSponsorImage) return LeftSponsorImageDesc;
                else
                if (index == RightSponsorImage) return RightSponsorImageDesc;

                return string.Empty;

            }
        }
        public byte this[string stringIndex]
        {
            get
            {

                if (stringIndex == FirstPageImageDesc) return FirstPageImage;
                else
                if (stringIndex == LeftSponsorImageDesc) return LeftSponsorImage;
                else
                if (stringIndex == RightSponsorImageDesc) return RightSponsorImage;

                throw new Exception();

            }
        }
        public byte FirstPageImage { get { return 0; } }
        public string FirstPageImageDesc { get { return "First Page Image"; } }
        public byte LeftSponsorImage { get { return 1; } }
        public string LeftSponsorImageDesc { get { return "Left Sponsor Image"; } }
        public byte RightSponsorImage { get { return 2; } }
        public string RightSponsorImageDesc { get { return "Right Sponsor Image"; } }

    }

    public static class ConstantObjects
    {
        public static List<VmPerson> PersonList { get; set; }

        public static bool AllowRegistration { get; set; }
        private static _ImageType _imageType;
        public static _ImageType ImageType
        {
            get
            {
                if (_imageType == null)
                {
                    _imageType = new _ImageType();
                }
                return _imageType;
            }
        }

        public enum TeamState
        {
            All = 0,
            NewUploaded = 1,
            AssignedToEditor = 2,
            AcceptedByEditor = 3,
            RejectedByEditor = 4,
            NeedToChangeByEditor = 5,
            AssignedToReviewer = 6,
            AcceptedByReviewer = 7,
            RejectedByReviewer = 8,
            NeedToChangeByReviewer = 9,
            WaitForAuthor = 10,
            Corrected = 11,
            HasStructuralError = 12,
            UnderReview = 13,
            AssignedToVolume = 14,
            WaitForEditorInChief = 15,
            WaitForEditor = 16,
            AcceptedByEditorInChief = 17,
            RejectedByEditorInChief = 18,
            NeedToChangeByEditorInChief = 19
        };

        public static TeamState GetTeamStateByNumber(int TeamStateCode)
        {
            switch (TeamStateCode)
            {

                case 0: return TeamState.All;
                case 1: return TeamState.NewUploaded;
                case 2: return TeamState.AssignedToEditor;
                case 3: return TeamState.AcceptedByEditor;
                case 4: return TeamState.RejectedByEditor;
                case 5: return TeamState.NeedToChangeByEditor;
                case 6: return TeamState.AssignedToReviewer;
                case 7: return TeamState.AcceptedByReviewer;
                case 8: return TeamState.RejectedByReviewer;
                case 9: return TeamState.NeedToChangeByReviewer;
                case 10: return TeamState.WaitForAuthor;
                case 11: return TeamState.Corrected;
                case 12: return TeamState.HasStructuralError;
                case 13: return TeamState.UnderReview;
                case 14: return TeamState.AssignedToVolume;
                case 15: return TeamState.WaitForEditorInChief;
                case 16: return TeamState.WaitForEditor;
                case 17: return TeamState.AcceptedByEditorInChief;
                case 18: return TeamState.RejectedByEditorInChief;
                case 19: return TeamState.NeedToChangeByEditorInChief;

                default: return TeamState.All;
            }
        }

        public static string GetTeamStateDescription(int? TeamStateCode)
        {
            switch (TeamStateCode.Value)
            {
                case 0: return "All";
                case 1: return "New Uploaded";
                case 2: return "Assigned To Editor";
                case 3: return "Accepted By Editor";
                case 4: return "Rejected By Editor";
                case 5: return "Need To Change By Editor";
                case 6: return "Assigned To Reviewer";
                case 7: return "Accepted By Reviewer";
                case 8: return "Rejected By Reviewer";
                case 9: return "Need To Change By Reviewer";
                case 10: return "Wait For Author";
                case 11: return "Corrected";
                case 12: return "Has Structural Error";
                case 13: return "Under Review";
                case 14: return "Assigned To Volume";
                case 15: return "Wait For Editor In Chief";
                case 16: return "Wait For Editor";
                case 17: return "Accepted By Editor In Chief";
                case 18: return "Rejected By Editor In Chief";
                case 19: return "Need To Change By Editor In Chief";

                default: return "";
            }

        }

        public static TeamState GetTeamStatetype(string TeamStateDescription)
        {
            switch (TeamStateDescription)
            {
                case "All": return TeamState.All;
                case "NewUploaded": return TeamState.NewUploaded;
                case "HasStructuralError": return TeamState.HasStructuralError;
                case "AssignedToReviewer": return TeamState.AssignedToReviewer;
                case "UnderReview": return TeamState.UnderReview;
                case "NeedToChangeByReviewer": return TeamState.NeedToChangeByReviewer;
                case "AcceptedByReviewer": return TeamState.AcceptedByReviewer;
                case "WaitForAuthor": return TeamState.WaitForAuthor;
                case "Corrected": return TeamState.Corrected;
                case "RejectedByReviewer": return TeamState.RejectedByReviewer;
                case "RejectedByEditor": return TeamState.RejectedByEditor;
                case "AcceptedByEditor": return TeamState.AcceptedByEditor;
                case "NeedToChangeByEditor": return TeamState.NeedToChangeByEditor;
                case "AssignedToEditor": return TeamState.AssignedToEditor;
                case "AssignedToVolume": return TeamState.AssignedToVolume;
                case "WaitForEditorInChief": return TeamState.WaitForEditorInChief;
                case "WaitForEditor": return TeamState.WaitForEditor;
                case "AcceptedByEditorInChief": return TeamState.AcceptedByEditorInChief;
                case "RejectedByEditorInChief": return TeamState.RejectedByEditorInChief;
                case "NeedToChangeByEditorInChief": return TeamState.NeedToChangeByEditorInChief;

                default: return TeamState.All;
            }
        }
        public static int GetTeamStateTypeNumber(TeamState TeamState)
        {
            switch (TeamState)
            {
                case TeamState.All: return 0;
                case TeamState.NewUploaded: return 1;
                case TeamState.AssignedToEditor: return 2;
                case TeamState.AcceptedByEditor: return 3;
                case TeamState.RejectedByEditor: return 4;
                case TeamState.NeedToChangeByEditor: return 5;
                case TeamState.AssignedToReviewer: return 6;
                case TeamState.AcceptedByReviewer: return 7;
                case TeamState.RejectedByReviewer: return 8;
                case TeamState.NeedToChangeByReviewer: return 9;
                case TeamState.WaitForAuthor: return 10;
                case TeamState.Corrected: return 11;
                case TeamState.HasStructuralError: return 12;
                case TeamState.UnderReview: return 13;
                case TeamState.AssignedToVolume: return 14;
                case TeamState.WaitForEditorInChief: return 15;
                case TeamState.WaitForEditor: return 16;
                case TeamState.AcceptedByEditorInChief: return 17;
                case TeamState.RejectedByEditorInChief: return 18;
                case TeamState.NeedToChangeByEditorInChief: return 19;

                default: return 0;
            }

        }


        public static Dictionary<Guid, Guid> LoggedUsers = new Dictionary<Guid, Guid>();
        public enum SystemRoles
        {
            Admin = 0,
            Advisor = 1,
            Judge = 2,
            Leader = 3,
            Student = 4,
            CoAdvisor = 5,
            SafetyAdmin = 6,
            Lab = 7,
        }
        public enum Approval
        {
            Pending = 0,
            Approve = 1,
            Reject = 2,
        }
        public enum PayStatus
        {
            NotPayed = 0,
            Payed = 1,
        }

        public static string GetSystemRolesString(SystemRoles systemRoles)
        {
            switch (systemRoles)
            {
                case SystemRoles.Admin: return "Admin";
                case SystemRoles.Advisor: return "Advisor";
                case SystemRoles.Judge: return "Judge";
                case SystemRoles.Leader: return "Leader";
                case SystemRoles.Student: return "Student";
                case SystemRoles.CoAdvisor: return "Co-Advisor";
                case SystemRoles.SafetyAdmin: return "Safety Admin";
                case SystemRoles.Lab: return "Lab";

                default: return "";
            }
        }
        public enum QuestionType
        {
            Faculty = 0,
            Judge = 1,
            Student = 2

        }
        public enum AnswerType
        {
            TextBox = 0,
            RadioButton = 1,
            CheckBox = 2,
            Description = 3,
            SingleSelectList = 4,
            MultySelectList = 5
        }
        public enum SubmissionRule
        {
            PreliminaryReport = 1,
            OpenTaskTestPlan = 2,
            WrittrnReport = 3,
            ExprementalSafety = 4,
        }
         public enum PaymentType
        {
            Bench_Scale = 1,
            Desktop = 2,
 
        }
    }
}
