using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIControllerBattle : Singleton<UIControllerBattle>
{
    [Header("Character Sprites")]
    public Sprite[] m_CharacterSprites;
    public Dictionary<string, Sprite> m_CharacterSpritesDict = new Dictionary<string, Sprite>();

    [Header("Team UI Elements")]
    public GameObject m_TeamInformation;
    public Image m_TeamIcon;
    public TMP_Text m_TeamHPText;
    public TMP_Text m_TeamTypeText;
    public TMP_Text m_TeamAttackText;
    public TMP_Text m_TeamDefenseText;
    public TMP_Text m_TeamCharacterNameText;

    [Header("Enemy UI Elements")]
    public GameObject m_EnemyInformation;
    public Image m_EnemyIcon;
    public TMP_Text m_EnemyHPText;
    public TMP_Text m_EnemyTypeText;
    public TMP_Text m_EnemyAttackText;
    public TMP_Text m_EnemyDefenseText;
    public TMP_Text m_EnemyCharacterNameText;

    [Header("Normal UI Elements")]
    public Image m_EndingScreen;
    public TMP_Text m_EndingText;
    public Image m_UnlockedCharacterIcon;
    public Image m_CancelMenu;
    public Button m_AttackButton;
    public Button m_HelperMenuButton;
    public GameObject m_HelperMenu;

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
        m_CharacterSpritesDict.Add("NotUnlocked", m_CharacterSprites[11]);

        UpdateTeamInformation(BattleController.get.m_LastSelectedTeamMember);
        UpdateEnemyInformation(BattleController.get.m_LastSelectedEnemy);
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerClick();

        // if player is battling, update UI
        if (BattleController.get.RoundStarted)
        {
            UpdateTeamInformation(BattleController.get.m_LastSelectedTeamMember);
            UpdateEnemyInformation(BattleController.get.m_LastSelectedEnemy);
        }
    }

    void CheckPlayerClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !BattleController.get.RoundStarted)
        {
            // gets current mouse position
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // checks if the cell is interactable
            Collider2D collider = Physics2D.OverlapPoint(new Vector2(mouseWorldPos.x, mouseWorldPos.y));

            // if nothing was hit return
            if (collider == null)
            {
                return;
            }
            // if a character was hit
            else if (collider.tag == "Character")
            {
                // get reference
                Character charTmp = collider.gameObject.GetComponent<Character>();

                // check if player or enemy and then update the UI
                if (charTmp.m_TeamStatus == "Player")
                {
                    UpdateTeamInformation(charTmp);
                }
                else if (charTmp.m_TeamStatus == "Enemy")
                {
                    UpdateEnemyInformation(charTmp);
                }
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            bool tmp = m_CancelMenu.gameObject.activeInHierarchy;

            m_CancelMenu.gameObject.SetActive(!tmp);
        }
    }

    void UpdateTeamInformation(Character _char)
    {
        // set the correct sprite
        m_TeamIcon.sprite = m_CharacterSpritesDict[_char.m_CharacterName];

        // if it's not the adventurer, sprite needs to be flipped
        if (_char.m_CharacterName != "Adventurer")
        {
            m_TeamIcon.rectTransform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
        else
        {
            m_TeamIcon.rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

        // if player character is dead, don't show negative hp
        if (_char.m_Health < 0)
        {
            m_TeamHPText.text = "HP: 0";
        }
        else
        {
            m_TeamHPText.text = "HP: " +_char.m_Health.ToString();
        }

        m_TeamTypeText.text = "Type: " + _char.m_Type.ToString();
        m_TeamAttackText.text = "Attack: " + _char.m_AttackDamage.ToString();
        m_TeamDefenseText.text = "Defense: " + _char.m_Defense.ToString();
        m_TeamCharacterNameText.text = _char.m_CharacterName;

        BattleController.get.m_LastSelectedTeamMember = _char;
    }

    void UpdateEnemyInformation(Character _char)
    {
        m_EnemyIcon.sprite = m_CharacterSpritesDict[_char.m_CharacterName];

        // if player character is dead, don't show negative hp
        if (_char.m_Health < 0)
        {
            m_EnemyHPText.text = "HP: 0";
        }
        else
        {
            m_EnemyHPText.text = "HP: " + _char.m_Health.ToString();
        }

        m_EnemyTypeText.text = "Type: " + _char.m_Type.ToString();
        m_EnemyAttackText.text = "Attack: " + _char.m_AttackDamage.ToString();
        m_EnemyDefenseText.text = "Defense: " + _char.m_Defense.ToString();
        m_EnemyCharacterNameText.text = _char.m_CharacterName;

        BattleController.get.m_LastSelectedEnemy = _char;
    }
}
