using System;

public class Observable<T> : IObservable {
    T value;
    bool isSetting = false;

    public Observable(T initialValue) {
        value = initialValue;
    }

    public T Value {
        get { return value; }
        set {
            if(isSetting) {
                return;
            }

            isSetting = true;
            this.value = value;
            ValueChanged();
            isSetting = false;
        }
    }

    public static implicit operator T(Observable<T> observable) {
        return observable.Value;
    }

    public object GetValue() {
        return Value;
    }

    public void SetValue(object value) {
        Value = (T)value;
    }

    public event Action ValueChanged = delegate { };
}