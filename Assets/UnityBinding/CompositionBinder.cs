using UnityEngine;

public class CompositionBinder : Binder {
    public string property;
    IDataContext composedDataContext;

    protected override void CreateBindings() {
        if(!string.IsNullOrEmpty(property)) {
            AddBinding<IDataContext>(property, value => Compose(value));
        }
    }

    public override IObservable this[string name] {
        get { return composedDataContext[name]; }
    }

    public override void Invoke(string functionName) {
        InvokeCore(composedDataContext, functionName);
    }

    void Compose(IDataContext value) {
        foreach(var child in transform) {
            var go = ((Transform)child).gameObject;
            go.SetActive(false);
        }

        composedDataContext = value;

        if(value != null) {
            var name = value.GetType().Name;

            foreach(var child in transform) {
                var go = ((Transform)child).gameObject;

                if(go.name == name) {
                    go.SetActive(true);
                    break;
                }
            }
        }

        BroadcastDataContextChange(composedDataContext);
    }
}