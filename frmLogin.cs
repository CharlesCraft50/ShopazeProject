using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopaze
{
    public partial class frmLogin : Form
    {
        public User user;
        Home home;
        public static frmLogin loginForm;
        public frmLogin()
        {
            InitializeComponent();
            user = new User();
            loginForm = this;
        }

        

        private void button1_Click(object sender, EventArgs e)
        {
            try 
            { 
                if(user.VerifyLogin(txtUsername.Text, txtPassword.Text))
                {
                    user.SetUser(txtUsername.Text);
                    home = new Home(this);
                    home.Show();
                } 
                else
                {
                    MessageBox.Show("Incorrect Username or Password!");
                }
            } 
            catch(Exception ex)
            {
                MessageBox.Show("Unkown error occured! Please try again later. " + ex.Message);
            }
        }

        private void cbShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowPassword.Checked)
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

        private void linkLabel1_LinkClicked(object sender, EventArgs e)
        {
            frmRegister register = new frmRegister(this);
            register.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmForgotPassword forgotPassword = new frmForgotPassword();
            forgotPassword.ShowDialog();
        }
    }
}
