using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jule24.Domain
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Vehicle? Vehicle { get; set; }
        public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
        public ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();
    }
}
