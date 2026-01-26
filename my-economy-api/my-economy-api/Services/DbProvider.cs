using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using my_economy_api.Data;
using my_economy_api.Models;

namespace my_economy_api.Services
{
    public class DbProvider
    {
        private readonly ApplicationDbContext context;

        public DbProvider(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<List<FixedCost>> GetFixedCost()
        {
            return await context.FixedCosts.AsSingleQuery().ToListAsync();
        }

        public async Task<FixedCost> SaveFixedCost(FixedCost fx)
        {
            try
            {
                context.FixedCosts.Add(fx);
                await context.SaveChangesAsync();
                return fx;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}
