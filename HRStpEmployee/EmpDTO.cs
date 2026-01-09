using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace HRStpEmployee
{
    public class EmpDTO
    {

       // public string EmpGuid { get; set; }

        public string EmpCode { get; set;}

        public string FlfirstName { get; set; }

        public string FlsecondName { get; set; }

        public string FlthirdName { get; set; }

        public string FlfourthName { get; set; }

        public string SlfirstName { get; set; }

        public string SlsecondName { get; set; }

        public string SlthirdName { get; set; }

        public string SlfourthName { get; set; }

        public DateTime? DateofBirth { get; set; }
        public int Age { get; set; }

        public string GenderGuid { get; set; }

        public string? EnglishFullName { get; set; }

        public string? ArabicFullName { get; set; }

    }
}
