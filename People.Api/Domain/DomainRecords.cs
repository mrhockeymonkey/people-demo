namespace People.Api.Domain;

public record NonEmptyString(string Value)
{
    public string Value { get; init; } = string.IsNullOrWhiteSpace(Value)
        ? throw new ArgumentException("Cannot be null or whitespace")
        : Value.Trim();
}

// in same file for brevity
public record Username(string Value) : NonEmptyString(Value);
public record FirstName(string Value) : NonEmptyString(Value);
public record LastName(string Value) : NonEmptyString(Value);

public record Person(FirstName FirstName, LastName LastName, Username Username);