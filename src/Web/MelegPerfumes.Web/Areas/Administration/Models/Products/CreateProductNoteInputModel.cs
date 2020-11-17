namespace MelegPerfumes.Web.Areas.Administration.Models.Products
{
    using System.ComponentModel.DataAnnotations;

    public class CreateProductNoteInputModel
    {
        private const int NameMinimumLength = 2;

        private const int NameMaximumLength = 25;

        private const string NameErrorMessage = "Name should be at least 2 characters long and not more than 25 and first letter should be capital.";

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(NameMaximumLength, MinimumLength = NameMinimumLength, ErrorMessage = NameErrorMessage)]
        [RegularExpression(@"^([A-Z])([a-z\s]{1,24})$", ErrorMessage = NameErrorMessage)]
        public string Name { get; set; }
    }
}
