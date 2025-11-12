using System;
using System.Text.RegularExpressions;

namespace Semogly.Core.Domain.AccountContext.Validations;

public static partial class Regexes
{
    // Pattern: pelo menos 1 minúscula, 1 maiúscula, 1 dígito, 1 especial, 8–20 chars
    // Ajuste o pattern conforme sua regra.
    [GeneratedRegex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&_\-])[A-Za-z\d@$!%*?&_\-]{8,20}$",
                    RegexOptions.Compiled | RegexOptions.CultureInvariant)]
    public static partial Regex PasswordRegex();
}
