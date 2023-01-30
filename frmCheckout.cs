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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Shopaze
{
    public partial class frmCheckout : Form
    {
        Form CallFrom;
        CheckoutItem[] checkoutItem;
        double Cash;
        double TotalAmount;
        User user;
        frmChangeAddressBook.AddressBook addressBook;
        public frmCheckout(Form viaParamter, double totalAmount)
        {
            InitializeComponent();
            CallFrom = viaParamter;
            TotalAmount = totalAmount;
            user = frmLogin.loginForm.user;
        }

        public void populateItem()
        {
            checkoutItem = new CheckoutItem[Home.cartMap.Count];
            foreach (var x in Home.cartMap.Select((Entry, Index) => new { Entry, Index }))
            {
                if(x.Entry.Value.IsSelected)
                {
                    int i = x.Index;
                    ListItem obj = x.Entry.Value;
                    checkoutItem[i] = new CheckoutItem();
                    checkoutItem[i].Picture = obj.Picture;
                    checkoutItem[i].Title = obj.Title;
                    checkoutItem[i].Category = obj.Category;
                    checkoutItem[i].Price = obj.Price;
                    checkoutItem[i].Quantity = obj.Quantity;
                    checkoutItem[i].ProductCode = obj.ProductCode;
                    checkoutItem[i].Description = obj.Description;
                    checkoutItem[i].FromAddress = obj.FromAddress;
                    checkoutItem[i].ItemQuantity = obj.ItemQuantity;
                    flowLayoutPanel1.Controls.Add(checkoutItem[i]);
                }
            }

        }

        private void frmCheckout_Load(object sender, EventArgs e)
        {
            lblTotal.Text = TotalAmount.ToString();
            populateItem();

            if (!String.IsNullOrWhiteSpace(user.AddressBook))
            {
                addressNotSetPanel.Visible = false;
                btnOder.Enabled = true;
                addressBook = ParseJsonObject<frmChangeAddressBook.AddressBook>(user.AddressBook);
                lblRecipientsName.Text = Home.home.Truncate(addressBook.RecipientsName, 16);
                lblPhoneNumber.Text = addressBook.PhoneNumber;
                lblStreetAddress.Text = addressBook.StreetAddress;
                txtAddress.Text = addressBook.StreetAddress;
            }
            else
            {
                addressNotSetPanel.Visible = true;
                btnOder.Enabled = false;
            }
        }

        public static T ParseJsonObject<T>(string json) where T : class, new()
        {
            JObject jobject = JObject.Parse(json);
            return JsonConvert.DeserializeObject<T>(jobject.ToString());
        }

        private void txtMoneyAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtCashAmount.Text == "")
            {
                txtCashAmount.ForeColor = Color.Red;
                txtCashAmount.Text = "0";
            }

            if (Convert.ToDouble(lblTotal.Text) > Convert.ToDouble(txtCashAmount.Text))
            {
                txtCashAmount.ForeColor = Color.Red;
            }
            else
            {
                txtCashAmount.ForeColor = Color.Black;
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

        private void btnOder_Click(object sender, EventArgs e)
        {
            Cash = Convert.ToDouble(txtCashAmount.Text);
            if (Convert.ToDouble(lblTotal.Text) > Convert.ToDouble(txtCashAmount.Text))
            {
                MessageBox.Show("Insufficient Balance!");
            }
            else
            {
                if(txtAddress.Text != "")
                {
                    try
                    {
                        Orders order = new Orders();
                        bool check = false;
                        bool[] check2 = new bool[Home.cartMap.Count];

                        foreach (var x in Home.cartMap.Select((Entry, Index) => new { Entry, Index }))
                        {
                            if (x.Entry.Value.IsSelected)
                            {
                                int i = x.Index;
                                ListItem c = x.Entry.Value;
                                check = order.InsertOrder(c.ProductCode, frmLogin.loginForm.user.UserId, c.ItemQuantity, "Completed", "Paid", (Convert.ToDouble(c.Price) * c.ItemQuantity), c.FromAddress, txtAddress.Text, "Unfulfilled", c.ItemQuantity, System.Text.Json.JsonSerializer.Serialize(addressBook));
                                check2[i] = check;
                            }
                        }

                        if(check2[Home.cartMap.Count-1])
                        {
                            frmReceipt receipt = new frmReceipt(this, CallFrom, Cash, TotalAmount);
                            receipt.ShowDialog();
                        }
                    } 
                    catch(Exception ex)
                    {
                        MessageBox.Show("Unkown error occured! Please try again later. " + ex);
                    }
                }
                else
                {
                    MessageBox.Show("Address must not be empty!");
                }
            }
        }

        private void btnSetupAddressBook_Click(object sender, EventArgs e)
        {
            frmChangeAddressBook changeAddressBook = new frmChangeAddressBook();
            changeAddressBook.ShowDialog();
            if (!String.IsNullOrWhiteSpace(user.AddressBook))
            {
                addressNotSetPanel.Visible = false;
                btnOder.Enabled = true;
                addressBook = ParseJsonObject<frmChangeAddressBook.AddressBook>(user.AddressBook);
                lblRecipientsName.Text = Home.home.Truncate(addressBook.RecipientsName, 16);
                lblPhoneNumber.Text = addressBook.PhoneNumber;
                lblStreetAddress.Text = addressBook.StreetAddress;
                txtAddress.Text = addressBook.StreetAddress;
            }
        }
    }
}
