using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserType Type { get; set; }
        public Status Status { get; set; }
        public string ImgUrl { get; set; }

        public static ClientDto Create(Client client)
        {
            var dto = new ClientDto();
            dto.Id = client.Id;
            dto.Name = client.Name;
            dto.Email = client.Email;
            dto.Type = client.Type;
            dto.ImgUrl = client.ImgUrl;
            return dto;
        }

        public static List<ClientDto?> CreateList(IEnumerable<Client> clients)
        {
            List<ClientDto?> listDto = [];

            foreach (var c in clients)
            {
                listDto.Add(Create(c));
            }

            return listDto;
        }
    }
}
