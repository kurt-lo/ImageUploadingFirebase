using ImageUploadingFirebase.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using FireSharp.Interfaces;
using FireSharp.Config;
using Firebase.Storage;
using FireSharp.Response;

namespace ImageUploadingFirebase.Controllers
{
    public class ImageController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "YaBnwZscViorEnXOJc0OdkQQH5seXlfhiAZOYgFU",
            BasePath = "https://imageuploading-94654-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        
        public IActionResult Index()
        {
            //this is the image class in the models
            Image img = new Image();
            var display = Path.Combine(_webHostEnvironment.WebRootPath, "NewFolder");
            DirectoryInfo di = new DirectoryInfo(display);
            FileInfo[] fileinfo = di.GetFiles();
            img.FileImage = fileinfo;
            return View(img);
        }
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile file, Image image)
        {
            //pagsave ng image sa folder
            var imgsave = Path.Combine(_webHostEnvironment.WebRootPath, "NewFolder", file.FileName);
            var stream = new FileStream(imgsave, FileMode.Create);
            await file.CopyToAsync(stream);

            //pagstore sa firebase
            client = new FireSharp.FirebaseClient(config);
            var data = image;
            PushResponse response = client.Push("Images/", file.FileName);
            data.Id = response.Result.name;

            stream.Close();
            // using (Stream fileStream = new FileStream(path, FileMode.Create))
            // {
            //    await file.CopyToAsync(fileStream);
            //}
            return RedirectToAction("Index");
        }
    }
}
