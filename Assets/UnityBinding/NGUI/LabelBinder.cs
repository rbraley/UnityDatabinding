public class LabelBinder : Binder {
    UILabel label;

    public string text;

    void Awake() {
        label = GetComponent<UILabel>();
    }

    protected override void CreateBindings() {
        if(!string.IsNullOrEmpty(text)) {
            AddBinding<string>(text, value => label.text = value);
        }
    }
}