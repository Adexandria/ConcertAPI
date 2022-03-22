using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Concert.Application.Interface
{
   public interface IAuth
    {
        string TwoFactorAuthentication(string email);
        bool KeyVerification(string email, string key);
           
    }
}
