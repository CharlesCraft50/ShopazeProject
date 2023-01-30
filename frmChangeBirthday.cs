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
    public partial class frmChangeBirthday : Form
    {
        User user;
        public frmChangeBirthday()
        {
            InitializeComponent();
            user = frmLogin.loginForm.user;
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if(dateTimePicker1.Value.ToString() == "")
            {
                MessageBox.Show("Please select a date!");
                dateTimePicker1.Focus();
                return;
            }
            else
            {
                if(user.UpdateBirthday(user.UserId, dateTimePicker1.Value.ToShortDateString()))
                {
                    MessageBox.Show("Birthday set!");
                    this.Close();
                }
            }
        }
    }
}
