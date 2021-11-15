namespace Banks.Entities
{
    public interface IClientBuilder
    {
        void SetFirstName(string firstName);
        void SetLastName(string lastName);
        void SetAddress(string address);
        void SetPassportNumber(string passportNumber);
        void SetPhoneNumber(string phoneNumber);
    }
}