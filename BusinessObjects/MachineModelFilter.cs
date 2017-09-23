namespace BusinessObjects
{
    public class MachineModelFilter : Filter
    {
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public int? MarkId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool? RequestVisible { get; set; }
    }
}