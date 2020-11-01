namespace MelegPerfumes.Common
{
    public static class NotificationMessages
    {
        public const string RegistrationWelcome = "Welcome to Meleg perfumes, {0}!";
        public const string LoggedOut = "Logged out successfully";
        public const string PasswordChanged = "Password changed successfully";
        public const string PasswordSet = "Password set successfully";
        public const string InvalidPassword = "Invalid password";
        public const string ProfileDetailsUpdated = "Details updated successfully";

        public const string AccountDeleted = "We're sorry to see you go. Your account was deleted.";

        public const string SubsribedSuccessfully = "You have successfully subscribed.";
        public const string SubscriberEmailExists = "This email has already been subscribed.";

        public const string AccountDeleteError =
            "An error occured while deleting your account. Try again or contact support.";

        public const string ProductsInTheCartQuantityError = "The quantity of products in the cart cannot be a negative number.";
        public const string ProductCreateSuccessfully = "You have successfully created a product.";
        public const string ProductCreateError = "Something went wrong.";

        public const string CannotDeleteThisProductInTheCartError = "Cannot delete this product in the cart error.";
    }
}
