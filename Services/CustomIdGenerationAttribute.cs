namespace MicroFinance.Services
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CustomIdGenerationAttribute : Attribute
    {
        public bool GenerateCustomId { get; }
        public CustomIdGenerationAttribute(bool generateCustomId)
        {
            GenerateCustomId = generateCustomId;
        }
    }
}
