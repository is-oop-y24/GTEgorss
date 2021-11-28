namespace BackupsExtra.Entities
{
    public interface IAdvancedLimit : ILimit
    {
        public void AddBasicLimit(IBasicLimit basicLimit);
    }
}