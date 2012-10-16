public class BubblingAction : Binder {
    public enum Trigger {
        OnClick,
        OnMouseOver,
        OnMouseOut,
        OnPress,
        OnRelease,
        OnDoubleClick,
    }

    public string functionName;
    public UIButtonMessage.Trigger trigger = UIButtonMessage.Trigger.OnClick;

    bool mStarted = false;
    bool mHighlighted = false;

    void Start() {
        mStarted = true;
    }

    void OnEnable() {
        if(mStarted && mHighlighted)
            OnHover(UICamera.IsHighlighted(gameObject));
    }

    void OnHover(bool isOver) {
        if(enabled) {
            if(((isOver && trigger == UIButtonMessage.Trigger.OnMouseOver) ||
                (!isOver && trigger == UIButtonMessage.Trigger.OnMouseOut)))
                Send();
            mHighlighted = isOver;
        }
    }

    void OnPress(bool isPressed) {
        if(enabled) {
            if(((isPressed && trigger == UIButtonMessage.Trigger.OnPress) ||
                (!isPressed && trigger == UIButtonMessage.Trigger.OnRelease)))
                Send();
        }
    }

    void OnClick() {
        if(enabled && trigger == UIButtonMessage.Trigger.OnClick)
            Send();
    }

    void OnDoubleClick() {
        if(enabled && trigger == UIButtonMessage.Trigger.OnDoubleClick)
            Send();
    }

    void Send() {
        if(string.IsNullOrEmpty(functionName))
            return;
        
        Invoke(functionName);
    }
}