using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;

namespace Domain.Entities
{
    public class Shop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public ShopType Type { get; set; }

        public Status Status { get; set; } = Status.Active;

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public bool IsPremium { get; set; }

        public int AppoimentFrecuence { get; set; } //en minutos

        public TimeOnly TimeStart { get; set; }

        public TimeOnly TimeEnd { get; set; }

        public List<Days> WorkDays { get; set; }

        public string ImgUrl { get; set; }

        public Shop() { }

        public Shop(string name, string address, string phone, string email, ShopType type, int appoimentFrecuence, int startHour, int startMin, int endHour, int endMin, List<Days> workDays, string imgUrl)
        {
            Name = name;
            Type = type;
            Address = address;
            Phone = phone;
            Email = email;
            IsPremium = false;
            AppoimentFrecuence = appoimentFrecuence;
            TimeStart = new TimeOnly(startHour, startMin);
            TimeEnd = new TimeOnly(endHour, endMin);
            WorkDays = new List<Days>(workDays);
            ImgUrl = imgUrl;
        }
    }
}
