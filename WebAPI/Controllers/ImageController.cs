using Microsoft.AspNetCore.Mvc;
using System.Drawing.Imaging;
using System.Drawing;

namespace WebAPI.Controllers;

using Model;

[ApiController]
[Route("image")]
public class ImageController : ControllerBase{
    [HttpGet("test")]
    public object Test(){

        EdimagesContext context = new EdimagesContext();

        return context.Imagens.ToList();
    }
    [HttpPost("save")]
    public object Save([FromBody]Base64Image img){
        EdimagesContext context = new EdimagesContext();

        Imagen img2 = new Imagen();
        img2.Bytes = Convert.FromBase64String(img.Image);
        img2.Title = img.Title;
        img2.Uri = randString();

        context.Imagens.Add(img2);
        context.SaveChanges();

        return new{
            Status = "Success!",
            Message = "Data saved on database!",
            Data = img2.Uri
        };
    }

    private string randString(){
        
        int seed = unchecked((int)DateTime.Now.Ticks);
        Random rand = new Random(seed);
        
        byte[] randData = new byte[12];
        rand.NextBytes(randData);

        var base64Uri = Convert.ToBase64String(randData);
        return base64Uri.Replace('/', 'X');

    }

    [HttpGet("get/{url}")]
    public object Get(string url){
        EdimagesContext context = new EdimagesContext();
        var img = context.Imagens.FirstOrDefault(x => x.Uri == url);
        if(img == null){
            return new {
                status = "Fail",
                message = "Image not found."
            };
        }
        var bytes = img.Bytes;
        return File(bytes, "image/jpeg");
    }

    [HttpPost("effect")]

    public object Effect([FromBody]Base64Image img){
        var imgBytes = Convert.FromBase64String(img.Image);

        MemoryStream ms = new MemoryStream(imgBytes);
        Bitmap bmp = Bitmap.FromStream(ms) as Bitmap;

        Graphics g = Graphics.FromImage(bmp);
        g.Clear(Color.Blue);

        ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Jpeg);

        EdimagesContext context = new EdimagesContext();

        Imagen image = new Imagen();
        image.Bytes = ms.GetBuffer();
        image.Title = img.Title;
        image.Uri = randString();

        context.Imagens.Add(image);
        context.SaveChanges();

        return new{
            Status = "Success",
            Message = "Data salved on database.",
            Data = image.Uri
        };
    }    

    [HttpPost("create/{wid}x{hei}({r},{g},{b}),{title}")]

    public object Create(int wid, int hei, int r, int g, int b, string title){
        Bitmap bmp = new Bitmap(wid, hei);
        var graphics = Graphics.FromImage(bmp);

        var color = Color.FromArgb(r, g, b);
        graphics.Clear(color);

        MemoryStream ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Jpeg);

        Imagen img = new Imagen();
        img.Title = title;
        img.Uri = randString();
        img.Bytes = ms.GetBuffer();

        EdimagesContext context = new EdimagesContext();
        context.Imagens.Add(img);
        context.SaveChanges();

        return new{
            status = "Success",
            message = "Image created successful",
            uri = img.Uri
        };
    }

    [HttpPost("drawline/{url},({x1},{y1}),({x2},{y2}),({r},{g},{b}),{wid}")]

    public object drawLine(string url, int x1, int y1, int x2, int y2, int r, int g, int b, int wid){
        EdimagesContext context = new EdimagesContext();
        var img = context.Imagens.FirstOrDefault(x => x.Uri == url);
        if (img == null){
            return new{
                status = "fail",
                message = "Image not found"
            };
        }
        MemoryStream ms = new MemoryStream(img.Bytes);
        Bitmap bmp = Bitmap.FromStream(ms) as Bitmap;
        var graphic = Graphics.FromImage(bmp);

        var color = Color.FromArgb(r,g,b);
        var pen = new Pen(color, wid);
        graphic.DrawLine(pen, x1, y1, x2, y2);

        ms = new MemoryStream();
        bmp.Save(ms, ImageFormat.Jpeg);

        img.Bytes = ms.GetBuffer();
        context.SaveChanges();

        return new{
            status = "Success",
            message = "Image successfully changed"
        };
    }
}