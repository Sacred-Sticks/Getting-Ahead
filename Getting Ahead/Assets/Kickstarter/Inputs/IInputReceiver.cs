using Kickstarter.Identification;

public interface IInputReceiver
{
    public void SubscribeToInputs(Player player);

    public void UnsubscribeToInputs(Player player);
}
