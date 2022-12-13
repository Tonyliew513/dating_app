using System;
using System.Collections.Generic;
using System.Text;

namespace SharedModel.SerilogModels
{
    public static class GlobalVariable
    {
        public static string DBConnectionString { get; set; }

        public static string DBConnectionStringOrleans { get; set; }

        public static string DBconnectionString_Serilog { get; set; }

        public static string HostingEnvironment { get; set; }
    }
}
