using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Roc.Validation
{
    public class ValidateHelper
    {
        public static IEnumerable<ValidateResult> GetValidateResults(ValidateType type, object obj)
        {
            return GetValidateResults<object>(type, obj);
        }
        /// <summary>
        /// 获得错误的验证结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static IEnumerable<ValidateResult> GetValidateResults<T>(List<T> list)
        {
            if (list != null && list.Count > 0)
            {
                List<ValidateResult> results = new List<ValidateResult>();
                foreach (var obj in list)
                {
                    var rs = GetValidateResults(ValidateType.Error, obj);
                    if (rs != null && rs.Count() > 0) results.AddRange(rs);
                }
                return results;
            }
            return null;
        }
        /// <summary>
        /// 获得错误的验证结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o"></param>
        /// <returns></returns>
        public static IEnumerable<ValidateResult> GetValidateResults<T>(T o)
        {
            return GetValidateResults<T>(ValidateType.Error, o);
        }
        /// <summary>
        /// 获得验证结果
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="type">验证结果类型</param>
        /// <param name="o">实体</param>
        /// <returns></returns>
        public static IEnumerable<ValidateResult> GetValidateResults<T>(ValidateType type, T o)
        {
            if (o == null) return null;
            var classType = o.GetType();
            var ps = classType.GetProperties();
            if (ps != null && ps.Count() > 0)
            {
                List<ValidateResult> results = new List<ValidateResult>();
                foreach (var p in ps)
                {
                    object[] unvalidates = p.GetCustomAttributes(typeof(UnValidateAttribute), true);
                    if (unvalidates != null && unvalidates.Length > 0) continue;

                    object value = p.GetValue(o, null);
                    
                    object[] errors = p.GetCustomAttributes(typeof(ErrorAttribute), true);
                    ErrorType errorType = ErrorType.ALL;
                    if (errors != null && errors.Length > 0)
                        errorType = (errors.FirstOrDefault() as ErrorAttribute).Type;

                    int propertyOrder = 100;
                    var orders = p.GetCustomAttributes(typeof(OrderAttribute), true);
                    if (orders != null && orders.Length > 0)
                        propertyOrder = (orders.FirstOrDefault() as OrderAttribute).Index;

                    object[] objs = p.GetCustomAttributes(typeof(BaseAttribute), true);
                    if (objs != null && objs.Length > 0)
                    {
                        foreach (var obj in objs)
                        {
                            BaseAttribute attr = obj as BaseAttribute;
                            var result = attr.Validate(p, value);
                            result.PropertyOrder = propertyOrder;
                            results.Add(result);
                            if (errorType == ErrorType.First && !result.IsValid) break;
                        }
                    }
                }
                IEnumerable<ValidateResult> rs = null;
                switch (type)
                {
                    case ValidateType.Success:
                        rs = results.Where(m => m.IsValid);
                        break;
                    case ValidateType.Error:
                        rs = results.Where(m => !m.IsValid);
                        break;
                    case ValidateType.ALL:
                        rs = results;
                        break;
                }
                if (rs != null)
                    rs = rs.OrderBy(m => m.PropertyOrder).ThenBy(m=>m.AttrOrder);
                return rs;
            }
            return null;
        }
    }

    public enum ValidateType
    {
        /// <summary>
        /// 成功的
        /// </summary>
        Success = 1,
        /// <summary>
        /// 失败的
        /// </summary>
        Error = 2,
        /// <summary>
        /// 所有的
        /// </summary>
        ALL = 3
    }
}
