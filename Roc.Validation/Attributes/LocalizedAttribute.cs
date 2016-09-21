using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Roc.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class LocalizedAttribute : Attribute
    {
        private PropertyInfo _property;
        private Type _resourceType;
        private string _resourceId;

        public LocalizedAttribute(string resourceId, Type type)
            : this(resourceId, type, LocalizedType.Value)
        {

        }
        public LocalizedAttribute(string resourceId, Type type, LocalizedType ltype)
        {
            this._resourceType = type;
            this._resourceId = resourceId;
            this.Type = ltype;
            this.InitProperty();
        }
        public LocalizedType Type { get; set; }
        /// <summary>
        /// 资源类型
        /// </summary>
        public Type ResourceType
        {
            get { return _resourceType; }
            set
            {
                _resourceType = value;
                this.InitProperty();
            }
        }
        /// <summary>
        /// 读取 资源值
        /// </summary>
        public string Value
        {
            get
            {
                if (this._property != null)
                {
                    object obj = this._property.GetValue(_property.DeclaringType, null);
                    if (obj != null) return obj.ToString();
                }
                return string.Empty;
            }
        }

        private void InitProperty()
        {
            if (this._resourceType != null)
                this._property = this._resourceType.GetProperty(this._resourceId, BindingFlags.Static | BindingFlags.Public);
        }
    }

    public enum LocalizedType
    {
        DisplayName = 1,
        ErrorMsg = 2,
        RegexErrorMsg = 3,
        IDCardErrorMsg = 4,
        MaxErrorMsg = 5,
        RangeErrorMsg = 6,
        StringLenthErrorMsg = 7,
        Value = 100
    }
}
