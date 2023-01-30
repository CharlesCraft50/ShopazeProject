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
    public partial class OrderItem : UserControl
    {
        public OrderItem()
        {
            InitializeComponent();
        }

        private Image _picture;
        private string _title;
        private string _category;
        private string _price;
        private string _quantity;
        private string _productcode;
        private string _orderid;
        private string _description;
        private string _fromaddress;
        private int _itemquantity;
        private double _totalamount;
        private string _status;
        private string _deliverystatus;

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
            set { _quantity = value; lblQuantity.Text = value; }
        }

        public string ProductCode
        {
            get { return _productcode; }
            set { _productcode = value; txtProductCode.Text = value; }
        }

        public string OrderId
        {
            get { return _orderid; }
            set { _orderid = value; txtOrderId.Text = value; }
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
            set { _itemquantity = value; lblItemQuantity.Text = "Qty: " + value.ToString(); string items = (value == 1) ?  "Total(" + value + " item): " : "Total(" + value + " items): "; lblItems.Text = items; lblAmountTotal.Text = "₱" + (Convert.ToDouble(Price) * value).ToString(); }
        }

        public double TotalAmount
        {
            get { return _totalamount; }
            set { _totalamount = value; }
        }

        public string Status
        {
            get { return _status; }
            set { _status = value; lblStatus.Text = value; if (value == "Completed") { lblStatus.ForeColor = Color.DarkOliveGreen; } else if (value == "Shipping") { lblStatus.ForeColor = Color.DarkOrange; } else if (value == "Cancelled") { lblStatus.ForeColor = Color.DimGray; btnCancel.Visible = false; btnDelete.Visible = true; } }
        }

        public string DeliveryStatus
        {
            get { return _deliverystatus; }
            set { _deliverystatus = value; lblStatus.Text = value; if (value == "Shipping") { lblStatus.ForeColor = Color.SteelBlue; } }
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            Orders orders = new Orders();
            orders.DeleteOrder(_orderid);
            MyOrdersControl.myOrdersControl.populateItems("Returning");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure?", "Do you want cancel order?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                Orders orders = new Orders();
                if (orders.CancelOrder(_orderid, _itemquantity, _productcode))
                {
                    MessageBox.Show("Order cancelled!");
                    MyOrdersControl.myOrdersControl.populateItems("Returning");
                    Home.home.populateItems();
                }
                else
                {
                    MessageBox.Show("Unknown error occured! Please try again later.");
                }
            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }
    }
}
