namespace Domain.Event;

public class CartItemModifiedEvent : BaseEvent
{
    public CartItemModifiedEvent(CartItem cartItem)
    {
        CartItem = cartItem;
    }

    public CartItem CartItem { get; }
}