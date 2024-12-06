using Domain.Recipes;

namespace Domain.Ingradients
{
    public class Ingredient
    {
        public IngredientId Id { get; set; }  // Ідентифікатор інгредієнта
        public string Name { get; set; }  // Назва інгредієнта
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
       

        // Конструктор, який вимагає параметри
        public Ingredient(Guid newGuid, string name)
        {
            Id = IngredientId.New();  // Генерація нового ID для інгредієнта
            Name = name;
        }

        private Ingredient(IngredientId id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Ingredient New(string name)
        {
            return new Ingredient(IngredientId.New(), name);
        }

        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}