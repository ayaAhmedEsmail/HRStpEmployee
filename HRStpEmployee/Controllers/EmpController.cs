using HRStpEmployee.Models;
using HRStpEmployee.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRStpEmployee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpController(IEmpRepo emp) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get(int pageNum)
        {
            int Pagesize = 10;
            var employees = await emp.GetAllEmp(pageNum, Pagesize);
            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Employee e)
        {
            var createdEmp = await emp.CreateEmp(e);
            return CreatedAtAction(nameof(Get), new { empCode = createdEmp.EmployeeCode }, createdEmp);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string empCode)
        {
            await emp.DeleteEmp(empCode);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(EmpDTO dto)
        {
            string EmpCode = dto.EmpCode;
            var employee =await emp.GetEmpByCode(EmpCode);

            if (employee == null) return NotFound();

            employee.EmployeeCode=EmpCode;
            employee.EnglishFullName= dto.EnglishFullName;
            employee.ArabicFullName= dto.ArabicFullName;
            employee.DateofBirth= dto.DateofBirth;
            employee.GenderGuid= dto.GenderGuid;
            employee.FlfirstName = dto.FlfirstName;
            employee.FlsecondName= dto.FlsecondName;
            employee.FlthirdName= dto.FlthirdName;
            employee.FlfourthName= dto.FlfourthName;
            employee.SlfirstName= dto.SlfirstName;
            employee.SlsecondName= dto.SlsecondName;
            employee.SlthirdName= dto.SlthirdName;
            employee.SlfourthName= dto.SlfourthName;

            employee.ArabicFullName= dto.ArabicFullName = employee.getFullArName();
            employee.EnglishFullName= dto.EnglishFullName = employee.getFullEngName();



            employee= await emp.UpdateEmp(employee);
               return Ok(dto);
        }


    }
}
