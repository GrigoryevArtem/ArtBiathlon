namespace ArtBiathlon.Exceptions
{
    public class AccountExistsException : Exception
    {
        public string Description { get; private set; }

        public AccountExistsException()
        {
            Description = "Аккаунт с таким логином уже существует";
        }
    }
}

