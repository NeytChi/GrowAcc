using GrowAcc.BusinessFlow;

namespace GrowAcc
{
    public class UserValidator
    {
        public bool IsOkay(string email, out Dictionary<string, string> errors)
        {
            errors = new Dictionary<string, string>();

            // if (string.IsNullOrWhiteSpace(user.Login))
            // {
            //     errors.Add("Login is required.");
            // }

            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add("Email", "Email is required.");
                return false;
            }

            if (!IsValidEmail(email))
            {
                errors.Add("Email", "Invalid email format.");
                return false;
            }

            return true;
        }
        public bool IsPasswordStored(string userPassword, string storedHash, ref Dictionary<string, string> errors)
        {
            if (PasswordHelper.VerifyPassword(userPassword, storedHash))
            {
                return true;
            }
            errors.Add("Password", "Password is incorrect.");
            return false;
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
        public bool IsPasswordTrue(string password, string confirmedPassword, ref Dictionary<string,string> errors)
        {
            bool hasUpperChar = false, hasLowerChar = false, hasDigit = false, hasSpecialChar = false;

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                errors.Add("Password", "Password can't be empty.");
                return false;
            }
            if (!password.Equals(confirmedPassword))
            {
                errors.Add("Password", "Your password isn't equals to confirmed password.");
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
                errors.Add("Password", "Your password doesn't have upper char.");
            }
            if (!hasLowerChar)
            {
                errors.Add("Password", "Your password doesn't have lower char.");
            }    
            if (!hasDigit)
            {
                errors.Add("Password", "Your password doesn't have digits.");
            }
            if (!hasSpecialChar)
            {
                errors.Add("Password", "Your password doesn't have digits.");
            }
            return hasUpperChar && hasLowerChar && hasDigit && hasSpecialChar;
        }
        public string ConvertPasswordForStore(string password)
        {
            return PasswordHelper.GetPasswordForStore(password);
        }
    }
}
