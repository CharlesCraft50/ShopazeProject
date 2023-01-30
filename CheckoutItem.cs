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
    public partial class CheckoutItem : UserControl
    {
        public CheckoutItem()
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
        private double _totalamount;

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
            set { _quantity = value; lblQuantity.Text = value + " left in stock"; }
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
            set { _itemquantity = value; lblItemQuantity.Text = "Qty: " + value.ToString(); string items = (value == 1) ? value + " item, Subtotal: " : value + " items, Subtotal: "; lblItems.Text = items; lblAmountTotal.Text = "₱" + (Convert.ToDouble(Price) * value).ToString(); }
        }

        public double TotalAmount
        {
            get { return _totalamount; }
            set { _totalamount = value; }
        }


    }
}
