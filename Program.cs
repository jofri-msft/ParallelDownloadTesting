namespace Microsoft.WindowsAzure.Storage.Blob
{
	using System; 
     	using System.IO; 
     	using System.Net; 
     	using System.Threading; 
     	using System.Diagnostics; 
     	using System.IO.Compression; 
     	using System.Security.Cryptography; 
     	using Microsoft.WindowsAzure.Storage; 
     	using Microsoft.WindowsAzure.Storage.Blob; 
     	using Microsoft.WindowsAzure.Storage.RetryPolicies; 
     	using System.Threading.Tasks; 
     	using System.Text; 
     	using System.Collections.Generic; 
     	using System.Linq;
 
    class Program
    {
	public static CloudBlob GetBlob() 
         { 
             string connectionString = "DefaultEndpointsProtocol=http;AccountName=xhjssa3zjl2rssalinuxvm;AccountKey=eh/Z9aH5uH0IfKVIX47cCx7JGoViewJXY1dEIKexZ8zh8kVJf0NuvdxexbAQvaULF1HFclJfUSgaezTIG1B0Rw=="; 
             CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString); 
 
 
             CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient(); 
             CloudBlobContainer blobContainer = blobClient.GetContainerReference("rffyg");
	     return (CloudBlob)blobContainer.GetBlobReference("00814");
         } 

        static void Main(string[] args)
        {
		CloudBlob blob = GetBlob();
		Stopwatch time = Stopwatch.StartNew();
		DoUploadDownloadFileTask(blob, Int32.Parse(args[0]) /*parallel IO count*/, Convert.ToInt64(args[1])*1024*1024 /* range size per IO */).Wait(); 

		time.Stop();
		Console.WriteLine("Parallel I/O Count {0}.", args[0]);
		Console.WriteLine("Download size per range {0} in MB.", args[1]);
		Console.WriteLine("Download has been completed in {0} seconds.", time.Elapsed.TotalSeconds.ToString());
		Console.ReadLine();
        }

	private static async Task DoUploadDownloadFileTask(CloudBlob blob, int parallelCount, long chunkSize)
        {
            string outputFileName = Path.GetTempFileName();

            try
            {
                long? offset = null;
                long? length = null;
                CloudBlob source = (CloudBlob)blob;
                ParallelDownloadSettings parallelDownloadSettings = new ParallelDownloadSettings(source, outputFileName, FileMode.Create, offset, length, parallelCount, chunkSize);
                ParallelDownload parallelDownload = ParallelDownload.Start(parallelDownloadSettings);

                await parallelDownload.Task;
            }
            finally
            {
                //File.Delete(outputFileName);
            }
	}
    }
}

