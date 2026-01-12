using HRStpEmployee;
using HRStpEmployee.Models;

public static class EmpExtensions
{
    public static int getAge(this Employee employee)
    {

        var date = DateTime.Now;
        var dob = employee.DateofBirth;
        if (dob == null) return 0;

        var age = date.Year - dob.Value.Year;
        return age;
    }
    public static string getFullEngName(this Employee q)
    {
       return q.EnglishFullName= $"{q.FlfirstName} {q.FlsecondName} {q.FlthirdName} {q.FlfourthName}";
    }
    public static string getFullArName(this Employee q)
    {
       return q.EnglishFullName= $"{q.SlfirstName} {q.SlsecondName} {q.SlthirdName} {q.SlfourthName}";
    }


}

