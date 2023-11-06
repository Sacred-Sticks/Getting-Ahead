public interface IObserver<T> : IObserver
{
    public void OnNotify(T argument);
}

public interface IObserver
{
    
}