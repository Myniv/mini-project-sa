﻿
using CompanyWeb.Domain.Models.Dtos;
using CompanyWeb.Domain.Models.Entities;
using CompanyWeb.Domain.Models.Helpers;
using CompanyWeb.Domain.Models.Requests;
using CompanyWeb.Domain.Models.Requests.Add;
using CompanyWeb.Domain.Models.Requests.Update;
using CompanyWeb.Domain.Models.Responses.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWeb.Domain.Services
{
    public interface IEmployeeService
    {
        Task<object> CreateEmployee(AddEmployeeRequest request);
        Task<List<object>> GetEmployees(int pageNumber, int perPage);
        Task<List<object>> GetAllEmployees();
        Task<object> GetEmployee(int id);
        Task<object> UpdateEmployee(int id, UpdateEmployeeRequest request);
        Task<object> DeleteEmployee(int id);

        // Search & Filter
        Task<List<EmployeeSearchResponse>> SearchEmployee(SearchEmployeeQuery query, PageRequest pageRequest);
        Task<object> SearchEmployee2(SearchEmployeeQuery2 query, PageRequest pageRequest);


        // Deactive Employee
        Task<object> DeactivateEmployee(int id, DeactivateEmployeeRequest request);
        Task<object> AssignEmployee(int id, int deptNo);


    }
}
