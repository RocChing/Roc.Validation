using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Roc.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public abstract class BaseAttribute : Attribute
    {
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Name 资源
        /// </summary>
        private LocalizedAttribute NameAttribute { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }
        /// <summary>
        /// ErrorMsg 资源
        /// </summary>
        private LocalizedAttribute ErrorMsgAttribute { get; set; }
        /// <summary>
        /// 是否空也验证 默认验证
        /// </summary>
        public bool ValidEmpty { get; set; }
        /// <summary>
        /// 默认验证失败时错误信息
        /// </summary>
        public string DefaultErrorMsg { get; protected set; }
        /// <summary>
        /// 默认验证通过时成功信息
        /// </summary>
        public string DefaultSuccessMsg { get; private set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Order { get; set; }

        public BaseAttribute()
            : this(100)
        {

        }
        public BaseAttribute(int order)
        {
            this.Order = order;
            this.ErrorMsg = string.Empty;
            this.ValidEmpty = true;
            this.DefaultErrorMsg = "验证失败";
            this.DefaultSuccessMsg = "验证通过";
        }
        /// <summary>
        /// 获得错误信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected virtual string GetErrorMsg(object value)
        {
            string v = value == null ? "" : value.ToString();
            return this.GetErrorMsg(v);
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool IsEmpty(object value)
        {
            if (value == null) return true;
            string s = value.ToString();
            return string.IsNullOrEmpty(s);
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <returns></returns>
        public abstract ValidateResult OnValidating(PropertyInfo p, object value);
        /// <summary>
        /// 验证之前出发
        /// </summary>
        /// <param name="p"></param>
        private void OnBeforeValidate(PropertyInfo p)
        {
            var list = GetLocalizedAttribute(p);

            this.NameAttribute = this.GetDisplayNameAttribute(list);
            this.ErrorMsgAttribute = this.GetErrorMsgAttribute(list);
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="p"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual ValidateResult Validate(PropertyInfo p, object value)
        {
            this.OnBeforeValidate(p);

            return this.OnValidating(p, value);
        }

        #region Private
        /// <summary>
        /// 获得错误信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private LocalizedAttribute GetErrorMsgAttribute(List<LocalizedAttribute> list)
        {
            Func<LocalizedAttribute, bool> func = null;
            Type type = this.GetType();
            if (type == typeof(RequiredAttribute))
                func = m => m.Type == LocalizedType.ErrorMsg;
            else if (type == typeof(RegexAttribute))
                func = m => m.Type == LocalizedType.RegexErrorMsg;
            else if (type == typeof(IDCardAttribute))
                func = m => m.Type == LocalizedType.IDCardErrorMsg;
            else if (type == typeof(MaxAttribute))
                func = m => m.Type == LocalizedType.MaxErrorMsg;
            else if (type == typeof(RangeAttribute))
                func = m => m.Type == LocalizedType.RangeErrorMsg;
            else if (type == typeof(StringLengthAttribute))
                func = m => m.Type == LocalizedType.StringLenthErrorMsg;
            else
                func = m => m.Type == LocalizedType.ErrorMsg;
            if (list != null && list.Count > 0)
                return list.FirstOrDefault(func);
            return null;
        }
        /// <summary>
        /// 获得显示名称
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private LocalizedAttribute GetDisplayNameAttribute(List<LocalizedAttribute> list)
        {
            if (list != null && list.Count > 0)
            {
                return list.FirstOrDefault(m => m.Type == LocalizedType.DisplayName);
            }
            return null;
        }
        private List<LocalizedAttribute> GetLocalizedAttribute(PropertyInfo p)
        {
            var attrs = p.GetCustomAttributes(typeof(LocalizedAttribute), true);
            if (attrs != null && attrs.Length > 0)
            {
                List<LocalizedAttribute> list = new List<LocalizedAttribute>();
                foreach (var item in attrs)
                {
                    LocalizedAttribute local = item as LocalizedAttribute;
                    list.Add(local);
                }
                return list;
            }
            return null;
        }
        /// <summary>
        /// 获得错误信息
        /// </summary>
        /// <returns></returns>
        private string GetErrorMsg()
        {
            string name = GetFiledName();
            string errorMsg = this.ErrorMsg;
            if (string.IsNullOrEmpty(errorMsg))
            {
                var attr = this.ErrorMsgAttribute;
                if (attr != null) errorMsg = attr.Value;
                if (string.IsNullOrEmpty(errorMsg))
                    errorMsg = name + this.DefaultErrorMsg;
            }
            return errorMsg;
        }
        /// <summary>
        /// 获得字段信息
        /// </summary>
        /// <returns></returns>
        private string GetFiledName()
        {
            var name = this.Name;
            if (string.IsNullOrEmpty(name))
            {
                var attr = this.NameAttribute;
                if (attr != null) name = attr.Value;
                if (string.IsNullOrEmpty(name)) name = "该字段";
                this.Name = name;
            }
            return name;
        }
        /// <summary>
        /// 替换错误文本
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string GetErrorMsg(string value)
        {
            string text = this.GetErrorMsg();
            if (string.IsNullOrEmpty(text)) return null;
            string pattern = @"\{(\w+)\}";
            var matches = Regex.Matches(text, pattern, RegexOptions.IgnoreCase);
            if (matches != null && matches.Count > 0)
            {
                var type = this.GetType();
                var ps = type.GetProperties();
                foreach (Match item in matches)
                {
                    string name = item.Groups[0].Value;
                    string lowerName = item.Groups[1].Value.ToLower();
                    var p = ps.FirstOrDefault(m => m.Name.ToLower() == lowerName);
                    if (p != null)
                    {
                        object obj = p.GetValue(this, null);
                        string s = obj == null ? "" : obj.ToString();
                        text = text.Replace(name, s);
                    }
                }
            }
            text = text.Replace("{CV}", value);
            return text;
        }
        #endregion
    }
    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidateResult
    {
        /// <summary>
        /// 信息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 是否验证通过
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// 属性排序
        /// </summary>
        public int PropertyOrder { get; set; }
        /// <summary>
        /// 特性排序
        /// </summary>
        public int AttrOrder { get; set; }

        public ValidateResult(bool valid)
            : this(valid, string.Empty, 10)
        {

        }

        public ValidateResult(bool valid, string msg, int order)
        {
            this.IsValid = valid;
            this.Msg = msg;
            this.AttrOrder = order;
            this.PropertyOrder = 100;
        }
    }

    public enum CompareType
    {
        /// <summary>
        /// 大于
        /// </summary>
        GreaterThan = 1,
        /// <summary>
        /// 大于等于
        /// </summary>
        GreaterEqualThan = 2,
        /// <summary>
        /// 小于
        /// </summary>
        LessThat = 3,
        /// <summary>
        /// 小于等于
        /// </summary>
        LessEqualThan = 4,
        /// <summary>
        /// 等于
        /// </summary>
        Equal = 5,
        /// <summary>
        /// 不等于
        /// </summary>
        NotEqual = 6,
        /// <summary>
        /// 在之间
        /// </summary>
        Between = 7,
        /// <summary>
        /// 在之间 包括左右等于
        /// </summary>
        BetweenWithEqual,
        /// <summary>
        /// 在之间 包括左等于
        /// </summary>
        BetweenWithLeftEqual,
        /// <summary>
        /// 在之间 包括右等于
        /// </summary>
        BetweenWithRightEqual
    }
}
