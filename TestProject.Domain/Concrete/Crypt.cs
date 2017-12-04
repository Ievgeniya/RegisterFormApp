using System;
using System.Collections.Generic;
using System.Linq;
namespace TestProjectDomain.Concrete
{
    public  static class Crypt 
    {
        public static int GetResult(string login, string password)
        {
            string salt = null, result = null;
            var db = new MyDB();
            var user = db.Users.FirstOrDefault(u => u.Email == login);
            db.Dispose();
            if (user != null)
            {
                salt = user.PasswordSalt;
                result = user.CryptedPassword;
                int res = Convert.ToInt32(salt) / 2;
                if (!String.IsNullOrEmpty(login))
                {
                    if (result != null && password.Trim() + res.ToString().Trim() == result.Trim())
                        return 0;
                    else
                        return 1;
                }
                else
                    return -1;

            }
            else return -1;
        }


    }
}
