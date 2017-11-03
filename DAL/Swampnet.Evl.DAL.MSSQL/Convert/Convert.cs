using Swampnet.Evl.Client;
using Swampnet.Evl.DAL.MSSQL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Swampnet.Evl.DAL.MSSQL
{
    static partial class Convert
    {
        /// <summary>
        /// Convert an IProperty to a Property
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static Property ToProperty(IProperty source)
        {
            return new Property()
            {
                Category = source.Category,
                Name = source.Name,
                Value = source.Value
            };
        }


        /// <summary>
        /// Convert an API IProperty to an InternalProperty
        /// </summary>
        internal static InternalProperty ToInternalProperty(IProperty source)
        {
            return new InternalProperty()
            {
                Category = source.Category.Truncate(225),
                Name = source.Name.Truncate(225),
                Value = source.Value == null ? "null" : source.Value
            };
        }
    }
}
