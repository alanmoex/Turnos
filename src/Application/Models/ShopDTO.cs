using Domain.Entities;
using Domain.Enums;

namespace Application.Models
{
    public class ShopDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ShopType Type { get; set; }
        public Status Status { get; set; }
        public string Address { get; set; } 
        public string Phone { get; set; }    
        public string Email { get; set; }
        public bool IsPremium { get; set; }
        public int AppoimentFrecuence { get; set; }
        public TimeOnly TimeStart { get; set; }
        public TimeOnly TimeEnd { get; set; }
        public List<Days> WorkDays { get; set; }
        public string ImgUrl { get; set; }

        public static ShopDTO Create(Shop shop)
        {
            var dto = new ShopDTO();
            dto.Id = shop.Id;
            dto.Name = shop.Name;
            dto.Type = shop.Type;
            dto.Status = shop.Status;
            dto.Address = shop.Address;
            dto.Phone = shop.Phone;
            dto.Email = shop.Email;
            dto.IsPremium = shop.IsPremium;
            dto.AppoimentFrecuence = shop.AppoimentFrecuence;
            dto.TimeStart = shop.TimeStart;
            dto.TimeEnd = shop.TimeEnd;
            dto.WorkDays = shop.WorkDays;
            dto.ImgUrl = shop.ImgUrl;
            
            return dto;
        }

        public static List<ShopDTO?> CreateList(IEnumerable<Shop> shops)
        {
            List<ShopDTO?> listDto = [];

            foreach (var s in shops)
            {
                listDto.Add(Create(s));
            }

            return listDto;
        }

    }
}
