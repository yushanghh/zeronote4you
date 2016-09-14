using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ForYou.translate
{
    [DataContract]//序列化 
    public class send_info
    {
        [DataMember]//指定是json转换的对象
        private string user_name { set; get; }
        [DataMember]
        private string user_password { set; get; }
    }
}
