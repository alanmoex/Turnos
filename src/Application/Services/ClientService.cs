using Application.Models.Requests;
using Application.Models;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Interfaces;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Data;
using System.Xml.Linq;

namespace Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IRepositoryUser _userRepository;
        private readonly IEmailService _emailService;

        public ClientService(IClientRepository clientRepository, IRepositoryUser userRepository, IEmailService emailService)
        {
            _clientRepository = clientRepository;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public List<ClientDto?> GetAllClients()
        {
            var clientsList = _clientRepository.GetAll();

            return ClientDto.CreateList(clientsList);
        }

        public ClientDto? GetClientById(int id)
        {
            var client = _clientRepository.GetById(id);

            if (client == null)
                throw new NotFoundException(nameof(Client), id);

            return ClientDto.Create(client);
        }

        public ClientDto CreateNewClient(ClientCreateRequest clientCreateRequest)
        {
            bool validationFlag = ValidatePassword(clientCreateRequest.Password);
            if (validationFlag)
            {
                validationFlag = ValidateName(clientCreateRequest.Name);
                if (validationFlag)
                {
                    validationFlag = ValidateEmail(clientCreateRequest.Email);
                    if (validationFlag)
                    {
                        var existentUser = _userRepository.GetByEmail(clientCreateRequest.Email);

                        if (existentUser != null)
                        {
                            throw new Exception("El email que intenta utilizar ya existe");
                        }
                    }
                }
            }

            if (validationFlag)
            {
                var newClient = new Client();

                newClient.Name = clientCreateRequest.Name;
                newClient.Email = clientCreateRequest.Email;
                newClient.Password = clientCreateRequest.Password;
                newClient.Type = UserType.Client;
                _clientRepository.Add(newClient);
                _emailService.AccountCreationConfirmationEmail(newClient.Email, newClient.Name);
                return ClientDto.Create(newClient);
            }
            else
            {
                throw new ValidationException("Los datos ingresados no son válidos");
            }
        }

        public void ModifyClientData(int id, ClientUpdateRequest clientUpdateRequest)
        {
            var client = _clientRepository.GetById(id);

            if (!string.IsNullOrEmpty(clientUpdateRequest.Name.Trim())) client.Name = clientUpdateRequest.Name;

            if (!string.IsNullOrEmpty(clientUpdateRequest.Password.Trim()))
            {
                if (ValidatePassword(clientUpdateRequest.Password))
                {
                    client.Password = clientUpdateRequest.Password;
                }
                else
                {
                    throw new ValidationException("Los datos ingresados no son válidos");
                }
            } 

            _clientRepository.Update(client);
        }

        public void PermanentDeletionClient(int id)
        {
            var client = _clientRepository.GetById(id);

            if (client == null)
                throw new NotFoundException(nameof(Client), id);

            _clientRepository.Delete(client);
        }

        public void LogicalDeletionClient(int id)
        {
            var client = _clientRepository.GetById(id);

            if (client == null)
                throw new NotFoundException(nameof(Client), id);

            if (client.Status == Status.Inactive)
            {
                throw new Exception("El cliente especificado ya se encuentra inactivo");
            }

            client.Status = Status.Inactive;
            _clientRepository.Update(client);
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
            _userRepository.SavePassResetCode(email,resetCode,expirationTime);

            //Se envia mail con el pass para recuperar la resetear la contraseña.
            _emailService.SendPasswordRestCode(email, resetCode, user.Name);
        }

        public void ResetPassword (ResetPasswordRequest request) //cambia su contraseña con la clabe pedida
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

            var userUpdateDto = new ClientUpdateRequest();
            userUpdateDto.Password = request.NewPassword;
            userUpdateDto.Name = user.Name;

            ModifyClientData(user.Id, userUpdateDto);
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
        private bool ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name.Trim()))
            {
                return false;
            } 
            return true;
        }
        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email.Trim()))
            { 
                return false;
            }
            else
            {
                try
                {
                    MailAddress mail = new MailAddress(email);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }
        }
    }
}
