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
    public partial class frmChangeEmail : Form
    {
        private System.Windows.Forms.Timer timer1;
        private int counter = 60;
        string messageBody = "";
        int confirmationCode = 0;
        User user;
        Form CallFrom;
        string actionRequest;
        public frmChangeEmail(Form viaParameter)
        {
            InitializeComponent();
            CallFrom = viaParameter;
            user = frmLogin.loginForm.user;
        }
        private void frmChangeEmail_Load(object sender, EventArgs e)
        {
            CallFrom.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if(validateInfo(txtEmail.Text))
                {
                    if (user.CheckEmail(txtEmail.Text))
                    {
                        MessageBox.Show("Account already exists!");
                    }
                    else
                    {
                        counter = 60;
                        confirmationCode = GenerateRandomNo();
                        txtConfirmationCode.Text = confirmationCode.ToString();
                        actionRequest = "Change Shopaze Email";
                        sendMail(txtEmail.Text, user.Username);
                    }
                }
            }
            catch (InvalidEmailException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool validateInfo(string email)
        {
            bool isEmailValid = false;
            MailAddress address = new MailAddress(email);
            isEmailValid = (address.Address == email);

            if (!isEmailValid)
            {
                throw new InvalidEmailException("Email is not valid!");
            }
            else
            {
                isEmailValid = true;
            }

            return isEmailValid;
        }

        private void emailVerificationPanel_Click(object sender, EventArgs e)
        {
            counter = 60;
            confirmationCode = GenerateRandomNo();
            txtConfirmationCode.Text = confirmationCode.ToString();
            sendMail(txtEmail.Text, user.Username);
        }

        public void sendMail(string email, string username)
        {
            string usernameToUse = (user.FirstName != null && user.LastName != null) ? user.FirstName + " " + user.LastName + "," : user.Username + ",";
            messageBody = @"<head> <style type='text/css'> body {font-family: Lucida Console; margin: 0; } pre { font-size: 16px;  } </style> </head> <body> <div style='padding: 30px; border: 1px solid #99b4d1; text-align: center; background-color: #99b4d1;'><img src='https://i.imgur.com/p6LPpUw.png' /></div><div style='text-align: center; padding: 10px;'><h1>Verify your email</h1><div style='display: inline-block; text-align: left;'><p>" + username + "</p><pre style='color: red;'>DO NOT SHARE THIS WITH ANYONE!!!</pre><pre>You are receiving this message following your request to <b>" + actionRequest + ".</b></pre><pre>If you made this request, please enter the 4 digit code on the Email Verification Page:</pre><input type='text' style='font-size: 30px; padding: 10px; text-align: center; display: block; margin-left: auto; margin-right: auto;' id='yourCode' value='" + confirmationCode + "' readonly><pre style='color: red;'>Do not share this code to anyone under any circumstances!</pre><pre>if you do not request to <b>" + actionRequest + "</b>, you may ignore this email.</pre></div></div><div style='text-align: center;'><br><br><img src='https://i.imgur.com/c1VL8eq.png'></div><br><br><br><br></body>";
            if (EmailNow(messageBody, email, username))
            {
                MessageBox.Show("Email has been sent to: " + email + "\nPlease check your inbox!");
                verifyCodePanel.Visible = true;
                lblEmailVerification.Text = "Resend The Code in 60s";
                emailVerificationPanel.Enabled = false;
                lblEmailVerification.Enabled = false;
                btnSubmit.Enabled = false;
                btnSubmit.Text = "Submit (60s)";
                lblEmail.Text = email;
                btnForward.Visible = true;

                timer1 = new System.Windows.Forms.Timer();
                timer1.Tick += new EventHandler(timer1_Tick);
                timer1.Interval = 1000;
                timer1.Start();
                lblEmailVerification.Text = "Resend The Code in " + counter.ToString() + "s";
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            counter--;
            lblEmailVerification.Text = "Resend The Code in " + counter.ToString() + "s";
            btnSubmit.Text = "Submit (" + counter.ToString() + "s)";
            if (counter == 0)
            {
                timer1.Stop();
                emailVerificationPanel.Enabled = true;
                lblEmailVerification.Enabled = true;
                btnSubmit.Enabled = true;
                lblEmailVerification.Text = "Resend The Code";
                btnSubmit.Text = "Submit";
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
            const string subject = "Shopaze - Email Verification";
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
                if (user.UpdateEmail(user.UserId, txtEmail.Text))
                {
                    MessageBox.Show("Email Succesfully change!");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Wrong code!");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            verifyCodePanel.Visible = false;
        }

        private void btnForward_Click(object sender, EventArgs e)
        {
            verifyCodePanel.Visible = true;
        }
    }
}
