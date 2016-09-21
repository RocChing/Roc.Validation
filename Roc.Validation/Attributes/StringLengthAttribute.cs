using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Roc.Validation
{
    /// <summary>
    /// 字符串length比较
    /// </summary>
    public class StringLengthAttribute : BaseAttribute
    {
        /// <summary>
        /// 最大长度
        /// </summary>
        public int Max { get; set; }
        /// <summary>
        /// 最小长度
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        /// 比较类型
        /// </summary>
        public CompareType Type { get; set; }

        public StringLengthAttribute(int value, CompareType type)
            : this(value, value, type)
        { }

        public StringLengthAttribute(int min, int max)
            : this(min, max, CompareType.BetweenWithEqual)
        {

        }

        public StringLengthAttribute(int min, int max, CompareType type)
        {
            this.Max = max;
            this.Min = min;
            this.Type = type;
        }

        public override ValidateResult OnValidating(PropertyInfo p, object value)
        {
            bool valid = false;
            if (IsEmpty(value)) return new ValidateResult(valid, "值不能为Null", this.Order);

            int len = value.ToString().Length;
            int min = this.Min;
            int max = this.Max;

            switch (this.Type)
            {
                case CompareType.GreaterThan:
                    valid = len > min;
                    break;
                case CompareType.GreaterEqualThan:
                    valid = len >= min;
                    break;
                case CompareType.LessThat:
                    valid = len < max;
                    break;
                case CompareType.LessEqualThan:
                    valid = len <= max;
                    break;
                case CompareType.Between:
                    valid = min < len && len < max;
                    break;
                case CompareType.BetweenWithEqual:
                    valid = min <= len && len <= max;
                    break;
                case CompareType.BetweenWithLeftEqual:
                    valid = min <= len && len < max;
                    break;
                case CompareType.BetweenWithRightEqual:
                    valid = min < len && len <= max;
                    break;
            }
            string errorMsg = valid ? base.DefaultSuccessMsg : base.GetErrorMsg(value);
            return new ValidateResult(valid, errorMsg, this.Order);
        }
    }
}
