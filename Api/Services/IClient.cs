using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services
{
    public interface IClient
    {
        Task<Stream> GetPage(string uri);
    }
}
