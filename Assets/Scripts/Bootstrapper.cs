public class Bootstrapper : BootstrapperBase {
    Bootstrapper() {
        Container.Singleton<MainMenu>();
        Container.Singleton<SettingsMenu>();
        Container.Singleton<Introduction>();
        Container.RegisterSingleton(typeof(Conductor), "Conductor", typeof(Conductor));
    }
}