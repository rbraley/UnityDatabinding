public class SliderBinder : Binder {
    UISlider slider;

    public string value;
    Observable<float> valueObserver;

    void Awake() {
        slider = GetComponent<UISlider>();
    }

    protected override void CreateBindings() {
        if(!string.IsNullOrEmpty(value)) {
            valueObserver = AddBinding<float>(value, v => slider.sliderValue = v);
            slider.eventReceiver = gameObject;
            slider.functionName = "OnValueChanged";
        }
    }

    void OnValueChanged() { 
        valueObserver.Value = slider.sliderValue;
    }
}