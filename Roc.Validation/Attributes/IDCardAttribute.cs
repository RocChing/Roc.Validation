using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Roc.Validation
{
    /// <summary>
    /// 身份证号验证
    /// </summary>
    public class IDCardAttribute : BaseAttribute
    {
        public override ValidateResult OnValidating(PropertyInfo p, object value)
        {
            if (!base.ValidEmpty && base.IsEmpty(value))
                return new ValidateResult(true, "不进行验证", base.Order);

            string s = string.Empty;
            if (value != null) s = value.ToString();
            bool valid = false;
            if (!string.IsNullOrEmpty(s) && s.Length == 18)
            {
                int[] wis = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
                int[] ais = new int[] { 1, 0, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                int sum = 0;
                int num = 0;
                for (int i = 0; i < 17; i++)
                {
                    int.TryParse(s[i].ToString(), out num);
                    sum += (wis[i] * num);
                }

                int result = ais[sum % 11];
                int seq = 0;
                char code = s[17];
                if (code == 'X' || code == 'x') seq = 10;
                else
                    int.TryParse(code.ToString(), out seq);

                valid = result == seq;
            }
            string msg = valid ? base.DefaultSuccessMsg : base.GetErrorMsg(value);
            return new ValidateResult(valid, msg, this.Order);
        }
    }
}
