using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Swampnet.Evl.Web.Entities
{
    public class MetaData
    {
        public ActionMetaData[] ActionMetaData { get; set; }
        public ExpressionOperator[] Operators { get; set; }
        public MetaDataCapture[] Operands { get; set; }
    }


    public class Option
    {
        public Option()
        {
        }

        public Option(string display, string value)
            : this()
        {
            Display = display;
            Value = value;
        }

        public string Display { get; set; }
        public string Value { get; set; }
    }


    public class ActionMetaData
    {
        public string Type { get; set; }
        public MetaDataCapture[] Properties { get; set; }
    }



    public class ExpressionOperator
    {
        public ExpressionOperator()
        {
        }

        public ExpressionOperator(string code, string display, bool isGroup)
            : this()
        {
            Code = code;
            Display = display;
            IsGroup = isGroup;
        }


        public string Code { get; set; }
        public string Display { get; set; }
        public bool IsGroup { get; set; }
    }

    public class MetaDataCapture
    {
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public string DataType { get; set; }
        public Option[] Options { get; set; }
    }
}
