using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CargoApi.Controllers.Helper
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImagesController : ControllerBase
    {


        [HttpPost]
        [Route("uploadImage")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadImage(IFormFile thumbnail)
        {
            try
            {
                if (thumbnail != null && thumbnail.Length > 0)
                {
                    var fileName = Path.GetFileName(thumbnail.FileName);
                    //  var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName; // generate a unique file name to avoid conflicts
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);



                    if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    }



                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await thumbnail.CopyToAsync(stream);
                    }
                    return Ok("File uploaded successfully");
                }
                else
                {
                    return BadRequest("Invalid file");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
    }
}
