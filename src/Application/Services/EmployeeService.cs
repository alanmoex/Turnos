using Application.Interfaces;
using Application.Models;
using Application.Models.Requests;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRepositoryUser _userRepository;
        private readonly IEmailService _emailService;

        public EmployeeService(IEmployeeRepository employeeRepository, IRepositoryUser userRepository, IEmailService emailService)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public bool Create(EmployeeCreateRequestDTO request)
        {
            if (request.Name == null || request.Email == null || request.Password == null || request.Password == null)
            {
                return false; //no se puede crear
            }

            var newEmployee = new Employee();
            newEmployee.Name = request.Name;
            newEmployee.Email = request.Email;
            newEmployee.Password = request.Password;
            newEmployee.ShopId = request.ShopId;
            newEmployee.Type = Domain.Enums.UserType.Employee;

            try
            {
                _employeeRepository.Add(newEmployee);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public bool Delete(int id)
        {
            var employee = _employeeRepository.GetById(id);

            if (employee == null)
            {
                return false;
            }

            try
            {
                _employeeRepository.Delete(employee);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public List<EmployeeResponseDTO?> GetAll()
        {
            try
            {
                var list = _employeeRepository.GetAll();

                return EmployeeResponseDTO.CreateList(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public List<EmployeeResponseDTO?> GetAllByShopId(int shopId)
        {
            try
            {
                var list = _employeeRepository.GetAllByShopId(shopId);

                return EmployeeResponseDTO.CreateList(list);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public EmployeeResponseDTO? GetById(int id)
        {
            var employee = _employeeRepository.GetById(id);

            return EmployeeResponseDTO.Create(employee);

        }

        public List<EmployeeResponseDTO?> GetAvailables(int shopId)
        {
            try
            {
                var appointments = _employeeRepository.GetAvailables(shopId);

                if (appointments != null)
                {
                    List<EmployeeResponseDTO?> employeesAvailable = new List<EmployeeResponseDTO>();

                    foreach (var a in appointments)
                    {
                        var employee = _employeeRepository.GetById(a.ProviderId);
                        var employeeToAdd = EmployeeResponseDTO.Create(employee);
                        employeesAvailable.Add(employeeToAdd);
                    };
                    return employeesAvailable;
                }
                return null;
            }
            catch (Exception ex)
            {
               throw new Exception(ex.ToString());
            }

        }

        public bool Update(int id, EmployeeUpdateRequest request)
        {
            var employee = _employeeRepository.GetById(id);

            if (employee == null) return false;

            employee.Email = request.Email;
            employee.Name = request.Name;
            employee.Password = request.Password;

            try
            {
                _employeeRepository.Update(employee);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }

        public void RequestPassReset(string email) //Pide la clave para cambiar su contraseña
        {
            var user = _userRepository.GetByEmail(email)
                ?? throw new NotFoundException($"{email} is not registered");
            //genero un codigo de 6 digitos para recuperar la pass
            //GUID: valor único de 16 bytes, substring: extrae los 6 primeros caracteres.
            var resetCode = Guid.NewGuid().ToString().Substring(0, 6);

            // el tiempo de expiración del codigo 15 minutos
            var expirationTime = DateTime.UtcNow.AddMinutes(15);

            //se guarda los datos en la bd
            _userRepository.SavePassResetCode(email, resetCode, expirationTime);

            //Se envia mail con el pass para recuperar la resetear la contraseña.
            _emailService.SendPasswordRestCode(email, resetCode, user.Name);
        }

        public void ResetPassword(ResetPasswordRequest request) //cambia su contraseña con la clabe pedida
        {
            var user = _userRepository.GetByEmail(request.email)
                ?? throw new NotFoundException($"{request.email} is not registered");

            //Valida si expiro el codigo
            if (DateTime.UtcNow > user.ResetCodeExpiration)
            {
                throw new Exception("the password recovery code has expired");
            }

            if (request.Code != user.PasswordResetCode)
            {
                throw new Exception("The recovery code is not correct ");
            }

            if (!ValidatePassword(request.NewPassword))
            {
                throw new Exception("The password does not meet requirements.");
            }

            var userUpdateDto = new EmployeeUpdateRequest();
            userUpdateDto.Password = request.NewPassword;
            userUpdateDto.Name = user.Name;
            userUpdateDto.Email = user.Email;

            Update(user.Id, userUpdateDto);
            _emailService.changePassword(user.Email, user.Name);
        }

        private bool ValidatePassword(string password)
        {
            //comprobamos si la contraseña es nula o tiene menos de 8 caracteres
            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                return false;
            }

            /*con esta expresión regular verificaremos que la contraseña contenga al menos una letra y un número*/
            string pattern = @"^(?=.*[a-zA-Z])(?=.*\d).+$";
            //la siguiente función devolverá true si hay match, y false en caso contrario
            return Regex.IsMatch(password, pattern);
        }
    }
}
