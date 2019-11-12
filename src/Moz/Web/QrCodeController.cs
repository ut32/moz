using System;
using System.DrawingCore.Imaging;
//using System.DrawingCore.Imaging;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace Moz.Web
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class QrCodeController : Controller
    {
        [Route("/qrcode")]  
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult Generate([FromQuery] string txt)
        {
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(txt, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20);
            using (var stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, ImageFormat.Jpeg);
                var data = new byte[stream.Length];
                stream.Seek(0, SeekOrigin.Begin);
                stream.Read(data, 0, Convert.ToInt32(stream.Length));
                return File(data, @"image/jpeg");
            }
        }
    }
}