namespace People.Api.Domain;

// https://github.com/zoran-horvat/optional/blob/main/Source/Optional/Option.cs
// Zoran is king
public struct Option<T> where T : class
{
    private T? _content;

    public static Option<T> Some(T obj) => new() { _content = obj };
    public static Option<T> None() => new();

    public Option<TResult> Map<TResult>(Func<T, TResult> map) where TResult : class 
        => _content is null
            ? Option<TResult>.None()
            : Option<TResult>.Some(map(_content));

    public T Reduce(Func<T> orElse) => _content ?? orElse();
    public bool IsSome() => _content is not null;
    public bool IsNone() => !IsSome();
}