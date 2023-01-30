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
    public partial class Home : Form
    {
        Form CallFrom;
        Products prdt;
        public static Dictionary<string, ListItem> cartMap = new Dictionary<string, ListItem>();
        public static CartItem[] cartItem;
        public static frmCart cart;
        public static Home home;
        string category;
        string searchBar;
        int lastHeight = 0;
        int lastTop = 0;
        public Home(Form viaParameter)
        {
            InitializeComponent();
            CallFrom = viaParameter;
            CallFrom.Hide();
            home = this;
        }

        public void setPageNumber(string num)
        {
            txtPageNumber.Text = num;
        }

        public void selectCategory(string val)
        {
            cbCategory.SelectedIndex = cbCategory.FindString(val);
        }

        public void populateItems()
        {
            if (flowLayoutPanel1.Controls.Count > 0)
            {
                flowLayoutPanel1.Controls.Clear();
            }

            prdt = new Products();
            if (txtSearchBar.Text == "")
            {
                searchBar = "";
            }
            else
            {
                searchBar = txtSearchBar.Text;
            }
            if(cbCategory.SelectedItem != null)
            {
                if (cbCategory.SelectedItem.ToString() == "Everything Else")
                {
                    category = "";
                }
                else
                {
                    category = cbCategory.SelectedItem.ToString();
                }
            }
            else
            {
                cbCategory.SelectedIndex = 0;
            }
            

            prdt.SearchProduct(searchBar, category, flowLayoutPanel1, cbOrderBy.SelectedItem.ToString(), Convert.ToInt32(txtPageNumber.Text), 6);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtPageNumber.Text = "1";
            populateItems();
        }

        private void Home_Load(object sender, EventArgs e)
        {
            populateItems();
            lblWelcomeUser.Text = "Welcome " + frmLogin.loginForm.user.Username + "!";
        }

        private void cbOrderBy_TextChanged(object sender, EventArgs e)
        {
            txtPageNumber.Text = "1";
            populateItems();
        }

        private void cbCategory_TextChanged(object sender, EventArgs e)
        {
            txtPageNumber.Text = "1";
            populateItems();
        }

        private void btnCart_Click(object sender, EventArgs e)
        {
            selectPanel.Height = btnCart.Height;
            selectPanel.Top = btnCart.Top;
            cart = new frmCart(this);
            cart.ShowDialog();
            selectPanel.Height = lastHeight;
            selectPanel.Top = lastTop;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            searchPanel1.Visible = true;
            searchPanel2.Visible = true;
            accountDetailsPanel.Visible = false;
            myOrdersTabsPanel.Visible = false;
            selectPanel.Height = btnHome.Height;
            lastHeight = btnHome.Height;
            selectPanel.Top = btnHome.Top;
            lastTop = btnHome.Top;
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(flowLayoutPanel1);
            flowLayoutPanel1.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            mainPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
        }

        private void btnMyOrders_Click(object sender, EventArgs e)
        {
            searchPanel1.Visible = false;
            searchPanel2.Visible = false;
            accountDetailsPanel.Visible = false;
            myOrdersTabsPanel.Visible = true;
            selectPanel.Height = btnMyOrders.Height;
            lastHeight = btnMyOrders.Height;
            selectPanel.Top = btnMyOrders.Top;
            selectPanel.Top = btnMyOrders.Top;
            mainPanel.Controls.Clear();
            MyOrdersControl myOrders = new MyOrdersControl();

            mainPanel.Controls.Add(myOrders);
            myOrders.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            myOrders.Dock = DockStyle.Fill;
            mainPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
        }

        private void btnAccount_Click(object sender, EventArgs e)
        {
            searchPanel1.Visible = false;
            searchPanel2.Visible = false;
            accountDetailsPanel.Visible = true;
            myOrdersTabsPanel.Visible = false;
            selectPanel.Height = btnAccount.Height;
            lastHeight = btnAccount.Height;
            selectPanel.Top = btnAccount.Top;
            selectPanel.Top = btnAccount.Top;
            mainPanel.Controls.Clear();
            MyAccountControl myAccount = new MyAccountControl();

            mainPanel.Controls.Add(myAccount);
            myAccount.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            myAccount.Dock = DockStyle.Fill;
            mainPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Close();
        }

        private void flowLayoutPanel1_Layout(object sender, LayoutEventArgs e)
        {
            
            flowLayoutPanel1.SuspendLayout();
            foreach (FlowLayoutPanel c in flowLayoutPanel1.Controls.OfType<FlowLayoutPanel>())
            {
                if (c is FlowLayoutPanel) c.Width = flowLayoutPanel1.ClientSize.Width - 25;
                c.Dock = DockStyle.Bottom;
            }
            flowLayoutPanel1.ResumeLayout();
        }

        public string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        private void btnToShip_Click(object sender, EventArgs e)
        {
            MyOrdersControl.myOrdersControl.populateItems("Unfulfilled");
        }

        private void btnToReceive_Click(object sender, EventArgs e)
        {
            MyOrdersControl.myOrdersControl.populateItems("Arrived");
        }

        private void btnReceived_Click(object sender, EventArgs e)
        {
            MyOrdersControl.myOrdersControl.populateItems("Received");
        }

        private void btnCancelled_Click(object sender, EventArgs e)
        {
            MyOrdersControl.myOrdersControl.populateItems("Returning");
        }

        private void btnAddProduct_Click(object sender, EventArgs e)
        {
            AddProducts addProducts = new AddProducts();
            addProducts.ShowDialog();
        }
    }
}
