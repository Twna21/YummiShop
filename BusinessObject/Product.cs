using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public decimal UnitPrice { get; set; }
        public int QuantityPerUnit { get; set; }


        public virtual Category? Categories { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }

    }
}
