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
    public partial class MyOrdersControl : UserControl
    {
        public static MyOrdersControl myOrdersControl;
        public MyOrdersControl()
        {
            InitializeComponent();
            myOrdersControl = this;

        }

        public void populateItems(string deliverystatus)
        {
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                flowLayoutPanel1.Controls.Clear();
            }
            Orders orders = new Orders();
            orders.SelectOrders("", "", frmLogin.loginForm.user.UserId, flowLayoutPanel1, deliverystatus);
        }

        private void MyOrdersControl_Load(object sender, EventArgs e)
        {
            populateItems("Unfulfilled");
        }

        private void flowLayoutPanel1_Layout(object sender, LayoutEventArgs e)
        {
            flowLayoutPanel1.SuspendLayout();
            foreach (OrderItem c in flowLayoutPanel1.Controls.OfType<OrderItem>())
            {
                if (c is OrderItem) c.Width = flowLayoutPanel1.ClientSize.Width - 25;
            }
            flowLayoutPanel1.ResumeLayout();
        }
    }
}
