using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roc.Validation
{
    /// <summary>
    /// 手机号码验证
    /// </summary>
    public class MobileAttribute : RegexAttribute
    {
        public MobileAttribute()
        {
            base.Pattern = @"^1[3578][0-9]{9}$";
        }
    }
}
