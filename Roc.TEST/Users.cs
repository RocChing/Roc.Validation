using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Roc.Validation;

namespace Roc.TEST
{
    public class Users
    {
        [Required(Name = "用户ID")]
        public int? UserId { get; set; }

        [Order(20)]
        [StringLength(10, CompareType.LessEqualThan, Name = "用户名")]
        public string Name { get; set; }

        [Range(16, 100, CompareType.Between, Name = "年龄", ErrorMsg = "{Name}字段必须大于{Min}并且小于{Max}")]
        public int Age { get; set; }

        [Mobile(Name = "手机号")]
        public string Phone { get; set; }

        [Email(Name = "邮箱", Order = 30)]
        public string Email { get; set; }

        [Max(5000, Name = "薪资")]
        public double Money { get; set; }

        [StringLength(10, 200, Name = "描述", ErrorMsg = "{Name}字段长度必须介于{Min}和{Max}之间")]
        public string Desc { get; set; }

        [Required(Name = "创建时间")]
        public DateTime? CreateTime { get; set; }
    }
}
