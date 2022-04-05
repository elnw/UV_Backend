using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UV_Backend.Models;

namespace UV_Backend.Data
{
    public class UVData
    {
        private readonly AppDbContext _appDbContext;
        public UVData(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task RegisterUVData(List<Models.UV> uvData)
        {
            await _appDbContext.UVData.AddRangeAsync(uvData);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<UV> GetMostRecenUV(DateTime currentTime)
        {
            return await _appDbContext.UVData.FirstOrDefaultAsync(x=> x.HourForecast.Day == currentTime.Day &&
                                                                      x.HourForecast.Month == currentTime.Month &&
                                                                      x.HourForecast.Year == currentTime.Year &&
                                                                      x.HourForecast.Hour == currentTime.Hour);
        }
    }
}
