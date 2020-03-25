using BA_Praxis_Library;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [Header("UI Variables")]

    // shows if level is already unlocked or not
    public TMP_Text m_UnlockedText;

    // image for level preview
    public Sprite m_LevelPreviewImage;

    // Button reference
    public Button m_LevelButton;

    // ButtonSprites
    public Sprite m_UnlockedSprite;
    public Sprite m_LockedSprite;

    [Header("Level Variables")]
    // id for identification
    public int m_LevelID;

    // indicator if level is unlocked or not
    public bool m_IsUnlocked;

    // Level Selector
    public GameObject m_LevelSelector;
    private LevelSelector m_levelSelectorInstance;

    // Start is called before the first frame update
    void Start()
    {
        m_levelSelectorInstance = m_LevelSelector.GetComponent<LevelSelector>();

        if(PlayerData.LastSelectedLevel == m_LevelID)
        {
            m_levelSelectorInstance.m_LevelPreviewImage.sprite = m_LevelPreviewImage;
        }
    }

    // shows if player can use button or not
    public void ShowLevelButtonState()
    {
        // shows the current state of the button
        if (m_IsUnlocked)
        {
            m_UnlockedText.text = "Unlocked";
            m_LevelButton.image.sprite = m_UnlockedSprite;
            m_LevelButton.interactable = true;
        }
        else
        {
            m_UnlockedText.text = "Locked";
            m_LevelButton.image.sprite = m_LockedSprite;
            m_LevelButton.interactable = false;
        }
    }

    // sets the last selected level id to the level bound to this button
    public void Level_Button()
    {
        m_levelSelectorInstance.m_LastSelectedLevel = m_LevelID;
        m_levelSelectorInstance.m_LevelPreviewImage.sprite = m_LevelPreviewImage;
    }

    // returns true if level is already unlocked
    // false otherwhise
    public void GetLevelStatus()
    {
        ServerResponse response = UserRequestLibrary.GetLevelStatusForUser(m_LevelID);

        if(response.LevelStatus.UnlockStatus == 1)
        {
            m_IsUnlocked = true;
            return;
        }

        m_IsUnlocked = false;
    }
}