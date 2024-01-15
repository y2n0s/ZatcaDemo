using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Zatca
{
    public class ZatcaItem
    {
        public ZatcaItem(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
        public ZatcaItem(string name, string value, string nameAr)
        {
            this.Name = name;
            this.Value = value;
            this.NameAr = nameAr;
        }
        public string Name { get; set; }
        public string Value { get; set; }

        public string NameAr { set; get; }
        public List<ZatcaItem> SubItems { get; set; } = new List<ZatcaItem>();
    }
}
