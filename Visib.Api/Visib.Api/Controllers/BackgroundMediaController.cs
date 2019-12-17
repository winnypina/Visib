using System.IO;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Visib.Api.Services;

namespace Visib.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackgroundMediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;

        public BackgroundMediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        // POST api/accounts
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Post()
        {
            var file = Request.Form.Files[0];
            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                using (var fileStream = new MemoryStream())
                {
                    file.CopyTo(fileStream);
                    await _mediaService.UploadAsync(fileName, fileStream);
                }
            }

            return new OkObjectResult("Upload Successful.");
        }
    }
}