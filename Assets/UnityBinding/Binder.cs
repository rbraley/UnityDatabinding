using System;
using UnityEngine;

public class Binder : MonoBehaviour, IDataContext, IInvoke {
    protected IDataContext DataContext;
    bool isBroadcastingDataContextChange = false;

    void Awake() {
        DataContext = FindDataContext(transform);
    }

    void Start() {
        CreateBindings();
    }

    public virtual IObservable this[string name] {
        get { return DataContext[name]; }
    }

    public void DataContextChanged(IDataContext dataContext) {
        if (isBroadcastingDataContextChange || !gameObject.activeInHierarchy) {
            return;
        }

        DataContext = dataContext;

        CreateBindings();
        BroadcastDataContextChange(dataContext);
    }

    protected virtual void BroadcastDataContextChange(IDataContext dc) {
        if (isBroadcastingDataContextChange) {
            return;
        }

        isBroadcastingDataContextChange = true;
        BroadcastMessage("DataContextChanged", dc, SendMessageOptions.DontRequireReceiver);
        isBroadcastingDataContextChange = false;
    }

    protected virtual void CreateBindings() { }

    IDataContext FindDataContext(Transform start) {
        var currentNode = start;
        
        while(currentNode != null) {
            var dataContext = currentNode.GetComponent(typeof(IDataContext)) as IDataContext;
            if(dataContext != null && dataContext != this) {
                return dataContext;
            }

            currentNode = currentNode.parent;
        }

        return null;
    }

    public virtual void Invoke(string functionName) {
        InvokeCore(DataContext, functionName);
    }

    protected void InvokeCore(IDataContext dc, string functionName) {
        var invoker = dc as IInvoke;
        if(invoker != null) {
            invoker.Invoke(functionName);
        }

        var behavior = dc as Component;
        if(behavior != null) {
            behavior.SendMessage(functionName, null, SendMessageOptions.DontRequireReceiver);
        }
        else {
            var dcType = dc.GetType();
            var method = dcType.GetMethod(functionName);

            if(method != null) {
                method.Invoke(dc, null);
            }else {
                var parentDC = FindDataContext(transform.parent);
                if(parentDC != null) {
                    InvokeCore(parentDC, functionName);
                }
            }
        }
    }

    public Observable<T> AddBinding<T>(string name, Action<T> setter) {
        if(DataContext == null) {
            DataContext = FindDataContext(transform);

            if (DataContext == null) {
                return null;
            }
        }

        var ob = DataContext[name];
        if(ob == null) {
            throw new Exception(string.Format("No observable named '{0}' could be found on '{1}'.", name, DataContext));
        }

        setter((T)ob.GetValue());

        ob.ValueChanged += () => {
            setter((T)ob.GetValue());
        };

        return (Observable<T>)ob;
    }
}