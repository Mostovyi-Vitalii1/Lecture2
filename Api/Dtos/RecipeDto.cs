namespace Api.Dtos;

public class RecipeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Instructions { get; set; }
    public TimeSpan PreparationTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<IngredientRequestDto> Ingredients { get; set; } = new();
}
