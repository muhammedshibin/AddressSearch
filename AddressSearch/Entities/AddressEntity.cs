using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace AddressSearch.Entities
{
    public class AddressEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string Address { get; set; }
        [MaxLength(50)]
        public string City { get; set; }
        [MaxLength(50)]
        public string State { get; set; }
    }
}
