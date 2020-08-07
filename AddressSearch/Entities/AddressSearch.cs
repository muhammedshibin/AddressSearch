using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressSearch.Entities
{
    public class SearchAddress
    {
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int frequency { get; set; }
    }
}
