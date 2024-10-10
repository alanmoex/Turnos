using Domain.Entities;
using Domain.Exceptions;
using Domain.Interface;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class RepositoryUser : RepositoryBase<User>, IRepositoryUser
    {
        private readonly AppDbContext _context;
        public RepositoryUser(AppDbContext context): base(context)
        {
            _context = context;
        }   
        
        public User? GetByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            return user;
        }

        public void SavePassResetCode(string email, string resetCode, DateTime expiration) //Guarda el nuevo codigo para recuperar la contraseña
        {
            var user = GetByEmail(email)
                ?? throw new Exception("User not found");
            user.PasswordResetCode = resetCode;
            user.ResetCodeExpiration = expiration;
            _context.SaveChanges();
        }
        public User GetByPassResetCode (string resetCode) //nos da el usuiaro cuando ponemos la clave de recuperar la contraseña
        {
            var user = _context.Users.FirstOrDefault(u => u.PasswordResetCode == resetCode)
                ?? throw new Exception("User not found");
            return user;
        }

    }
}
