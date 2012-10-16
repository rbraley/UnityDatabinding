public interface IDataContext {
    IObservable this[string name] { get; }
}