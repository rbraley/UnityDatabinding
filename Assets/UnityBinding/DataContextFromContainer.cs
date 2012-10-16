using UnityEngine;

public class DataContextFromContainer : MonoBehaviour, IDataContext, IInvoke {
    IDataContext dataContext;

    public string Key;

    public IObservable this[string name] {
        get {
            EnsureDataContext();
            return dataContext[name];
        }
    }

    public void Invoke(string name) {
        EnsureDataContext();
        dataContext.GetType().GetMethod(name).Invoke(dataContext, null);
    }

    void EnsureDataContext() {
        if(dataContext != null) {
            return;
        }

        dataContext = (IDataContext)IoC.GetInstance(null, Key);
    }
}