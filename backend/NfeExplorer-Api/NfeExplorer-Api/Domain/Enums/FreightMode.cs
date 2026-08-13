namespace NfeExplorer_Api.Domain.Enums;

public enum FreightMode
{
    Sender = 0,
    Recipient = 1,
    ThirdParty = 2,
    SenderOwnTransport = 3,
    RecipientOwnTransport = 4,
    NoFreight = 9
}
