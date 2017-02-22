using NDMOTC_Auction.WebPortal.Entities;
using System.Collections.Generic;

namespace NDMOTC_Auction.WebPortal.Models
{
    public class CheckoutViewModel
    {
        public IEnumerable<Item> SoldItems { get; set; }
        public IEnumerable<Item> UnsoldItems { get; set; }
        public IEnumerable<Guest> Guests { get; set; }
        public decimal? TotalSold { get; set; }
        public decimal? TotalPaid { get; set; }
        public decimal? TotalPledged { get; set; }
    }
}