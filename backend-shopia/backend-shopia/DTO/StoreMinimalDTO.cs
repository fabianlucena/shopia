namespace backend_shopia.DTO
{
    public class StoreMinimalDTO
    {
        public Guid Uuid { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }
    }
}
