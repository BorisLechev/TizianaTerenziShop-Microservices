namespace TizianaTerenzi.WebClient.ViewModels.Notes
{
    using System.ComponentModel.DataAnnotations;

    public class CreateNoteInputModel
    {
        private const string NameErrorMessage = "Name should be between 2 and 25 characters long and first letter should be capital.";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
        [StringLength(25, ErrorMessage = NameErrorMessage, MinimumLength = 2)]
        [RegularExpression(@"^([A-Z])([a-z\s]{1,24})$", ErrorMessage = NameErrorMessage)]
        public string Name { get; set; }
    }
}
