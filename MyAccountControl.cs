using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Shopaze
{
    public partial class MyAccountControl : UserControl
    {
        User user;
        public MyAccountControl()
        {
            InitializeComponent();
            user = frmLogin.loginForm.user;
        }

        private void MyAccountControl_Load(object sender, EventArgs e)
        {
            lblUsername.Text = user.Username;

            if (!(user.FirstName == null || user.LastName == null))
            {
                lblFullName.Text = user.FirstName + " " + user.LastName;
            }
            
            if(user.ContactNumber != null)
            {
                //lblMobile.Text = user.ContactNumber;
            }

            lblEmail.Text = Regex.Replace(user.Email, @"(?<=^[A-Za-z0-9]{2}).*?(?=@)", m => new string('*', m.Length));

            if(user.Gender != null)
            {
                lblGender.Text = user.Gender;
            }

            if(user.Birthday != null)
            {
                lblBirthday.Text = user.Birthday;
            }

            if(!String.IsNullOrWhiteSpace(user.AddressBook))
            {
                var addressBook = ParseJsonObject<frmChangeAddressBook.AddressBook>(user.AddressBook);
                lblAddressBook.Text = addressBook.StreetAddress;
            }
        }

        public static T ParseJsonObject<T>(string json) where T : class, new()
        {
            JObject jobject = JObject.Parse(json);
            return JsonConvert.DeserializeObject<T>(jobject.ToString());
        }

        private void flowLayoutPanel1_Layout(object sender, LayoutEventArgs e)
        {
            flowLayoutPanel1.SuspendLayout();
            foreach (Panel c in flowLayoutPanel1.Controls.OfType<Panel>())
            {
                if (c is Panel) c.Width = flowLayoutPanel1.ClientSize.Width - 25;
            }
            flowLayoutPanel1.ResumeLayout();
        }

        private void passwordPanel_Click(object sender, EventArgs e)
        {
            frmEditDetails editDetails = new frmEditDetails("Change Password", "Reset Shopaze Password");
            editDetails.ShowDialog();
        }

        private void fullNamePanel_Click(object sender, EventArgs e)
        {
            frmEditDetails editDetails = new frmEditDetails("Change Fullname", null);
            editDetails.ShowDialog();
            if (!(user.FirstName == null || user.LastName == null))
            {
                lblFullName.Text = user.FirstName + " " + user.LastName;
            }
        }

        private void changeEmailPanel_Click(object sender, EventArgs e)
        {
            frmEditDetails editDetails = new frmEditDetails("Change Email", "Change Shopaze Email");
            editDetails.ShowDialog();
            lblEmail.Text = Regex.Replace(user.Email, @"(?<=^[A-Za-z0-9]{2}).*?(?=@)", m => new string('*', m.Length));
        }

        private void genderPanel_Click(object sender, EventArgs e)
        {
            frmChangeGender changeGender = new frmChangeGender();
            changeGender.ShowDialog();
            if (user.Gender != null)
            {
                lblGender.Text = user.Gender;
            }
        }

        private void birthdayPanel_Click(object sender, EventArgs e)
        {
            frmChangeBirthday changeBirthday = new frmChangeBirthday();
            changeBirthday.ShowDialog();
            if (user.Birthday != null)
            {
                lblBirthday.Text = user.Birthday;
            }
        }

        private void addressBookPanel_Click(object sender, EventArgs e)
        {
            frmChangeAddressBook changeAddressBook = new frmChangeAddressBook();
            changeAddressBook.ShowDialog();
            if (!String.IsNullOrWhiteSpace(user.AddressBook))
            {
                var addressBook = ParseJsonObject<frmChangeAddressBook.AddressBook>(user.AddressBook);
                lblAddressBook.Text = addressBook.StreetAddress;
            }
        }
    }
}
