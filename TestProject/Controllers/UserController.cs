using System;
using System.Linq;
using System.Web.Mvc;
using TestProjectDomain.Entities;
using TestProjectDomain.Concrete;
using System.Web.Security;
using NLog;
using TestProject.Repository;

namespace TestProject.Controllers
{
    [Authorize]
    public class UserController:Controller
    {
        CrudOperation crud = new CrudOperation();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private MyDB db = new MyDB();

        [AllowAnonymous]
        public ActionResult Create()
        {
            return View("Registration");
        }

        public ActionResult Update(int? userId)
        {
          
            Users user = crud.GetInfo(userId,null);
            return View("Update", user);
        }



        [AllowAnonymous]
        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel logindata)
        {
            if (ModelState.IsValid)
            {
                var authResult = Membership.ValidateUser(logindata.Email, logindata.Password);
                if (authResult)
                {
                    Users targetUser = crud.GetInfo(null,logindata.Email);

                    FormsAuthentication.SetAuthCookie(targetUser.Email, false);
                    logger.Info("Logged with id",targetUser.UserId);
                    return RedirectToAction ("Get", new { userid = targetUser.UserId });
                }
                logger.Info("Incorrected login values");
                ViewBag.Message = "Логин/Пароль введены неправильно";
                return View("Login");
            }
            ViewBag.Message = "Логин/Пароль введены неправильно";
            return View("Login");
        }

        [HttpPost]
        public ActionResult Update_Check(Users user)
        {
            if (ModelState.IsValid)
            {
                logger.Info("Start to update user info");
                int result = crud.UpdateUser(user);
                if (result >= 1)
                {
                    var userDb = crud.GetInfo(user.UserId, null);
                    logger.Info("User with id{0} was updated", user.UserId);
                    return RedirectToAction("Get", userDb);
                }
                else
                {
                    logger.Info("User with id{0} was not updated", user.UserId);
                    return View("Registration");
                }
            }
            else return View("Registration");
        }


        [HttpPost]
        [AllowAnonymous]
        public ActionResult Get(RegisterUsers user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Random r = new Random();
                    int salt = r.Next(0, 100);
                    int res = salt/2;
                    string cryptedPassword = user.Password + res.ToString();
                        user.CryptedPassword = cryptedPassword;
                        user.PasswordSalt = salt.ToString();
                        var newUser = new Users
                        {
                            Name = user.Name,
                            Email = user.Email,
                            Surname = user.Surname,
                            CryptedPassword = user.CryptedPassword,
                            PasswordSalt = salt.ToString(),
                            Mobile = user.Mobile
                        };
                        int? result = crud.AddUser(newUser);
                    if (result!=null)
                    {
                        FormsAuthentication.SetAuthCookie(newUser.Email, false);
                        var createduser = crud.GetInfo(result, null);
                        UsersData usersData = new UsersData()
                        {
                            Surname = createduser.Surname,
                            Name = createduser.Name,
                            Email = createduser.Email,
                            Mobile = createduser.Mobile,
                            UserId = createduser.UserId
                        };
                        return View("Get", usersData);
                    }
                    else
                    {
                        ViewBag.Message = "Пользователь уже существует";
                         return View("Registration");
                    }
                    
                }
                else
                    return View("Registration");
            }
            catch (Exception ex)
            {
                logger.Info("Error to save user - ", ex.Message);
                return null;

            }
        }


        public ActionResult Get(int? userid)
        {
            Users targetUser = crud.GetInfo(userid, null); 
            UsersData usersData = new UsersData()
            {
                UserId = targetUser.UserId,
                Email = targetUser.Email,
                Mobile = targetUser.Mobile,
                Name = targetUser.Name,
                Surname = targetUser.Surname
            };
            return View("Get", usersData);
        }
        public ActionResult GetUpdate(int? userid)
        {
            Users user = crud.GetInfo(userid, null); 
            ViewBag.IsUpdateMode = true;
            RegisterUsers reguser = new RegisterUsers
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Mobile = user.Mobile,
                UserId = user.UserId
            };
            return View("Registration",reguser);
        }

        public ActionResult ConfirmUpdate()
        {          
            return View("Get");
        }
        public ActionResult Delete(int? userid)
        {
            bool deleteresult = crud.DeleteUser(userid);
            if (deleteresult)
            {
                logger.Info("User {0} was deleted", userid);
                return RedirectToAction("Login");
            }
            else return RedirectToAction("Get", new { userid = userid });

        }

    }
}