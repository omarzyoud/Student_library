using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace NsfwSpyNS.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NsfwSpyController : ControllerBase
    {
        private readonly INsfwSpy _nsfwSpy;

        public NsfwSpyController(INsfwSpy nsfwSpy)
        {
            _nsfwSpy = nsfwSpy;
        }

        // ... Other endpoints ...

        [HttpPost("checknudity")]
        public ActionResult<string> CheckNudity(IFormFile file)
        {
            try
            {
                var fileBytes = ConvertFormFileToByteArray(file);

                // Classify the image for nudity
                var result = _nsfwSpy.ClassifyImage(fileBytes);

                // Check the result and return appropriate response
                if (result.IsNsfw)
                {
                    return Ok("The image contains nudity.");
                }
                else
                {
                    return Ok("The image is safe.");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return BadRequest($"Error: {ex.Message}");
            }
        }

        private byte[] ConvertFormFileToByteArray(IFormFile file)
        {
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
