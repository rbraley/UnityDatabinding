# Unity Databinding

This is a basic databinding framework for Unity3D. The general mechanism can be extended to work with any GUI framework and can even be used to databind aspects of the scene hierarchy itself. This package contains compositional and IoC related binders as well as some example binders for the NGUI library.

## Usage

1. First, you must inherit from BootstrapperBase to create your custom Bootstrapper. You should use this to configure the IoC container any any other framework-related components.
2. Attach your custom Boostrapper to a GameObject, preferrably a dedicated node at the root of the hierarchy.
3. Setup a root DataContext for binding. There are two out-of-the-box options for this. You can create a ViewModel by inheriting from ViewModelBehavior and attaching that directly to a GameObject. Or, you can create a ViewModel by inheriting from ViewModel (does not inherit from anything Unity specific). You then need to register it with the IoC container and use a DataContextFromContainer behavior on a GameObject, configured to point to the container key. As always, you can create your own behavior that implements IDataContext and locates the context in any way you choose.
4. Add Binders to GameObjects. Binders databind properties from your DataContext to aspects of the GameObject (and it's components) to which they are attached. You can compose in entire sub-hierarchies using the CompositionBinder, data bind in child data contexts with the DataContextBinder or synchronize GUI control state using GUI-system-specific binders. Two example NGUI binders are provided as examples.
5. When building your ViewModels for use as DataContexts, you must declare your bindable properties as Observable<T>. There is a helper method for this on the ViewModel and ViewModelBehavior base classes. This is done to eliminate the need for reflection and simplify the databinding system.

## Bonus Features
This framework contains optional components derived from the Caliburn.Micro Xaml framework. Those components include a simple IoC container and an EventAggregator. The DataContextFromContainer requires the IoC container. But, if you don't use that behavior, the IoC is not needed at all by this framework. Furthermore, if you aren't using the IoC of the EventAggregator, you technically don't actually need a Bootstrapper either.

## Sample Scene
A sample scene is included in this repo. It demonstrates usage of composed ViewModels, IoC and the EventAggregator. It also demonstrates some NGUI bindings, container bindings and compositional binding used for basic screen navigation. The compositional binding simply activates/deactivates various parts of the scene hierarchy based on the active ViewModel. This could easily be extended to do things like:

1. Autmatically load views from the resources folder.
2. Play animations when composing in/out parts of the hierarchy.
3. Dynamically construct game objects.
4. Download asset bundles autmatcially and extract custom views.
5. Switch to different views based on screen size, orientation or skin.

## Note 
This system has not yet been tested on mobile devices. It has not been used in a production game. It is the product of an afternoon or two of playing around with NGUI by a novice Unity3D developer (albeit an experience front-end developer). This code is primarily provided as an example of how a databinding system might be built with the intention of getting feedback from the community on whether this is valuable enough to pursuse more fully and bring to full prodution-ready quality.