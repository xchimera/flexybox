﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlexyDomain.Models
{
    //Lavet af Vijeeth
    public class CustomerFlow : EntityPersist
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public IList<StepAnswer> Answers { get; set; }
        public IList<Product> Products { get; set; }
        public IList<CustomerFile> FlowFiles { get; set; }
    }
}
