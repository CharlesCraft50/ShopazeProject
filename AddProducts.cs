using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopaze
{
    public partial class AddProducts : Form
    {
        public static string myConn = String.Format(ConfigurationManager.ConnectionStrings["connString"].ConnectionString, Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + @"\Database1.mdf");
        public AddProducts()
        {
            InitializeComponent();
        }

        public bool InsertProduct(string product_name, string category, byte[] picture, string description, double price, string from_address, int quantity)
        {
            int rows;

            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("INSERT INTO Product (product_name, category, picture, description, price, uploaded_date, from_address, delete_flag) VALUES (@ProductName, @Category, @Picture, @Description, @Price, GETDATE(), @FromAddress, 0)", con))
                {
                    com.Parameters.AddWithValue("ProductName", product_name);
                    com.Parameters.AddWithValue("Category", category);
                    com.Parameters.AddWithValue("Picture", picture);
                    com.Parameters.AddWithValue("Description", description);
                    com.Parameters.AddWithValue("Price", price);
                    com.Parameters.AddWithValue("FromAddress", from_address);
                    rows = com.ExecuteNonQuery();

                    com.CommandText = "INSERT INTO Inventory (product_code, quantity) VALUES (@@IDENTITY, @Quantity)";
                    com.Parameters.AddWithValue("Quantity", quantity);
                    com.ExecuteNonQuery();

                    return (rows > 0) ? true : false;
                }
            }
                
        }

        byte[] ConvertImageToBytes(Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public Image ConvertByteArrayToImage(byte[] data)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                return Image.FromStream(ms);
            }
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter= "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG", Multiselect=false})
            {
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    listItem1.Picture = new Bitmap(ofd.FileName);
                    txtFileName.Text = ofd.FileName;
                }
            }
        }

        private void revertPlaceholder()
        {
            listItem1.Picture = null;
            listItem1.Title = "Text Placeholder";
            listItem1.Category = "Category Placeholder";
            listItem1.Description = "Description Placeholder here Description Placeholder here Description Placeholder here Description Placeholder here";
            listItem1.Price = "0";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(ValidateInfo())
            {
                bool success = InsertProduct(txtProductName.Text, cbCategory.SelectedItem.ToString(), ConvertImageToBytes(listItem1.Picture), txtDescription.Text, Convert.ToDouble(txtPrice.Text), txtFromAddress.Text, Convert.ToInt32(txtQuantity.Text));
                if (success)
                {
                    MessageBox.Show("Product Added!");
                    txtProductName.Text = "";
                    txtFileName.Text = "";
                    txtDescription.Text = "";
                    txtPrice.Text = "";
                    revertPlaceholder();
                }
                else
                {
                    MessageBox.Show("Something went wrong. Please try again later!");
                }
            } 
            else
            {
                MessageBox.Show("Please fill up the form!");
            }
        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {
            if (listItem1.Title == "")
            {
                listItem1.Title = "Text Placeholder";
            }
            else
            {
                listItem1.Title = txtProductName.Text;
            }

            lblProductNameCount.Text = "(" + txtProductName.Text.Length + "/150 Characters)";
            if (txtProductName.Text.Length <= 5)
            {
                lblProductNameCount.ForeColor = Color.Red;
            }
            else if(txtProductName.Text.Length > 150)
            {
                lblProductNameCount.ForeColor = Color.Red;
            }
            else
            {
                lblProductNameCount.ForeColor = Color.Gray;
            }
        }

        private bool ValidateInfo()
        {
            if (txtProductName.Text.Length <= 5)
            {
                MessageBox.Show("Product name must be atleast minimum of 5 characters!");
                return false;
            } 
            else if (txtProductName.Text.Length >= 150)
            {
                MessageBox.Show("Product name exceeds the maximum value required!");
                return false;
            }

            if (txtProductName.Text == "" ||
                string.IsNullOrEmpty(cbCategory.Text) ||
                txtPrice.Text == "")
            {
                return false;
            }

            return true;
        }

        private void txtPrice_KeyPress(object sender, KeyPressEventArgs e)
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

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if(txtDescription.Text == "")
            {
                listItem1.Description = "Description Placeholder here Description Placeholder here Description Placeholder here Description Placeholder here";
            }
            else
            {
                listItem1.Description = txtDescription.Text;
            }
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            if (txtPrice.Text == "")
            {
                txtPrice.Text = "";
            }
            else
            {
                listItem1.Price = txtPrice.Text;
            }
        }

        private void txtQuantity_KeyPress(object sender, KeyPressEventArgs e)
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

        private void cbCategory_TextChanged(object sender, EventArgs e)
        {
            listItem1.Category = cbCategory.SelectedItem.ToString();
        }

        private void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            if (txtQuantity.Text == "" || txtQuantity.Text == "0")
            {
                txtQuantity.Text = "1";
            }
            else
            {
                listItem1.Quantity = txtQuantity.Text;
            }
        }

        private void txtFromAddress_TextChanged(object sender, EventArgs e)
        {
            listItem1.FromAddress = txtFromAddress.Text;
        }

        private void AddProducts_Load(object sender, EventArgs e)
        {
            listItem1.Title = "Text Placeholder Text Placeholder Text Placeholder Text Placeholder Text Placeholder Text Placeholder";
            listItem1.Category = "Category Placeholder";
            listItem1.Price = "0";
            listItem1.Quantity = txtQuantity.Text;
            listItem1.Description = "Description Placeholder here Description Placeholder here Description Placeholder here Description Placeholder here";
            listItem1.FromAddress = "Location Address Location Location Address Location";
            listItem1.SoldOutVisible = false;
        }
    }
}
