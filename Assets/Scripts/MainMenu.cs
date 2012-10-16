using UnityEngine;

public class MainMenu : ViewModel {
    readonly IEventAggregator eventAggregator;

    public Observable<string> DisplayName;

    public MainMenu(IEventAggregator eventAggregator) {
        this.eventAggregator = eventAggregator;

        DisplayName = Observable("DisplayName", "Main Menu");
    }

    public void NewGame() {
        eventAggregator.Publish(new Navigate<SettingsMenu>());
    }

    public void LoadGame() {
        eventAggregator.Publish(new Navigate<SettingsMenu>());
    }

    public void SaveGame() {
        eventAggregator.Publish(new Navigate<SettingsMenu>());
    }

    public void Settings() {
        eventAggregator.Publish(new Navigate<SettingsMenu>());
    }

    public void Quit() {
        Application.Quit();
    }
}