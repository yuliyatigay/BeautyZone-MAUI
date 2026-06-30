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
        public virtual List<Procedure> Procedures { get; set; }
        public string ProceduresDescription =>
            Procedures == null || 
            !Procedures.Any() ? string.Empty :
                string.Join(", ", Procedures.Select(p => p.Name));
    }
}
