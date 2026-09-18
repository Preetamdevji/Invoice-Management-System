using System;
using System.Collections.Generic;
using System.Text;

namespace CustomerService.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public int? PersonId { get; set; }       // not nullable — Store customers out of scope
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";
    }
}
