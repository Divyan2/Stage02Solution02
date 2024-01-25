using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Model
{
    public class DynamicCsvDataModel
    {
        [Name("Username")]
        public string Username { get; set; }

        [Name("Identifier")]
        public int Identifier { get; set; }

        [Name("First name")]
        public string FirstName { get; set; }

        [Name("Last name")]
        public string LastName { get; set; }
    }
}
