using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIController : Singleton<UIController>
{
    [Header("UI Variables")]
    // Ingame UI
    public Image m_CastleBanner;
    public Image m_MountainBanner;

    // TeamBuildingMenu UI
    public GameObject m_TeamBuildingMenu;
    private bool m_teamBuildingMenuActiveStatus = false;
    private TeamBuildingMenu m_teamBuildingMenuInstance;

    // last chosen character information
    [HideInInspector]
    public bool m_chooseCharacter = false;
    public Image m_chosenCharacterImage;
    public Character m_lastChosenCharacter;

    // LevelSelector UI
    public GameObject m_LevelSelector;
    private bool m_levelSelectorActiveStatus = false;
    
    [Header("Character References")]
    // all character sprites
    public Sprite[] m_CharacterSprites;
    private Dictionary<string, Sprite> m_CharacterSpritesDict = new Dictionary<string, Sprite>();

    // Grid of the world
    public Grid m_Grid;

    // Start is called before the first frame update
    void Start()
    {
        // Add all sprite references to the dictionary
        m_CharacterSpritesDict.Add("Adventurer", m_CharacterSprites[0]);
        m_CharacterSpritesDict.Add("Bandit", m_CharacterSprites[1]);
        m_CharacterSpritesDict.Add("Golem", m_CharacterSprites[2]);
        m_CharacterSpritesDict.Add("Mandrake", m_CharacterSprites[3]);
        m_CharacterSpritesDict.Add("Rat", m_CharacterSprites[4]);
        m_CharacterSpritesDict.Add("Red Ogre", m_CharacterSprites[5]);
        m_CharacterSpritesDict.Add("Satyr", m_CharacterSprites[6]);
        m_CharacterSpritesDict.Add("Shade", m_CharacterSprites[7]);
        m_CharacterSpritesDict.Add("Wasp", m_CharacterSprites[8]);
        m_CharacterSpritesDict.Add("Werewolf", m_CharacterSprites[9]);
        m_CharacterSpritesDict.Add("Yeti", m_CharacterSprites[10]);
        m_CharacterSpritesDict.Add("", m_CharacterSprites[11]);
        
        m_teamBuildingMenuInstance = m_TeamBuildingMenu.GetComponent<TeamBuildingMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        // Checks every frame what the player is currently hovering
        CheckForMenuHover();

        // check if player has chosen a character in the team building menu
        CharacterChosen();
    }

    // checks if player has chosen a character in the team building menu
    // if yes, the icon follow the mouse
    public void CharacterChosen()
    {
        if (m_chooseCharacter)
        {
            m_chosenCharacterImage.gameObject.SetActive(true);
            m_chosenCharacterImage.rectTransform.position = Input.mousePosition /* + (new Vector3(50.0f, -50.0f, 0.0f))*/;
        }
        else if (m_chooseCharacter == false)
        {
            m_chosenCharacterImage.gameObject.SetActive(false);
        }
    }

    // returns the last chosen character, if one was chosen |
    // returns null else
    public Character ChangeCharacter()
    {
        if (m_chooseCharacter)
        {
            foreach (var member in m_teamBuildingMenuInstance.m_CurrentTeamSlots)
            {
                if (member.m_CurrentTeamSlot.m_CharacterName == m_lastChosenCharacter.m_CharacterName)
                    return null;
            }

            return m_lastChosenCharacter;
        }

        return null;
    }

    // clears the all variables vor choosing a character
    public void ClearCharacterChosen()
    {
        m_chooseCharacter = false;
        m_lastChosenCharacter = null;
        m_chosenCharacterImage.sprite = null;
    }
    
    // changes the current state of the Team Building Menu | on <=> off
    public void ToggleTeambuildingMenu()
    {
        m_teamBuildingMenuActiveStatus = !(m_TeamBuildingMenu.gameObject.activeInHierarchy);

        m_TeamBuildingMenu.gameObject.SetActive(m_teamBuildingMenuActiveStatus);
    }

    // changes the current state of the Level Selector | on <=> off
    public void ToggleLevelSelector()
    {
        m_levelSelectorActiveStatus = !(m_LevelSelector.activeInHierarchy);

        m_LevelSelector.SetActive(m_levelSelectorActiveStatus);
    }

    // deactivates the castle banner in the scene
    public void DisableMountainBanner()
    {
        m_MountainBanner.gameObject.SetActive(false);
    }

    // activates the castle banner in the scene
    public void EnableMountainBanner()
    {
        m_MountainBanner.gameObject.SetActive(true);
        m_MountainBanner.rectTransform.position = Input.mousePosition + (new Vector3(0f, 75f, 0f));
    }

    // deactivates the castle banner in the scene
    public void DisableCastleBanner()
    {
        m_CastleBanner.gameObject.SetActive(false);
    }

    // activates the castle banner in the scene
    public void EnableCastleBanner()
    {
        m_CastleBanner.gameObject.SetActive(true);
        m_CastleBanner.rectTransform.position = Input.mousePosition + (new Vector3(0f, 75f, 0f));
    }

    // handles player hovering when no menu is opened
    public void CheckForMenuHover()
    {
        // If Team Building Menu or Level Selector is already active, dont check for mouse hover
        if (!(m_teamBuildingMenuActiveStatus) && !(m_levelSelectorActiveStatus))
        {
            // gets current mouse position
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // gets the cell that the mouse cursor currently hovers
            Vector3Int coordinate = m_Grid.WorldToCell(mouseWorldPos);

            // checks if the cell is interactable
            Collider2D collider = Physics2D.OverlapPoint(new Vector2(coordinate.x, coordinate.y));

            // if is not interactable
            if (collider == null)
            {
                DisableCastleBanner();
                DisableMountainBanner();
                return;
            }
            // if it is
            else
            {
                if (collider.tag == "Mountain")
                {
                    EnableMountainBanner();
                }
                else if (collider.tag == "Castle")
                {
                    EnableCastleBanner();
                }
            }
        }
    }

    public Sprite GetCorrectSprite(string _charName)
    {
        return m_CharacterSpritesDict[_charName];
    }
}