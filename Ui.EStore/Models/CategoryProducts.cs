using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Domain.EStore;

namespace Ui.EStore.Models
{
    public class CategoryProducts
    {
        public MainCategory MainCategory { get; set; }
        public List<Product> Products { get; set; }
    }
}