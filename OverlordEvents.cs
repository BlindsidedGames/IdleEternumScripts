using System;

public static class OverlordEvents
{
    public static event Action UpdateTimescale;

    public static void OnUpdateTimescale()
    {
        UpdateTimescale?.Invoke();
    }
}