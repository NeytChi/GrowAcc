using GrowAcc.BusinessFlow;
using GrowAcc.Culture;
using System.Net;

namespace GrowAcc
{
    public class UserValidator
    {
        private PasswordHelper passwordHelper = new PasswordHelper();

        public bool IsOkay(string email, out Dictionary<string, string> errors, string culture)
        {
            errors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email", CultureConfiguration.Get("EmailEmpty", culture));
                return false;
            }

            if (!IsValidEmail(email))
            {
                errors.Add("Email", CultureConfiguration.Get("EmailInvalid", culture));
                return false;
            }

            return true;
        }
        public bool CheckPassword(string userPassword, string storedHash, string storedSalt)
        {
            return passwordHelper.VerifyPassword(userPassword, storedHash, storedSalt);
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public bool IsPasswordTrue(string password, string confirmedPassword, string culture, ref Dictionary<string, string> errors)
        {
            bool hasUpperChar = false, hasLowerChar = false, hasDigit = false, hasSpecialChar = false;

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add("Password", CultureConfiguration.Get("PasswordEmpty", culture));
                return false;
            }
            if (password.Length < 8)
            {
                errors.Add("Password", CultureConfiguration.Get("PasswordLength", culture));
                return false;
            }
            if (!password.Equals(confirmedPassword))
            {
                errors.Add("Password", CultureConfiguration.Get("PasswordConfirm", culture));
                return false;
            }

            foreach (var c in password)
            {
                if (char.IsUpper(c)) hasUpperChar = true;
                if (char.IsLower(c)) hasLowerChar = true;
                if (char.IsDigit(c)) hasDigit = true;
                if (!char.IsLetterOrDigit(c)) hasSpecialChar = true;
            }
            if (!hasUpperChar)
            {
                errors.Add("Password", CultureConfiguration.Get("PasswordUpperChar", culture));
            }
            if (!hasLowerChar)
            {
                errors.Add("Password", CultureConfiguration.Get("PasswordLowerChar", culture));
            }
            if (!hasDigit)
            {
                errors.Add("Password", CultureConfiguration.Get("PasswordDigitChar", culture));
            }
            if (!hasSpecialChar)
            {
                errors.Add("Password", CultureConfiguration.Get("PasswordSpecialChar", culture));
            }
            return hasUpperChar && hasLowerChar && hasDigit && hasSpecialChar;
        }
        /// <summary>
        /// Getting a user's string password - dictionary with key 'Password' with value hashed password and key 'Salt' with value of the salt for this password.
        /// </summary>
        public Dictionary<string, string> GetCombinationHashPassword(string password)
        {
            var salt = passwordHelper.GenerateSalt();
            var hashedPassword = passwordHelper.HashPasswordWithSalt(password, salt);
            return new Dictionary<string, string>()
            {
                { "Salt", salt },
                { "Password", hashedPassword }
            };
        }
        public string CreateConfirmToken()
        {
            return passwordHelper.GenerateToken();
        }
        public string CreateNewPassword()
        {
            return passwordHelper.GenerateNewPassword();
        }
    }
}