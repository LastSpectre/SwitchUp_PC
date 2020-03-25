using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCharacter : MonoBehaviour
{
    [Header("UI Variables")]
    // icon of character
    public Image m_Icon;

    // UI stats of character
    public TMP_Text m_CharNameText;
    public TMP_Text m_TypeText;
    public TMP_Text m_AttackText;
    public TMP_Text m_Defense;
    public TMP_Text m_HP;
    public TMP_Text m_Level;

    [Header("Character Variables")]
    // id of this character slot
    public int m_CharacterSlotID;

    // contains stat of character currently chosen
    public Character m_CurrentTeamSlot;

    // Start is called before the first frame update
    void Start()
    {
        // if player already chose a team, load the character from into the slot
        if (PlayerData.CharacterInSlots.Count >= m_CharacterSlotID && PlayerData.CharacterInSlots[m_CharacterSlotID - 1] != null)
        {
            LoadCharacterSlotOnEnable(PlayerData.CharacterInSlots[m_CharacterSlotID - 1]);
            m_Icon.sprite = UIController.get.GetCorrectSprite(m_CurrentTeamSlot.m_CharacterName);
        }
    
        ShowTeamSlotState();
    }

    // update the stats of the current character in slot and update the UI
    public void ShowTeamSlotState()
    {
        m_CurrentTeamSlot.GetStats();

        ChangeStatTexts();
    }

    // handle the character slot change
    public void Button_ChangeCharacter()
    {
        // adventurer can't be taken out
        // main character always has to be in the team
        if (m_CurrentTeamSlot.m_CharacterName == "Adventurer")
            return;

        // tries to get a new character 
        Character charTmp = UIController.get.ChangeCharacter();

        // only try to change char if player chose one
        if (charTmp != null)
        {
            // set character to last chosen char and updates UI
            m_CurrentTeamSlot = charTmp;
            m_Icon.sprite = UIController.get.GetCorrectSprite(m_CurrentTeamSlot.m_CharacterName);

            ChangeStatTexts();

            UIController.get.ClearCharacterChosen();
        }
    }

    // changes the UI Text
    private void ChangeStatTexts()
    {
        m_CharNameText.text = m_CurrentTeamSlot.m_CharacterName;
        m_TypeText.text = "Type: " + m_CurrentTeamSlot.m_Type;
        m_AttackText.text = "Attack: " + m_CurrentTeamSlot.m_AttackDamage.ToString();
        m_Defense.text = "Defense: " + m_CurrentTeamSlot.m_Defense.ToString();
        m_HP.text = "HP: " + m_CurrentTeamSlot.m_Health.ToString();
        m_Level.text = "Level: " + m_CurrentTeamSlot.m_Level.ToString();
    }

    // gets the correct character on scene reload
    public void LoadCharacterSlotOnEnable(string _charName)
    {
        switch (_charName)
        {
            case "Adventurer":
                m_CurrentTeamSlot = gameObject.AddComponent<Adventurer>();
                break;
            case "Bandit":
                m_CurrentTeamSlot = gameObject.AddComponent<Bandit>();
                break;
            case "Golem":
                m_CurrentTeamSlot = gameObject.AddComponent<Golem>();
                break;
            case "Mandrake":
                m_CurrentTeamSlot = gameObject.AddComponent<Mandrake>();
                break;
            case "Rat":
                m_CurrentTeamSlot = gameObject.AddComponent<Rat>();
                break;
            case "Red Ogre":
                m_CurrentTeamSlot = gameObject.AddComponent<RedOgre>();
                break;
            case "Satyr":
                m_CurrentTeamSlot = gameObject.AddComponent<Satyr>();
                break;
            case "Shade":
                m_CurrentTeamSlot = gameObject.AddComponent<Shade>();
                break;
            case "Wasp":
                m_CurrentTeamSlot = gameObject.AddComponent<Wasp>();
                break;
            case "Werewolf":
                m_CurrentTeamSlot = gameObject.AddComponent<Werewolf>();
                break;
            case "Yeti":
                m_CurrentTeamSlot = gameObject.AddComponent<Yeti>();
                break;
            default:
                m_CurrentTeamSlot = gameObject.AddComponent<EmptyCharacter>();
                break;
        }
    }
}
