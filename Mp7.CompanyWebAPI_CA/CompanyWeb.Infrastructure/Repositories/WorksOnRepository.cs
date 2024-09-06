﻿using CompanyWeb.Domain.Models.Dtos;
using CompanyWeb.Domain.Models.Entities;
using CompanyWeb.Domain.Models.Requests;
using CompanyWeb.Domain.Models.Requests.Add;
using CompanyWeb.Domain.Repositories;
using LMS.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyWeb.Infrastructure.Repositories
{
    public class WorksOnRepository : IWorksOnRepository 
    {
        private readonly CompanyDbContext Context;
        public WorksOnRepository(CompanyDbContext context)
        {
            Context = context;
        }

        public async Task<WorksOnDetailDto> Create(AddWorksOnRequest wo)
        {

            var newWo = new Workson()
            {
                Empno = wo.Empno,
                Projno = wo.Projno,
                Dateworked = wo.Dateworked,
                Hoursworked = wo.Hoursworked,
            };

            Context.Worksons.Add(newWo);
            await Context.SaveChangesAsync();

            return new WorksOnDetailDto()
            {
                Empno = wo.Empno,
                Projno = wo.Projno,
                Dateworked = wo.Dateworked,
                Hoursworked = wo.Hoursworked,
            };
        }

        public async Task<Workson> Delete(int projNo, int empNo)
        {

            var wo = await (from value in Context.Worksons
                            where (value.Projno == projNo && value.Empno == empNo)
                            select value).FirstOrDefaultAsync();
            if (wo == null)
            {
                return null;
            }

            Context.Worksons.Remove(wo);
            await Context.SaveChangesAsync();

            return wo;
        }

        public async Task<Workson> GetWorkson(int projNo, int empNo)
        {

            var wo = await (from value in Context.Worksons
                            where (value.Projno == projNo && value.Empno == empNo)
                            select value).FirstOrDefaultAsync();

            if (wo == null)
            {
                return null;
            }
            return wo;
        }

        public async Task<List<Workson>> GetWorksons(int pageNumber, int perPage)
        {
            return await Context.Worksons
            .OrderBy(ob => ob.Empno)
            .Skip((pageNumber - 1) * perPage)
            .Take(perPage)
            .ToListAsync<Workson>();
        }
        public async Task<IQueryable<Workson>> GetAllWorksons()
        {
            return Context.Worksons;
        }

        public async Task<Workson> Update(int projNo, int empNo, UpdateWorksOnRequest wo)
        {

            var w = await (from value in Context.Worksons
                           where (value.Projno == projNo && value.Empno == empNo)
                           select value).FirstOrDefaultAsync();

            if (w == null)
            {
                return null;
            }

            w.Dateworked = wo.Dateworked;
            w.Hoursworked = wo.Hoursworked;

            Context.Worksons.Update(w);
            await Context.SaveChangesAsync();

            return w;
        }

        public async Task<int> GetProjectTotalHoursByProjectNumber(int projNo)
        {
            return await Context.Worksons.Where(w=>w.Projno == projNo).Select(s=>s.Hoursworked).SumAsync();
        }

        public async Task<int> GetProjectCountByEmployeeNumber(int empNo)
        {
            return await Context.Worksons.Where(w => w.Empno == empNo).CountAsync();
        }
    }
}
