# Unity Databinding

This is a basic databinding framework for Unity3D. The general mechanism can be extended to work with any GUI framework and can even be used to databind aspects of the scene hierarchy itself. This packaged contains compositional and IoC related binders as well as some example binders for the NGUI library.

## How to use it

1. First, you must inherit from BootstrapperBase to create your custom Bootstrapper. You should use this to configure the IoC container any any other framework-related components.
2. Attach your custom Boostrapper to a GameObject, preferrably a dedicated node at the root of the hierarchy.
3. Setup a root DataContext for binding. There are two options for this. You can create a ViewModel by inheriting from ViewModelBehavior and attaching that directly to a GameObject. Or, you can create a ViewModel by inheriting from ViewModel (does not inherit from anything Unity specific), then you can register it with the IoC container and use a DataContextFromContainer behavior on a GameObject, configured to point to the container key. As always, you can create your own behavior that implements IDataContext and locates the context in any way you choose.
4. Add Binders to GameObjects. Binders databind property from your DataContext to aspects of the GameObject (and it's components) to which they are attached. You can compose in entire sub-hierarchies using the CompositionBinder, data bind in child data contexts with the DataContextBinder or synchronize GUI control state using GUI-system-specific binders. Two example NGUI binders are provided as examples.
5. When building your ViewModels for use as DataContexts, you must declare your bindable properties as Observable<T>. There is a helper method for this on the ViewModel and ViewModelBehavior base classes.

## Bonus Features
This framework contains optional components derived from the Caliburn.Micro Xaml framework. Those components include a simple IoC container and an EventAggregator.

## Note 
This system has not yet been tested on mobile devices yet. 