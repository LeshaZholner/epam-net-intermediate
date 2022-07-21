using System.Diagnostics.CodeAnalysis;

namespace Catalog.Services.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string message) : base(message) { }

    public static void ThrowIfNull([NotNull] object? entity, string message)
    {
        if (entity is null)
            throw new EntityNotFoundException(message);
    }
}
