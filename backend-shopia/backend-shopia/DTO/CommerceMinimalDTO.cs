namespace backend_shopia.DTO
{
    public class CommerceMinimalDTO
    {
        public Guid Uuid { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }
    }
}
