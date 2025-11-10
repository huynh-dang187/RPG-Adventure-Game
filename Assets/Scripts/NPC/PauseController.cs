using UnityEngine;

public static class PauseController
{
    public static bool IsGamePaused { get; private set; }

    public static void SetPause(bool pause)
    {
        IsGamePaused = pause;

        if (pause)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }
}
