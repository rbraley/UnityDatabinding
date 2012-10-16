public class Conductor : ViewModel, IHandle<Navigate> {
    public Observable<IDataContext> ActiveItem;

    public Conductor(IEventAggregator eventAggregator, Introduction introduction) {
        ActiveItem = Observable<IDataContext>("ActiveItem", introduction);
        eventAggregator.Subscribe(this);
    }

    public void Handle(Navigate message) {
        ActiveItem.Value = (IDataContext)IoC.GetInstance(message.ScreenType, null);
    }
}