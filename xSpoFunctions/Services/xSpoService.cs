using Ada.Net.Lib.Models.AdaPools;
using Google.Cloud.Storage.V1;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace xSpoFunctions.Services
{
    public class xSpoService
    {
        public async Task<List<StakePool>> GetxSpoExtendedDetails()
        {
            try
            {

                var allianceMembers = await AllianceMembers.LoadxSpoAllianceMembers();

                if (allianceMembers == null || allianceMembers.AdaPools == null || allianceMembers.AdaPools.Members == null || allianceMembers.AdaPools.Members.List == null)
                    return null;

                List<StakePool> pools = new List<StakePool>();

                foreach (var mem in allianceMembers.AdaPools.Members.List)
                {
                    pools.Add(await StakePool.LoadStakePooleDetailsAsync(mem.Value.PoolId));
                }

                return pools;
            }
            catch(Exception err)
            {
                Console.WriteLine($"An error occured fetching pool details: {err.Message}");
                return null;
            }
        }

        public async Task<bool> SavePoolDetailsToGCS(List<StakePool> pools)
        {
            try
            {
                var fileName = "xSpoPools.json";
                var bucketName = "meroada-public";
                var content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(pools));

                var obj = new Google.Apis.Storage.v1.Data.Object
                {
                    Bucket = bucketName,
                    Name = fileName,
                    ContentType = "text/json",
                    //Metadata = new Dictionary<string, string>
                    //{

                    //}
                };

                var gcsStorage = StorageClient.Create();

                obj = gcsStorage.UploadObject(obj, new MemoryStream(content));
                return true;

                //using(var f = File.OpenRead(filePath))
                //{
                //    string objectName = Path.GetFileName(filePath);
                //    await gcsStorage.UploadObjectAsync("meroada-public", objectName, null, f);
                //    Console.WriteLine($"Uploaded {objectName}");
                //    return true;
                //}
            }
            catch(Exception err)
            {
                Console.WriteLine($"Problem uploading file to GCS: {err.Message}");
                return false;
            }

        }
    }
}
