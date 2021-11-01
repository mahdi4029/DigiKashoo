using System;
using System.Collections.Generic;
using System.Text;
using Dgk.Domain.Attributes;

namespace Dgk.Domain.User
{
    [Auditable]
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
    }
}
