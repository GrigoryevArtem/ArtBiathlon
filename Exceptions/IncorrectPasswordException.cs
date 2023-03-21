namespace ArtBiathlon.Exceptions
{
    public class IncorrectPasswordException : Exception
    {
        public string Description { get; private set; }

        public IncorrectPasswordException()
        {
            Description = "Неправильный пароль";
        }
    }
}
