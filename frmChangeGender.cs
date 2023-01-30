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
    public partial class frmChangeGender : Form
    {
        User user;
        public frmChangeGender()
        {
            InitializeComponent();
            user = frmLogin.loginForm.user;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(cbGender.Text == "")
            {
                MessageBox.Show("Please select gender!");
            }
            else
            {
                if (cbGender == null || cbGender.SelectedIndex < 0)
                {
                    MessageBox.Show("Invalid gender!");
                }
                else
                {
                    if (user.UpdateGender(user.UserId, cbGender.Text))
                    {
                        MessageBox.Show("Gender set!");
                        this.Close();
                    }
                }
                
            }
        }
    }
}
