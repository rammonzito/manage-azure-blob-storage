using System;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.IO;
using System.Threading.Tasks;

namespace BlobStorageV12
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Azure Blob Storage V12 - .NET sample");
            string connectionString = Environment.GetEnvironmentVariable("CONNECT_STR");
            Console.WriteLine(connectionString); //Here you can check if your environment variable is working
            Console.ReadKey();
        }
    }
}
