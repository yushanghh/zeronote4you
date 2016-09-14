using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_WYSIWYG_HTML_Editor;

namespace ForYou.ForYouControl
{
    static class User
    {
        public static String UserId;
        public static List<object> UserGroup;
        public static List<object> UserNote;
        public static int NextGroupNum;
        public static int NextNoteNum;
        public static string SelectGroup;
        public static String UserPath;//用户文件夹的所在的路径
        public static WebEditor rtbContent;
    }
}
