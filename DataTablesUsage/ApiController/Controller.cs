using System;
using System.Collections.Generic;
using System.Linq;
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

            var start = int.Parse(Request.Form["start"].ToString());
            var length = int.Parse(Request.Form["length"].ToString());
            var search = Request.Form["search[value]"].ToString();

            mockDataList = mockDataList.Skip(start).Take(length).ToList();

            return Ok(mockDataList);
        }
    }
}
