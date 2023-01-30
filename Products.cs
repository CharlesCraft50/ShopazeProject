using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Shopaze
{
    class Products
    {
        public static string myConn = String.Format(ConfigurationManager.ConnectionStrings["connString"].ConnectionString, Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + @"\Database1.mdf");
        private string product_code;
        private string product_name;
        private string category;
        private Image picture;
        private string description;
        private string price;
        private string uploaded_date;
        private string from_address;
        private string delete_flag;
        private int product_count;

        public string ProductCode
        {
            get { return product_code; }
            set { product_code = value; }
        }

        public string ProductName
        {
            get { return product_name; }
            set { product_name = value; }
        }

        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        public Image Picture
        {
            get { return picture; }
            set { picture = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Price
        {
            get { return price; }
            set { price = value; }
        }

        public string UploadedDate
        {
            get { return uploaded_date; }
            set { uploaded_date = value; }
        }

        public string FromAddress
        {
            get { return from_address; }
            set { from_address = value; }
        }

        public string DeleteFlag
        {
            get { return delete_flag; }
            set { delete_flag = value; }
        }

        public int ProductCount
        {
            get { return product_count; }
            set { product_count = value; }
        }

        public void SearchProduct(string query, string category, FlowLayoutPanel flowLayoutPanel, string orderby, int pageNumber, int rowsOfPage)
        {
            string queryString = "";
            string queryString2 = "";
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();

                switch (orderby)
                {
                    case "Relevance":
                        queryString = "SELECT Product.product_code, Product.product_name, Product.category, Product.picture, Product.description, Product.price, Inventory.quantity, Product.uploaded_date, Product.from_address, Product.delete_flag FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1 ORDER BY CASE WHEN product_name LIKE @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1 THEN 0 WHEN product_name LIKE '% %' + @ProductName + '% %' AND category LIKE '%' + @Category + '%' AND delete_flag != 1 THEN 1 WHEN product_name LIKE '%' + @ProductName AND category LIKE '%' + @Category + '%' AND delete_flag != 1 THEN 2 ELSE 3 END, product_name";
                        queryString2 = "SELECT COUNT(*) FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1";
                        break;
                    case "Top Sales":
                        queryString = "SELECT Product.product_code, Product.product_name, Product.category, MAX(Product.picture) AS picture, Product.description, MAX(Product.price) AS price, MAX(Inventory.quantity) AS quantity, Product.uploaded_date, Product.from_address, CAST(Product.delete_flag AS INT) AS delete_flag, SUM(b.order_quantity) AS order_quantity, b.order_status FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code RIGHT JOIN (SELECT Orders.product_code, SUM(Orders.order_quantity) AS order_quantity, Orders.order_status AS order_status FROM Orders GROUP BY Orders.product_code, Orders.order_status ) AS b ON Product.product_code = b.product_code WHERE Product.product_code IN (SELECT Orders.product_code FROM Orders GROUP BY Orders.product_code ) AND product_name LIKE '%' + @ProductName + '%'AND category LIKE '%' + @Category + '%'AND delete_flag != 1 AND b.order_status != 'Cancelled'GROUP BY Product.product_code, Product.product_name, Product.category, Product.description, Product.uploaded_date, Product.from_address, b.order_status, CAST(Product.delete_flag AS INT) ORDER BY MAX(CASE WHEN b.order_status = 'Cancelled' THEN 1 ELSE 0 END ) ASC, SUM(b.order_quantity) DESC";
                        queryString2 = "SELECT COUNT(*) FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code RIGHT JOIN(SELECT Orders.product_code, SUM(Orders.order_quantity) AS order_quantity, Orders.order_status AS order_status FROM Orders GROUP BY Orders.product_code, Orders.order_status ) AS b ON Product.product_code = b.product_code WHERE Product.product_code IN(SELECT Orders.product_code FROM Orders GROUP BY Orders.product_code ) AND product_name LIKE '%' + @ProductName + '%'AND category LIKE '%' + @Category + '%'AND delete_flag != 1 AND b.order_status != 'Cancelled'";
                        break;
                    case "Lowest Price":
                        queryString = "SELECT Product.product_code, Product.product_name, Product.category, Product.picture, Product.description, Product.price, Inventory.quantity, Product.uploaded_date, Product.from_address, Product.delete_flag FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1 ORDER BY price ASC";
                        queryString2 = "SELECT COUNT(*) FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1";
                        break;
                    case "Highest Price":
                        queryString = "SELECT Product.product_code, Product.product_name, Product.category, Product.picture, Product.description, Product.price, Inventory.quantity, Product.uploaded_date, Product.from_address, Product.delete_flag FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1 ORDER BY price DESC";
                        queryString2 = "SELECT COUNT(*) FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1";
                        break;
                    case "Newly Listed":
                        queryString = "SELECT Product.product_code, Product.product_name, Product.category, Product.picture, Product.description, Product.price, Inventory.quantity, Product.uploaded_date, Product.from_address, Product.delete_flag FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1 ORDER BY uploaded_date DESC";
                        queryString2 = "SELECT COUNT(*) FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1";
                        break;
                    case "Oldest":
                        queryString = "SELECT Product.product_code, Product.product_name, Product.category, Product.picture, Product.description, Product.price, Inventory.quantity, Product.uploaded_date, Product.from_address, Product.delete_flag FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1 ORDER BY uploaded_date ASC";
                        queryString2 = "SELECT COUNT(*) FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1";
                        break;
                    case "Highest Quantity":
                        queryString = "SELECT Product.product_code, Product.product_name, Product.category, Product.picture, Product.description, Product.price, Inventory.quantity, Product.uploaded_date, Product.from_address, Product.delete_flag FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1 ORDER BY Inventory.quantity DESC";
                        queryString2 = "SELECT COUNT(*) FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1";
                        break;
                    case "Lowest Quantity":
                        queryString = "SELECT Product.product_code, Product.product_name, Product.category, Product.picture, Product.description, Product.price, Inventory.quantity, Product.uploaded_date, Product.from_address, Product.delete_flag FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1 ORDER BY Inventory.quantity ASC";
                        queryString2 = "SELECT COUNT(*) FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1";
                        break;
                    default:
                        queryString = "SELECT Product.product_code, Product.product_name, Product.category, Product.picture, Product.description, Product.price, Inventory.quantity, Product.uploaded_date, Product.from_address, Product.delete_flag FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1 ORDER BY CASE WHEN product_name LIKE @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1 THEN 0 WHEN product_name LIKE '% %' + @ProductName + '% %' AND category LIKE '%' + @Category + '%' AND delete_flag != 1 THEN 1 WHEN product_name LIKE '%' + @ProductName AND category LIKE '%' + @Category + '%' AND delete_flag != 1 THEN 2 ELSE 3 END, product_name";
                        queryString2 = "SELECT COUNT(*) FROM Product RIGHT JOIN Inventory ON Product.product_code = Inventory.product_code WHERE product_name LIKE '%' + @ProductName + '%' AND category LIKE '%' + @Category + '%' AND delete_flag != 1";
                        break;
                }

                using (SqlCommand com = new SqlCommand(queryString + " OFFSET (@PageNumber-1)*@RowsOfPage ROWS FETCH NEXT @RowsOfPage ROWS ONLY", con))
                {
                    com.Parameters.AddWithValue("@ProductName", query);
                    com.Parameters.AddWithValue("@Category", category);
                    com.Parameters.AddWithValue("@PageNumber", pageNumber);
                    com.Parameters.AddWithValue("@RowsOfPage", rowsOfPage);
                    using (SqlDataAdapter reader = new SqlDataAdapter(com))
                    {
                        DataSet ds = new DataSet();
                        reader.Fill(ds);
                        //ProductCount = ds.Tables[0].Rows.Count;
                        //var totalPage = ((ProductCount - 1) / 6) + 1;
                        ListItem[] listItems = new ListItem[ds.Tables[0].Rows.Count];
                        //MessageBox.Show(totalPage.ToString());
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if(Convert.ToInt32(row["delete_flag"]) == 0)
                            {
                                int i = (int)ds.Tables[0].Rows.IndexOf(row);
                                listItems[i] = new ListItem();
                                
                                //Picture Data
                                Byte[] bytePictureData = new byte[0];
                                bytePictureData = (Byte[])(row["picture"]);
                                MemoryStream stmPictureData = new MemoryStream(bytePictureData);
                                listItems[i].Picture = Image.FromStream(stmPictureData);

                                listItems[i].Title = row["product_name"].ToString();
                                listItems[i].Category = row["category"].ToString();
                                listItems[i].Price = row["price"].ToString();
                                listItems[i].Quantity = row["quantity"].ToString();
                                listItems[i].ProductCode = row["product_code"].ToString();
                                listItems[i].Description = row["description"].ToString();
                                listItems[i].FromAddress = row["from_address"].ToString();

                                flowLayoutPanel.Controls.Add(listItems[i]);
                            }
                        }

                        com.CommandText = queryString2;
                        int totalPages = (int)com.ExecuteScalar();
                        FlowLayoutPanel pageNumberPanel = new FlowLayoutPanel();
                        if ((((totalPages - 1) / 6) + 1) != 1)
                        {
                            PageNumberControl[] pageNumbers = new PageNumberControl[((totalPages - 1) / 6) + 1];
                            for (int i = 0; i < pageNumbers.Length; i++)
                            {
                                pageNumbers[i] = new PageNumberControl();
                                pageNumbers[i].Number = (pageNumbers.Length - i).ToString();
                                pageNumberPanel.Controls.Add(pageNumbers[i]);
                            }
                        }
                        pageNumberPanel.Size = new Size(flowLayoutPanel.Width - 100, 46);
                        pageNumberPanel.FlowDirection = FlowDirection.RightToLeft;
                        flowLayoutPanel.Controls.Add(pageNumberPanel);
                        pageNumberPanel.Dock = DockStyle.Bottom;
                    }
                }
            }
        }
    }
}
