namespace ArtBiathlon.Exceptions
{
    public class AccountNotFoundException : Exception
    {
        public string Description { get; private set; }

        public AccountNotFoundException()
        {
            Description = "Аккаунт не найден";
        }
    }
}
