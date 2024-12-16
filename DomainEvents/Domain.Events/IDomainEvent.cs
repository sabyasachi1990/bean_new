
namespace Domain.Events
{
    /// <summary>
    /// Marker interface to indicate that a class is an event raised by a Domain
    /// - Can be subscribed and unsubscribed to    
    /// - Must be immutable
    /// - Processed synchronously
    /// - Must be Represented by verbs in the past tense
    /// - Should capture the state of something interesting which affects the Domain
    /// - Can be processed across disconnected Aggregate roots
    /// - Ensures root consistency across the entire Domain
    /// </summary>
    public interface IDomainEvent : IMessage { }
}
