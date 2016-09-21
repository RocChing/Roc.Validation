using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Roc.Validation
{
    /// <summary>
    /// 正则表达式验证
    /// </summary>
    public class RegexAttribute : BaseAttribute
    {
        /// <summary>
        /// 正则表达式
        /// </summary>
        public string Pattern { get; set; }

        public RegexOptions Options { get; set; }

        public RegexAttribute()
        {
            this.Options = RegexOptions.IgnoreCase;
            base.DefaultErrorMsg = "格式不正确";
        }

        public override ValidateResult OnValidating(PropertyInfo p, object value)
        {
            if (!base.ValidEmpty && base.IsEmpty(value))
                return new ValidateResult(true, "不进行验证", this.Order);

            bool valid = false;
            string pattern = this.Pattern;
            if (!string.IsNullOrEmpty(pattern) && !base.IsEmpty(value))
                valid = Regex.IsMatch(value.ToString(), pattern, this.Options);

            string msg = valid ? base.DefaultSuccessMsg : base.GetErrorMsg(value);
            return new ValidateResult(valid, msg, this.Order);
        }
    }
}
