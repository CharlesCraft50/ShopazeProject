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
    public partial class CartItem : UserControl
    {
        public CartItem()
        {
            InitializeComponent();
            lblQuantity.BackColor = Color.FromArgb(128, 0, 0, 0);
        }

        private Image _picture;
        private string _title;
        private string _category;
        private string _price;
        private string _quantity;
        private string _productcode;
        private string _description;
        private string _fromaddress;
        private int _itemquantity;
        public bool _select;

        public Image Picture
        {
            get { return _picture; }
            set { _picture = value; pictureBox.Image = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; lblTitle.Text = value; }
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; lblCategory.Text = value; }
        }

        public string Price
        {
            get { return _price; }
            set { _price = value; lblPrice.Text = value; }
        }

        public string Quantity
        {
            get { return _quantity; }
            set { _quantity = value; lblQuantity.Text = value + " left in stock"; txtItemQuantity.Maximum = Convert.ToInt32(value); }
        }

        public string ProductCode
        {
            get { return _productcode; }
            set { _productcode = value; txtProductCode.Text = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public string FromAddress
        {
            get { return _fromaddress; }
            set { _fromaddress = value; lblFromAddress.Text = value; }
        }

        public int ItemQuantity
        {
            get { return _itemquantity; }
            set { _itemquantity = value; txtItemQuantity.Value = value; }
        }

        public bool IsSelected
        {
            get { return btnSelect.Checked; }
            set { btnSelect.Checked = value; }
        }

        private void txtItemQuantity_ValueChanged(object sender, EventArgs e)
        {
            ItemQuantity = (int)txtItemQuantity.Value;
            frmCart.amountMap[ProductCode] = Convert.ToDouble(Price) * ItemQuantity;
            if (IsSelected)
            {
                frmCart.lblTotalAmount.Text = (frmCart.amountMap.Sum(x => x.Value)).ToString();
            }
            
        }

        private void btnSelect_CheckedChanged(object sender, EventArgs e)
        {
            IsSelected = !_select;
            if (IsSelected)
            {
                if (!frmCart.amountMap.ContainsKey(ProductCode))
                {
                    frmCart.amountMap.Add(ProductCode, Convert.ToDouble(Price) * ItemQuantity);
                }
            }
            else
            {
                if (frmCart.amountMap.ContainsKey(ProductCode))
                {
                    frmCart.amountMap.Remove(ProductCode);
                }
            }
            frmCart.lblTotalAmount.Text = (frmCart.amountMap.Sum(x => x.Value)).ToString();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if(_select)
            {
                IsSelected = false;
                _select = false;
            }
            else
            {
                IsSelected = true;
                _select = true;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            frmCart.amountMap.Remove(ProductCode);
            frmCart.lblTotalAmount.Text = (frmCart.amountMap.Sum(x => x.Value)).ToString();
            Home.cartMap.Remove(ProductCode);
            Button Delete = (Button)sender;
            UserControl UC = (UserControl)Delete.Parent;
            frmCart.flowLayoutPanel.Controls.Remove(UC);
            UC.Dispose(); 
        }

        private void CartItem_Load(object sender, EventArgs e)
        {
            if(btnSelect.Checked)
            {
                IsSelected = true;
                _select = true;
            }
        }
    }
}
