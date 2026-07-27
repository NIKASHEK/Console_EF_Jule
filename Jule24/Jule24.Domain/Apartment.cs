using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jule24.Domain
{
    public class Apartment
    {
        public int Id {  get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Person> People { get; set; } = new List<Person>();
    }
}
