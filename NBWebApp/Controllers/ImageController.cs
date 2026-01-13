using Microsoft.AspNetCore.Mvc;

namespace NBWebApp.Controllers
{
    /// <summary>
    /// 图像接口
    /// </summary>
    public class ImageController : Controller
    {
        IConfiguration _config;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public ImageController(IConfiguration config)
        {
            _config = config;
        }
        /// <summary>
        /// 图像接口
        /// </summary>
        /// <param name="name">图像名</param>
        /// <returns></returns>
        [HttpGet("Image/{name}")]
        public IActionResult Image(string name)
        {
            string savepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Image");
            var imgPath = Path.Combine(savepath, name);
            if (System.IO.File.Exists(imgPath))
            {
                using (var sw = new FileStream(imgPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var bytes = new byte[sw.Length];
                    sw.Read(bytes, 0, bytes.Length);
                    sw.Close();
                    return new FileContentResult(bytes, "image/jpeg");
                }
            }
            else
            {
                return NotFound();
            }

        }
        /// <summary>
        /// 波形接口
        /// </summary>
        /// <param name="name">图像名</param>
        /// <returns></returns>
        [HttpGet("Wave/{name}")]
        public IActionResult Wave(string name)
        {
            string savepath =_config.GetSection("WavePath").Value;
            var imgPath = Path.Combine(savepath, name);
            if (System.IO.File.Exists(imgPath))
            {
                using (var sw = new FileStream(imgPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var bytes = new byte[sw.Length];
                    sw.Read(bytes, 0, bytes.Length);
                    sw.Close();
                    return new FileContentResult(bytes, "image/tiff");
                }
            }
            else
            {
                return NotFound();
            }

        }
    }
}
