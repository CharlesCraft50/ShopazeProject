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
    public partial class ListItem : UserControl
    {
        ProductPage page;
        public ListItem()
        {
            InitializeComponent();
            pictureBox.Controls.Add(pbSoldOut);
            pbSoldOut.Location = new Point(0, 0);
            pbSoldOut.BackColor = Color.Transparent;
        }

        private Image _picture;
        private string _title;
        private string _category;
        private string _price;
        private string _quantity;
        private string _productcode;
        private string _description;
        private string _fromaddress;
        private int _itemquantity = 1;
        private bool _select;

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
            set { _quantity = value; lblQuantity.Text = value + " left in stock"; if (Convert.ToInt32(value) <= 0) { pbSoldOut.Visible = true; lblTitle.Enabled = false; pictureBox.Enabled = false; } }
        }

        public string ProductCode
        {
            get { return _productcode; }
            set { _productcode = value; txtProductCode.Text = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; lblDescription.Text = value; }
        }

        public string FromAddress
        {
            get { return _fromaddress; }
            set { _fromaddress = value; lblFromAddress.Text = value; }
        }

        public int ItemQuantity
        {
            get { return _itemquantity; }
            set { _itemquantity = value; }
        }

        public bool IsSelected
        {
            get { return _select; }
            set { _select = value; }
        }

        public bool SoldOutVisible
        {
            get { return pbSoldOut.Visible; }
            set { pbSoldOut.Visible = value; }
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {
            page = new ProductPage(this);
            page.ShowDialog();

        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            page = new ProductPage(this);
            page.ShowDialog();
        }

        private void lblCategory_Click(object sender, EventArgs e)
        {
            Home.home.selectCategory(Category);
        }

        private void lblCategory_MouseEnter(object sender, EventArgs e)
        {
            lblCategory.Font = new Font(lblCategory.Font.Name, lblCategory.Font.SizeInPoints, FontStyle.Underline);
        }

        private void lblCategory_MouseLeave(object sender, EventArgs e)
        {
            lblCategory.Font = new Font(lblCategory.Font.Name, lblCategory.Font.SizeInPoints, FontStyle.Regular);
        }

        private void ListItem_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.Snow;
        }

        private void ListItem_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void lblTitle_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.Snow;
        }

        private void lblTitle_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.Snow;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void panel3_MouseEnter(object sender, EventArgs e)
        {
            this.BackColor = Color.Snow;
        }

        private void panel3_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }
    }
}
