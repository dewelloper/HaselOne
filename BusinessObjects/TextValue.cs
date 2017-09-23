using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class TextValue
    {
        public string Text { get; set; }
        public int Value { get; set; }

        public TextValue()
        {
        }

        public TextValue(int value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}