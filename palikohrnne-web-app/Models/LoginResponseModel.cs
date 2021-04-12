using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace palikohrnne_web_app.Models
{
    public class LoginResponseModel
    {
        public int code { get; set; }
        public string expire { get; set; }
        public string token { get; set; }
    }
}
