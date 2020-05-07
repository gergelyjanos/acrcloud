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
        public async Task<IActionResult> Post(IFormCollection form)
        {
            try
            {
                //var StreamId = form["stream_id"];
                //var StreamUrl = form["stream_url"];
                //var Data = form["data"];
                var StreamId = "stream_id";
                var StreamUrl = "stream_url";
                var Data = "data";
                CloudTable table = _tableStorageManager.GetTable();

                var AcrData = new AcrCallback(
                    StreamId,
                    _tableStorageManager.GetRowKeyFromTimeString(DateTime.Now.ToString("yyyyMMddHHmmss")))
                {
                    StreamId = StreamId,
                    StreamUrl = StreamUrl,
                    Data = Data,
                    Timestamp = DateTime.Now,
                };
                TableOperation insertOperation = TableOperation.Insert(AcrData);
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
