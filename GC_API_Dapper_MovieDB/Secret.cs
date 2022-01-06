using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GC_API_Dapper_MovieDB
{
    public class Secret
    {
        //Remember to make a gitignore and put secret.cs in it
        public static string Connection { get; set; } = "server=127.0.0.1;uid=root;pwd=1111;database=movierentals";
    }
}
