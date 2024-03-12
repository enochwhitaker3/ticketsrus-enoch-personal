using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorClassLib.Services;

namespace TicketsAreUs.Services
{
    public class EnvironmentService : IEnvironmentService
    {
        public bool IsWebApp => false;
    }
}
