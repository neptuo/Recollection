﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Neptuo;
using Neptuo.Recollections.Entries.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Recollections.Entries.Controllers
{
    [Route("internal/migrations")]
    public class MigrationController : Controller
    {
        private readonly ImageService service;
        private readonly DataContext dbContext;

        public MigrationController(ImageService service, DataContext dbContext)
        {
            Ensure.NotNull(service, "service");
            Ensure.NotNull(dbContext, "dbContext");
            this.service = service;
            this.dbContext = dbContext;
        }

        [HttpGet("convertimageformat")]
        public async Task<IActionResult> ConvertImageFormat()
        {
            if (IsRemoteRequest())
                return NotFound();

            var images = await dbContext.Images
                .Include(i => i.Entry)
                .ToListAsync();

            foreach (var image in images)
            {
                ImagePath path = service.GetPath(image.Entry, image);

                if (DeleteFileIfDifferentExtension(path.Preview, service.GetPreviewFileExtension()) || DeleteFileIfDifferentExtension(path.Thumbnail, service.GetThumbnailFileExtension()))
                    await service.ComputeOtherSizesAsync(image.Entry, image);
            }


            return Ok();
        }

        private bool IsRemoteRequest() => Request.Host.Host != "localhost" && Request.Host.Host != "127.0.0.1";

        private bool DeleteFileIfDifferentExtension(string path, string extension)
        {
            if (Path.GetExtension(path) != extension)
            {
                System.IO.File.Delete(path);
                return true;
            }

            return false;
        }
    }
}
