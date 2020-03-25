using UnityEngine;
using UnityEngine.UI;

public class ChooseCharacter : MonoBehaviour
{
    [Header("UI Variables")]
    // icon of the character bound to this slot
    public Image m_CharacterIcon;

    public Sprite m_CharacterSprite;
    public Sprite m_EmptyIcon;
    public Button m_ChooseCharacterButton;

    [Header("Character Variables")]
    // character information about the character bound to this slot
    public Character m_character;
    
    private void Awake()
    {
        ShowCharacterState();
    }

    // updates the UI of the choose character slot |
    // if character is not unlocked -> disable button
    public void ShowCharacterState()
    {
        // Get the current character stats from the server
        m_character.GetStats();

        // if character has not been unlocked, show empty icon
        if (m_character.m_UnlockStatus == 0)
        {
            m_CharacterIcon.sprite = m_EmptyIcon;
            m_ChooseCharacterButton.gameObject.SetActive(false);
        }
        // if character has been unlocked show the char icon
        else
        {
            m_CharacterIcon.sprite = m_CharacterSprite;
            m_ChooseCharacterButton.gameObject.SetActive(true);
        }
    }

    // on click sets the last chosen character to the character bound to this slot
    public void Button_ChooseCharacter()
    {
        UIController.get.m_lastChosenCharacter = m_character;
        UIController.get.m_chosenCharacterImage.sprite = m_CharacterSprite;
        UIController.get.m_chooseCharacter = true;
    }
}