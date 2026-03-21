using DotNetBuildingBlocks.Abstractions;
using DotNetBuildingBlocks.Guards;

namespace DotNetBuildingBlocks.ConsoleDemo
{
    /// <summary>
    /// Customer class
    /// </summary>
    public sealed class Customer : IHasId<Guid>
    {
        /// <summary>
        /// Customer constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        public Customer(Guid id, string name, string email)
        {
            Id = Guard.AgainstEmpty(id);
            Name = Guard.AgainstNullOrWhiteSpace(name);
            Email = Guard.AgainstNullOrWhiteSpace(email);
        }

        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; }
    }
}
