namespace BackupsExtra.Entities
{
    public interface IAdvancedLimit : ILimit
    {
        void AddBasicLimit(IBasicLimit basicLimit);
    }
}