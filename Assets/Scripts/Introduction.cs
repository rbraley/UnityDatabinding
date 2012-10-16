public class Introduction : ViewModel {
    readonly IEventAggregator eventAggregator;

    Observable<string> DisplayName;
    Observable<string> Instructions;

    public Introduction(IEventAggregator eventAggregator) {
        this.eventAggregator = eventAggregator;

        DisplayName = Observable("DisplayName", "Introduction");
        Instructions = Observable("Instructions",
            "This sample demonstrates a basic databinding system for Unity3D with extensions for NGUI. Here's how to use it:" +
            "\n1. Inherit from BootstrapperBase and attach your custom Bootstrapper to a GameObject. In this sample, see the Bootstrapper game object in the hierarchy." + 
            "\n2. Use DataContextFromContainer to resolve an initial data context from the IoC container or inherit your ViewModel from ViewModelBehavior. Attach one of these to a GameObject. See the UIRoot in the hierarchy." +
            "\n3. Attach Binders to GameObjects. See labels for NGUI example and ActiveItem for a composition example."
            );
    }

    public void Start() {
        eventAggregator.Publish(new Navigate<MainMenu>());
    }
}