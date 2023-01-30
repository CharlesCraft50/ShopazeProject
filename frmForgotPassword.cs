using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopaze
{
    public partial class frmForgotPassword : Form
    {
        private System.Windows.Forms.Timer timer1;
        private int counter = 60;
        string messageBody = "";
        int confirmationCode = 0;
        User user;
        string actionRequest;
        string[] account;
        string UserId, Email, Username, FirstName, LastName = ""; 

        public frmForgotPassword()
        {
            InitializeComponent();
            user = new User();
            
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            account = user.ResetPassword(txtUsername.Text);
            if (account[0] != null)
            {
                UserId = account[0];
                Email = account[1];
                Username = account[2];
                FirstName = account[3];
                LastName = account[4];

                counter = 60;
                confirmationCode = GenerateRandomNo();
                txtConfirmationCode.Text = confirmationCode.ToString();
                actionRequest = "Reset Shopaze Password";
                sendMail(Email, Username);
            }
            else
            {
                MessageBox.Show("Account does not exist!");
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            counter--;
            lblEmailVerification.Text = "Resend The Code in " + counter.ToString() + "s";

            if (counter == 0)
            {
                timer1.Stop();
                emailVerificationPanel.Enabled = true;
                lblEmailVerification.Enabled = true;
                lblEmailVerification.Text = "Resend The Code";
            }
        }

        private void emailVerificationPanel_Click(object sender, EventArgs e)
        {
            counter = 60;
            confirmationCode = GenerateRandomNo();
            txtConfirmationCode.Text = confirmationCode.ToString();
            sendMail(Email, Username);
        }

        public void sendMail(string email, string username)
        {
            string usernameToUse = (FirstName != null && LastName != null) ? FirstName + " " + LastName + "," : Username + ",";
            messageBody = @"<head> <style type='text/css'> body {font-family: Lucida Console; margin: 0; } pre { font-size: 16px;  } </style> </head> <body> <div style='padding: 30px; border: 1px solid #99b4d1; text-align: center; background-color: #99b4d1;'><img src='https://i.imgur.com/p6LPpUw.png' /></div><div style='text-align: center; padding: 10px;'><h1>Verify your email</h1><div style='display: inline-block; text-align: left;'><p>" + username + "</p><pre style='color: red;'>DO NOT SHARE THIS WITH ANYONE!!!</pre><pre>You are receiving this message following your request to <b>" + actionRequest + ".</b></pre><pre>If you made this request, please enter the 4 digit code on the Email Verification Page:</pre><input type='text' style='font-size: 30px; padding: 10px; text-align: center; display: block; margin-left: auto; margin-right: auto;' id='yourCode' value='" + confirmationCode + "' readonly><pre style='color: red;'>Do not share this code to anyone under any circumstances!</pre><pre>if you do not request to <b>" + actionRequest + "</b>, you may ignore this email.</pre></div></div><div style='text-align: center;'><br><br><img src='https://i.imgur.com/c1VL8eq.png'></div><br><br><br><br></body>";
            if (EmailNow(messageBody, email, username))
            {
                MessageBox.Show("Email has been sent to: " + Regex.Replace(email, @"(?<=^[A-Za-z0-9]{2}).*?(?=@)", m => new string('*', m.Length)) + "\nPlease check your inbox!");
                verifyCodePanel.Visible = true;
                lblEmailVerification.Text = "Resend The Code in 60s";
                emailVerificationPanel.Enabled = false;
                lblEmailVerification.Enabled = false;
                lblEmail.Text = Regex.Replace(email, @"(?<=^[A-Za-z0-9]{2}).*?(?=@)", m => new string('*', m.Length));

                timer1 = new System.Windows.Forms.Timer();
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Interval = 1000;
                timer1.Start();
                lblEmailVerification.Text = "Resend The Code in " + counter.ToString() + "s";
            }
        }

        public int GenerateRandomNo()
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            return _rdm.Next(_min, _max);
        }

        public bool EmailNow(string htmlString, string emailTo, string usernameTo)
        {
            bool verify = false;
            var fromAddress = new MailAddress("shopazeofficial@gmail.com", "Shopaze");
            var toAddress = new MailAddress(emailTo, usernameTo);
            const string fromPassword = "gcobihviyjwctsdx";
            string subject = "Shopaze - " + actionRequest;
            string body = htmlString;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                IsBodyHtml = true,
                Body = body
            })
            {
                try
                {
                    smtp.Send(message);
                    verify = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unknown error occured! " + ex.Message);
                }
            }

            return verify;
        }

        private void btnVerifyCode_Click(object sender, EventArgs e)
        {
            if (txtConfirmationCodeProcess.Text == txtConfirmationCode.Text)
            {
                frmChangePassword changePassword = new frmChangePassword(this, UserId);
                this.Close();
                changePassword.ShowDialog();
            }
            else
            {
                MessageBox.Show("Wrong code!");
            }
        }
    }
}
