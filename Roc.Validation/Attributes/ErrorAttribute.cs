using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roc.Validation
{
    /// <summary>
    /// 错误显示方式
    /// </summary>
     [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class ErrorAttribute : Attribute
    {
        public ErrorAttribute()
            : this(ErrorType.ALL)
        {

        }
        public ErrorAttribute(ErrorType type)
        {
            this.Type = type;
        }

        public ErrorType Type { get; set; }
    }
    /// <summary>
    /// 错误显示方式类型
    /// </summary>
    public enum ErrorType
    {
        First = 1,
        ALL = 2
    }
}
