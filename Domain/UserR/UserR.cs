using Domain.Recipes;

namespace Domain.Users;

public class UserR
{
    public UserRId Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }

    public ICollection<FavoriteRecipes> FavoriteRecipes { get; } = new List<FavoriteRecipes>();

    private UserR(
        UserRId id,
        string firstName,
        string lastName,
        DateTime updatedAt)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        FullName = $"{firstName} {lastName}";
        UpdatedAt = updatedAt;
    }

    public static UserR New(UserRId id, string firstName, string lastName)
        => new(id, firstName, lastName, DateTime.UtcNow);

    public void UpdateDetails(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        FullName = $"{firstName} {lastName}";
        UpdatedAt = DateTime.UtcNow;
    }
}