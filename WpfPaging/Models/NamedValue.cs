using System;
using System.Collections.Generic;
using System.Text;

namespace WpfPaging.Models
{
    public class NamedValue
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    public class NamedValue<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
    }
}
