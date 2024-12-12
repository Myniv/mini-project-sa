
using CompanyWeb.Domain.Models.Dtos;
using CompanyWeb.Domain.Models.Entities;
using CompanyWeb.Domain.Models.Helpers;
using CompanyWeb.Domain.Models.Requests;
using CompanyWeb.Domain.Models.Requests.Add;
using CompanyWeb.Domain.Models.Requests.Update;
using CompanyWeb.Domain.Services;
using CompanyWeb.WebApi.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CompanyWeb.WebApi.Controllers
{
    public class EmployeesController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<EmployeesController> _logger;


        public EmployeesController(IEmployeeService employeeService, IWebHostEnvironment environment, ILogger<EmployeesController> logger)
        {
            _employeeService = employeeService;
            _environment = environment;
            _logger = logger;

        }

        /// <summary>
        /// Get all employees
        /// </summary>

        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     GET /Employees
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return all employee data </returns>
        // GET: api/Employees
        [Authorize(Roles = "Administrator, HR Manager, Employee Supervisor")]
        [HttpGet]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetEmployees([FromQuery] int pageNumber, int perPage)
        {
            var response = await _employeeService.GetEmployees(pageNumber, perPage);
            if (response == null)
            {
                return null;
            }
            return Ok(response);
        }

        // NEW ======>
        /// <summary>
        /// Get all employees
        /// </summary>

        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     GET /Employees/all
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return all employee data </returns>
        // GET: api/Employees
        // [Authorize(Roles = "Administrator, HR Manager, Employee Supervisor, Department Manager, Employee")]
        [HttpGet("all")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllEmployees()
        {
            var response = await _employeeService.GetAllEmployees();
            if (response == null)
            {
                return null;
            }
            return Ok(response);
        }


        /// <summary>
        /// Get employee data by ID
        /// </summary>

        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     GET /Employees/1
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return employee data by ID </returns>
        // GET: api/Employees/5
        [Authorize(Roles = "Administrator, HR Manager, Employee, Department Manager, Employee Supervisor, Employee")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Employee>> GetEmployee([FromRoute] int id)
        {
            var action = await _employeeService.GetEmployee(id);
            if (action == null)
            {
                return NotFound();
            }
            return Ok(action);
        }


        /// <summary>
        /// Create employee
        /// </summary>

        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     POST /Employees
        ///     {
        ///         "fName": "Zahra",
        ///         "lName": "Safitri",
        ///         "address": "Jakarta, Indonesia",
        ///         "dob": "1998-08-20",
        ///         "sex": "Female",
        ///         "salary": 6000000,
        ///         "ssn": "081-71-XXXX",
        ///         "emailAddress": "zahra@gmail.com",
        ///         "phoneNumber": "082293019931",
        ///         "position": "Staff",
        ///         "deptNo": 1,
        ///         "empType": "Permanent",
        ///         "empLevel": 1,
        ///         "empDependents": [
        ///             {
        ///                 "fName": "Anton",
        ///                 "lName": "Syahputra",
        ///                 "sex": "Male",
        ///                 "birthDate": "2001-12-31",
        ///                 "relation": "Family"
        ///             }
        ///          ]
        ///      }
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return employee data that just created </returns>
        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        //[Authorize(Roles = "Administrator, HR Manager")]
        [HttpPost]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostEmployee([FromBody] AddEmployeeRequest request)
        {
            var action = await _employeeService.CreateEmployee(request);
            if (action == null)
            {
                return NotFound();
            }
            return Ok(action);
        }

        /// <summary>
        /// Update employee data by ID
        /// </summary>

        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     PUT /Employees/1
        ///     {
        ///         "fName": "Zahra",
        ///         "lName": "Safitri",
        ///         "address": "Jakarta, Indonesia",
        ///         "dob": "1998-08-20",
        ///         "sex": "Female",
        ///         "salary": 6000000,
        ///         "ssn": "081-71-XXXX",
        ///         "emailAddress": "zahra@gmail.com",
        ///         "phoneNumber": "082293019931",
        ///         "position": "Staff",
        ///         "deptNo": 1,
        ///         "empType": "Permanent",
        ///         "empLevel": 1,
        ///         "empDependents": [
        ///             {
        ///                 "fName": "Anton",
        ///                 "lName": "Syahputra",
        ///                 "sex": "Male",
        ///                 "birthDate": "2001-12-31",
        ///                 "relation": "Family"
        ///             }
        ///          ]
        ///      }
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return employee data that just updated </returns>
        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Administrator, HR Manager, Employee, Department Manager, Employee Supervisor")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutEmployee([FromRoute] int id, [FromBody] UpdateEmployeeRequest request)
        {
            var action = await _employeeService.UpdateEmployee(id, request);
            if (action == null)
            {
                return NotFound();
            }
            return Ok(action);
        }
        /// <summary>
        /// Delete employee by ID
        /// </summary>

        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     DELETE /Employees/1
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return employee data that just deleted </returns>
        // DELETE: api/Employees/5
        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteEmployee([FromRoute] int id)
        {
            var action = await _employeeService.DeleteEmployee(id);
            if (action == null)
            {
                return NotFound();
            }
            return Ok(action);
        }


        /// <summary>
        /// Search employee by parameters
        /// </summary>

        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     POST /Employees/search?searchBy=dept_keyword=IT_sortBy=name_isDescending=false
        ///     {
        ///         "pageNumber": 1,
        ///         "perPage": 30
        ///     }
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return employee data filtered by search parameters</returns>
        [Authorize(Roles = "Administrator, HR Manager, Employee Supervisor")]
        [HttpPost("search")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchEmployee([FromQuery] SearchEmployeeQuery query, [FromBody] PageRequest pageRequest)
        {
            var response = await _employeeService.SearchEmployee(query, pageRequest);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [Authorize(Roles = "Administrator, HR Manager, Employee Supervisor")]
        [HttpPost("search2")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchEmployee2([FromQuery] SearchEmployeeQuery2 query, [FromBody] PageRequest pageRequest)
        {
            var response = await _employeeService.SearchEmployee2(query, pageRequest);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        /// <summary>
        /// Search employee by parameters
        /// </summary>

        /// <remarks>
        /// 
        /// Sample request:
        ///
        ///     PUT /Employees/14/deactivate
        ///     {
        ///         "deactivateReason": "Left"
        ///     }
        ///     
        /// </remarks>
        /// <param name="request"></param>
        /// <returns> return employee data that just deactivate</returns>
        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpPut("{id}/deactivate")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeactivateEmployee([FromRoute] int id, [FromBody] DeactivateEmployeeRequest request)
        {
            var response = await _employeeService.DeactivateEmployee(id, request);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [Authorize(Roles = "Administrator, HR Manager")]
        [HttpPut("{id}/assign")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignEmployee([FromRoute] int id, [FromBody] AssignEmployeeRequest request)
        {
            var response = await _employeeService.AssignEmployee(id, request.DeptNo);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }


        [Authorize(Roles = "Employee")]
        [HttpPost("leave")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LeaveRequest([FromBody] EmployeeLeaveRequest request)
        {
            var response = await _employeeService.LeaveRequest(request);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [Authorize(Roles = "Employee Supervisor, HR Manager")]
        [HttpPost("leave-approval")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> LeaveApproval([FromBody] EmployeeLeaveApprovalRequest request)
        {
            var response = await _employeeService.LeaveApproval(request);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        //[Authorize(Roles = "")]
        [HttpGet("leave")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllLeaveRequest()
        {
            var response = await _employeeService.GetAllLeaveRequest();
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
        [HttpGet("leave/{id}")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetLeaveRequestId([FromRoute] int id)
        {
            var response = await _employeeService.GetLeaveRequestId(id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // EMPLOYEE REPROT PDF
        [HttpGet("report-pdf/{deptNo}")]
        public async Task<IActionResult> GetEmployeeReportPDF([FromRoute] int deptNo)
        {
            var fileName = "EmployeeReport.pdf";
            var response = await _employeeService.GenerateEmployeeReportPDF(deptNo);
            return File(response, "application/pdf", fileName);
        }

        // EMPLOYEE REPORT JSON
        [HttpGet("report/{deptNo}")]
        public async Task<IActionResult> GetEmployeeReport([FromRoute] int deptNo, [FromQuery] int page)
        {
            var response = await _employeeService.GetEmployeeReport(deptNo, page);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // [HttpPost("upload")]
        // public async Task<IActionResult> Upload(IFormFile file)
        // {
        //     try
        //     {
        //         long MaxFileSize = 2 * 1024 * 1024; // 2MB
        //         string[] AllowedFileTypes = new[] {
        //     "application/pdf",
        //     "application/msword",
        //     "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        // };

        //         // Log the received file details
        //         _logger.LogInformation($"Received file: {file?.FileName}");
        //         _logger.LogInformation($"Received file Content-Type: {file?.ContentType}");
        //         _logger.LogInformation($"Received file size: {file?.Length}");

        //         if (file == null || file.Length == 0)
        //         {
        //             _logger.LogInformation("Error: File is empty.");
        //             return BadRequest("File is empty");
        //         }

        //         if (file.Length > MaxFileSize)
        //         {
        //             _logger.LogInformation("Error: File size exceeds the limit.");
        //             return BadRequest("File size exceeds 2MB limit");
        //         }

        //         if (!AllowedFileTypes.Contains(file.ContentType))
        //         {
        //             _logger.LogInformation($"Error: Unsupported file type. Content-Type: {file.ContentType}");
        //             return BadRequest("Only PDF and Word documents are allowed");
        //         }

        //         string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

        //         // Log the target upload folder
        //         _logger.LogInformation($"Upload folder: {uploadsFolder}");

        //         if (!Directory.Exists(uploadsFolder))
        //         {
        //             _logger.LogInformation("Uploads folder does not exist. Creating it...");
        //             Directory.CreateDirectory(uploadsFolder);
        //         }

        //         string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        //         string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //         // Log the final file path
        //         _logger.LogInformation($"Saving file to: {filePath}");

        //         // Save file to directory
        //         using (var fileStream = new FileStream(filePath, FileMode.Create))
        //         {
        //             await file.CopyToAsync(fileStream);
        //         }

        //         _logger.LogInformation("File uploaded successfully.");
        //         return Ok("File uploaded successfully");
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogInformation($"Internal server error: {ex.Message}");
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {

                long MaxFileSize = 5 * 1024 * 1024; // 5MB

                string[] AllowedFileTypes = new[] {
                    "application/pdf",
                    "application/msword",
                    "image/jpeg",
                    "image/jpg",
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                    };

                if (file == null || file.Length == 0)

                    return BadRequest("File is empty");

                if (file.Length > MaxFileSize)

                    return BadRequest("File size exceeds 5MB limit");

                if (!AllowedFileTypes.Contains(file.ContentType))

                    return BadRequest("Only PDF, Word, Jpg and Jpeg documents are allowed");

                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))

                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Save file to directory

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return Ok("File uploaded succesfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }


    }
}
