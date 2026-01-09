using HRStpEmployee.Models;

namespace HRStpEmployee.Repo
{
    public interface IEmpRepo
    {
        Task<PaginationResult<EmpDTO>> GetAllEmp(int currntPage, int size);
        Task<Employee> GetEmpByCode(string empCode);
        Task<Employee> CreateEmp(Employee emp);
        Task DeleteEmp(string empCode);
        Task<Employee> UpdateEmp(Employee emp);
    }
}
