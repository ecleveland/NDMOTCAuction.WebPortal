using NDMOTC_Auction.WebPortal.Entities;
using NDMOTC_Auction.WebPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NDMOTC_Auction.WebPortal.Helpers
{
    public class DbHelper
    {
        private NDMOTCAuction_db_devEntities db = new NDMOTCAuction_db_devEntities();

        public InvoiceModel GetInvoice()
        {
            var model = new InvoiceModel();

            return model;
        }

        //private MapGuestModel(Guest guest)
    }
}