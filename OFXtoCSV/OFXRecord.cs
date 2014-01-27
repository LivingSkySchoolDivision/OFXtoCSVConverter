using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OFXtoCSV
{
    class OFXRecord
    {
        public string TransactionTypeString { get; set; }
        public string DatePosted { get; set; }
        public string TransactionAmount { get; set; }
        public string TransactionID { get; set; }
        public string SIC { get; set; }
        public string Name { get; set; }

        public OFXRecord(string transtype, string dateposted, string transactionamount, string transid, string sic, string name)
        {
            this.TransactionTypeString = transtype;
            this.DatePosted = dateposted;
            this.TransactionAmount = transactionamount;
            this.TransactionID = transid;
            this.SIC = sic;
            this.Name = name;
        }

        public override string ToString()
        {
            return "OFXRecord: { Type: " + this.TransactionTypeString + ", Date: " + this.DatePosted + ", Amount: " + this.TransactionAmount + ", ID: " + this.TransactionID + ", SIC: " + this.SIC + ", Name: " + this.Name + "}";
            
        }

    }
}
