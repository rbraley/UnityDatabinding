using System;

public class Navigate {
    public Type ScreenType { get; set; }
}

public class Navigate<T> : Navigate where T : IDataContext {
    public Navigate() {
        ScreenType = typeof(T);
    }
}