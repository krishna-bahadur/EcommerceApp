using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcommerceApp.Domain.Entities
{
    public class Cart
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public virtual ICollection<CartItem> Items { get; set; } = new List<CartItem>();

        private Cart() { }
        public Cart(Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
        }

        public void AddItem(Product product, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Qunatity must be greater than zero", nameof(quantity));

            var existingItem = Items.FirstOrDefault(x => x.ProductId == product.Id);

            if(existingItem != null)
            {
                existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            }
            else
            {
                decimal price = product.DiscountPrice.HasValue ? product.DiscountPrice.Value : product.Price;
                Items.Add(new CartItem(Id, product.Id, quantity, price, product.Name!));
            }
        }

        public void RemoveItem(Guid productId)
        {
            var item = Items.FirstOrDefault(x => x.ProductId == productId);
            if(item != null)
            {
                Items.Remove(item);
            }
        }

        public void UpdateItemQuantity(Guid productId, int quantity)
        {
            var item = Items.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                    Items.Remove(item);
                else
                    item.UpdateQuantity(quantity);
            }
        }

        public decimal CalculateTotal()
        {
            return Items.Sum(x => x.LineTotal);
        }

        public void Clear()
        {
            Items.Clear();
        }

    }
}
