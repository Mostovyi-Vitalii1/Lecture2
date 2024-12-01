namespace Domain.Users
{
    public record UserRId(Guid Value)
    {
        public static UserRId New() => new(Guid.NewGuid());
        public static UserRId Empty() => new(Guid.Empty);

        public override string ToString() => Value.ToString();

        // Оператор явного перетворення в Guid?
        public static explicit operator Guid?(UserRId userRId) => userRId.Value == Guid.Empty ? null : userRId.Value;
        public static implicit operator Guid(UserRId id) => id.Value;
        // Оператор неявного перетворення з Guid? в UserRId
        public static implicit operator UserRId(Guid? value) => new(value ?? Guid.Empty);
    }
}