using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Text.Json;

namespace Shopaze
{
    public class User
    {
        private string _email;
        public static string myConn = String.Format(ConfigurationManager.ConnectionStrings["connString"].ConnectionString, Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + @"\Database1.mdf");
        public string UserId { get; set; }
        public string Email { get { return _email.ToLower(); } set { _email = value.ToLower(); } }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ContactNumber { get; set; }
        public string Gender { get; set; }
        public string Birthday { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressBook { get; set; }
        public string UserType { get; set; }
        public string CreationDate { get; set; }
        public string DeleteFlag { get; set; }

        bool isEmailNotExist, isUsernameNotExist, isValidEmailUsername = false;

        public bool VerifyLogin(string username, string password)
        {
            int rows;
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("SELECT COUNT(*) FROM Users WHERE (LOWER(username) = LOWER(@Username) OR LOWER(email) = LOWER(@Email)) AND password = @Password", con))
                {
                    com.Parameters.AddWithValue("Username", username);
                    com.Parameters.AddWithValue("Email", username);
                    com.Parameters.AddWithValue("Password", password);
                    rows = (int)com.ExecuteScalar();
                }
            }

            return (rows > 0) ? true : false;
        }

        public bool CheckUser(string email, string username)
        {
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("SELECT COUNT(*) FROM Users WHERE LOWER(email) = LOWER(@Email)", con))
                {
                    com.Parameters.AddWithValue("Email", email);
                    if ((int)com.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Email already exist!");
                    }
                    else
                    {
                        isEmailNotExist = true;
                    }

                    com.CommandText = "SELECT COUNT(*) FROM Users WHERE username = LOWER(@Username)";
                    com.Parameters.AddWithValue("Username", username);
                    if ((int)com.ExecuteScalar() > 0)
                    {
                        MessageBox.Show("Username already exist!");
                    }
                    else
                    {
                        isUsernameNotExist = true;
                    }
                }
            }

            if (isEmailNotExist && isUsernameNotExist)
            {
                isValidEmailUsername = true;
            }

            return isValidEmailUsername;
        }

        public string[] ResetPassword(string account)
        {
            string[] val = new string[5];
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("SELECT user_id, email, username, first_name, last_name FROM Users WHERE LOWER(email) = LOWER(@Email) OR LOWER(username) = LOWER(@Username)", con))
                {
                    com.Parameters.AddWithValue("Email", account);
                    com.Parameters.AddWithValue("Username", account);
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if(reader[0] != null && reader[1] != null && reader[2] != null)
                            {
                                val[0] = reader[0].ToString();
                                val[1] = reader[1].ToString();
                                val[2] = reader[2].ToString();
                                val[3] = reader[3].ToString();
                                val[4] = reader[4].ToString();
                            }
                            else
                            {
                                val[0] = null;
                                val[1] = null;
                                val[2] = null;
                                val[3] = null;
                                val[4] = null;
                            }
                        }
                    }
                }
            }

            return val;
        }

        public void SetUser(string username)
        {
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("SELECT * FROM Users WHERE LOWER(username) = LOWER(@Username) OR LOWER(email) = LOWER(@Email)", con))
                {
                    com.Parameters.AddWithValue("Username", username);
                    com.Parameters.AddWithValue("Email", username);
                    using (SqlDataReader reader = com.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserId = reader[0].ToString();
                            Email = reader[1].ToString();
                            Username = reader[2].ToString();
                            Password = reader[3].ToString();
                            ContactNumber = reader[4].ToString();
                            Gender = reader[5].ToString();
                            Birthday = reader[6].ToString();
                            FirstName = reader[7].ToString();
                            LastName = reader[8].ToString();
                            AddressBook = reader[9].ToString();
                            UserType = reader[10].ToString();
                            CreationDate = reader[11].ToString();
                            DeleteFlag = reader[12].ToString();
                        }
                    }
                }
            }
        }

        public bool RegisterUser(string email, string username, string password, string usertype)
        {
            int rows;

            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("INSERT INTO Users (email, username, password, user_type, creation_date, delete_flag) VALUES (@Email, @Username, @Password, @UserType, GETDATE(), 0)", con))
                {
                    com.Parameters.AddWithValue("Email", email);
                    com.Parameters.AddWithValue("Username", username);
                    com.Parameters.AddWithValue("Password", password);
                    com.Parameters.AddWithValue("UserType", usertype);

                    rows = com.ExecuteNonQuery();
                }
            }
            return (rows > 0) ? true : false;
        }

        public bool VerifyUser(string email)
        {
            int rows;
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("UPDATE Users SET user_type = 'user_verified' WHERE LOWER(email) = LOWER(@Email)", con))
                {
                    com.Parameters.AddWithValue("Email", email);
                    rows = com.ExecuteNonQuery();
                    UserType = "user_verified";
                }
            }

            return (rows > 0) ? true : false;
        }

        public bool UpdatePassword(string userid, string newpassword)
        {
            int rows;
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("UPDATE Users SET password = @Password WHERE user_id = @UserId", con))
                {
                    com.Parameters.AddWithValue("Password", newpassword);
                    com.Parameters.AddWithValue("UserId", userid);
                    rows = com.ExecuteNonQuery();
                    Password = newpassword;
                }
            }

            return (rows > 0) ? true : false;
        }

        public bool UpdateFullname(string userid, string firstname, string lastname)
        {
            int rows;
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("UPDATE Users SET first_name = @FirstName, last_name = @LastName WHERE user_id = @UserId", con))
                {
                    com.Parameters.AddWithValue("FirstName", firstname);
                    com.Parameters.AddWithValue("LastName", lastname);
                    com.Parameters.AddWithValue("UserId", userid);
                    
                    rows = com.ExecuteNonQuery();
                    FirstName = firstname;
                    LastName = lastname;
                }
            }

            return (rows > 0) ? true : false;
        }

        public bool CheckEmail(string email)
        {
            int rows;
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("SELECT COUNT(*) FROM Users WHERE LOWER(email) = LOWER(@Email)", con))
                {
                    com.Parameters.AddWithValue("Email", email);
                    rows = (int)com.ExecuteScalar();
                }
            }

            return (rows > 0) ? true : false;
        }

        public bool UpdateEmail(string userid, string email)
        {
            int rows;
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("UPDATE Users SET email = LOWER(@Email) WHERE user_id = @UserId", con))
                {
                    com.Parameters.AddWithValue("Email", email);
                    com.Parameters.AddWithValue("UserId", userid);
                    com.ExecuteNonQuery();

                    rows = com.ExecuteNonQuery();
                    Email = email;
                }
            }

            return (rows > 0) ? true : false;
        }

        public bool UpdateGender(string userid, string gender)
        {
            int rows;
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("UPDATE Users SET gender = @Gender WHERE user_id = @UserId", con))
                {
                    com.Parameters.AddWithValue("Gender", gender);
                    com.Parameters.AddWithValue("UserId", userid);
                    com.ExecuteNonQuery();

                    rows = com.ExecuteNonQuery();
                    Gender = gender;
                }
            }

            return (rows > 0) ? true : false;
        }

        public bool UpdateBirthday(string userid, string birthday)
        {
            int rows;
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("UPDATE Users SET birthday = @Birthday WHERE user_id = @UserId", con))
                {
                    com.Parameters.AddWithValue("Birthday", birthday);
                    com.Parameters.AddWithValue("UserId", userid);
                    com.ExecuteNonQuery();

                    rows = com.ExecuteNonQuery();
                    Birthday = birthday;
                }
            }

            return (rows > 0) ? true : false;
        }

        public bool UpdateAddressBook(string userid, string addressbook)
        {
            int rows;
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("UPDATE Users SET address_book = @AddressBook WHERE user_id = @UserId", con))
                {
                    com.Parameters.AddWithValue("AddressBook", addressbook);
                    com.Parameters.AddWithValue("UserId", userid);
                    com.ExecuteNonQuery();

                    rows = com.ExecuteNonQuery();
                    AddressBook = addressbook;
                }
            }

            return (rows > 0) ? true : false;
        }

    }
}
