using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities.Messages
{
    public enum ErrorMessageCode
    {
        UsernameAlreadyExist = 101,
        EmailAlreadyExist = 102,
        UserIsNotActive = 151,
        UsernameOrPassWrong = 152,
        CheckYourEmail = 153
    }
}
