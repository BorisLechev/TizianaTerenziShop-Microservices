namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Data.Models;
    using TizianaTerenzi.Services.Data;
    using TizianaTerenzi.Web.Areas.Administration.Models.Products;

    public class ProductsController : AdministrationController
    {
        private readonly INotesService notesService;

        private readonly IProductTypesService productTypesService;

        private readonly IFragranceGroupsService fragranceGroupsService;

        private readonly IProductsService productsService;

        private readonly ICommentsService commentsService;

        private readonly ICloudinaryService cloudinaryService;

        public ProductsController(
            INotesService notesService,
            IProductTypesService productTypesService,
            IFragranceGroupsService fragranceGroupsService,
            IProductsService productsService,
            ICommentsService commentsService,
            ICloudinaryService cloudinaryService)
        {
            this.notesService = notesService;
            this.productTypesService = productTypesService;
            this.fragranceGroupsService = fragranceGroupsService;

            this.productsService = productsService;
            this.commentsService = commentsService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var notes = await this.GetAllNotesAsync();
            var productTypes = await this.GetAllProductTypesAsync();
            var fragranceGroups = await this.GetAllFragranceGroupsAsync();

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
                var notes = await this.GetAllNotesAsync();
                var productTypes = await this.GetAllProductTypesAsync();
                var fragranceGroups = await this.GetAllFragranceGroupsAsync();

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

                return this.RedirectToAction("All", "Products");
            }

            this.Success(NotificationMessages.CreateProductSuccessfully);

            return this.RedirectToAction("All", "Products");
        }

        [HttpGet]
        [Route("/administration/product/edit/{productId}")]
        public async Task<IActionResult> Edit(int? productId)
        {
            if (productId == null)
            {
                this.NotFound();
            }

            var product = await this.productsService.GetProductByIdAsync(productId);

            if (product == null)
            {
                this.NotFound();
            }

            var productTypes = await this.GetAllProductTypesWithSelectedTypeAsync(productId);
            var fragranceGroups = await this.GetAllFragranceGroupsWithSelectedGroupAsync(productId);
            var notes = await this.GetAllNotesWithSelectedNotesAsync(productId);

            var editProductViewModel = new EditProductInputModel
            {
                Id = productId.Value,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                YearOfManufacture = product.YearOfManufacture,
                FragranceGroups = fragranceGroups,
                ProductTypes = productTypes,
                Notes = notes,
            };

            return this.View(editProductViewModel);
        }

        [HttpPost]
        [Route("/administration/product/edit/{productId}")]
        public async Task<IActionResult> Edit(EditProductInputModel inputModel, int? productId)
        {
            if (productId == null)
            {
                this.NotFound();
            }

            var notes = await this.GetAllNotesWithSelectedNotesAsync(productId);
            inputModel.Notes = notes;

            var product = await this.productsService.GetProductByIdAsync(productId);

            if (!this.ModelState.IsValid)
            {
                var productTypes = await this.GetAllProductTypesAsync();
                var fragranceGroups = await this.GetAllFragranceGroupsAsync();

                inputModel.Id = productId.Value;
                inputModel.Name = product.Name;
                inputModel.Description = product.Description;
                inputModel.Price = product.Price;
                inputModel.YearOfManufacture = product.YearOfManufacture;
                inputModel.FragranceGroups = fragranceGroups;
                inputModel.ProductTypes = productTypes;
                inputModel.Notes = notes;

                return this.View(inputModel);
            }

            string pictureUrl = await this.cloudinaryService.UploadPictureAsync(inputModel.Picture, inputModel.Name);

            var noteIdsCollection = new List<int>();

            foreach (var noteName in inputModel.Notes)
            {
                var note = await this.notesService.FindNoteByNameAsync(noteName.Text);

                if (note != null)
                {
                    noteIdsCollection.Add(note.Id);
                }
                else
                {
                    this.Error(NotificationMessages.NoteNotFound);

                    return this.NotFound();
                }
            }

            await this.notesService.DeleteProductNotesAsync(productId);

            product.Name = inputModel.Name;
            product.Description = inputModel.Description;
            product.Picture = pictureUrl;
            product.Price = inputModel.Price;
            product.YearOfManufacture = inputModel.YearOfManufacture;
            product.FragranceGroupId = inputModel.FragranceGroupId;
            product.ProductTypeId = inputModel.ProductTypeId;
            product.Notes = noteIdsCollection.Select(id => new ProductNotes
            {
                NoteId = id,
            })
            .ToList();

            var result = await this.productsService.EditProductAsync(product);

            if (result == true)
            {
                this.Success(NotificationMessages.EditProductSuccessfully);

                return this.RedirectToAction("All", "Products");
            }

            this.Error(NotificationMessages.EditProductError);

            return this.RedirectToAction("All", "Products");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return this.NotFound();
            }

            try
            {
                await this.notesService.DeleteProductNotesAsync(id);
                await this.commentsService.DeleteRangeAsync(id);
                await this.productsService.DeleteProductAsync(id);

                this.Success(NotificationMessages.DeleteProductSuccessfully);

                return this.LocalRedirect("/products/all");
            }
            catch (Exception)
            {
                this.Error(NotificationMessages.DeleteProductError);

                return this.RedirectToAction("All", "Products");
            }
        }

        private async Task<IEnumerable<SelectListItem>> GetAllNotesAsync()
        {
            var notes = (await this.notesService.GetAllNotesAsync())
                .Select(n => new SelectListItem
                {
                    Text = n.Name,
                    Value = n.Id.ToString(),
                });

            return notes;
        }

        private async Task<IEnumerable<SelectListItem>> GetAllNotesWithSelectedNotesAsync(int? productId)
        {
            var noteIds = await this.notesService.GetAllNoteIdsByProductAsync(productId);
            var notes = (await this.notesService.GetAllNotesAsync())
                .Select(n => new SelectListItem
                {
                    Text = n.Name,
                    Value = n.Id.ToString(),
                    Selected = noteIds.Any(id => id == n.Id),
                });

            return notes;
        }

        private async Task<IEnumerable<SelectListItem>> GetAllProductTypesAsync()
        {
            var productTypes = (await this.productTypesService.GetAllProductTypes())
                .Select(pt => new SelectListItem
                {
                    Text = pt.Name,
                    Value = pt.Id.ToString(),
                });

            return productTypes;
        }

        private async Task<IEnumerable<SelectListItem>> GetAllProductTypesWithSelectedTypeAsync(int? productId)
        {
            var productTypeId = await this.productsService.GetProductTypeIdByProductIdAsync(productId);
            var productTypes = (await this.productTypesService.GetAllProductTypes())
                .Select(pt => new SelectListItem
                {
                    Text = pt.Name,
                    Value = pt.Id.ToString(),
                    Selected = pt.Id == productTypeId,
                });

            return productTypes;
        }

        private async Task<IEnumerable<SelectListItem>> GetAllFragranceGroupsAsync()
        {
            var fragranceGroups = (await this.fragranceGroupsService.GetAllFragranceGroups())
                .Select(fg => new SelectListItem
                {
                    Text = fg.Name,
                    Value = fg.Id.ToString(),
                });

            return fragranceGroups;
        }

        private async Task<IEnumerable<SelectListItem>> GetAllFragranceGroupsWithSelectedGroupAsync(int? productId)
        {
            var fragranceGroupId = await this.productsService.GetFragranceGroupIdByProductIdAsync(productId);
            var fragranceGroups = (await this.fragranceGroupsService.GetAllFragranceGroups())
                .Select(fg => new SelectListItem
                {
                    Text = fg.Name,
                    Value = fg.Id.ToString(),
                    Selected = fg.Id == fragranceGroupId,
                });

            return fragranceGroups;
        }
    }
}
