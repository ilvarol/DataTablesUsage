using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataTablesUsage.Models
{
    //DataTables expects this class to return as a response.
    public class Result
    {
        public int draw { get; set; }

        public int recordsTotal { get; set; }

        public int recordsFiltered { get; set; }

        public dynamic data { get; set; }

        public int error { get; set; }
    }
}
