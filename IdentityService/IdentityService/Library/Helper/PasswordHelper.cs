using System;
using System.Security.Cryptography;
using System.Text;

namespace IdentityService.Library.Helper
{
    public class PasswordHelper
    {
        public static String ConvertToSHA512(String password)
        {
            String hash;
            var data = Encoding.UTF8.GetBytes(password);
            using (SHA512 shaM = new SHA512Managed())
            {
                hash = BitConverter.ToString(shaM.ComputeHash(data)).Replace("-", "");
            }

            return hash;
        }

    }
}
