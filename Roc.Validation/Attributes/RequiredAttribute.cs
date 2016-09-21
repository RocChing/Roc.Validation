using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Roc.Validation
{
    public class RequiredAttribute : BaseAttribute
    {
        public RequiredAttribute()
        {
            base.DefaultErrorMsg = "不能为空";
        }

        public override ValidateResult OnValidating(PropertyInfo p, object value)
        {
            bool valid = false;
            Type type = p.PropertyType;

            if (value != null)
            {
                if (Nullable.GetUnderlyingType(type) == null)
                    valid = true;
                else if (type.IsType<string>())
                    valid = !string.IsNullOrEmpty(value.ToString());
                else if (type.IsType<DateTime>())
                {
                    if (value != null)//很重要
                    {
                        DateTime dt = Convert.ToDateTime(value);
                        valid = dt > DateTime.MinValue;
                    }
                }
                else if (type.IsType<Guid>())
                {
                    valid = (Guid)value != Guid.Empty;
                }
                else
                {
                    valid = true;
                }
            }

            string msg = valid ? base.DefaultSuccessMsg : base.GetErrorMsg(value);
            return new ValidateResult(valid, msg, base.Order);
        }
    }
}
