using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;


namespace dEV3
{
    public interface ICalcService
    {
        int sum(int a, int b);
        int prod(int a, int b);
        double division(int a, int b);
        int substract(int a, int b);
    }
    class CalcService : ICalcService
    {
        public int sum(int a, int b)
        {
            return a + b;
        }
        public int substract(int a, int b)
        {
            return a - b;
        }
        public int prod(int a, int b)
        {
            return a * b;
        }
        public double division(int a, int b)
        {
            return a / b;
        }
    }

    public class CalcServiceMiddleware
    {
        private readonly RequestDelegate _next;
        public CalcServiceMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, ICalcService calcService)
        {
            var resp = context.Response;
            var req = context.Request;

            StringBuilder sb = new StringBuilder();

            int first_number;
            if (!int.TryParse(req.Query["first"], out first_number))
            {
                resp.StatusCode = 403;
                await resp.WriteAsync("Bad first argument.");
                return;
            }
            sb.Append(first_number);

            int second_number;
            if (!int.TryParse(req.Query["second"], out second_number))
            {
                resp.StatusCode = 403;
                await resp.WriteAsync("Bad second argument.");
                return;
            }
            sb.Append(first_number);

            var op = req.Query["op"];

            context.Response.ContentType = "text/html;charset=utf-8";
            string result;
            switch (op)
            {
                case "prod":
                    result = $"{first_number} * {second_number} = {calcService.prod(first_number, second_number)}";
                    break;
                case "plus":
                    result = $"{first_number} + {second_number} = {calcService.sum(first_number, second_number)}";
                    break;
                case "minus":
                    result = $"{first_number} - {second_number} = {calcService.substract(first_number, second_number)}";
                    break;
                case "division":
                    result = $"{first_number} / {second_number} = {calcService.division(first_number, second_number)}";
                    break;
                default:
                    resp.StatusCode = 403;
                    await resp.WriteAsync("Bad operation argument." + op);
                    return;
            }
            await context.Response.WriteAsync(result);

            //await _next.Invoke(context);
        }
    }
}
