namespace TizianaTerenzi.WebClient.Areas.Administration.Controllers
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Refit;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Cloudinary;
    using TizianaTerenzi.Services.Data.Comments;
    using TizianaTerenzi.Services.Data.FragranceGroups;
    using TizianaTerenzi.Services.Data.Notes;
    using TizianaTerenzi.Services.Data.Votes;
    using TizianaTerenzi.WebClient.Services.Administration;
    using TizianaTerenzi.WebClient.Services.Products;
    using TizianaTerenzi.WebClient.ViewModels.Products;

    public class ProductsController : AdministrationController
    {
        private readonly INotesService notesService;

        private readonly IFragranceGroupsService fragranceGroupsService;

        private readonly IProductsService productsService;

        private readonly ICommentsService commentsService;

        private readonly ICommentVotesService commentVotesService;

        private readonly IProductVotesService productVotesService;

        private readonly ICloudinaryService cloudinaryService;
        private readonly IProductsGatewayService productsGatewayService;
        private readonly IAdministrationService administrationService;

        public ProductsController(
            INotesService notesService,
            IFragranceGroupsService fragranceGroupsService,
            IProductsService productsService,
            ICommentsService commentsService,
            ICommentVotesService commentVotesService,
            IProductVotesService productVotesService,
            ICloudinaryService cloudinaryService,
            IProductsGatewayService productsGatewayService,
            IAdministrationService administrationService)
        {
            this.notesService = notesService;
            this.fragranceGroupsService = fragranceGroupsService;

            this.productsService = productsService;
            this.commentsService = commentsService;
            this.commentVotesService = commentVotesService;
            this.productVotesService = productVotesService;
            this.cloudinaryService = cloudinaryService;
            this.productsGatewayService = productsGatewayService;
            this.administrationService = administrationService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var productDropdowns = await this.productsGatewayService.PrepareDropdownsForProductCreation();

            return this.View(productDropdowns.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductInputModel inputModel)
        {
            if (this.ModelState.IsValid == false)
            {
                var productDropdowns = await this.productsGatewayService.PrepareDropdownsForProductCreation();

                inputModel.ProductTypes = productDropdowns.Data.ProductTypes;
                inputModel.FragranceGroups = productDropdowns.Data.FragranceGroups;
                inputModel.Notes = productDropdowns.Data.Notes;

                return this.View(inputModel);
            }

            var stream = inputModel.Picture.OpenReadStream();
            StreamPart pictureStream = new StreamPart(stream, inputModel.Picture.FileName, inputModel.Picture.ContentType);

            var result = await this.administrationService.CreateProductAsync(inputModel, pictureStream);

            if (result.Succeeded == false)
            {
                this.Error(NotificationMessages.CreateProductError);

                return this.RedirectToAction(nameof(DashboardController.Index), "Dashboard");
            }

            this.Success(NotificationMessages.CreateProductSuccessfully);

            return this.RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int productId)
        {
            if (productId <= 0)
            {
                this.NotFound();
            }

            var editProductInputModel = await this.productsGatewayService.PrepareDataForProductEditing(productId);

            return this.View(editProductInputModel.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProductInputModel inputModel)
        {
            if (inputModel.ProductId <= 0)
            {
                this.NotFound();
            }

            if (this.ModelState.IsValid == false)
            {
                var productDropdowns = await this.productsGatewayService.PrepareDropdownsForProductCreation();

                inputModel.ProductTypes = productDropdowns.Data.ProductTypes;
                inputModel.FragranceGroups = productDropdowns.Data.FragranceGroups;
                inputModel.Notes = productDropdowns.Data.Notes;

                return this.View(inputModel);
            }

            StreamPart pictureStream = new StreamPart(new MemoryStream(), string.Empty);

            if (inputModel.Picture != null)
            {
                var stream = inputModel.Picture.OpenReadStream();
                pictureStream = new StreamPart(stream, inputModel.Picture.FileName, inputModel.Picture.ContentType);
            }

            var result = await this.administrationService.EditProductAsync(inputModel, pictureStream);

            if (result.Succeeded)
            {
                this.Success(NotificationMessages.EditProductSuccessfully);

                return this.LocalRedirect("/products/all");
            }

            this.Error(NotificationMessages.EditProductError);

            return this.LocalRedirect("/products/all");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            if (productId <= 0)
            {
                return this.NotFound();
            }

            try
            {
                await this.notesService.SoftDeleteAllProductNotesAsync(productId);
                await this.commentVotesService.DeleteRangeByProductIdAsync(productId);
                await this.commentsService.DeleteRangeByProductIdAsync(productId);
                await this.productVotesService.DeleteProductVotesAsync(productId);
                //await this.productsService.DeleteProductAsync(productId);

                this.Success(NotificationMessages.DeleteProductSuccessfully);

                return this.LocalRedirect("/products/all");
            }
            catch (Exception)
            {
                this.Error(NotificationMessages.DeleteProductError);

                return this.LocalRedirect("/products/all");
            }
        }
    }
}
