using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [Header("UI Variables")]

     // [HideInInspector]
    public int m_LastSelectedLevel;

    public Image m_LevelPreviewImage;

    public LevelButton[] m_LevelButtons;

    private void Awake()
    {
        UpdateLevelSelectorWindow();
    }

    private void OnEnable()
    {
        UpdateLevelSelectorWindow();
    }

    // updates the states of all buttons
    private void UpdateLevelSelectorWindow()
    {
        foreach (var levelButton in m_LevelButtons)
        {
            levelButton.GetLevelStatus();
            levelButton.ShowLevelButtonState();
        }

        m_LastSelectedLevel = PlayerData.LastSelectedLevel;
    }

    // set the chosen level and load into battle scene
    public void StartBattle()
    {
        PlayerData.LastSelectedLevel = m_LastSelectedLevel;

        SingletonReset.Reset();

        SceneManager.LoadScene(3);
    }
}