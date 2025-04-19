
namespace FullStackCrud.Server.Controllers
{
    [Serializable]
    internal class DbUpdateConcurrencyException : Exception
    {
        public DbUpdateConcurrencyException()
        {
        }

        public DbUpdateConcurrencyException(string? message) : base(message)
        {
        }

        public DbUpdateConcurrencyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}