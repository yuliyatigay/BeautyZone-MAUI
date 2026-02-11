using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class BeautyTech
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public virtual List<Guid> Procedures { get; set; }
    }
}
