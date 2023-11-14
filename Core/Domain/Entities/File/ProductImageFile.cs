using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.File
{
    public class ProductImageFile : File
    {
        public bool Showcase { get; set; }
        public ICollection<Product> Product { get; set; }
    }
}
