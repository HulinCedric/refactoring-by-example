using System.Collections.Generic;
using System.Linq;

namespace TellDontAskKata.Main.Domain;

public record UnknownProducts(string Message)
{
    public UnknownProducts(IEnumerable<UnknownProduct> products) : this(
        "Unknown product(s): " + string.Join(", ", products.Select(m => m.Name)))
    {
    }
}