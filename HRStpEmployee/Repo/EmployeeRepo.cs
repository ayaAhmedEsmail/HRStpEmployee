using HRStpEmployee.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HRStpEmployee.Repo
{
    public class EmployeeRepo: IEmpRepo
    {


        public readonly ApplicationDbContext context;


        public EmployeeRepo(ApplicationDbContext dbContext) {


            context = dbContext;
           var _employee = dbContext.HrstpEmployees;
        }



        public async Task<Employee> CreateEmp(Employee e)
        {


            e.EmployeeGuid = Guid.NewGuid().ToString();

            var LastCode = 
                context.HrstpEmployees
                .Select(e => e.EmployeeCode)
                .MaxAsync();
            var code= int.Parse( await LastCode) + 1;

            e.EmployeeCode = code.ToString();


            e.ArabicFullName= e.getFullArName();
            e.EnglishFullName= e.getFullEngName();


            var emp =  await context.HrstpEmployees.AddAsync(e);
            await context.SaveChangesAsync();

            return e;
        }

        public async Task DeleteEmp(string empCode)
        {
             await context.HrstpEmployees
                .Where(e => e.EmployeeCode == empCode)
                .ExecuteDeleteAsync();
            await context.SaveChangesAsync();
            
        }

        public Task<PaginationResult<EmpDTO>> GetAllEmp(int currntPage, int size)
        {
            var res = PaginationResult<EmpDTO>.CreatePages(
                context.HrstpEmployees.Select( e=> new EmpDTO { 
                    EmpCode= e.EmployeeCode,
                    FlfirstName = e.FlfirstName,
                    FlsecondName = e.FlsecondName,
                    FlthirdName = e.FlthirdName,
                    FlfourthName = e.FlfourthName,
                    SlfirstName = e.SlfirstName,
                    SlsecondName = e.SlsecondName,
                    SlthirdName = e.SlthirdName,
                    SlfourthName = e.SlfourthName,
                    DateofBirth = e.DateofBirth,
                    Age = e.getAge(),
                    GenderGuid = e.GenderGuid,
                    EnglishFullName = e.getFullEngName(),
                    ArabicFullName = e.getFullArName(),
                } ).OrderBy(e=>e.EmpCode), currntPage, pageSize:size);


            return res;
        }

        public async Task<Employee> GetEmpByCode(string empCode)
        {
            return await context.HrstpEmployees
                .FirstOrDefaultAsync(e => e.EmployeeCode == empCode);
        }

        public async Task<Employee> UpdateEmp(Employee emp)
        {
            
            var existingEmp = await context.HrstpEmployees
                .FirstOrDefaultAsync(e => e.EmployeeGuid == emp.EmployeeGuid);

            if (existingEmp == null) throw new Exception("Employee not found");

            var codeExist = await context.HrstpEmployees
                .AnyAsync(x => x.EmployeeCode == emp.EmployeeCode && 
                          x.EmployeeCode != existingEmp.EmployeeCode);
            
            if (codeExist) throw new Exception("can't update Emp code. there is Emp with the same Code");
            
            if (existingEmp != null)
            {
                existingEmp.EmployeeCode = emp.EmployeeCode;
                existingEmp.FlfirstName = emp.FlfirstName;
                existingEmp.FlsecondName = emp.FlsecondName;
                existingEmp.FlthirdName = emp.FlthirdName;
                existingEmp.FlfourthName = emp.FlfourthName;
                existingEmp.SlfirstName = emp.SlfirstName;
                existingEmp.SlsecondName = emp.SlsecondName;
                existingEmp.SlthirdName = emp.SlthirdName;
                existingEmp.SlfourthName = emp.SlfourthName;
                existingEmp.DateofBirth = emp.DateofBirth;
                existingEmp.GenderGuid = emp.GenderGuid;

                existingEmp.ArabicFullName= existingEmp.getFullArName();
                existingEmp.EnglishFullName= existingEmp.getFullEngName();
                
                await context.SaveChangesAsync();
            }

            return existingEmp;

        }
    }
}