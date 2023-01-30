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
    public partial class ProductPage : Form
    {
        ListItem CallFrom;
        public ProductPage(ListItem viaParameter)
        {
            InitializeComponent();
            CallFrom = viaParameter;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Home.cart = new frmCart(this);
            Home.cart.addItem(CallFrom);
            Home.cart.ShowDialog();
        }

        private void ProductPage_Load(object sender, EventArgs e)
        {
            pictureBox.Image = CallFrom.Picture;
            lblTitle.Text = CallFrom.Title;
            lblCategory.Text = CallFrom.Category;
            lblPrice.Text = CallFrom.Price;
            lblQuantity.Text = CallFrom.Quantity + " left in stock";
            txtProductCode.Text = CallFrom.ProductCode;
            txtDescription.Text = CallFrom.Description;
            lblFromAddress.Text = CallFrom.FromAddress;
        }
    }
}
