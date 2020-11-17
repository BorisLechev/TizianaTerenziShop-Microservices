namespace MelegPerfumes.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MelegPerfumes.Common;
    using MelegPerfumes.Data.Models;
    using MelegPerfumes.Services.Data;
    using MelegPerfumes.Web.Areas.Administration.Models.Products;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class ProductsController : AdministrationController
    {
        private readonly INotesService notesService;

        private readonly IProductTypesService productTypesService;

        private readonly IFragranceGroupsService fragranceGroupsService;

        private readonly IProductsService productsService;

        private readonly ICloudinaryService cloudinaryService;

        public ProductsController(
            INotesService notesService,
            IProductTypesService productTypesService,
            IFragranceGroupsService fragranceGroupsService,
            IProductsService productsService,
            ICloudinaryService cloudinaryService)
        {
            this.notesService = notesService;
            this.productTypesService = productTypesService;
            this.fragranceGroupsService = fragranceGroupsService;

            this.productsService = productsService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var notes = await this.GetAllNotes();
            var productTypes = await this.GetAllProductTypes();
            var fragranceGroups = await this.GetAllFragranceGroups();

            var product = new CreateProductInputModel
            {
                ProductTypes = productTypes,
                FragranceGroups = fragranceGroups,
                Notes = notes,
            };

            return this.View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                var notes = await this.GetAllNotes();
                var productTypes = await this.GetAllProductTypes();
                var fragranceGroups = await this.GetAllFragranceGroups();

                inputModel.ProductTypes = productTypes;
                inputModel.FragranceGroups = fragranceGroups;
                inputModel.Notes = notes;

                return this.View(inputModel);
            }

            var notesIds = inputModel.NoteIds
                        .First()
                        .Split(",", StringSplitOptions.RemoveEmptyEntries)
                        .Select(int.Parse);

            string pictureUrl = await this.cloudinaryService.UploadPictureAsync(inputModel.Picture, inputModel.Name);

            var product = new Product
            {
                Name = inputModel.Name,
                Description = inputModel.Description,
                ProductTypeId = inputModel.ProductTypeId,
                FragranceGroupId = inputModel.FragranceGroupId,
                YearOfManufacture = inputModel.YearOfManufacture,
                Price = inputModel.Price,
                Notes = notesIds.Select(id => new ProductNotes
                {
                    NoteId = id,
                })
                .ToList(),
            };

            product.Picture = pictureUrl;

            var result = await this.productsService.CreateProductAsync(product);

            if (!result)
            {
                this.Error(NotificationMessages.CreateProductError);

                return this.LocalRedirect("/products/all");
            }

            this.Success(NotificationMessages.CreateProductSuccessfully);

            return this.LocalRedirect("/products/all");
        }

        [HttpGet]
        public IActionResult CreateNote()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote(CreateProductNoteInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var result = await this.notesService.CreateNoteAsync(inputModel.Name);

            if (result == false)
            {
                this.Error(NotificationMessages.CreateNoteError);

                return this.LocalRedirect("/home/index");
            }

            this.Success(NotificationMessages.CreateNoteSuccessfully);

            return this.LocalRedirect("/home/index");
        }

        private async Task<IEnumerable<SelectListItem>> GetAllNotes()
        {
            var notes = (await this.notesService.GetAllNotesAsync())
                .Select(n => new SelectListItem
                {
                    Text = n.Name,
                    Value = n.Id.ToString(),
                });

            return notes;
        }

        private async Task<IEnumerable<SelectListItem>> GetAllProductTypes()
        {
            var productTypes = (await this.productTypesService.GetAllProductTypes())
                .Select(pt => new SelectListItem
                {
                    Text = pt.Name,
                    Value = pt.Id.ToString(),
                });

            return productTypes;
        }

        private async Task<IEnumerable<SelectListItem>> GetAllFragranceGroups()
        {
            var fragranceGroups = (await this.fragranceGroupsService.GetAllFragranceGroups())
                .Select(fg => new SelectListItem
                {
                    Text = fg.Name,
                    Value = fg.Id.ToString(),
                });

            return fragranceGroups;
        }
    }
}
