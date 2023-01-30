using System;
using System.Collections.Generic;
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
    class Orders
    {
        public static string myConn = String.Format(ConfigurationManager.ConnectionStrings["connString"].ConnectionString, Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + @"\Database1.mdf");
        private string order_id;
        private string product_code;
        private string user_id;
        private string order_quantity;
        private string order_date_time;
        private string order_status;
        private string delivery_status;

        public string OrderId 
        {
    	    get { return order_id; }
            set { order_id = value; }
        }
        public string ProductCode
        {
            get { return product_code; }
            set { product_code = value; }
        }
        public string UserId
        {
            get { return user_id; }
            set { user_id = value; }
        }
        public string OrderQuantity
        {
            get { return order_quantity; }
            set { order_quantity = value; }
        }
        public string OrderDateTime
        {
            get { return order_date_time; }
            set { order_date_time = value; }
        }
        public string OrderStatus
        {
            get { return order_status; }
            set { order_status = value; }
        }
        public string DeliveryStatus
        {
            get { return delivery_status; }
            set { delivery_status = value; }
        }


        public bool InsertOrder(string product_code, string user_id, int order_quantity, string order_status, string payment_status, double payment_amount, string shipped_from, string shipped_to, string delivery_status, int itemquantity, string addressbook)
        {
            int rows;
            int newOrderId;

            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("INSERT INTO Orders (product_code, user_id, order_quantity, order_date_time, order_status, delete_flag) VALUES (@ProductCode, @UserId, @OrderQuantity, GETDATE(), @OrderStatus, '0'); SELECT CAST(scope_identity() AS int)", con))
                {
                    com.Parameters.AddWithValue("ProductCode", product_code);
                    com.Parameters.AddWithValue("UserId", user_id);
                    com.Parameters.AddWithValue("OrderQuantity", order_quantity);
                    com.Parameters.AddWithValue("OrderStatus", order_status);
                    newOrderId = (int)com.ExecuteScalar();

                    com.CommandText = "INSERT INTO Payment (order_id, payment_status, payment_amount) VALUES (@@IDENTITY, @PaymentStatus, @PaymentAmount)";
                    com.Parameters.AddWithValue("PaymentStatus", payment_status);
                    com.Parameters.AddWithValue("PaymentAmount", payment_amount);
                    com.ExecuteNonQuery();

                    com.CommandText = "INSERT INTO Delivery (order_id, shipped_from, shipped_to, delivery_status, address_book) VALUES (@OrderId, @ShippedFrom, @ShippedTo, @DeliveryStatus, @AddressBook)";
                    com.Parameters.AddWithValue("OrderId", newOrderId);
                    com.Parameters.AddWithValue("ShippedFrom", shipped_from);
                    com.Parameters.AddWithValue("ShippedTo", shipped_to);
                    com.Parameters.AddWithValue("DeliveryStatus", delivery_status);
                    com.Parameters.AddWithValue("AddressBook", addressbook);
                    com.ExecuteNonQuery();

                    com.CommandText = "UPDATE Inventory SET quantity = quantity - @ItemQuantity WHERE product_code = @ProductCodeInventory AND quantity > 0";
                    com.Parameters.AddWithValue("ItemQuantity", itemquantity);
                    com.Parameters.AddWithValue("ProductCodeInventory", product_code);
                    rows = com.ExecuteNonQuery();

                    return (rows > 0) ? true : false;
                }
            }

        }

        public void SelectOrders(string category, string product_name, string user_id, FlowLayoutPanel flowLayoutPanel, string deliverystatus)
        {
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("SELECT Product.product_code, Product.product_name, Product.category, MAX(Product.picture) AS picture, Product.description, MAX(Product.price) AS price, Product.uploaded_date, Product.from_address, CAST(b.delete_flag AS INT) AS delete_flag, SUM(b.order_quantity) AS order_quantity, MAX(b.order_date_time) AS order_date_time, b.order_status, MAX(b.order_id) AS order_id, c.delivery_status AS delivery_status, c.shipped_from FROM Product RIGHT JOIN (SELECT MAX(Orders.order_id) AS order_id, Orders.product_code, SUM(Orders.order_quantity) AS order_quantity, Orders.order_date_time AS order_date_time, Orders.order_status, CAST(Orders.delete_flag AS INT) AS delete_flag FROM Orders WHERE Orders.user_id = @UserId GROUP BY Orders.product_code, Orders.order_date_time, Orders.order_status, CAST(Orders.delete_flag AS INT) ) AS b ON Product.product_code = b.product_code RIGHT JOIN (SELECT Delivery.order_id, Delivery.shipped_from, Delivery.shipped_to, Delivery.delivery_status FROM Delivery ) as c ON c.order_id = b.order_id WHERE product_name LIKE '%' + @ProductName + '%'AND category LIKE '%' + @Category + '%'AND Product.product_code IN (SELECT Orders.product_code FROM Orders WHERE user_id = @UserId GROUP BY Orders.product_code ) GROUP BY Product.product_code, Product.product_name, Product.description, Product.from_address, Product.category, Product.uploaded_date, b.order_status, c.delivery_status, c.shipped_from, CAST(b.delete_flag AS INT) ORDER BY MAX(CASE WHEN b.order_status = 'Cancelled' THEN 1 ELSE 0 END ) DESC, MAX(b.order_date_time) DESC", con))
                {
                    com.Parameters.AddWithValue("ProductName", product_name);
                    com.Parameters.AddWithValue("Category", category);
                    com.Parameters.AddWithValue("UserId", user_id);

                    using (SqlDataAdapter reader = new SqlDataAdapter(com))
                    {
                        DataSet ds = new DataSet();
                        reader.Fill(ds);
                        int ProductCount = ds.Tables[0].Rows.Count;
                        var totalPage = ((ProductCount - 1) / 6) + 1;
                        OrderItem[] orderItems = new OrderItem[ds.Tables[0].Rows.Count];

                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if(Convert.ToInt32(row["delete_flag"]) == 0)
                            {
                                if(deliverystatus == "Returning")
                                {
                                    if (row["delivery_status"].ToString().Trim() == deliverystatus || row["delivery_status"].ToString().Trim() == "Returned" || row["delivery_status"].ToString().Trim() == "Cancelled")
                                    {
                                        int i = (int)ds.Tables[0].Rows.IndexOf(row);
                                        orderItems[i] = new OrderItem();

                                        //Picture Data
                                        Byte[] bytePictureData = new byte[0];
                                        bytePictureData = (Byte[])(row["picture"]);
                                        MemoryStream stmPictureData = new MemoryStream(bytePictureData);
                                        orderItems[i].Picture = Image.FromStream(stmPictureData);

                                        orderItems[i].Title = row["product_name"].ToString();
                                        orderItems[i].Category = row["category"].ToString();
                                        orderItems[i].Price = row["price"].ToString();
                                        //orderItems[i].Quantity = row["quantity"].ToString() + " left in stock";
                                        orderItems[i].ProductCode = row["product_code"].ToString();
                                        orderItems[i].OrderId = row["order_id"].ToString();
                                        orderItems[i].Description = row["description"].ToString();
                                        orderItems[i].FromAddress = row["from_address"].ToString();
                                        orderItems[i].ItemQuantity = Convert.ToInt32(row["order_quantity"]);
                                        orderItems[i].Status = row["order_status"].ToString();


                                        flowLayoutPanel.Controls.Add(orderItems[i]);
                                    }
                                }
                                else if(deliverystatus == "Unfullfiled")
                                {
                                    if (row["delivery_status"].ToString().Trim() == deliverystatus || row["delivery_status"].ToString().Trim() == "Shipping" || row["delivery_status"].ToString().Trim() == "Shipped")
                                    {
                                        int i = (int)ds.Tables[0].Rows.IndexOf(row);
                                        orderItems[i] = new OrderItem();

                                        //Picture Data
                                        Byte[] bytePictureData = new byte[0];
                                        bytePictureData = (Byte[])(row["picture"]);
                                        MemoryStream stmPictureData = new MemoryStream(bytePictureData);
                                        orderItems[i].Picture = Image.FromStream(stmPictureData);

                                        orderItems[i].Title = row["product_name"].ToString();
                                        orderItems[i].Category = row["category"].ToString();
                                        orderItems[i].Price = row["price"].ToString();
                                        //orderItems[i].Quantity = row["quantity"].ToString() + " left in stock";
                                        orderItems[i].ProductCode = row["product_code"].ToString();
                                        orderItems[i].OrderId = row["order_id"].ToString();
                                        orderItems[i].Description = row["description"].ToString();
                                        orderItems[i].FromAddress = row["from_address"].ToString();
                                        orderItems[i].ItemQuantity = Convert.ToInt32(row["order_quantity"]);
                                        orderItems[i].Status = row["order_status"].ToString();


                                        flowLayoutPanel.Controls.Add(orderItems[i]);
                                    }
                                }
                                else if(deliverystatus == "Arrived")
                                {
                                    if (row["delivery_status"].ToString().Trim() == deliverystatus || row["delivery_status"].ToString().Trim() == "Arrived" || row["delivery_status"].ToString().Trim() == "Collected")
                                    {
                                        int i = (int)ds.Tables[0].Rows.IndexOf(row);
                                        orderItems[i] = new OrderItem();

                                        //Picture Data
                                        Byte[] bytePictureData = new byte[0];
                                        bytePictureData = (Byte[])(row["picture"]);
                                        MemoryStream stmPictureData = new MemoryStream(bytePictureData);
                                        orderItems[i].Picture = Image.FromStream(stmPictureData);

                                        orderItems[i].Title = row["product_name"].ToString();
                                        orderItems[i].Category = row["category"].ToString();
                                        orderItems[i].Price = row["price"].ToString();
                                        //orderItems[i].Quantity = row["quantity"].ToString() + " left in stock";
                                        orderItems[i].ProductCode = row["product_code"].ToString();
                                        orderItems[i].OrderId = row["order_id"].ToString();
                                        orderItems[i].Description = row["description"].ToString();
                                        orderItems[i].FromAddress = row["from_address"].ToString();
                                        orderItems[i].ItemQuantity = Convert.ToInt32(row["order_quantity"]);
                                        orderItems[i].Status = row["order_status"].ToString();


                                        flowLayoutPanel.Controls.Add(orderItems[i]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }


        public bool CancelOrder(string orderId, int itemquantity, string product_code)
        {
            int rows;
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("UPDATE Orders SET order_status = 'Cancelled' WHERE order_id = @OrderId", con))
                {
                    com.Parameters.AddWithValue("OrderId", orderId);
                    com.ExecuteNonQuery();

                    com.CommandText = "UPDATE Payment SET payment_status = 'Refunding' WHERE order_id = @OrderIdPayment";
                    com.Parameters.AddWithValue("OrderIdPayment", orderId);
                    com.ExecuteNonQuery();

                    com.CommandText = "UPDATE Delivery SET delivery_status = 'Returning' WHERE order_id = @OrderIdDelivery";
                    com.Parameters.AddWithValue("OrderIdDelivery", orderId);
                    com.ExecuteNonQuery();

                    com.CommandText = "UPDATE Inventory SET quantity = quantity + @ItemQuantity WHERE product_code = @ProductCode";
                    com.Parameters.AddWithValue("ItemQuantity", itemquantity);
                    com.Parameters.AddWithValue("ProductCode", product_code);
                    rows = com.ExecuteNonQuery();

                    return (rows > 0) ? true : false;
                }
            }
        }

        public bool DeleteOrder(string orderId)
        {
            int rows;
            using (SqlConnection con = new SqlConnection(myConn))
            {
                con.Open();
                using (SqlCommand com = new SqlCommand("UPDATE Orders SET delete_flag = '1' WHERE order_id = @OrderId", con))
                {
                    com.Parameters.AddWithValue("OrderId", orderId);
                    rows = com.ExecuteNonQuery();

                    return (rows > 0) ? true : false;
                }
            }
        }


    }
}
