using System;

public interface IObservable {
    object GetValue();
    void SetValue(object value);
    event Action ValueChanged;
}