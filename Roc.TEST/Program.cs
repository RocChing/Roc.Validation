using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roc.Validation;

namespace Roc.TEST
{
    class Program
    {
        static void Main(string[] args)
        {
            Users u = new Users();
            u.Age = 12;
            u.CreateTime = null;
            u.Desc = "文字比较少";
            u.Email = "格式不对的邮箱";
            u.Phone = "13141071008";
            u.Money = 6000;
            u.Name = "这么长的名字能验证通过吗?";
            u.UserId = 1;

            //查找所有结果
            //var results = ValidateHelper.GetValidateResults<Users>(ValidateType.ALL, u);
            //只找错误结果
            var results = ValidateHelper.GetValidateResults<Users>(ValidateType.Error, u);
            if (results == null)
            {
                //实体为null 或者 没有找到任何属性
                return;
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("验证结果如下:");
            sb.AppendLine();
            foreach (var item in results)
            {
                sb.AppendFormat("PropertyOrder:[{2}],AttrOrder:[{3}] IsValid: {0}, Msg: {1}", item.IsValid, item.Msg, item.PropertyOrder, item.AttrOrder);
                sb.AppendLine();
            }

            Console.Write(sb.ToString());
            Console.Read();
        }
    }
}
