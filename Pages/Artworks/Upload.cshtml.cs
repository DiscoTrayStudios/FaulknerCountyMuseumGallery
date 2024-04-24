using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;


namespace FaulknerCountyMuseumGallery.Pages.Artworks
{
    public class UploadModel : PageModel
    {
        [BindProperty]
        public IFormFile Upload { get; set; }
        private string imagesDir;
        public string filePath;
        //private MagickImage watermark;

        public UploadModel(IWebHostEnvironment environment)
        {
            imagesDir = Path.Combine(environment.WebRootPath, "images");

            //watermark = new MagickImage("watermark.png");
            //watermark.Evaluate(Channels.Alpha, EvaluateOperator.Divide, 2);
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Upload != null)
            {
                Console.Write("inside of upload");
                Console.Write(Upload);
                string extension = ".jpg";
                switch (Upload.ContentType)
                {
                    case "image/png":
                        extension = ".png";
                        break;
                    case "image/gif":
                        extension = ".gif";
                        break;
                }

               var fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + extension;
               var filePath = Path.Combine(imagesDir, fileName);
               Console.Write("This is filepath: " + filePath);

                //using (var fs = System.IO.File.OpenWrite(filePath))
                //{
                //    await Upload.CopyToAsync(fs);
                //}

                // Add watermark to the uploaded image
                using (var image = new MagickImage(Upload.OpenReadStream()))//filePath
                {
                    //image.Composite(watermark, Gravity.Southeast, CompositeOperator.Over);
                    await image.WriteAsync(filePath);
                }
                Console.Write("This is filepath after saving it " + filePath);
                Console.Write("here is the file path before sending it to new page " + filePath);
                return RedirectToPage("Create", new {imagePath = filePath} );
                
            }
            else{
            Console.Write("something went wrong. here is the file path " + filePath);
            return RedirectToPage("Create", new {imagePath = filePath} );
            }
            //Response.Redirect("Create?ImageFileName="+ filePath);
        }
    }
}
