using UnityEngine;

public abstract class BootstrapperBase : MonoBehaviour {
    protected readonly SimpleContainer Container;

    protected BootstrapperBase() {
        Container = new SimpleContainer();

        IoC.GetInstance = Container.GetInstance;
        IoC.GetAllInstances = Container.GetAllInstances;
        IoC.BuildUp = Container.BuildUp;

        Container.Singleton<IEventAggregator, EventAggregator>();
    }
}