using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roc.Validation
{
    /// <summary>
    /// 不验证
    /// </summary>
     [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class UnValidateAttribute : Attribute
    {
        public UnValidateAttribute()
        {

        }
    }
}
