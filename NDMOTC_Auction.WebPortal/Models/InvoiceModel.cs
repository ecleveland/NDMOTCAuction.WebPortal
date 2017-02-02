using NDMOTC_Auction.WebPortal.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NDMOTC_Auction.WebPortal.Models
{
    public class InvoiceModel
    {
        public Guest Guest { get; set; }
        public List<Item> PurchasedItems { get; set; }
    }
}