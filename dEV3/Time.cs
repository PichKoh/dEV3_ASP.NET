using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace dEV3
{
    public interface ITimeService
    {
        string GetTime();
    }
    class ShortTimeService : ITimeService
    {
        public string GetTime()
        {
            int hour = DateTime.Now.Hour;
            string result = "";
            if (hour < 0 || hour > 24)
            {
                result = "Невірний час";
            }
            else if (hour < 6)
            {
                result = "Зараз ніч";
            }
            else if (hour < 12)
            {
                result = "Зараз ранок";
            }
            else if (hour < 18)
            {
                result = "Зараз обід";
            }
            else if (hour < 24)
            {
                result = "Зараз вечір";
            }
            return result;
        }
    }

    public class TimeMiddleware
    {
        private readonly RequestDelegate _next;
        public TimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, ITimeService timeService)
        {
            if (context.Request.Path == "/Time")
            {
                context.Response.ContentType = "text/html;charset=utf-8";
                await context.Response.WriteAsync($"<h>Time: {timeService.GetTime()}</h>");
                return;
            }
            await _next.Invoke(context);
        }
    }
}
