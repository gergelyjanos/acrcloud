using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TilosAcrCloudWebCore.Manager;

namespace TilosAcrCloudWebCore.Controllers
{
    [Route("Last")]
    [ApiController]
    public class RightOnController : ControllerBase
    {
        private const string TILOSHU_STREAMID = "s-M1xLrD2";
        private readonly TableStorageManager _tableStorageManager = new TableStorageManager();

        /// <summary>
        /// Az utolsó x bejegyzést adja vissza
        /// </summary>
        /// <param name="streamId">Az AcrCloud Stream ID</param>
        /// <param name="limit">Hány darab bejegyzést adjon vissza</param>
        /// <param name="offset">Honnan kezdje? 0=legutolsó</param>
        /// <returns>Az utolsó x bejegyzés tömbje</returns>
        //[AllowCrossSite]
        public async Task<IActionResult> Get(string streamId = TILOSHU_STREAMID, int limit = 1, int offset = 0)
        {
            var result = await _tableStorageManager.GetLast(streamId, limit, offset);
            return Ok(result);
        }

    }
}