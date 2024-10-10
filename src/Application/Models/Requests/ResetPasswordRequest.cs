using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Requests
{
    public class ResetPasswordRequest
    {
        public string email { get; set; }
        public string NewPassword { get; set; }
        public string Code { get; set; }
    }
}
