using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using TilosAcrCloudWebCore.Manager;
using TilosAcrCloudWebCore.Models;

namespace TilosAcrCloudWebCore.Controllers
{
    [Route("Create")]
    [ApiController]
    public class AcrCallbackController : ControllerBase
    {
        // Todo: Inject
        private readonly TableStorageManager _tableStorageManager = new TableStorageManager();

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FormCollection collection)
        {
            try
            {
                CloudTable table = _tableStorageManager.GetTable();

                var acrData = new AcrCallback(
                    collection["stream_id"],
                    _tableStorageManager.GetRowKeyFromTimeString(DateTime.Now.ToString("yyyyMMddHHmmss")))
                {
                    StreamId = collection["stream_id"],
                    StreamUrl = collection["stream_url"],
                    Data = collection["data"],
                    Timestamp = DateTime.Now,
                };
                TableOperation insertOperation = TableOperation.Insert(acrData);
                TableResult result = await table.ExecuteAsync(insertOperation);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }
    }
}
