using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;

namespace Roc.Validation
{
    public class RangeAttribute : BaseAttribute
    {
        /// <summary>
        /// 最大值
        /// </summary>
        public int Max { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public CompareType Type { get; set; }

        public RangeAttribute()
        {
            this.Type = CompareType.BetweenWithEqual;
        }

        public RangeAttribute(int min, int max, CompareType type)
        {
            this.Min = min;
            this.Max = max;
            this.Type = type;
        }

        public override ValidateResult OnValidating(PropertyInfo p, object value)
        {
            bool valid = false;
            int intValue = 0;
            if (!base.IsEmpty(value))
            {
                int.TryParse(value.ToString(), out intValue);
                int min = this.Min;
                int max = this.Max;
                switch (this.Type)
                {
                    case CompareType.GreaterThan:
                        valid = intValue > min;
                        break;
                    case CompareType.GreaterEqualThan:
                        valid = intValue >= min;
                        break;
                    case CompareType.LessThat:
                        valid = intValue < max;
                        break;
                    case CompareType.LessEqualThan:
                        valid = intValue <= max;
                        break;
                    case CompareType.Between:
                        valid = min < intValue && intValue < max;
                        break;
                    case CompareType.BetweenWithEqual:
                        valid = min <= intValue && intValue <= max;
                        break;
                    case CompareType.BetweenWithLeftEqual:
                        valid = min <= intValue && intValue < max;
                        break;
                    case CompareType.BetweenWithRightEqual:
                        valid = min < intValue && intValue <= max;
                        break;
                }
            }
            string errorMsg = valid ? base.DefaultSuccessMsg : base.GetErrorMsg(value);
            return new ValidateResult(valid, errorMsg, this.Order);
        }
    }
}
