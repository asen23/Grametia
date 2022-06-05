namespace Domain.Event;

public class CartItemModifiedEvent : BaseEvent
{
    public CartItemModifiedEvent(int changedAmount, long bookId)
    {
        ChangedAmount = changedAmount;
        BookId = bookId;
    }

    public int ChangedAmount { get; }
    public long BookId { get;  }
}