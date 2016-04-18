using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ObReg.Core
{
    public class OrderItem
    {
        public OrderItem()
        {
            IsNew = true;
            Id = -1;
            Status = 0;
            Count = 0;
            FinalCount = 0;
            Code = string.Empty;
        }

        internal OrderItem(DataRow data)
        {
            Id = (long)data["id"];
            Code = data["code"].ToString();
            Text = data["description"].ToString();
            Count = (long)data["count"];
            FinalCount = (long)data["finalCount"];
            ReceiveDate = Convert.ToDateTime(data["receiveDate"]);
            EstimatedDate = data["estimatedDate"] != string.Empty ? Convert.ToDateTime(data["estimatedDate"]) : (DateTime?)null;
            TerminationDate = data["terminationDate"] != string.Empty ? Convert.ToDateTime(data["terminationDate"]) : (DateTime?)null;
            Status = (long)data["status"];
            IsNew = false;
        }

        public long Id { get; set; }

        public string Code { get; set; }

        public string Text { get; set; }

        public long Count { get; set; }

        public long FinalCount { get; set; }

        public DateTime ReceiveDate { get; set; }

        public DateTime? EstimatedDate { get; set; }

        public DateTime? TerminationDate { get; set; }

        public long Status { get; set; }

        internal bool IsNew
        {
            get;
            set;
        }

        internal bool IsNewItem
        {
            get
            {
                return (Id == -1);
            }
        }

        internal bool IsUpdated { get; set; }

        internal void UpdateData(DataRow row)
        {
            row.ItemArray = new object[] { 
                (IsNewItem) ? null : (object)Id,
                Code ?? string.Empty,
                Text ?? string.Empty,
                Count,
                FinalCount,
                ReceiveDate.ToString(),
                EstimatedDate.ToString(),
                TerminationDate.ToString(),
                Status
            };
        }
    }
}
