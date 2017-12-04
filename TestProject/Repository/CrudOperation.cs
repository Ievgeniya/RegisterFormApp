using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using NLog;
using TestProjectDomain.Entities;

namespace TestProject.Repository
{
    public class CrudOperation
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["MyDb"].ToString();
            con = new SqlConnection(constr);

        }
        //To Add User    
        public int? AddUser(Users obj)
        {
            int i = 0;
            int? result = null;
            try
            {
                connection();
                SqlCommand com = new SqlCommand("[dbo].[AddNewUser]", con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@Name", obj.Name);
                com.Parameters.AddWithValue("@Surname", obj.Surname);
                com.Parameters.AddWithValue("@Email", obj.Email);
                com.Parameters.AddWithValue("@Mobile", obj.Mobile);
                com.Parameters.AddWithValue("@CryptedPassword", obj.CryptedPassword);
                com.Parameters.AddWithValue("@PasswordSalt", obj.PasswordSalt);
                SqlParameter retval = new SqlParameter("@ReturnValue", System.Data.SqlDbType.Int);
                retval.Direction = ParameterDirection.ReturnValue;
                com.Parameters.Add(retval);
                


                con.Open();
                i = com.ExecuteNonQuery();
                result = (int?)com.Parameters["@ReturnValue"].Value;
                con.Close();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return result;

        }

        // To view UserInfo
        public Users GetInfo(int? userid, string email)
        {
            Users user = new Users();
            if (userid != null || email!=null)
            {
                try
                {
                    connection();
                    SqlCommand com = new SqlCommand("dbo.GetUserInfo", con);
                    com.CommandType = CommandType.StoredProcedure;
                    if (userid == null)
                        com.Parameters.AddWithValue("@UserId", SqlDbType.Int).Value = DBNull.Value;
                    else
                        com.Parameters.AddWithValue("@UserId", userid);
                    if (email == null)
                        com.Parameters.AddWithValue("@Email", SqlDbType.NVarChar).Value = DBNull.Value;
                    else
                        com.Parameters.AddWithValue("@Email", email);


                    com.Parameters["@Email"].IsNullable = true;
                    com.Parameters["@UserId"].IsNullable = true;

                    con.Open();
                    SqlDataReader rd = com.ExecuteReader();
                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            user.Surname = (string)rd["Surname"];
                            user.Name = (string)rd["Name"];
                            user.Email = (string)rd["Email"];
                            user.Mobile = (string)rd["Mobile"];
                            user.CryptedPassword = (string)rd["CryptedPassword"];
                            user.PasswordSalt = (string)rd["PasswordSalt"];
                            user.UserId = (int)rd["UserId"];
                        }
                    }
                   con.Close();
                    

                }
                catch (Exception ex)
                {

                    logger.Error(ex);
                }
              
            }
            return user;


        }
        //To Update User details    
        public int UpdateUser(Users user)
        {
            try
            {
                connection();
                SqlCommand com = new SqlCommand("dbo.UpdateUser", con);

                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UserId", user.UserId);
                com.Parameters.AddWithValue("@Name", user.Name);
                com.Parameters.AddWithValue("@Surname", user.Surname);
                com.Parameters.AddWithValue("@Email", user.Email);
                com.Parameters.AddWithValue("@Mobile", user.Mobile);
                con.Open();
                
                return com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                
               logger.Error(ex);
                return 0;
            }
        }
        //to delete user
        public bool DeleteUser(int? id)
        {
            if (id != null)
            {
                connection();
                SqlCommand com = new SqlCommand("dbo.DeleteUser", con);

                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@UserId", id);

                con.Open();
                int i = com.ExecuteNonQuery();
                con.Close();
                if (i >= 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else return false;

        }
    }
}