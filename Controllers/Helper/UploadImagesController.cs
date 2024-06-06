using CargoApi.Custom_Models;
using CargoApi.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

namespace CargoApi.Controllers.Helper
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImagesController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly PRIORITY_WWDContext _context;

        public UploadImagesController(IWebHostEnvironment env, PRIORITY_WWDContext context)
        {
            _env = env;
            _context = context;
        }

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
                    return Ok(new { message = "File uploaded successfully" });
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



        #region Get all images by Shipment Number


        [HttpPost]
        [Route("GetImages")]
        public async Task<IActionResult> GetImages([FromBody] GetImagesDto dto)
        {
            if (string.IsNullOrEmpty(dto.shipmentNumber))
            {
                return BadRequest("Shipment number is required.");
            }

            try
            {
                var reciptNmbrLst = await _context.Receipts
                                                  .Where(x => x.ShptNmbr == dto.shipmentNumber)
                                                  .Select(x => x.RcptNmbr)
                                                  .ToListAsync();

                string[] allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".tif" };
                string imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                var imageFiles = Directory.GetFiles(imageFolder)
                                          .Where(file => allowedExtensions.Contains(Path.GetExtension(file).ToLower()))
                                          .ToList();

                var filteredImages = imageFiles.Where(file =>
                    reciptNmbrLst.Any(receipt =>
                        Path.GetFileNameWithoutExtension(file).StartsWith($"{dto.shipmentNumber}-{receipt}-")
                    )).ToList();

                var images = new List<string>();
                foreach (var imagePath in filteredImages)
                {
                    var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
                    var imageBase64 = Convert.ToBase64String(imageBytes);
                    images.Add(imageBase64);
                }

                return Ok(images);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }





        #endregion
    }
}
