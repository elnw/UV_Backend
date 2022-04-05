using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using RestSharp;
using UV_Backend.Entities.Responses.Uv;
using System.Collections.Generic;
using UV_Backend.Helpers.Logic;
using UV_Backend.Data;
using System;

namespace UV_Backend.Logic.Uv
{
    public class UVLogic
    {
        private readonly IConfiguration configuration;
        private readonly UVData _uvData;

        public UVLogic(IConfiguration configuration, UVData uvData)
        {
            this.configuration = configuration;
            _uvData = uvData;
        }
        private async Task<UVForecastResponse> FetchUVForecast()
        {
            try
            {
                var client = new RestClient(configuration.GetValue<string>("UV_API:base"));

                var request = new RestRequest(configuration.GetValue<string>("UV_API:resource"));
                request.AddParameter("lat", configuration.GetValue<string>("UV_API:default_lat"));
                request.AddParameter("lon", configuration.GetValue<string>("UV_API:default_long"));
                request.AddParameter("appid", configuration.GetValue<string>("UV_API:api_key"));

                return await client.GetAsync<UVForecastResponse>(request);
            }catch(Exception ex)
            {
                return null;
            }
           

        }

        public async Task<bool> RegisterUVForecast()
        {
            try
            {
                var data = await FetchUVForecast();
                var uvRegister = new List<Models.UV>(data.hourly.Count);

                foreach (var item in data.hourly)
                {
                    uvRegister.Add(new Models.UV { HourForecast = LogicHelpers.ConvertUTCToDatetime(item.dt), Uvi = item.uvi });
                }

                await _uvData.RegisterUVData(uvRegister);
                return true;
            }
            catch
            {
                return false;
            }


        }

        public async Task<float> GetCurrentUVI()
        {
            var currentTime = DateTime.Now;

            var uv = await _uvData.GetMostRecenUV(currentTime);

            if (uv == null)
            {
                return 0.0f;
            }
            else
            {
                return uv.Uvi;
            }
        }

        public async Task<float> CalculateExposureTime(string scoreDescription, float uvi)
        {
            return LogicHelpers.GetMED(scoreDescription) * uvi * 0.15f;
        }
    }
}
