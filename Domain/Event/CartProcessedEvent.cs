namespace Domain.Event;

public class CartProcessedEvent : BaseEvent
{
    public CartProcessedEvent(Cart cart, string paymentMethod, string courier)
    {
        Cart = cart;
        PaymentMethod = paymentMethod;
        Courier = courier;
    }

    public Cart Cart { get; }
    public string PaymentMethod { get; }
    public string Courier { get; }
}