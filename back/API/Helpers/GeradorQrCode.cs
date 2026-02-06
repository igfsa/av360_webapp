using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace API.Helpers;

public class GeradorQrCode
{
    public static byte[] GenerateQrCode(string url)
    {
        using var qrGenerator = new QRCodeGenerator();
        using var qrData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.Q);
        using var qrCode = new PngByteQRCode(qrData);

        return qrCode.GetGraphic(20);
    }
}
