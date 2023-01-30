using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaze
{
    class Payment
    {
        private string order_id;
        private string payment_status;
        private string payment_amount;

        public string OrderId
        {
            get { return order_id; }
            set { order_id = value; }
        }

        public string PaymentStatus
        {
            get { return payment_status; }
            set { payment_status = value; }
        }

        public string PaymentAmount
        {
            get { return payment_amount; }
            set { payment_amount = value; }
        }
    }
}
