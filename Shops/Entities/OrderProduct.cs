namespace Shops.Entities
{
    public class OrderProduct
    {
        public OrderProduct(string name, uint quantity)
        {
            Name = name;
            Quantity = quantity;
        }

        public string Name { get; }
        public uint Quantity { get; set; }
    }
}