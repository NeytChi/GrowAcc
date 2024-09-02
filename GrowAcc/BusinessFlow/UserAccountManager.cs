using GrowAcc.Models;
using GrowAcc.RequestCommands;
using System.ComponentModel.DataAnnotations;

namespace GrowAcc.BusinessFlow
{
    public class UserAccountManager
    {
        public UserAccountManager() { }
        
        /// <summary>
        /// Create a new user by request command. If validation of data is false, then it will return null.
        /// </summary>
        /// <param name="request">All fields is requered.</param>
        /// <returns></returns>
        public UserAccount Registration(UserAccountRegistrationCommand request)
        {
            /*
            if (ValidateUser(cache.user_email, cache.user_password, ref message) && ProfileIsTrue(cache, ref message)
                && validator.UserNameIsTrue(cache.user_fullname, ref message))
            {
                User currentUser = context.Users.Where(u => u.userEmail == cache.user_email).FirstOrDefault();
                if (currentUser == null)
                {
                    currentUser = Registrate(cache);
                    SendConfirmEmail(currentUser.userEmail, cache.culture, currentUser.userHash);
                    return true;
                }
                else
                    return RestoreUser(currentUser, ref message);
            }*/
            return null ;
        }
        public void Login()
        {

        }
        public void Logout()
        {

        }
    }
}
