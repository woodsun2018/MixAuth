using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MixAuth.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; } = "";

        [DataType(DataType.Password)]
        public string Password { get; set; } = "";
    }
}
