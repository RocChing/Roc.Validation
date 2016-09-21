using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roc.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class OrderAttribute : Attribute
    {
        /// <summary>
        /// 排序默认 100
        /// </summary>
        public int Index { get; set; }

        public OrderAttribute(int index)
        {
            this.Index = index;
        }

        public OrderAttribute() : this(100) { }
    }
}
