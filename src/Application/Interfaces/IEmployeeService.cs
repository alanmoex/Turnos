using Application.Models;
using Application.Models.Requests;

namespace Application.Interfaces
{
    public interface IEmployeeService
    {
        bool Create(EmployeeCreateRequestDTO request);
        bool Delete(int id);
        List<EmployeeResponseDTO?> GetAll();
        List<EmployeeResponseDTO?> GetAllByShopId(int shopId);
        List<EmployeeResponseDTO?> GetAvailables(int shopId);
        EmployeeResponseDTO? GetById(int id);
        bool Update(int id, EmployeeUpdateRequest request);

        void RequestPassReset(string email);
        void ResetPassword(ResetPasswordRequest request);

    }
}