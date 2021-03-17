namespace TizianaTerenzi.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using TizianaTerenzi.Common;
    using TizianaTerenzi.Services.Cloudinary;
    using TizianaTerenzi.Services.Data.Comments;
    using TizianaTerenzi.Services.Data.FragranceGroups;
    using TizianaTerenzi.Services.Data.Notes;
    using TizianaTerenzi.Services.Data.Products;
    using TizianaTerenzi.Services.Data.Votes;
    using TizianaTerenzi.Web.ViewModels.Products;

    public class ProductsController : AdministrationController
    {
        private readonly INotesService notesService;

        private readonly IProductTypesService productTypesService;

        private readonly IFragranceGroupsService fragranceGroupsService;

        private readonly IProductsService productsService;

        private readonly ICommentsService commentsService;

        private readonly ICommentVotesService commentVotesService;

        private readonly IProductVotesService productVotesService;

        private readonly ICloudinaryService cloudinaryService;

        public ProductsController(
            INotesService notesService,
            IProductTypesService productTypesService,
            IFragranceGroupsService fragranceGroupsService,
            IProductsService productsService,
            ICommentsService commentsService,
            ICommentVotesService commentVotesService,
            IProductVotesService productVotesService,
            ICloudinaryService cloudinaryService)
        {
            this.notesService = notesService;
            this.productTypesService = productTypesService;
            this.fragranceGroupsService = fragranceGroupsService;

            this.productsService = productsService;
            this.commentsService = commentsService;
            this.commentVotesService = commentVotesService;
            this.productVotesService = productVotesService;
            this.cloudinaryService = cloudinaryService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var notes = await this.notesService.GetAllNotesAsync();
            var productTypes = await this.productTypesService.GetAllProductTypesAsync();
            var fragranceGroups = await this.fragranceGroupsService.GetAllFragranceGroupsAsync();

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
                var notes = await this.notesService.GetAllNotesAsync();
                var productTypes = await this.productTypesService.GetAllProductTypesAsync();
                var fragranceGroups = await this.fragranceGroupsService.GetAllFragranceGroupsAsync();

                inputModel.ProductTypes = productTypes;
                inputModel.FragranceGroups = fragranceGroups;
                inputModel.Notes = notes;

                return this.View(inputModel);
            }

            string pictureUrl = await this.cloudinaryService.UploadPictureAsync(inputModel.Picture, inputModel.Name);
            var result = await this.productsService.CreateProductAsync(inputModel, pictureUrl);

            if (result == false)
            {
                this.Error(NotificationMessages.CreateProductError);

                return this.LocalRedirect("/products/all");
            }

            this.Success(NotificationMessages.CreateProductSuccessfully);

            return this.LocalRedirect("/products/all");
        }

        [HttpGet]
        [Route("/administration/product/edit/{productId}")]
        public async Task<IActionResult> Edit(int productId)
        {
            if (productId <= 0)
            {
                this.NotFound();
            }

            var productTypes = await this.productTypesService.GetAllProductTypesAsync();
            var fragranceGroups = await this.fragranceGroupsService.GetAllFragranceGroupsAsync();
            var notes = await this.notesService.GetAllNotesWithSelectedByProductIdAsync(productId);

            var editProductInputModel = await this.productsService.GetProductByIdAsync<EditProductInputModel>(productId);
            editProductInputModel.ProductTypes = productTypes;
            editProductInputModel.FragranceGroups = fragranceGroups;
            editProductInputModel.Notes = notes;

            return this.View(editProductInputModel);
        }

        [HttpPost]
        [Route("/administration/product/edit/{productId}")]
        public async Task<IActionResult> Edit(EditProductInputModel inputModel, int productId)
        {
            if (productId <= 0)
            {
                this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            string pictureUrl = await this.cloudinaryService.UploadPictureAsync(inputModel.Picture, inputModel.Name);

            var result = await this.productsService.EditProductAsync(inputModel, productId, pictureUrl);

            if (result == true)
            {
                this.Success(NotificationMessages.EditProductSuccessfully);

                return this.LocalRedirect("/products/all");
            }

            this.Error(NotificationMessages.EditProductError);

            return this.LocalRedirect("/products/all");
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
                await this.notesService.DeleteAllProductNotesAsync(id);
                await this.commentVotesService.DeleteRangeByProductIdAsync(id);
                await this.commentsService.DeleteRangeByProductIdAsync(id);
                await this.productVotesService.DeleteProductVotesAsync(id);
                await this.productsService.DeleteProductAsync(id);

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
