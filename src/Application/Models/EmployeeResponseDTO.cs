using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class EmployeeResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public UserType Type { get; set; }

        public string Status { get; set; }
        public string ImgUrl { get; set; }


        public static EmployeeResponseDTO? Create(Employee? employee)
        {
            if (employee == null) return null;

            var dto = new EmployeeResponseDTO();
            dto.Id = employee.Id;
            dto.Name = employee.Name;
            dto.Email = employee.Email;
            dto.Type = employee.Type;
            dto.Status = employee.Status.ToString();
            dto.ImgUrl = employee.ImgUrl;

            return dto;
        }

        public static List<EmployeeResponseDTO?> CreateList(IEnumerable<Employee> employees)
        {
            if (employees == null) return null;

            List<EmployeeResponseDTO?> listDto = [];

            foreach (var e in employees)
            {
                listDto.Add(Create(e));
            }

            return listDto;
        }
    }
}
