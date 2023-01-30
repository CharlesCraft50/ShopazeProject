using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopaze
{
    public partial class frmRegister : Form
    {
        frmLogin CallFrom;
        User user = new User();
        public frmRegister(frmLogin viaParameter)
        {
            InitializeComponent();
            CallFrom = viaParameter;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CallFrom.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(validateInfo(txtEmail.Text, txtUsername.Text, txtPassword.Text))
                {
                    var checkUser = user.CheckUser(txtEmail.Text, txtUsername.Text);
                    if (checkUser)
                    {
                        try
                        {
                            if(user.RegisterUser(txtEmail.Text, txtUsername.Text, txtPassword.Text, "user"))
                            {
                                MessageBox.Show("You are now registered!");
                                CallFrom.Show();
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show("Unknown Error occured! Please try again later.");
                            }
                        } catch(Exception ex)
                        {
                            MessageBox.Show("Unknown Error occured! Please try again later. " + ex.Message);
                        }
                    }
                }
            } 
            catch (InvalidEmailException ex)
            {
                MessageBox.Show(ex.Message);
            } 
            catch (InvalidUsernameException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (InvalidPasswordException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        public bool validateInfo(string email, string username, string password)
        {
            bool isValid = false;
            bool isEmailValid = false;
            if(email == "")
            {
                throw new InvalidEmailException("Email must not be empty!");
            }

            if(username == "")
            {
                throw new InvalidUsernameException("Username must not be empty!");
            }

            MailAddress address = new MailAddress(email);
            isEmailValid = (address.Address == email);
            Regex isUsernameValid = new Regex(@"^[a-zA-Z][a-zA-Z0-9]{5,11}$");

            if (!isEmailValid)
            {
                throw new InvalidEmailException("Email is not valid!");
            }

            if(!isUsernameValid.IsMatch(username))
            {
                throw new InvalidUsernameException("Username is not valid!");
            }

            if(password == "")
            {
                throw new InvalidPasswordException("Password must not be empty!");
            }
            else if(password.Length < 6)
            {
                throw new InvalidPasswordException("Password must be 6 characters long.");
            }

            if(isEmailValid && isUsernameValid.IsMatch(username) && password != "")
            {
                isValid = true;
            }

            return isValid;
        }

        private void frmRegister_Load(object sender, EventArgs e)
        {
            CallFrom.Hide();
        }

        private void txtUsername_KeyUp(object sender, KeyEventArgs e)
        {
            Regex isUsernameValid = new Regex(@"^[a-zA-Z][a-zA-Z0-9]{5,11}$");
            string txt = "6-12 characters long, no special chararcters and spaces.";

            if (txtUsername.Text == "")
            {
                txt = "6-12 characters long, no special chararcters and spaces.";
            }

            if (!isUsernameValid.IsMatch(txtUsername.Text))
            {
                if (txtUsername.Text.Length > 5 && txtUsername.Text.Length < 13)
                {
                    txt = "No special chararcters and spaces.";
                }
                else
                {
                    txt = "6-12 characters long, no special chararcters and spaces.";
                }
            }
            else
            {
                if (txtUsername.Text.Length > 5 && txtUsername.Text.Length < 13)
                {
                    txt = "";
                }
                else
                {
                    txt = "6-12 characters long.";
                }
            }

            lblUsernameValid.Text = txt;
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if(cbShowPassword.Checked)
            {
                cbShowPassword.ImageIndex = 1;
                txtPassword.UseSystemPasswordChar = false;
            }
            else
            {
                cbShowPassword.ImageIndex = 0;
                txtPassword.UseSystemPasswordChar = true;
            }
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if(txtPassword.Text == "")
            {
                lblPasswordValid.Text = "Password must be 6 characters long.";
            }
            else if(txtPassword.Text.Length < 6)
            {
                lblPasswordValid.Text = "Your password must be at least 6 characters long.";
            }
            else
            {
                lblPasswordValid.Text = "";
            }
        }
    }

    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(string email) : base(email)
        {

        }
    }

    public class InvalidUsernameException : Exception
    {
        public InvalidUsernameException(string username) : base(username)
        {

        }
    }

    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string password) : base(password)
        {

        }
    }
}
