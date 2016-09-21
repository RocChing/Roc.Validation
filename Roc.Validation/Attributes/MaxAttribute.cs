using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;

namespace Roc.Validation
{
    /// <summary>
    /// 设置 最大值 数字类型,Datetime
    /// </summary>
    public class MaxAttribute : BaseAttribute
    {
        /// <summary>
        /// 最大值
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 比较类型
        /// </summary>
        public CompareType Type { get; set; }

        public MaxAttribute()
            : this(null, CompareType.LessEqualThan)
        {

        }
        public MaxAttribute(object obj)
            : this(obj, CompareType.LessEqualThan)
        {

        }
        public MaxAttribute(object max, CompareType type)
        {
            this.Value = max;
            this.Type = type;
            this.SetDefaultErrorMsg();
        }

        public override ValidateResult OnValidating(PropertyInfo p, object value)
        {
            bool valid = false;
            if (this.Value == null) return new ValidateResult(valid, "最大值没有设置", this.Order);
            if (value == null) return new ValidateResult(valid, "值为null", this.Order);

            var type = p.PropertyType;
            dynamic maxValue = 0;
            dynamic orginalValue = 0;
            if (type.IsType<decimal>())
            {
                maxValue = decimal.Parse(this.Value.ToString());
                orginalValue = decimal.Parse(value.ToString());
            }
            else if (type.IsNumeric())
            {
                maxValue = double.Parse(this.Value.ToString());
                orginalValue = double.Parse(value.ToString());
            }
            else if (type.IsType<DateTime>())
            {
                maxValue = DateTime.Parse(this.Value.ToString());
                orginalValue = DateTime.Parse(value.ToString());
            }
            else orginalValue = value;
            switch (this.Type)
            {
                case CompareType.LessThat:
                    valid = orginalValue < maxValue;
                    break;
                default:
                    valid = orginalValue <= maxValue;
                    break;
            }
            string msg = valid ? base.DefaultSuccessMsg : this.GetErrorMsg(value);
            return new ValidateResult(valid, msg, this.Order);
        }
        protected override string GetErrorMsg(object value)
        {
            return base.GetErrorMsg(value);
        }
        private void SetDefaultErrorMsg()
        {
            string msg = string.Empty;
            switch (this.Type)
            {
                case CompareType.LessThat:
                    msg = "必须小于";
                    break;
                default:
                    msg = "必须小于等于";
                    break;
            }
            base.DefaultErrorMsg = msg + this.Value;
        }
    }
}
