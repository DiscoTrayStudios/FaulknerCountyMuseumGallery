#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using FaulknerCountyMuseumGallery.Data;
using FaulknerCountyMuseumGallery.Models;
using FaulknerCountyMuseumGallery.Pages.Courses;
using Microsoft.AspNetCore.Authorization;
//imports for image upload
using ImageMagick;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;


namespace FaulknerCountyMuseumGallery.Pages.Artworks
{
    [Authorize]
    public class CreateModel : ArtistMediumCollectionPageModel
    {
        private readonly FaulknerCountyMuseumGallery.Data.GalleryContext _context;

        [BindProperty]
        public Artwork Artwork { get; set; }
        public string Title { get; set; }
        public string AccessionNumber { get; set; }
        public string ImageURL { get; set; }
        public string Size { get; set; }
        public string Status { get; set; }
        public string Donor { get; set; }
        //new image upload
        public string NamePath {get;set;}
        public string relNamePath{get;set;}
        // this is the relative path of the file; for displaying on create page
        


        public CreateModel(FaulknerCountyMuseumGallery.Data.GalleryContext context)
        {
            _context = context;
        }


        public IActionResult OnGet(int? artistID, int? collectionID, int? mediumID,
            string title = "", string accessionNumber = "", string imageURL = "",
            string size = "", string status = "", string donor = "", string? imagePath = "")
        {
            Title = title;
            AccessionNumber = accessionNumber;
            ImageURL = imageURL;
            Size = size;
            Status = status;
            Donor = donor;
            NamePath = imagePath;
            relNamePath = imagePath;
            PopulateArtistsDropDownList(_context, artistID);
            PopulateMediumsDropDownList(_context, mediumID);
            PopulateCollectionsDropDownList(_context, collectionID);

            
            Console.Write("Here is the imagepathname: ", imagePath);
            if (imagePath == null){
                imagePath = "No file selected";
                Console.Write("The image path isn't working" + imagePath);
            }
            else{
                
                NamePath = imagePath.Replace("%2F", "/");
                relNamePath = imagePath.Replace("%2F", "/");
                relNamePath = relNamePath.Substring(relNamePath.Length - 20);
                
                
                Console.Write("here is the relative path" + relNamePath);
                Console.Write("The image path IS working" + NamePath);
                
            }
            return Page();
        }

        public IActionResult OnPostCreateArtistAsync()
        {
            if (HttpContext.Request.Form != null)
            {
                var title = HttpContext.Request.Form["Artwork.Title"];
                var accessionNumber = HttpContext.Request.Form["Artwork.AccessionNumber"];
                var imageURL = HttpContext.Request.Form["Artwork.ImageLink"];
                var status = HttpContext.Request.Form["Artwork.Status"];
                var donor = HttpContext.Request.Form["Artwork.Donor"];
                var size = HttpContext.Request.Form["Artwork.Size"];
                var mediumID = HttpContext.Request.Form["Artwork.MediumID"];
                var collectionID = HttpContext.Request.Form["Artwork.CollectionID"];

                return RedirectToPage("/Artists/Create", new
                {
                    prevPage = Request.Path,
                    title,
                    accessionNumber,
                    imageURL,
                    status,
                    donor,
                    size,
                    mediumID,
                    collectionID
                });
            }
            else
            {
                return Page();
            }

        }

        public IActionResult OnPostCreateMediumAsync()
        {
            if (HttpContext.Request.Form != null)
            {
                var title = HttpContext.Request.Form["Artwork.Title"];
                var accessionNumber = HttpContext.Request.Form["Artwork.AccessionNumber"];
                var imageURL = HttpContext.Request.Form["Artwork.ImageLink"];
                var status = HttpContext.Request.Form["Artwork.Status"];
                var donor = HttpContext.Request.Form["Artwork.Donor"];
                var size = HttpContext.Request.Form["Artwork.Size"];
                var artistID = HttpContext.Request.Form["Artwork.ArtistID"];
                var collectionID = HttpContext.Request.Form["Artwork.CollectionID"];

                return RedirectToPage("/Mediums/Create", new
                {
                    prevPage = Request.Path,
                    title,
                    accessionNumber,
                    imageURL,
                    status,
                    donor,
                    size,
                    artistID,
                    collectionID
                });
            }
            else
            {
                return Page();
            }

        }

        public IActionResult OnPostCreateCollectionAsync()
        {
            if (HttpContext.Request.Form != null)
            {
                var title = HttpContext.Request.Form["Artwork.Title"];
                var accessionNumber = HttpContext.Request.Form["Artwork.AccessionNumber"];
                var imageURL = HttpContext.Request.Form["Artwork.ImageLink"];
                var status = HttpContext.Request.Form["Artwork.Status"];
                var donor = HttpContext.Request.Form["Artwork.Donor"];
                var size = HttpContext.Request.Form["Artwork.Size"];
                var artistID = HttpContext.Request.Form["Artwork.ArtistID"];
                var mediumID = HttpContext.Request.Form["Artwork.MediumID"];

                return RedirectToPage("/Collections/Create", new
                {
                    prevPage = Request.Path,
                    title,
                    accessionNumber,
                    imageURL,
                    status,
                    donor,
                    size,
                    mediumID,
                    artistID
                });
            }
            else
            {
                return Page();
            }

        }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var emptyArtwork = new Artwork();
            Console.Write("testing!!!!");
            
            if (await TryUpdateModelAsync<Artwork>(
                emptyArtwork,
                "artwork",
                s => s.ArtworkID,
                s => s.ArtistID,
                s => s.MediumID,
                s => s.CollectionID,
                s => s.Title,
                s => s.AccessionNumber,
                s => s.ImageLink,
                s => s.Size,
                s => s.Status,
                s => s.Donor))
            {
                
                _context.Artworks.Add(emptyArtwork);
                await _context.SaveChangesAsync();

                

                /*File upload into folder*/
                /*https://learn.microsoft.com/en-us/answers/questions/807026/upload-image-to-asp-net-razor-page*/
                /*
                if (FileUpload.FormFile.Length > 0)
                {
                    using (var stream = new FileStream(Path.Combine(_hostenvironment.WebRootPath, "uploadfiles", FileUpload.FormFile.FileName), FileMode.Create))
                    {
                        await FileUpload.FormFile.CopyToAsync(stream);
                    }
                }
                */

                return RedirectToPage("./Index");
            }

            PopulateArtistsDropDownList(_context, emptyArtwork.ArtistID);
            PopulateMediumsDropDownList(_context, emptyArtwork.MediumID);
            PopulateCollectionsDropDownList(_context, emptyArtwork.CollectionID);


            return Page();



        }
    }
}