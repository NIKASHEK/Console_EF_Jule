using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jule24.Domain
{
    public class Quote
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public Person? Person { get; set; } 

        public Guid? PersonId { get; set; }
        public Author? Author { get; set; }// თუ ანონიმური ციტატები არ დაიშვება = null!; და არა ?
        public int? AuthorId { get; set; } // თუ ანონიმური ციტატები არ დაიშვება = null!; და არა ?
    }
}
