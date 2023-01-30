using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;

namespace Shopaze
{
    public partial class frmCart : Form
    {
        public static Form CallFrom;
        public static Dictionary<string, double> amountMap = new Dictionary<string, double>();
        public static Label lblTotalAmount;
        public static FlowLayoutPanel flowLayoutPanel;
        public frmCart(Form viaParameter)
        {
            InitializeComponent();
            CallFrom = viaParameter;
            lblTotalAmount = lblTotal;
            flowLayoutPanel = flowLayoutPanel1;
        }

        public void addItem(ListItem listItem)
        {
            if(Home.cartMap.ContainsKey(listItem.ProductCode))
            {
                //listItem.ItemQuantity = listItem.ItemQuantity + 1;
                //Home.cartMap[listItem.Code] = listItem;

                /*
                foreach (CartItem c in flowLayoutPanel1.Controls.OfType<CartItem>())
                {
                    if (c.Code == listItem.Code)
                    {
                        c.ItemQuantity = c.ItemQuantity + 1;
                    }
                }*/
            }
            else
            {
                Home.cartMap.Add(listItem.ProductCode, listItem);
            }
            
            
        }

        public void populateItem()
        {
            Home.cartItem = new CartItem[Home.cartMap.Count];
            int countIsSelected = 0;
            int countItem = 0;
            foreach (var x in Home.cartMap.Select((Entry, Index) => new { Entry, Index }))
            {
                int i = x.Index;
                countItem += 1;
                ListItem obj = x.Entry.Value;
                if (obj.IsSelected)
                {
                    countIsSelected += 1;
                }
                Home.cartItem[i] = new CartItem();
                Home.cartItem[i].Picture = obj.Picture;
                Home.cartItem[i].Title = obj.Title;
                Home.cartItem[i].Category = obj.Category;
                Home.cartItem[i].Price = obj.Price;
                Home.cartItem[i].Quantity = obj.Quantity;
                Home.cartItem[i].ProductCode = obj.ProductCode;
                Home.cartItem[i].Description = obj.Description;
                Home.cartItem[i].FromAddress = obj.FromAddress;
                Home.cartItem[i].ItemQuantity = obj.ItemQuantity;
                Home.cartItem[i].IsSelected = obj.IsSelected;
                flowLayoutPanel1.Controls.Add(Home.cartItem[i]);
            }

            if(countItem == countIsSelected)
            {
                btnSelectAll.Checked = true;
                isCheckedAll = false;
            }
            
        }

        private void frmCart_Load(object sender, EventArgs e)
        {
            populateItem();
            lblTotalAmount.Text = (frmCart.amountMap.Sum(x => x.Value)).ToString();
        }

        private void frmCart_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                foreach (CartItem c in flowLayoutPanel1.Controls.OfType<CartItem>())
                {
                    ListItem obj = new ListItem();
                    obj.Picture = c.Picture;
                    obj.Title = c.Title;
                    obj.Category = c.Category;
                    obj.Price = c.Price;
                    obj.Quantity = c.Quantity;
                    obj.ProductCode = c.ProductCode;
                    obj.Description = c.Description;
                    obj.FromAddress = c.FromAddress;
                    obj.ItemQuantity = c.ItemQuantity;
                    obj.IsSelected = c.IsSelected;
                    Home.cartMap[c.ProductCode] = obj;
                }

                if(CallFrom is ProductPage)
                {
                    CallFrom.Close();
                }
            }
        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            if(Convert.ToDouble(lblTotal.Text) != 0)
            {
                foreach (CartItem c in flowLayoutPanel1.Controls.OfType<CartItem>())
                {
                    ListItem obj = new ListItem();
                    obj.Picture = c.Picture;
                    obj.Title = c.Title;
                    obj.Category = c.Category;
                    obj.Price = c.Price;
                    obj.Quantity = c.Quantity;
                    obj.ProductCode = c.ProductCode;
                    obj.Description = c.Description;
                    obj.FromAddress = c.FromAddress;
                    obj.ItemQuantity = c.ItemQuantity;
                    obj.IsSelected = c.IsSelected;
                    Home.cartMap[c.ProductCode] = obj;
                }

                frmCheckout checkout = new frmCheckout(this, frmCart.amountMap.Sum(x => x.Value));
                checkout.ShowDialog();
            }
            else 
            {
                MessageBox.Show("Cart should not be empty!");
            }
        }

        private void txtMoneyAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        bool isCheckedAll = false;
        private void btnSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            isCheckedAll = btnSelectAll.Checked;
            if(!isCheckedAll)
            {
                foreach (CartItem c in flowLayoutPanel1.Controls.OfType<CartItem>())
                {
                    c.IsSelected = false;
                    c._select = false;
                }
            }
            else
            {
                
                foreach (CartItem c in flowLayoutPanel1.Controls.OfType<CartItem>())
                {
                    c.IsSelected = true;
                    c._select = true;
                }
            }

        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            if(btnSelectAll.Checked && !isCheckedAll)
            {
                btnSelectAll.Checked = false;
            }
            else
            {
                btnSelectAll.Checked = true;
                isCheckedAll = false;
            }
        }

        private void lblTotal_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void flowLayoutPanel1_Layout(object sender, LayoutEventArgs e)
        {
            flowLayoutPanel1.SuspendLayout();
            foreach (CartItem c in flowLayoutPanel1.Controls.OfType<CartItem>())
            {
                if (c is CartItem) c.Width = flowLayoutPanel1.ClientSize.Width-10;
            }
            flowLayoutPanel1.ResumeLayout();
        }
    }
}
