using UnityEngine;

public class PauseControl : MonoBehaviour
{
    private bool m_IsGamePaused;
    
    public bool IsGamePaused
    {
        get => m_IsGamePaused;
        set => m_IsGamePaused = value;
    }
}
