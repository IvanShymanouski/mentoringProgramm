using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCL1
{
    [AttributeUsageAttribute(AttributeTargets.Class,  AllowMultiple = false)]
    public class CustomAttr: Attribute
    {
        public string Name { get; private set; }
        public string Email { get; private set; }

        public CustomAttr(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}
