using Application.Models.Requests;
using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IClientService
    {
        ClientDto? GetClientById(int id);

        List<ClientDto?> GetAllClients();

        ClientDto CreateNewClient(ClientCreateRequest clientCreateRequest);

        void ModifyClientData(int id, ClientUpdateRequest clientUpdateRequest);

        void PermanentDeletionClient(int id);

        void LogicalDeletionClient(int id);

        void RequestPassReset(string email);

        void ResetPassword(ResetPasswordRequest request);
    }
}
