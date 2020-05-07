using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Table;
using TilosAcrCloudWebCore.Manager;
using TilosAcrCloudWebCore.Models;

namespace TilosAcrCloudWebCore.Controllers
{
    [Route("Last")]
    [ApiController]
    public class RightOnController : ControllerBase
    {
        private const string TILOSHU_STREAMID = "s-M1xLrD2";
        private readonly TableStorageManager _tableStorageManager = new TableStorageManager();

        [HttpGet]
        public async Task<List<AcrCallback>> Get(string streamId = TILOSHU_STREAMID, int limit = 1, int offset = 0)
        {
            var result = await _tableStorageManager.GetLast(streamId, limit, offset);
            return result;
        }
    }
}