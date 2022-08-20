using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VeliErdemCoskun_Hafta2.Controllers
{
    public class CommonResponse<Entity>
    {
        public CommonResponse()
        {

        }
        public CommonResponse(Entity data)
        {
            Data = data;
        }
        public CommonResponse(string error)
        {
            Error = error;
            Success = false;
        }

        public bool Success { get; set; } = true;
        public string Error { get; set; }
        public Entity Data { get; set; }
    }

    public class Employee
    {        
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength: 120, MinimumLength = 5, ErrorMessage = "Name must be range in 5-120 characters.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]+$", ErrorMessage = "Please don't use special characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 120, MinimumLength = 5, ErrorMessage = "Last name must be range in 5-120 characters.")]
        [RegularExpression(@"^[a-zA-Z''-'\s]+$", ErrorMessage = "Please don't use special characters.")]
        public string LastName { get; set; }

        [Range(type: typeof(DateTime), minimum: "11/11/1945", maximum: "10/10/2002", ErrorMessage = "Date of birth bust be between 11/11/1945 and 10/10/2002")]
        public string DateOfBirth { get; set; }

        [EmailAddress(ErrorMessage = "Email adress is not valid.")]
        [RegularExpression(@"^[a-z\s]+@[a-z\s]+\.[a-z\s]+$", ErrorMessage = "Please don't use uppercase letters, numbers and special characters.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Phone number is not valid")]
        [RegularExpression(@"^[?!0+$]+[0-9\s]+$", ErrorMessage = "Please use area code with '+'.")]
        public string PhoneNumber { get; set; }

        [Range(minimum: 2000, maximum: 9000, ErrorMessage = "Invalid salary input. Salary must be in range 2000-9000")]
        public int Salary { get; set; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
    {
        public StaffController()
        {

        }

        private CommonResponse<List<Employee>> GetList()
        {
            List<Employee> list = new();
            list.Add(new Employee
            {
                Id = 1,
                Name = "Erdem",
                LastName = "Coskun",
                DateOfBirth = "17/09/1999",
                Email = "erdemcoskun@gmail.com",
                PhoneNumber = "+905376902763",
                Salary = 5000
            });
            list.Add(new Employee
            {
                Id = 2,
                Name = "Batu",
                LastName = "Yilmaz",
                DateOfBirth = "17/10/1989",
                Email = "batuyilmaz@gmail.com",
                PhoneNumber = "+905355502954",
                Salary = 6000
            });
            list.Add(new Employee
            {
                Id = 3,
                Name = "Ayşe",
                LastName = "Demir",
                DateOfBirth = "01/06/1993",
                Email = "ayse@demir.com",
                PhoneNumber = "+905891687546",
                Salary = 9000
            });
            return new CommonResponse<List<Employee>>(list);
        }

        [HttpGet]
        [Route("GetAll")]
        public CommonResponse<List<Employee>> GetAll()
        {
            return GetList();
        }

        [HttpGet]
        [Route("GetById{id}")]
        public CommonResponse<Employee> GetById([FromRoute] int id)
        {
            CommonResponse<List<Employee>> list = GetList();
            Employee employee = list.Data.Where(x => x.Id == id).FirstOrDefault();
            return new CommonResponse<Employee>(employee);
        }

        [HttpPost]
        [Route("RegisterStaff")]
        public CommonResponse<List<Employee>> RegisterStaff([FromBody] Employee employee)
        {
            var list = GetList().Data;
            list.Add(employee);
            return new CommonResponse<List<Employee>>(list);
        }

        [HttpPut]
        [Route("UpdateStaffInformations")]
        public CommonResponse<List<Employee>> UpdateStaffInformations(int id, [FromBody] Employee request)
        {
            List<Employee> list = GetList().Data;
            Employee employee = list.Where(x => x.Id == id).FirstOrDefault();
            list.Remove(employee);
            request.Id = id;
            list.Add(request);
            return new CommonResponse<List<Employee>>(list.ToList());
        }

        [HttpDelete]
        [Route("Delete{id}")]
        public CommonResponse<List<Employee>> Delete([FromRoute] int id)
        {
            List<Employee> list = GetList().Data;
            Employee employee = list.Where(x => x.Id == id).FirstOrDefault();
            list.Remove(employee);
            return new CommonResponse<List<Employee>>(list);
        }
    }
}
