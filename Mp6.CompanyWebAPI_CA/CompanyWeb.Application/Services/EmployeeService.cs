using CompanyWeb.Application.Mappers;
using CompanyWeb.Domain.Models.Auth;
using CompanyWeb.Domain.Models.Dtos;
using CompanyWeb.Domain.Models.Entities;
using CompanyWeb.Domain.Models.Helpers;
using CompanyWeb.Domain.Models.Options;
using CompanyWeb.Domain.Models.Requests;
using CompanyWeb.Domain.Models.Requests.Add;
using CompanyWeb.Domain.Models.Requests.Update;
using CompanyWeb.Domain.Models.Responses.Employee;
using CompanyWeb.Domain.Repositories;
using CompanyWeb.Domain.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CompanyWeb.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ICompanyService _companyService;
        private readonly IDepartementRepository _departementRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeDependentRepository _employeeDependentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CompanyOptions _companyOptions;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeService(ICompanyService companyService,
            IDepartementRepository departementRepository,
            IEmployeeRepository employeeRepository,
            IEmployeeDependentRepository employeeDependentRepository,
            IOptions<CompanyOptions> companyOptions,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _companyService = companyService;
            _departementRepository = departementRepository;
            _employeeRepository = employeeRepository;
            _employeeDependentRepository = employeeDependentRepository;
            _companyOptions = companyOptions.Value;
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<object> AssignEmployee(int id, int deptNo)
        {
            var emp = await _employeeRepository.GetEmployee(id);
            if (emp == null)
            {
                return null;
            }
            emp.Deptno = deptNo;
            await _employeeRepository.Update(emp);


            var dependents = await _employeeDependentRepository.GetEmployeeDependentByEmpNo(id);
            return emp.ToEmployeeResponse(dependents);
        }

        public async Task<object> CreateEmployee(AddEmployeeRequest request)
        {
            var response = CreateResponse();
            var newEmp = new Employee()
            {
                Fname = request.Fname,
                Lname = request.Lname,
                Dob = request.Dob,
                Sex = request.Sex,
                Address = request.Address,
                EmailAddress = request.EmailAddress,
                PhoneNumber = request.PhoneNumber,
                Position = request.Position,
                Deptno = request.Deptno,
                EmpLevel = request.EmpLevel,
                EmpType = request.EmpType,
                Ssn = request.Ssn,
                Salary = request.Salary,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                DirectSupervisor = request.DirectSupervisor,
                UpdatedAt = null,
            };
            int deptId = await _departementRepository.GetDepartementIdByName("IT");
            if (request.Deptno == deptId)
            {
                int memberCount = _companyService.GetEmployeeIT().Result.Count;
                if (memberCount >= _companyOptions.MaxDepartementMemberIT)
                {
                    response.Message = $"IT Employee full ({memberCount}/9). Please try again";
                    return response;
                }
            }

            var data = await _employeeRepository.Create(newEmp);

            foreach (var item in request.EmpDependents)
            {
                var newEmpDependent = new EmployeeDependent()
                {
                    DependentEmpno = data.Empno,
                    BirthDate = item.BirthDate,
                    Fname = item.Fname,
                    Lname = item.Lname,
                    Sex = item.Sex,
                    Relation = item.Relation
                };
                await _employeeDependentRepository.Create(newEmpDependent);
            }

            var dependents = await _employeeDependentRepository.GetEmployeeDependentByEmpNo(data.Empno);

            response.Status = true;
            response.Message = "Success";
            response.Data = data.ToEmployeeResponse(dependents);
            return response;
        }

        public async Task<object> DeactivateEmployee(int id, DeactivateEmployeeRequest request)
        {
            var emp = await _employeeRepository.GetEmployee(id);
            if (emp == null || emp.IsActive == false)
            {
                return null;
            }
            if (emp.IsActive == true)
            {
                emp.IsActive = false;
                emp.UpdatedAt = DateTime.UtcNow;
                emp.DeactivateReason = request.DeactivateReason;
            }

            var response = await _employeeRepository.Update(emp);
            var dependents = await _employeeDependentRepository.GetEmployeeDependentByEmpNo(id);
            return response.ToEmployeeResponse(dependents);

        }

        public async Task<object> DeleteEmployee(int id)
        {
            var e = await _employeeRepository.GetEmployee(id);
            if (e == null)
            {
                return null;
            }
            var response = await _employeeRepository.Delete(e);
            var dependents = await _employeeDependentRepository.GetEmployeeDependentByEmpNo(id);
            await _employeeDependentRepository.Delete(id);

            var user = await _userManager.FindByEmailAsync(e.EmailAddress);
            if (user == null)
            {
                return null;
            }
            await _userManager.DeleteAsync(user);

            return response.ToEmployeeResponse(dependents);
        }

        public async Task<object> GetEmployee(int id)
        {
            var emp = await _employeeRepository.GetEmployee(id);
            var employee = await _employeeRepository.GetAllEmployees();
            var dependents = await _employeeDependentRepository.GetEmployeeDependentByEmpNo(id);
            return emp.ToEmployeeDetailResponse(dependents);
        }

        public async Task<List<object>> GetEmployees(int pageNumber, int perPage)
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);

            var employees = await _employeeRepository.GetEmployees(pageNumber, perPage);

            var allEmployees = await _employeeRepository.GetAllEmployees();
            var userRequest = allEmployees.Where(w => w.AppUserId == user.Id).FirstOrDefault();


            if (roles.Any(x => x == "Employee Supervisor"))
            {
                return employees
               .Where(w => w.DirectSupervisor == userRequest.Empno)
               .Select(s => s.ToEmployeeResponse(
                   _employeeDependentRepository.GetEmployeeDependentByEmpNo(s.Empno).Result
                   ))
               .ToList<object>();
            }

            return employees
                .Select(s => s.ToEmployeeResponse(
                    _employeeDependentRepository.GetEmployeeDependentByEmpNo(s.Empno).Result
                    ))
                .ToList<object>();
        }

        public async Task<List<object>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployees();
            return employees.Select(s => s.ToEmployeeResponse(null)).ToList<object>();

        }

        public async Task<List<EmployeeSearchResponse>> SearchEmployee(SearchEmployeeQuery query, PageRequest pageRequest)
        {
            var employees = await _employeeRepository.GetAllEmployees();

            bool isKeyWord = !string.IsNullOrWhiteSpace(query.KeyWord);
            bool isSearchBy = !string.IsNullOrWhiteSpace(query.SearchBy);
            bool isSort = !string.IsNullOrWhiteSpace(query.SortBy);

            Console.WriteLine(query.KeyWord);
            if (isKeyWord && isSearchBy)
            {
                if (query.SearchBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    employees = employees
                   .Where(x => x.Fname.ToLower().Contains(query.KeyWord.ToLower())
                   || x.Lname.ToLower().Contains(query.KeyWord.ToLower()));
                };
                if (query.SearchBy.Equals("dept", StringComparison.OrdinalIgnoreCase))
                {
                    employees = employees
                   .Where(x => x.DeptnoNavigation.Deptname.ToLower().Contains(query.KeyWord.ToLower()));
                };
                if (query.SearchBy.Equals("position", StringComparison.OrdinalIgnoreCase))
                {
                    employees = employees
                   .Where(x => x.Position.ToLower().Contains(query.KeyWord.ToLower()));
                };
                if (query.SearchBy.Equals("level", StringComparison.OrdinalIgnoreCase))
                {
                    employees = employees
                   .Where(x => x.EmpLevel == int.Parse(query.KeyWord));
                };
                if (query.SearchBy.Equals("type", StringComparison.OrdinalIgnoreCase))
                {
                    employees = employees
                   .Where(x => x.EmpType.ToLower().Contains(query.KeyWord.ToLower()));
                };
            }

            if (isSort)
            {
                employees = SortEmployeeByField(employees, query.SortBy, query.isDescending);
            }

            if (query.isActive == false)
            {
                employees = employees.Where(w => w.IsActive == false);
            }
            else
            {
                employees = employees.Where(w => w.IsActive == true);
            }

            return await employees
                .Skip((pageRequest.PageNumber - 1) * pageRequest.PerPage)
                .Take(pageRequest.PerPage)
                .Select(s => s.ToEmployeeSearchResponse(s.DeptnoNavigation.Deptname))
                .ToListAsync();
        }

        public async Task<object> SearchEmployee2(SearchEmployeeQuery2 query, PageRequest pageRequest)
        {
            var employees = await _employeeRepository.GetAllEmployees();

            var temp = employees.AsQueryable();

            //Search
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                temp = temp.Where(b => b.Fname.ToLower().Contains(query.KeyWord.ToLower()) ||
                b.Lname.ToLower().Contains(query.KeyWord.ToLower()) ||
                b.DeptnoNavigation.Deptname.ToLower().Contains(query.KeyWord.ToLower()) ||
                b.Position.ToLower().Contains(query.KeyWord.ToLower()) ||
                b.EmpType.ToLower().Contains(query.KeyWord.ToLower()) ||
                b.EmpLevel.ToString().ToLower().Contains(query.KeyWord));
            }

            if (!string.IsNullOrEmpty(query.Fname))
            {
                temp = temp.Where(b => b.Fname.ToLower().Contains(query.Fname.ToLower()));
            }
            if (!string.IsNullOrEmpty(query.Lname))
            {
                temp = temp.Where(b => b.Lname.ToLower().Contains(query.Lname.ToLower()));
            }
            if (!string.IsNullOrEmpty(query.Position))
            {
                temp = temp.Where(b => b.Position.ToLower().Contains(query.Position.ToLower()));
            }
            if (!string.IsNullOrEmpty(query.EmpType))
            {
                temp = temp.Where(b => b.EmpType.ToLower().Contains(query.EmpType.ToLower()));
            }


            //Sort
            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                switch (query.SortBy)
                {

                    case "department":
                        temp = query.SortOrder.Equals("asc") ? temp.OrderBy(s => s.DeptnoNavigation.Deptname) : temp.OrderByDescending(s => s.DeptnoNavigation.Deptname);
                        break;
                    case "position":
                        temp = query.SortOrder.Equals("asc") ? temp.OrderBy(s => s.Position) : temp.OrderByDescending(s => s.Position);
                        break;
                    case "type":
                        temp = query.SortOrder.Equals("asc") ? temp.OrderBy(s => s.EmpType) : temp.OrderByDescending(s => s.EmpType);
                        break;
                    case "level":
                        temp = query.SortOrder.Equals("asc") ? temp.OrderBy(s => s.EmpLevel) : temp.OrderByDescending(s => s.EmpLevel);
                        break;
                    default:
                        temp = query.SortOrder.Equals("asc") ? temp.OrderBy(s => s.Fname) : temp.OrderByDescending(s => s.Fname);
                        break;
                }
            }

            if (query.isActive == false)
            {
                temp = temp.Where(w => w.IsActive == false);
            }
            else
            {
                temp = temp.Where(w => w.IsActive == true);
            }

            var total = temp.Count();

            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);
            var roles = await _userManager.GetRolesAsync(user);


            var userRequest = employees.Where(w => w.AppUserId == user.Id).FirstOrDefault();


            if (roles.Any(x => x == "Employee Supervisor"))
            {
                var data1 = employees
                .Skip((pageRequest.PageNumber - 1) * pageRequest.PerPage)
                .Take(pageRequest.PerPage)
                .Where(w => w.DirectSupervisor == userRequest.Empno)
                .Select(s => s.ToEmployeeSearchResponse(s.DeptnoNavigation.Deptname))
                .ToListAsync();

                return new
                {
                    Total = total,
                    Data = data1
                };
            }

            var data = temp
                .Skip((pageRequest.PageNumber - 1) * pageRequest.PerPage)
                .Take(pageRequest.PerPage)
                .Select(s => s.ToEmployeeSearchResponse(s.DeptnoNavigation.Deptname))
                .ToListAsync();

            return new
            {
                Total = total,
                Data = data
            };
        }

        public async Task<object> UpdateEmployee(int id, UpdateEmployeeRequest request)
        {
            // Step 1: Fetch employee by ID
            Console.WriteLine($"Step 1: Attempting to fetch employee with ID: {id}");
            var e = await _employeeRepository.GetEmployee(id);
            if (e == null)
            {
                Console.WriteLine($"Step 1.1: Employee with ID {id} not found.");
                return null; // Employee not found
            }
            Console.WriteLine($"Step 1.2: Employee found. Email: {e.EmailAddress}");

            // Step 2: Fetch user by email
            Console.WriteLine($"Step 2: Attempting to fetch user by email: {e.EmailAddress}");
            var updateUser = await _userManager.FindByEmailAsync(e.EmailAddress);
            if (updateUser == null)
            {
                Console.WriteLine($"Step 2.1: No user found with email: {e.EmailAddress}");
                return null; // Associated user not found
            }
            Console.WriteLine($"Step 2.2: User found. Updating email to: {request.EmailAddress}");

            // Step 3: Update user details
            updateUser.Email = request.EmailAddress;
            e.EmailAddress = request.EmailAddress;
            await _userManager.UpdateAsync(updateUser);

            // Continue with the remaining update logic
            e.Address = request.Address;
            e.Position = request.Position;
            e.Dob = request.Dob;
            e.Fname = request.Fname;
            e.Lname = request.Lname;
            e.Sex = request.Sex;
            e.UpdatedAt = DateTime.UtcNow;
            e.PhoneNumber = request.PhoneNumber;
            e.Deptno = request.Deptno;
            e.DirectSupervisor = request.DirectSupervisor;

            // Add more conditional logic if needed
            var roles = await _userManager.GetRolesAsync(updateUser);
            if (roles.Any(x => x == "Employee Supervisor" || x == "Administrator"))
            {
                e.DirectSupervisor = request.DirectSupervisor;
            }

            if (roles.Any(x => x == "Administrator"))
            {
                e.Ssn = request.Ssn;
                e.Salary = request.Salary;
            }

            e.EmpType = request.EmpType;
            e.EmpLevel = request.EmpLevel;

            Console.WriteLine("Step 3: Updating employee in repository.");
            var response = await _employeeRepository.Update(e);

            // Handle dependents
            Console.WriteLine("Step 4: Updating employee dependents.");
            await _employeeDependentRepository.Delete(id);
            foreach (var item in request.EmpDependents)
            {
                var newEmpDependent = new EmployeeDependent
                {
                    DependentEmpno = id,
                    BirthDate = item.BirthDate,
                    Fname = item.Fname,
                    Lname = item.Lname,
                    Sex = item.Sex,
                    Relation = item.Relation
                };
                await _employeeDependentRepository.Create(newEmpDependent);
            }

            Console.WriteLine("Step 5: Fetching updated dependents for response.");
            var dependents = await _employeeDependentRepository.GetAllEmployeeDependents();
            return response.ToEmployeeResponse(dependents.Where(w => w.DependentEmpno == id).ToList());
        }


        MSEmployeeDetailResponse CreateResponse()
        {
            return new MSEmployeeDetailResponse()
            {
                Status = false,
                Message = "",
                Data = null,
            };
        }

        private IQueryable<Employee> SortEmployeeByField(IQueryable<Employee> employees, string field, bool isDescending)
        {
            if (field.Equals("name", StringComparison.OrdinalIgnoreCase))
            {
                employees = isDescending ? employees.OrderByDescending(x => x.Fname) : employees.OrderBy(x => x.Fname);
            }
            if (field.Equals("departement", StringComparison.OrdinalIgnoreCase))
            {
                employees = isDescending ? employees.OrderByDescending(x => x.DeptnoNavigation.Deptname) : employees.OrderBy(x => x.DeptnoNavigation.Deptname);

            }
            if (field.Equals("position", StringComparison.OrdinalIgnoreCase))
            {
                employees = isDescending ? employees.OrderByDescending(x => x.Position) : employees.OrderBy(x => x.Position);

            }
            if (field.Equals("level", StringComparison.OrdinalIgnoreCase))
            {
                employees = isDescending ? employees.OrderByDescending(x => x.EmpLevel) : employees.OrderBy(x => x.EmpLevel);

            }
            if (field.Equals("type", StringComparison.OrdinalIgnoreCase))
            {
                employees = isDescending ? employees.OrderByDescending(x => x.EmpType) : employees.OrderBy(x => x.EmpType);

            }
            if (field.Equals("updateDate", StringComparison.OrdinalIgnoreCase))
            {
                employees = isDescending ? employees.OrderByDescending(x => x.UpdatedAt) : employees.OrderBy(x => x.UpdatedAt);

            }
            return employees;
        }


    }
}
