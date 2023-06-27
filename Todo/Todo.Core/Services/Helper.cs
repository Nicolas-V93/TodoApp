using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pra.Todo.Core
{
    class Helper
    {
        public static string GetConnectionString() 
        { 
            return @"Data Source=LAPTOP-J0K9LE2M\SQLEXPRESS;Initial Catalog=TodoApp; Integrated security=true;";
        }
        public static string HandleQuotes(string value) 
        { 
            return value.Trim().Replace("'", "''"); 
        }
    }
}
