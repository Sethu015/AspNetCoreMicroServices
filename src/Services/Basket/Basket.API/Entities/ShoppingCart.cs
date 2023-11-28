namespace Basket.API.Entities
{
    public class ShoppingCart
    {
        public string Username { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCart()
        {

        }

        public ShoppingCart(string userName)
        {
            Username = userName;
        }

        public decimal TotalPrice
        {   get
            {
                decimal totalPrice = 0;
                foreach (ShoppingCartItem item in Items)
                {
                    totalPrice += item.Price * item.Quantity;
                }
                return totalPrice;
            } 
        }
    }
}
