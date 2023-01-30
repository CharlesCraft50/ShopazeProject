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
    public partial class frmReceipt : Form
    {
        Form CallFrom;
        Form CallFrom2;
        Label title;
        Label price;
        Panel[] panel;
        double Cash;
        double TotalAmount;
        public frmReceipt(Form viaParameter, Form viaParameter2, double cash, double totalAmount)
        {
            InitializeComponent();
            CallFrom = viaParameter;
            CallFrom2 = viaParameter2;
            Cash = cash;
            TotalAmount = totalAmount;
        }

        public void populateItem()
        {
            panel = new Panel[Home.cartMap.Count];
            foreach (var x in Home.cartMap.Select((Entry, Index) => new { Entry, Index }))
            {
                int i = x.Index;
                ListItem obj = x.Entry.Value;
                title = new Label();
                if (obj.ItemQuantity > 1)
                {
                    title.Text = obj.Title + " (x" + obj.ItemQuantity + ")";
                }
                else
                {
                    title.Text = obj.Title;
                }
                title.AutoSize = true;
                title.AutoEllipsis = true;
                //title.Dock = DockStyle.Left;
                title.TextAlign = ContentAlignment.TopLeft;

                price = new Label();
                price.Text = obj.Price;
                price.AutoSize = false;
                price.Dock = DockStyle.Right;
                price.TextAlign = ContentAlignment.TopRight;

                panel[i] = new Panel();
                panel[i].Size = new Size(flowLayoutPanel1.Width, 15);
                panel[i].Controls.Add(title);
                panel[i].Controls.Add(price);

                flowLayoutPanel1.Controls.Add(panel[i]);
            }

        }

        private void frmReceipt_Load(object sender, EventArgs e)
        {
            populateItem();
            lblTotal.Text = TotalAmount.ToString();
            lblCash.Text = Cash.ToString();
            lblChange.Text = (Cash - TotalAmount).ToString();
            
        }

        private void frmReceipt_FormClosing(object sender, FormClosingEventArgs e)
        {
            Home.home.populateItems();
            frmCart.flowLayoutPanel.Controls.Clear();
            frmCart.amountMap.Clear();
            Home.cartMap.Clear();
            CallFrom.Close();
            CallFrom2.Close();
        }
    }
}
