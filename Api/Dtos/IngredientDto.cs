using Domain.Ingradients;

namespace Api.Dtos
{
    public class IngredientResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        // Метод для конвертації з доменної моделі в DTO
        public static IngredientResponseDto FromDomainModel(Domain.Ingradients.Ingredient ingredient)
        {
            return new IngredientResponseDto
            {
                Id = ingredient.Id.Value, // Отримуємо Guid з IngredientId
                Name = ingredient.Name,
            };
        }
    }

    public class IngredientRequestDto
    {
        
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }

        // Конструктор для зручного створення об'єкта
        public IngredientRequestDto(string name, decimal quantity, string unit)
        {
            Name = name;
            Quantity = quantity;
            Unit = unit;
        }

        public static IngredientResponseDto FromDomainModel(Domain.Ingradients.Ingredient ingredient)
        {
            return new IngredientResponseDto
            {
                Id = ingredient.Id.Value, 
                Name = ingredient.Name,
            };
        }

    }
}