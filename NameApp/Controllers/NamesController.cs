using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NameApp.Context;
using NameApp.Model;
using NameApp.Service;
using System;
using System.Collections.Generic;

namespace NameApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NamesController : ControllerBase
    {
        private readonly ILogger<NamesController> _logger;
        private readonly IValidateInput _validate;

        public NamesController(ILogger<NamesController> logger, IValidateInput v)
        {
            _logger = logger;
            _validate = v;
        }
        [HttpGet]
        public NamesResponse Get()
        {
            var response = new NamesResponse();
            response.Names = new List<Names>();
            response.DefaultResponse = new DefaultResponse();
            try
            {
                var options = new DbContextOptionsBuilder<NameContext>()
               .UseInMemoryDatabase(databaseName: "Test")
               .Options;
                using (var context = new NameContext(options))
                {
                    var names = context.Names.AsQueryable();
                    foreach (var nam in names)
                    {
                        response.Names.Add(nam);
                    }
                    response.Names.Reverse();
                }
                response.DefaultResponse.StatusCode = 200;
                response.DefaultResponse.Success = true;
                response.DefaultResponse.StatusMessage = "Success";
            }
            catch (Exception ex)
            {
                response.DefaultResponse.StatusCode = 505;
                response.DefaultResponse.Success = false;
                response.DefaultResponse.StatusMessage = ex.Message;
            }
            return response;
        }
        [HttpPost]
        public NameResponse Post(string name)
        {

            var options = new DbContextOptionsBuilder<NameContext>()
            .UseInMemoryDatabase(databaseName: "Test")
            .Options;

            var res = new NameResponse();
            res.DefaultResponse = new DefaultResponse();
            if (name.Length > 6 || name.Length == 0)
            {
                res.DefaultResponse.StatusCode = 400;
                res.DefaultResponse.StatusMessage = "Name can't be longer than 6 characters";
                res.DefaultResponse.Success = false;
                return res;
            }
            var isValid = _validate.IsNameValid(name);
            if (!isValid)
            {
                res.DefaultResponse.StatusCode = 400;
                res.DefaultResponse.StatusMessage = "Name should only contains small letters a-z";
                res.DefaultResponse.Success = false;
                return res;
            }

            using (var context = new NameContext(options))
            {
                var n = new Names
                {
                    Name = name
                };

                var listSize = context.Names.ToListAsync();
                var length = listSize.Result.Count;
                if (length == 10)
                {
                    var n1 = context.Names.FirstOrDefaultAsync();
                    context.Names.Remove(n1.Result);
                }

                context.Names.Add(n);
                context.SaveChanges();
                res.Name = name;
                res.DefaultResponse.StatusCode = 200;
                res.DefaultResponse.StatusMessage = "success";
                res.DefaultResponse.Success = true;
            }

            return res;
        }
    }
}
