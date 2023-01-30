using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;

namespace Shopaze
{
    public partial class frmChangeAddressBook : Form
    {
        User user;
        public frmChangeAddressBook()
        {
            InitializeComponent();
            user = frmLogin.loginForm.user;
        }

        public class AddressBook
        {
            public string RecipientsName { get; set; }
            public string PhoneNumber { get; set; }
            public string StreetAddress { get; set; }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                bool recipientsNameValid, phoneNumberValid, streetAddressValid = false; 
                if(txtRecipientsName.Text == "" || txtPhoneNumber.Text == "" || txtStreetAddress.Text == "")
                {
                    MessageBox.Show("Please fill up the form!");
                }
                
                if(!(new Regex("^(?<firstchar>(?=[A-Za-z]))((?<alphachars>[A-Za-z])|(?<specialchars>[A-Za-z]['-](?=[A-Za-z]))|(?<spaces> (?=[A-Za-z])))*$").IsMatch(txtRecipientsName.Text)))
                {
                    throw new InvalidRecipientsNameException("Recipient's Name is not valid!");
                }
                else
                {
                    recipientsNameValid = true;
                }
                
                if(!(new Regex(@"((^(\+)(\d){12}$)|(^\d{11}$))").IsMatch(txtPhoneNumber.Text)))
                {
                    throw new InvalidPhoneNumberException("Phone Number is not valid!");
                }
                else
                {
                    phoneNumberValid = true;
                }
                
                if(!(new Regex(@"^(\d+) ?([A-Za-z](?= ))? (.*?) ([^ ]+?) ?((?<= )APT)? ?((?<= )\d*)?$").IsMatch(txtStreetAddress.Text)))
                {
                    throw new InvalidStreetAddressException("Street Address is not valid!");
                }
                else
                {
                    streetAddressValid = true;
                }

                if(recipientsNameValid && phoneNumberValid && streetAddressValid)
                {
                    AddressBook addressBook = new AddressBook
                    {
                        RecipientsName = txtRecipientsName.Text,
                        PhoneNumber = txtPhoneNumber.Text,
                        StreetAddress = txtStreetAddress.Text
                    };

                    if (user.UpdateAddressBook(user.UserId, JsonSerializer.Serialize(addressBook)))
                    {
                        MessageBox.Show("Address Book Set!");
                        this.Close();
                    }
                }
            }
            catch (InvalidRecipientsNameException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (InvalidPhoneNumberException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (InvalidStreetAddressException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }

    public class InvalidRecipientsNameException : Exception
    {
        public InvalidRecipientsNameException(string recipientsName) : base(recipientsName)
        {

        }
    }

    public class InvalidPhoneNumberException : Exception
    {
        public InvalidPhoneNumberException(string phoneNumber) : base(phoneNumber)
        {

        }
    }

    public class InvalidStreetAddressException : Exception
    {
        public InvalidStreetAddressException(string streetAddress) : base(streetAddress)
        {

        }
    }
}
