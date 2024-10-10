using Domain.Interface;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepositoryUser: IRepositoryBase<User>
    {
        User? GetByEmail(string email);
        void SavePassResetCode(string email, string resetCode, DateTime expiration);
        User GetByPassResetCode(string resetCode);
    }
}
