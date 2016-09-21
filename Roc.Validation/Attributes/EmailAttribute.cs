using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Roc.Validation
{
    /// <summary>
    /// 邮箱验证
    /// </summary>
    public class EmailAttribute : RegexAttribute
    {
        public EmailAttribute()
        {
            base.Pattern = @"^[a-zA-Z0-9.!#$%&'*+\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$";
        }
    }
}
