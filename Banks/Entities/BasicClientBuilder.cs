namespace Banks.Entities
{
    public class BasicClientBuilder : IClientBuilder
    {
        private Client _client = new Client();

        public BasicClientBuilder()
        {
            Reset();
        }

        public void Reset()
        {
            _client = new Client();
        }

        public void SetFirstName(string firstName)
        {
            _client.SetFirstName(firstName);
        }

        public void SetLastName(string lastName)
        {
            _client.SetLastName(lastName);
        }

        public void SetAddress(string address)
        {
            _client.SetAddress(address);
        }

        public void SetPassportNumber(string passportNumber)
        {
            _client.SetPassportNumber(passportNumber);
        }

        public void SetPhoneNumber(string phoneNumber)
        {
            _client.SetPhoneNumber(phoneNumber);
        }

        public Client GetProduct()
        {
            return _client;
        }
    }
}