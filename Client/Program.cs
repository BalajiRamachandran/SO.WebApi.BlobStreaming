
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = "http://localhost:60067/api/blob";    // swap with cloud URI when you deploy to Azure

            var random = new Random(Environment.TickCount);

            var twentyMb = 1024*1024*20;

            var data = new byte[twentyMb];

            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (byte) random.Next(0, 256);
            }

            var http = new HttpClient();

            var result = http.PostAsync(uri, new ByteArrayContent(data)).Result;

            Console.WriteLine(result.EnsureSuccessStatusCode().Content.ReadAsStringAsync().Result);
        }
    }
}
