using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using projectTest.Domain.DSO;
using projectTest.Domain.Models;
using projectTest.Repository.Interfaces;

namespace projectTest.Repository
{
    public class OperationBlobRepository : IDummyRepository
    {

        public async Task AddDummyAsync(DummyDso item)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=vkblob;AccountKey=AozKsPKH41VekUT5Wi2ni+PD0x1RTlkcGMzKlqe9bVseWhN4II7CJ6EqjJJwnpYxCBEm6ZQJRnCc+AStYYbErA==;EndpointSuffix=core.windows.net");
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("dummies");
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference(item.Id + ".txt");

            byte[] bytes = null;
            using (var ms = new MemoryStream())
            {
                TextWriter tw = new StreamWriter(ms);
                tw.Write(JsonConvert.SerializeObject(item));
                tw.Flush();
                ms.Position = 0;
                bytes = ms.ToArray();

                blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length).GetAwaiter().GetResult();
                blob.Properties.ContentType = "text/plain";
                blob.SetPropertiesAsync().GetAwaiter().GetResult();
            }
        }

        public async Task DeleteDummyAsync(Guid id)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=vkblob;AccountKey=AozKsPKH41VekUT5Wi2ni+PD0x1RTlkcGMzKlqe9bVseWhN4II7CJ6EqjJJwnpYxCBEm6ZQJRnCc+AStYYbErA==;EndpointSuffix=core.windows.net");
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("dummies");
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference(id + ".txt");

            await blob.DeleteIfExistsAsync();
        }

        public async Task<List<DummyDso>> GetDummyAsync(string query)
        {
            List<DummyDso> ret = new List<DummyDso>();
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=vkblob;AccountKey=AozKsPKH41VekUT5Wi2ni+PD0x1RTlkcGMzKlqe9bVseWhN4II7CJ6EqjJJwnpYxCBEm6ZQJRnCc+AStYYbErA==;EndpointSuffix=core.windows.net");
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("dummies");
            if (!(await container.ExistsAsync()))
            {
                throw new Exception("NOT FOUND");
            }
            var directory = container.GetDirectoryReference(".");

            BlobContinuationToken continuationToken = null;
            do
            {
                var listingResult = await directory.ListBlobsSegmentedAsync(false, BlobListingDetails.None, 100, continuationToken, null, null);
                continuationToken = listingResult.ContinuationToken;
                foreach (var blob in listingResult.Results)
                {
                    if (blob is CloudBlockBlob)
                        ret.Add(JsonConvert.DeserializeObject<DummyDso>(await((CloudBlockBlob)blob).DownloadTextAsync()));
                }
            } while (continuationToken != null);

            return ret;
        }

        public async Task<DummyDso> GetDummyAsync(Guid id)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=smbs;AccountKey=sh0tpYNcRdQkdEfjOze60sNNaoANCgQtrDVhLGYuEuu9/W+wD1/gEKEYbtN2atJSdSaCifrOkaFCCdzAfng/vQ==;EndpointSuffix=core.windows.net");
            var client = storageAccount.CreateCloudBlobClient();
            var container = client.GetContainerReference("dummies");
            if (!(await container.ExistsAsync()))
            {
                throw new Exception("NOT FOUND");
            }
            var blob = container.GetBlockBlobReference(id.ToString() + ".txt");

            return JsonConvert.DeserializeObject<DummyDso>(await blob.DownloadTextAsync());
        }

        public async Task<bool> UpdateDummyAsync(Guid id, JsonPatchDocument item)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=smbs;AccountKey=sh0tpYNcRdQkdEfjOze60sNNaoANCgQtrDVhLGYuEuu9/W+wD1/gEKEYbtN2atJSdSaCifrOkaFCCdzAfng/vQ==;EndpointSuffix=core.windows.net");
                var client = storageAccount.CreateCloudBlobClient();
                var container = client.GetContainerReference("dummies");
                await container.CreateIfNotExistsAsync();

                var blob = container.GetBlockBlobReference(id + ".txt");

                byte[] bytes = null;
                using (var ms = new MemoryStream())
                {
                    TextWriter tw = new StreamWriter(ms);
                    tw.Write(JsonConvert.SerializeObject(item));
                    tw.Flush();
                    ms.Position = 0;
                    bytes = ms.ToArray();

                    blob.UploadFromByteArrayAsync(bytes, 0, bytes.Length).GetAwaiter().GetResult();
                    blob.Properties.ContentType = "text/plain";
                    blob.SetPropertiesAsync().GetAwaiter().GetResult();
                }
                return true;
            }
            catch { }
            return false;
        }

        Task<DummyDso> IDummyRepository.AddDummyAsync(DummyDso item)
        {
            throw new NotImplementedException();
        }
    }
}
