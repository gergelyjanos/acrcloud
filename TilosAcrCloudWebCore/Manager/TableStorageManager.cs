using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TilosAcrCloudWebCore.Models;

namespace TilosAcrCloudWebCore.Manager
{
    public class TableStorageManager
    {
        //private const string STORAGE_ACCOUNT_NAME = "devstoreaccount1";
        //private const string STORAGE_ACCOUNT_KEY = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";

        private const string TILOSHU_STREAMID = "s-M1xLrD2";

        public async Task<List<AcrCallback>> GetLast(string streamId, int limit = 1, int offset = 0)
        {
            CloudTable table = GetTable();
            TableQuery<AcrCallback> query = new TableQuery<AcrCallback>().Where(GetPartitionFilter(streamId));

            List<AcrCallback> playlist = new List<AcrCallback>();
            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<AcrCallback> resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (AcrCallback customer in resultSegment.Results)
                {
                    playlist.Add(customer);
                }
            } while (token != null && playlist.Count < (limit + offset));

            var result = playlist.Skip(offset).Take(limit);
            return result.ToList();
        }

        public CloudTable GetTable()
        {
            CloudStorageAccount storageAccount = GetStorageAccount();
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("AcrData");
            table.CreateIfNotExistsAsync();
            return table;
        }

        private static CloudStorageAccount GetStorageAccount()
        {
            CloudStorageAccount storageAccount = 
                CloudStorageAccount.Parse("UseDevelopmentStorage=true;");
            
            //CloudStorageAccount storageAccount = new CloudStorageAccount(
            //    new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
            //        STORAGE_ACCOUNT_NAME,
            //        STORAGE_ACCOUNT_KEY),
            //    true);

            return storageAccount;
        }

        /// <summary>
        /// Visszaadja a RowKey-t az idő string alapján. 
        /// A kódolás: év, hó, nap, stb-re minden összetevőt kivon 9999-ből. 
        /// Így fordított sorrendben tárolja az Azure, és szemre vissza lehet fejteni, hogy melyik RowKey milyen időpontot rejt.
        /// </summary>
        /// <param name="sTime">Idő string formátum yyyyMMddHHmmss</param>
        /// <returns></returns>
        public string GetRowKeyFromTimeString(string sTime)
        {
            var y = int.Parse(sTime.Substring(0, 4));
            var mo = int.Parse(sTime.Substring(4, 2));
            var d = int.Parse(sTime.Substring(6, 2));
            var h = int.Parse(sTime.Substring(8, 2));
            var mi = int.Parse(sTime.Substring(10, 2));
            var s = int.Parse(sTime.Substring(12, 2));
            var dTime = new DateTime(y, mo, d, h, mi, s);
            var retVal = new StringBuilder();
            retVal.Append((9999 - y).ToString());
            retVal.Append((9999 - mo).ToString());
            retVal.Append((9999 - d).ToString());
            retVal.Append((9999 - h).ToString());
            retVal.Append((9999 - mi).ToString());
            retVal.Append((9999 - s).ToString());
            return retVal.ToString();
        }

        private string GetPartitionFilter(string streamId = TILOSHU_STREAMID)
        {
            return TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, streamId);
        }
    }
}
