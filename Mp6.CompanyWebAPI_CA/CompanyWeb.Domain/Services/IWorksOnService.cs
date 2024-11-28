﻿using CompanyWeb.Domain.Models.Entities;
using CompanyWeb.Domain.Models.Requests;
using CompanyWeb.Domain.Models.Requests.Add;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWeb.Domain.Services
{
    public interface IWorksOnService
    {
        Task<object> CreateWorksOn(AddWorksOnRequest request);
        Task<object> UpdateWorksOn(int projNo, int empNo, UpdateWorksOnRequest request);
        Task<object> DeleteWorksOn(int projNo, int empNo);
        Task<object> GetWorksons(int pageNumber, int perPage);
        Task<List<Workson>> GetAllWorksons();

        Task<object> GetWorkson(int projNo, int empNo);

    }
}
