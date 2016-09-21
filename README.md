# Roc.Validation
一个基于反射,用于.Net后台验证的项目

实体 
 [Required(Name = "用户ID")]
 public int? UserId { get; set; }

 [Order(20)]
 [StringLength(10, CompareType.LessEqualThan, Name = "用户名")]
 public string Name { get; set; }

 [Range(16, 100, CompareType.Between, Name = "年龄", ErrorMsg = "{Name}字段必须大于{Min}并且小于{Max}")]
 public int Age { get; set; }

 [Mobile(Name = "手机号")]
 public string Phone { get; set; }
 
 验证代码 

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
