using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "invalid Email Address")]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters.")]
        public string Password { get; set; }

        [Required]
        public UserType Type { get; set; }
       
        public Status Status { get; set; } = Status.Active;


        // Almacena el código para recuperar la contraseña
        public string PasswordResetCode { get; set; }

        // Almacenar la fecha y hora de caducidad del código se usa para hacer validaciones.
        public DateTime? ResetCodeExpiration { get; set; }
        public string ImgUrl { get; set; }

        public User() { }

        public User(string name, string email, string password, UserType type)
        {
            Name = name;
            Email = email;
            Password = password;
            Type = type;
        }
    }
}
