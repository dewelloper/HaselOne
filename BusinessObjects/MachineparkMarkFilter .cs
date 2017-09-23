using Newtonsoft.Json;

namespace BusinessObjects
{
    public class MachineparkMarkFilter : Filter
    {
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public bool? IsOwnerMachine { get; set; }
    }
}