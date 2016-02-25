using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRP4ME.Model
{
    public class SalesOrder
    {
        public string customer_name { get; set; }
        public System.DateTime required_date { get; set; }
        public string so_number { get; set; }
        public string item_code { get; set; }
        public string name { get; set; }
        public string unit { get; set; }
        public string unit_cost { get; set; }
        public string description { get; set; }
        public string quantity { get; set; }
        public string quantity_received { get; set; }
        public string back_ordered { get; set; }
        public string attachment { get; set; }
        public string upload_image { get; set; }
        public string user { get; set; }
        public string level { get; set; }


        public bool SaveNewSO()
        {



            return true;
        }
    
    
    
    
    }

   


}
