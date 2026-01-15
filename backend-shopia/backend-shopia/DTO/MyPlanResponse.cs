namespace backend_shopia.DTO
{
    public class MyPlanResponse
    {
        public required Guid Uuid { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public required decimal Price { get; set; }

        public required Dictionary<string, Int64> Limits { get; set; }

        public required UsedPlanDTO Used { get; set; }
    }
}
