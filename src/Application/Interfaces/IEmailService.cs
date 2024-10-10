using Application.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailService
    {

        void SendMail(EmailDTO request);
        void AccountCreationConfirmationEmail(string addressee, string nameUser);
        void SendPasswordRestCode(string addressee, string resetCode,string userName);
        public void changePassword(string addressee, string userName);
    }
}
