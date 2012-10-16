public class SettingsMenu : ViewModel
{
    readonly IEventAggregator eventAggregator;

    public Observable<string> DisplayName;
    public Observable<float> Volume;

    public SettingsMenu(IEventAggregator eventAggregator) {
        this.eventAggregator = eventAggregator;

        DisplayName = Observable("DisplayName", "Settings");
        Volume = Observable("Volume", .25f);
    }

    public void Back() {
        eventAggregator.Publish(new Navigate<MainMenu>());
    }
}