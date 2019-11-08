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
        static async Task Main(string[] args)
        {
            Console.WriteLine("Azure Blob Storage V12 - .NET sample");
            string connectionString = Environment.GetEnvironmentVariable("CONNECT_STR");
            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            string containerName = "blobfromapp" + Guid.NewGuid().ToString();
            BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);

            string localPath = "../data/";
            string fileName = "myFile" + Guid.NewGuid().ToString() + ".txt";
            string localFilePath = Path.Combine(localPath, fileName);

            await File.WriteAllTextAsync(localFilePath, "Hello, Mediums");

            BlobClient blobClient = containerClient.GetBlobClient(fileName);

            Console.WriteLine($"Uploading to Blob Storage:\n\t { blobClient.Uri }\n");

            using FileStream uploadFileStream = File.OpenRead(localFilePath);
            await blobClient.UploadAsync(uploadFileStream);
            uploadFileStream.Close();

            await foreach (BlobItem item in containerClient.GetBlobsAsync())
                Console.WriteLine(item.Name);

            string downloadFilePath = localFilePath.Replace(".txt", "Download.txt");
            Console.WriteLine($"Downloading blob to\n\t{ downloadFilePath }\n");

            BlobDownloadInfo download = await blobClient.DownloadAsync();

            using FileStream downloadFileStream = File.OpenWrite(downloadFilePath);
            await download.Content.CopyToAsync(downloadFileStream);
            downloadFileStream.Close();

            Console.WriteLine("Type y to Delete or n to exit:\n");
            bool choise = Console.ReadLine() == "y" ? true : false;

            if (choise){
                await containerClient.DeleteAsync();
                File.Delete(localFilePath);
                File.Delete(downloadFilePath);
                Console.WriteLine("Done!");
            } else
                Console.WriteLine("Exiting..");
        }
    }
}
