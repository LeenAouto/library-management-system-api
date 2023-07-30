using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Book
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }

        [MaxLength(250)]
        public string Author { get; set; }

        [MaxLength(250)]
        public string Publisher { get; set; }

        public int? PublishYear { get; set; }

        public bool IsAvailable { get; set; }

    }
}
