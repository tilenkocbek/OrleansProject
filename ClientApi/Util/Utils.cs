using System.Net;
using System.Net.Http;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;

namespace ClientApi.Util
{
    /// <summary>
    /// This class holds utility methods that can be used by Web Api
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Check if specified email address is of valid type.
        /// </summary>
        /// <param name="email"> email address to check. </param>
        /// <returns>true if email is valid, false otherwise.</returns>
        public static bool IsEmailValid(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static void SplitEmailAddress(string email, out string username, out string domain)
        {
            var splittedEmail = email.Split("@");
            username = splittedEmail[0];
            domain = splittedEmail[1];
        }
    }
}
