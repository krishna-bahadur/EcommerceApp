namespace EcommerceApp.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Cart? Cart { get; set; }
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public string? ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;

        private CartItem() { }

        public CartItem(Guid cartId, Guid productId, int quantity, decimal unitPrice, string productName)
        {
            Id = Guid.NewGuid();
            CartId = cartId;
            ProductId = productId;
            Quantity = quantity;
            UnitPrice =  unitPrice;
            ProductName = productName;
        }

        public void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
        }


    }
}
