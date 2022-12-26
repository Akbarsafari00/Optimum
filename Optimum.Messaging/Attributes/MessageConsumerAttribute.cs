namespace Optimum.Messaging.Attributes;

public class MessageConsumerAttribute : Attribute
{
    public MessageConsumerAttribute(string serviceId)
    {
        ServiceId = serviceId;
    }

    public string ServiceId { get; set; }
}