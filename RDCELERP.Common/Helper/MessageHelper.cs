namespace RDCELERP.Common.Helper
{
    public static class MessageHelper
    {
        public const string SomethingWrong = "Something went wrong";
        public const string SuccessMessage = "{0} {1} successfully {2}";
        public const string Message = "{0} {1} {2}";
        public const string EmailExist = "Email already exist if you have an account please sign in";
        public const string AccountDeleted = "Account Deleted Recently Try After Some Time";
        public const string PhoneExist = "Phone Number already exist if you have an account please sign in";
        public const string UserNameExist = "User Name already exist";
        public const string IsExistsMessage = "{0} is already exists.";
        public const string PassParaMessage = "Please pass {0}.";
        public const string WrongIdMessage = "Please pass correct {0}.";
        public const string NullMessage = "{0} can't  be null.";
        public const string EmailNotRegistred = "Email address is not registered";
        public const string NoFound = "{0} not Found.";
        public const string SubjectResetPassword = "Totem - Reset Password";
        public const string EmailSentFailedWhileResetPassword = "There is an error while sending email for {0} password.";
        public const string FilesMissing = "No files added to upload";
        public const string PostIdMissing = "PostId missing";
        public const string MemorieIdMissing = "MemorieId missing";

        public const string AccessRestricted = "Please contact administrator";

        public const string LoginAttemptsFailed = "repeated failed login attempts Account is blocked Please Check Email";

        public const string MediaTypeInvalid = "Invalid media type";
        public const string StoryAlreadySeen = "Data Already Added in Database";

        public static class TblUser
        {
            public const string UserBlocked = "Restricted access You are Blocked By This User";
            public const string ReportedBlocked = "Report inappropriate or abusive things on Totem";
            public const string EmailConfirmFail = "Email address is not confirmed";
            public const string ActiveMessage = "This account is not activated yet.Please contact administration";
            public const string ConfirmPass = "Password Confirmed";
            public const string ConfirmPassNot = "Password and Confirm Password not Matched";
           
            public const string LoginFail = "Please enter correct email address or password";
            public const string BLoginFail = "Please enter email address or password";
            public const string PLoginFail = "Please enter correct old password";
            public const string ELoginFail = "Please enter correct Email";
            public const string AdminLoginFail = "Please enter Admin email address and password";
            public const string EmailSentFailedWhileActivation = "There is an error while sending email address for account activation";
            public const string EmailSentReceiveEmailUpdatesSave = "There is an error while sending email address for Updates";
            public const string SubjectAccountActivation = "Account Activation";
            public const string ReceiveEmailUpdatesSave = "Totem News Letter";
            public const string EarlyToParty = "Coming Soon Totem";
            public const string AdminEarlyToParty = "Totem user is Early To Party";
            public const string AccountNotActiveMessage = "This account is not activated yet.Please activate your account";
            public const string ActiveSuccessMessage = "This account is now activated.You can log in now";
            public const string ActiveFailMessage = "There is an error while account activation";
            public const string Enquery = "Totem Enquiry Submitted";
            public const string EmailSentFailedWhileEnquery = "There is an error while sending email address for Enquery Submitted";
            public const string EnquerySubmitionFail = "Error while Enquery Submition";
        }

        public static class Login
        {
            public const string LoginSuccessMessage = " Login successfully ";
        }
        public static class ForgotPassword
        {
            public const string ForgotPasswordSuccess = "Password reset link has been sent successfully";
            public const string ForgotPasswordFail = "Error while sending reset password link";
        }
        public static class ResetPassword
        {
            public const string SamePasswordError = "New password should not be same as old password";
            public const string ResetPasswordFail = "Error while reseting password";
        }
        public static class ChangePassword
        {
            public const string ChangePasswordSuccess = "Password updated successfully";
            public const string PasswordNotMatch = "Please enter correct old password";
            public const string NewOldPasswordSameMatch = "Old Password and New Password should be different";
        }
        public static class Document
        {
            public const string DuplicateTitle = "File name already exist";
        }

    }
}
