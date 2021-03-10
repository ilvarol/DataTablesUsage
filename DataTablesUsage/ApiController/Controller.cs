using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using DataTablesUsage.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataTablesUsage.Pages
{
    [Route("[controller]")]
    [ApiController]
    public class DataTablesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Test()
        {
            var jsonString = System.IO.File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}/Mock Data/MOCK_DATA.json");
            var mockDataList = JsonSerializer.Deserialize<List<MockData>>(jsonString);

            //draw is about how many times request is dataTables
            //in order to prevent Cross Site Scripting
            var draw = int.Parse(Request.Form["draw"].ToString());

            var start = int.Parse(Request.Form["start"].ToString());
            var length = int.Parse(Request.Form["length"].ToString());

            var orderColumn = Request.Form["order[0][column]"].ToString();
            var orderDirection = Request.Form["order[0][dir]"].ToString();

            var search = Request.Form["search[value]"].ToString().ToLower();

            var total = mockDataList.Count;

            //checking all columns if there is
            mockDataList = mockDataList.Where(w =>
            w.Email.ToLower().Contains(search)          ||
            w.Gender.ToLower().Contains(search)         ||
            w.Name.ToLower().Contains(search)           ||
            w.Surname.ToLower().Contains(search)        ||
            w.Id.ToString().ToLower().Contains(search)
            ).Skip(start).Take(length).ToList();

            //getting count info
            var filteredTotal = mockDataList.Where(w =>
            w.Email.ToLower().Contains(search)  ||
            w.Gender.ToLower().Contains(search) ||
            w.Name.ToLower().Contains(search) ||
            w.Surname.ToLower().Contains(search) ||
            w.Id.ToString().ToLower().Contains(search)
            ).Count();

            //thanks to this class, we can reach property of mockdata using string property name value
            PropertyInfo pinfo = typeof(MockData).GetProperty(orderColumn);

            switch (orderDirection)
            {
                case "asc":
                    mockDataList = mockDataList.OrderBy(o => pinfo.GetValue(o, null)).ToList();
                    break;
                case "desc":
                    mockDataList = mockDataList.OrderByDescending(o => pinfo.GetValue(o, null)).ToList();
                    break;
            }

            Result result = new Result()
            {
                data = mockDataList,
                recordsTotal = total,
                recordsFiltered = search.Length > 0 ? filteredTotal : total,
                draw = draw
            };

            return Ok(result);
        }
    }
}
