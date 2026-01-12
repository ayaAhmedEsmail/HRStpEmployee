using HRStpEmployee.Models;
using HRStpEmployee.Repo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;

namespace HRStpEmployee.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpController(ApplicationDbContext dbContext) : ControllerBase
    {
        //IQueryable emp = dbContext.HrstpEmployees;


        [HttpGet("{empCode}")]
        public async Task<IActionResult> GetByCode(string empCode, CancellationToken cancellation)
        {
            var emp = await dbContext.HrstpEmployees.Where(e => e.EmployeeCode == empCode)
                .Select(
                e => new EmpDTO
                {
                    EmpCode = e.EmployeeCode,
                    FlfirstName = e.FlfirstName,
                    FlsecondName = e.FlsecondName,
                    FlthirdName = e.FlthirdName,
                    FlfourthName = e.FlfourthName,
                    SlfirstName = e.SlfirstName,
                    SlsecondName = e.SlsecondName,
                    SlthirdName = e.SlthirdName,
                    SlfourthName = e.SlfourthName,
                    DateofBirth = e.DateofBirth,
                    Age = e.DateofBirth.HasValue ? DateTime.Now.Year - e.DateofBirth.Value.Year : 0,
                    ArabicFullName = e.getFullArName(),
                    EnglishFullName = e.getFullEngName(),
                }).FirstOrDefaultAsync(cancellation);
            return Ok(emp);
        }

        [HttpGet("get/{pageNum}")]
        public async Task<IActionResult> Get(int pageNum, CancellationToken cancellationToken)
        {
            int Pagesize = 10;
            var result = await PaginationResult<EmpDTO>.CreatePages(dbContext.HrstpEmployees.Select(
                e => new EmpDTO
                {
                    EmpCode = e.EmployeeCode,
                    FlfirstName= e.FlfirstName,
                    FlsecondName= e.FlsecondName,
                    FlthirdName= e.FlthirdName,
                    FlfourthName=e.FlfourthName, 
                    SlfirstName= e.SlfirstName,
                    SlsecondName=  e.SlsecondName,
                    SlthirdName=e.SlthirdName,
                    SlfourthName=e.SlfourthName,
                    DateofBirth= e.DateofBirth,
                    GenderGuid=e.GenderGuid,

                   // Age=e.getAge(),
                    Age= e.DateofBirth.HasValue ? DateTime.Now.Year - e.DateofBirth.Value.Year : 0,
                    ArabicFullName =e.getFullArName(),
                    EnglishFullName=e.getFullEngName(),
                }).OrderBy(e=>e.EmpCode),pageNum,Pagesize,cancellationToken) ;


            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(EmpDTO e, CancellationToken cancellationToken)
        {
            var LastCode = await dbContext.HrstpEmployees.Select(
                e => e.EmployeeCode
                ).MaxAsync(cancellationToken);

            var newCode = (int.Parse(LastCode) + 1).ToString();

            e.EmpCode = newCode;


            var NewEmp = new Employee
            {
                EmployeeGuid = Guid.NewGuid().ToString(),
                EmployeeCode = e.EmpCode,
                FlfirstName = e.FlfirstName,
                FlsecondName = e.FlsecondName,
                FlthirdName = e.FlthirdName,
                FlfourthName = e.FlfourthName,
                SlfirstName = e.SlfirstName,
                SlsecondName = e.SlsecondName,
                SlthirdName = e.SlthirdName,
                SlfourthName = e.SlfourthName,
                DateofBirth = e.DateofBirth,
                ArabicFullName = e.ArabicFullName,
                EnglishFullName = e.EnglishFullName,
                GenderGuid = e.GenderGuid
            };
            
            dbContext.HrstpEmployees.AddAsync(NewEmp,cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);


            NewEmp.ArabicFullName = NewEmp.getFullArName();
            NewEmp.EnglishFullName = NewEmp.getFullEngName();

            //return Ok();

            return CreatedAtAction(nameof(GetByCode), new { empCode = newCode },
                new EmpDTO { EmpCode = newCode,
                    FlfirstName = NewEmp.FlfirstName,
                    FlsecondName = NewEmp.FlsecondName,
                    FlthirdName = NewEmp.FlthirdName,
                    FlfourthName = NewEmp.FlfourthName,
                    SlfirstName = NewEmp.SlfirstName,
                    SlsecondName = NewEmp.SlsecondName,
                    SlthirdName = NewEmp.SlthirdName,
                    SlfourthName=NewEmp.SlfourthName,
                    GenderGuid=NewEmp.GenderGuid,
                    DateofBirth=NewEmp.DateofBirth,
                    Age=NewEmp.DateofBirth.HasValue ? DateTime.Now.Year - e.DateofBirth.Value.Year : 0,
                    EnglishFullName = NewEmp.EnglishFullName,
                    ArabicFullName = NewEmp.ArabicFullName });

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string empCode, CancellationToken cancellationToken)
        {
            await dbContext.HrstpEmployees
               .Where(e => e.EmployeeCode == empCode)
               .ExecuteDeleteAsync(cancellationToken);
            // No need to call SaveChangesAsync when using ExecuteDeleteAsync
            //await dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(EmpDTO dto, string empCode,CancellationToken cancellationToken)
        {

            // Get the existing employee
            var existingEmp = await dbContext.HrstpEmployees
                           .FirstOrDefaultAsync(e => e.EmployeeCode == empCode,cancellationToken);


            // If not found, return NotFound
            if (existingEmp == null) return BadRequest("There's No Employee with this Code");


            // Check if the new employee code already exists for another employee
            var code = await dbContext.HrstpEmployees
                .AnyAsync(x => x.EmployeeCode == dto.EmpCode &&
                          dto.EmpCode != existingEmp.EmployeeCode, cancellationToken);
            if (code) return BadRequest("can't update Emp code. there is Emp with the same Code");

            // Update the employee properties
            if (existingEmp != null)
            {
                existingEmp.EmployeeCode = dto.EmpCode;
                existingEmp.FlfirstName = dto.FlfirstName;
                existingEmp.FlsecondName = dto.FlsecondName;
                existingEmp.FlthirdName = dto.FlthirdName;
                existingEmp.FlfourthName = dto.FlfourthName;
                existingEmp.SlfirstName = dto.SlfirstName;
                existingEmp.SlsecondName = dto.SlsecondName;
                existingEmp.SlthirdName = dto.SlthirdName;
                existingEmp.SlfourthName = dto.SlfourthName;
                existingEmp.DateofBirth = dto.DateofBirth;
                existingEmp.GenderGuid = dto.GenderGuid;

                existingEmp.ArabicFullName = existingEmp.getFullArName();
                existingEmp.EnglishFullName = existingEmp.getFullEngName();

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            dto.ArabicFullName = existingEmp.getFullArName();
            dto.EnglishFullName = existingEmp.getFullEngName();
            return Ok(dto);
        }


    }
}
