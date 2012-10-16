using System.Collections.Generic;
using UnityEngine;

public class ViewModelBehavior : MonoBehaviour, IDataContext {
    readonly Dictionary<string, IObservable> observables = new Dictionary<string, IObservable>();

    IObservable IDataContext.this[string name] {
        get { 
            IObservable observable;
            observables.TryGetValue(name, out observable);
            return observable;
        }
    }

    protected Observable<T> Observable<T>(string name, T initialValue = default(T)) {
        var ob = new Observable<T>(initialValue);
        observables[name] = ob;
        return ob;
    }
}