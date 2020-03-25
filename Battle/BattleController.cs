using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA_Praxis_Library;

public class BattleController : Singleton<BattleController>
{
    [Header("AudioSource")]
    public AudioSource m_AudioSource;

    // spawnpoints for the battle
    [Header("Spawnpoints")]
    public Transform[] m_TeamSpawnpoints;
    public Transform[] m_EnemySpawnpoints;

    [Header("Characters")]
    // all character types that can be spawned
    public GameObject[] m_CharacterTypes;

    // dictionary references the spawnable character with the name
    Dictionary<string, GameObject> m_CharacterDict = new Dictionary<string, GameObject>();

    // Team Character Components
    [HideInInspector]
    public Character m_LastSelectedTeamMember; // team member the player currently has selected
    [HideInInspector]
    public List<Character> m_Team = new List<Character>(); // all remaining team members

    // Battle Round Components (Player)
    private GameObject m_teamMember; // Gameobject Reference of current Team Character
    private Vector3 m_teamMemberLastPos; // Last position of current Team Character
    private Animator m_teamMemberAnimator;
    private int m_teamMemberSpriteLayer;

    // Enemy Character Components
    [HideInInspector]
    public List<Character> m_Enemies = new List<Character>(); // all remaining enemies
    private List<Character> m_CharactersThatCanBeUnlocked = new List<Character>(); // list of characters the player could unlock after beating the level

    // Battle Round Components (Enemy Target)
    [HideInInspector]
    public Character m_LastSelectedEnemy; // enemy target the player has chosen
    private GameObject m_enemyTarget; // current enemy the player has selected
    private Animator m_enemyTargetAnimator;
    private int m_enemyTargetSpriteLayer;

    // Battle Round Components (Enemy Attacker)
    private Character m_enemyAttackerCharacter; // character script reference of enemy attacker
    private GameObject m_enemyAttacker; // gameobject reference of enemy attacker
    private Vector3 m_enemyAttackerLastPos; // last position of the enemy attacker
    private Animator m_enemyAttackerAnimator;
    private int m_enemyAttackerSpriteLayer;
    [HideInInspector]
    public Character m_enemy_ChosenTeamPlayerTarget; // character of player that enemy is going to attack
    private Animator m_enemy_ChosenTeamPlayerTargetAnimator;
    private int m_enemy_ChosenTeamPlayerTargetSpriteLayer;

    // Battle States
    public bool RoundStarted { get; private set; } = false;
    private bool m_roundInitialized = false;

    [HideInInspector]
    public bool m_PlayerAttacked = false;
    private bool m_playerMoveBack = false;

    private bool m_enemyTargetChoosen = false;
    [HideInInspector]
    public bool m_EnemyAttacked = false;
    private bool m_enemyMoveBack = false;

    // Movespeed how fast Characters walk towards each other
    public float m_MoveSpeed = 4.0f;
    private Vector3 m_direction;

    private void Awake()
    {
        // add all character types to the dictionary with their name as key
        m_CharacterDict.Add("Adventurer", m_CharacterTypes[0]);
        m_CharacterDict.Add("Bandit", m_CharacterTypes[1]);
        m_CharacterDict.Add("Golem", m_CharacterTypes[2]);
        m_CharacterDict.Add("Mandrake", m_CharacterTypes[3]);
        m_CharacterDict.Add("Rat", m_CharacterTypes[4]);
        m_CharacterDict.Add("Red Ogre", m_CharacterTypes[5]);
        m_CharacterDict.Add("Satyr", m_CharacterTypes[6]);
        m_CharacterDict.Add("Shade", m_CharacterTypes[7]);
        m_CharacterDict.Add("Wasp", m_CharacterTypes[8]);
        m_CharacterDict.Add("Werewolf", m_CharacterTypes[9]);
        m_CharacterDict.Add("Yeti", m_CharacterTypes[10]);

        SpawnPlayerTeam();
        SpawnEnemyTeam();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // If player pushed button to start the battle
        if (RoundStarted)
        {
            HandleRound();
        }
    }

    // initiates the battle phase
    public void StartRound()
    {
        RoundStarted = true;
    }

    // handles the complete battle
    // first: player attacks chosen enemy
    // then: enemy attacks random team member
    // lastly: reset all battle states
    private void HandleRound()
    {
        // check if player has won
        if (m_Enemies.Count <= 0)
        {
            ResetBattlePhase();
            UIControllerBattle.get.m_AttackButton.gameObject.SetActive(false);
            UIControllerBattle.get.m_HelperMenuButton.gameObject.SetActive(false);

            // Activate the ending screen
            UIControllerBattle.get.m_EndingScreen.gameObject.SetActive(true);

            string tmp = CheckForCharacterUnlock();

            UIControllerBattle.get.m_EndingText.text = "You won!\n" + tmp;

            if (PlayerData.LastSelectedLevel < 5)
            {
                ServerResponse sr = UserRequestLibrary.UnlockLevelForUser(PlayerData.LastSelectedLevel + 1);
            }

            return;
        }

        // if player lost
        if (m_Team.Count <= 0)
        {
            ResetBattlePhase();
            UIControllerBattle.get.m_AttackButton.gameObject.SetActive(false);
            UIControllerBattle.get.m_HelperMenuButton.gameObject.SetActive(false);

            // Activate the ending screen
            UIControllerBattle.get.m_EndingScreen.gameObject.SetActive(true);

            UIControllerBattle.get.m_EndingText.text = "You lost.. try again with a better team!";

            return;
        }

        // round start
        if (!m_roundInitialized)
        {
            // get gameobject of current team member
            m_teamMember = m_LastSelectedTeamMember.gameObject;

            // save last position of team member
            m_teamMemberLastPos = m_teamMember.transform.position;

            // get animator
            m_teamMemberAnimator = m_teamMember.GetComponent<Animator>();

            // attacker should overlap attacked
            // save original sorting layer
            m_teamMemberSpriteLayer = m_teamMember.GetComponent<SpriteRenderer>().sortingOrder;
            m_teamMember.GetComponent<SpriteRenderer>().sortingOrder = 15;

            // get gameobject of current enemy
            m_enemyTarget = m_LastSelectedEnemy.gameObject;

            // get animator
            m_enemyTargetAnimator = m_enemyTarget.GetComponent<Animator>();

            // let enemy sprite be under player sprite
            // save original sorting layer
            m_enemyTargetSpriteLayer = m_enemyTarget.GetComponent<SpriteRenderer>().sortingOrder;
            m_enemyTarget.GetComponent<SpriteRenderer>().sortingOrder = 14;

            // direction where team member is headed
            m_direction = m_enemyTarget.transform.position - m_teamMember.transform.position;
            m_direction.Normalize();

            // round start done
            m_roundInitialized = true;

            // run animation
            m_teamMemberAnimator.SetBool("IsRunning", true);
        }

        // player attack turn
        else if (!m_PlayerAttacked)
        {
            // if is not playing animation
            if (m_teamMemberAnimator != null && !m_teamMemberAnimator.GetBool("IsAttacking"))
            {
                // if enemy was reached, attack
                if (m_teamMember.transform.position.x >= m_enemyTarget.transform.position.x - 1.5f)
                {
                    m_teamMemberAnimator.SetBool("IsAttacking", true);
                    m_enemyTargetAnimator.SetTrigger("IsHurt");
                }
                // if enemy was not reached yet, return, so position can be updated
                else
                {
                    // move towards enemy
                    m_teamMember.transform.position += m_direction * m_MoveSpeed * Time.deltaTime;
                    return;
                }
            }
            // if already is playing animation | wait for animation event to be called
            else
            {
                return;
            }
        }

        // player moves back to original position
        else if (!m_playerMoveBack)
        {
            // if player has returned to his original position
            if (m_teamMember.transform.position.x <= m_teamMemberLastPos.x)
            {
                m_teamMember.GetComponent<Animator>().SetBool("IsRunning", false);

                // Reset Sprite Layers
                m_teamMember.GetComponent<SpriteRenderer>().sortingOrder = m_teamMemberSpriteLayer;
                m_enemyTarget.GetComponent<SpriteRenderer>().sortingOrder = m_enemyTargetSpriteLayer;

                m_playerMoveBack = true;
            }
            // if not move backwards
            else
            {
                m_teamMember.transform.position -= m_direction * m_MoveSpeed * Time.deltaTime;
                return;
            }
        }

        // check if enemy dead
        else if (m_LastSelectedEnemy.m_Health <= 0)
        {
            if (m_enemyTargetAnimator != null && !m_enemyTargetAnimator.GetBool("IsDead"))
            {
                m_enemyTargetAnimator.SetBool("IsDead", true);
            }

            return;
        }

        // enemy chooses a target to attack
        else if (!m_enemyTargetChoosen)
        {
            // choose a random target from the remaining team members
            // Remember: Random.Next(inclusive, exclusive)
            System.Random rand = new System.Random();
            m_enemy_ChosenTeamPlayerTarget = m_Team[rand.Next(0, m_Team.Count)];

            System.Random random = new System.Random();
            m_enemyAttackerCharacter = m_Enemies[random.Next(0, m_Enemies.Count)];

            // save spritelayer order for chosen target
            m_enemy_ChosenTeamPlayerTargetSpriteLayer = m_enemy_ChosenTeamPlayerTarget.GetComponent<SpriteRenderer>().sortingOrder;
            m_enemy_ChosenTeamPlayerTarget.GetComponent<SpriteRenderer>().sortingOrder = 14;
            m_enemy_ChosenTeamPlayerTargetAnimator = m_enemy_ChosenTeamPlayerTarget.GetComponent<Animator>();

            // get gameobject of current enemy
            m_enemyAttacker = m_enemyAttackerCharacter.gameObject;

            // save last position of current enemy
            m_enemyAttackerLastPos = m_enemyAttacker.transform.position;

            // get animator
            m_enemyAttackerAnimator = m_enemyAttacker.GetComponent<Animator>();

            // let enemy sprite be over player sprite
            // save original sorting layer
            m_enemyAttackerSpriteLayer = m_enemyAttacker.GetComponent<SpriteRenderer>().sortingOrder;
            m_enemyAttacker.GetComponent<SpriteRenderer>().sortingOrder = 15;

            // direction where enemy attacker is headed
            m_direction = m_enemy_ChosenTeamPlayerTarget.transform.position - m_enemyAttacker.transform.position;
            m_direction.Normalize();

            m_enemyAttacker.GetComponent<Animator>().SetBool("IsRunning", true);

            m_enemyTargetChoosen = true;
        }

        // enemy is attacking
        else if (!m_EnemyAttacked)
        {
            // if is not playing animation
            if (m_enemyAttackerAnimator != null && !m_enemyAttackerAnimator.GetBool("IsAttacking"))
            {
                // if player was reached, attack
                if (m_enemyAttacker.transform.position.x <= m_enemy_ChosenTeamPlayerTarget.transform.position.x + 1.5f)
                {
                    m_enemyAttackerAnimator.SetBool("IsAttacking", true);
                    m_enemy_ChosenTeamPlayerTargetAnimator.SetTrigger("IsHurt");
                }
                // if enemy was not reached yet, return, so position can be updated
                else
                {
                    // move towards enemy
                    m_enemyAttacker.transform.position += m_direction * m_MoveSpeed * Time.deltaTime;
                    return;
                }
            }
            // if already is playing animation | wait for animation event to be called
            else
            {
                return;
            }
        }

        // enemy moves back to original position
        else if (!m_enemyMoveBack)
        {
            // if player has returned to his original position
            if (m_enemyAttacker.transform.position.x >= m_enemyAttackerLastPos.x)
            {
                m_enemyAttacker.GetComponent<Animator>().SetBool("IsRunning", false);

                // Reset Sprite Layers
                m_enemy_ChosenTeamPlayerTarget.GetComponent<SpriteRenderer>().sortingOrder = m_enemy_ChosenTeamPlayerTargetSpriteLayer;
                m_enemyAttacker.GetComponent<SpriteRenderer>().sortingOrder = m_enemyAttackerSpriteLayer;

                m_enemyMoveBack = true;
            }
            // if not move backwards
            else
            {
                m_enemyAttacker.transform.position -= m_direction * m_MoveSpeed * Time.deltaTime;
                return;
            }
        }

        // team member targeted by enemy dead
        else if (m_enemy_ChosenTeamPlayerTarget.m_Health <= 0)
        {
            if (m_enemy_ChosenTeamPlayerTargetAnimator != null && !m_enemy_ChosenTeamPlayerTargetAnimator.GetBool("IsDead"))
            {
                m_enemy_ChosenTeamPlayerTargetAnimator.SetBool("IsDead", true);
            }
            return;
        }

        // round finished
        else
        {
            ResetBattlePhase();
        }
    }

    // if battle ends reset all states
    public void ResetBattlePhase()
    {
        // Reset Battle States
        RoundStarted = false;
        m_roundInitialized = false;
        m_PlayerAttacked = false;
        m_playerMoveBack = false;
        m_enemyTargetChoosen = false;
        m_EnemyAttacked = false;
        m_enemyMoveBack = false;

        // Reactivate Button for next round
        UIControllerBattle.get.m_AttackButton.gameObject.SetActive(true);
    }

    void SpawnPlayerTeam()
    {
        // spawn the player team that was chosen in main menu
        for (int i = 0; i < PlayerData.CharacterInSlots.Count; i++)
        {
            // spawn character at the correct team spawnpoint
            GameObject go = Instantiate(m_CharacterDict[PlayerData.CharacterInSlots[i]], m_TeamSpawnpoints[i]);

            // save character in team list
            Character TeamMemberReference = go.GetComponent<Character>();

            m_Team.Add(TeamMemberReference);

            // set characters status to player
            TeamMemberReference.m_TeamStatus = "Player";

            // update stats
            TeamMemberReference.GetStats();

            // if it is the first spawned, set as last selected team member
            if (i == 0)
            {
                m_LastSelectedTeamMember = TeamMemberReference;
            }

            // set x-scale * (- 1) for team characters (that are not the Adventurer
            if (i > 0)
                go.gameObject.transform.localScale = new Vector3(go.gameObject.transform.localScale.x * (-1.0f), go.gameObject.transform.localScale.y, go.gameObject.transform.localScale.z);

            // bottom chars should overlap top chars
            if (i > 0 && i % 2 != 0)
                go.GetComponent<SpriteRenderer>().sortingOrder = 11;
            else
                go.GetComponent<SpriteRenderer>().sortingOrder = 10;
        }
    }

    void SpawnEnemyTeam()
    {
        ServerResponse response = UserRequestLibrary.GetLevelData(PlayerData.LastSelectedLevel);

        // spawn enemies that are deposited in the database
        for (int i = 0; i < response.LevelData.Enemies.Count; i++)
        {
            // spawn enemy at the correct enemy spawnpoint
            GameObject go = Instantiate(m_CharacterDict[response.LevelData.Enemies[i].Name], m_EnemySpawnpoints[i]);

            Character EnemyReference = go.GetComponent<Character>();

            // if it is the first spawned, set as last selected enemy
            if (i == 0)
            {
                m_LastSelectedEnemy = EnemyReference;
            }

            // set character spawned to enemy team
            EnemyReference.m_TeamStatus = "Enemy";

            // set character values got from database
            EnemyReference.m_AttackDamage = response.LevelData.Enemies[i].AttackDamage;
            EnemyReference.m_Defense = response.LevelData.Enemies[i].Defense;
            EnemyReference.m_Health = response.LevelData.Enemies[i].HP;

            // save enemy in enemy list
            m_Enemies.Add(EnemyReference);

            // bottom chars should overlap top chars
            if (i > 0 && i % 2 != 0)
                go.GetComponent<SpriteRenderer>().sortingOrder = 11;
            else
                go.GetComponent<SpriteRenderer>().sortingOrder = 10;
        }

        // check which characters the player can unlock
        foreach (var charToBeUnlocked in m_Enemies)
        {
            // get current status character in database
            ServerResponse sr = UserRequestLibrary.GetCurrentCharStats(charToBeUnlocked.m_CharacterName);

            // if character has not unlocked the character yet
            if (sr.Characters[0].UnlockStatus == 0)
            {
                // add it to the list of characters that can be unlocked
                m_CharactersThatCanBeUnlocked.Add(charToBeUnlocked);
            }
        }
    }

    // check if player unlocked a character
    string CheckForCharacterUnlock()
    {
        // check if there are any chars that can be unlocked
        if (m_CharactersThatCanBeUnlocked.Count <= 0)
        {
            UIControllerBattle.get.m_UnlockedCharacterIcon.sprite = UIControllerBattle.get.m_CharacterSpritesDict["NotUnlocked"];
            return "You've already unlocked all characters in this Level!";
        }
        // if there are any
        else
        {
            // check if a character is going to be unlocked
            System.Random rand = new System.Random();
            int unlockChance = rand.Next(0, 100);

            // player missed chance to unlock character
            if (unlockChance < 50)
            {
                UIControllerBattle.get.m_UnlockedCharacterIcon.sprite = UIControllerBattle.get.m_CharacterSpritesDict["NotUnlocked"];
                return "Nobody joined your team this time. Try again another time!";
            }
            // player hit chance to unlock character
            else
            {
                // calculate which character should be unlocked
                int characterIndex = rand.Next(0, m_CharactersThatCanBeUnlocked.Count);

                // Unlock character for player
                ServerResponse responseDelServer = UserRequestLibrary.UnlockCharacterForUser(m_CharactersThatCanBeUnlocked[characterIndex].m_CharacterName);

                UIControllerBattle.get.m_UnlockedCharacterIcon.sprite = UIControllerBattle.get.m_CharacterSpritesDict[m_CharactersThatCanBeUnlocked[characterIndex].m_CharacterName];
                return $"The {m_CharactersThatCanBeUnlocked[characterIndex].m_CharacterName} joined your team!";
            }
        }
    }
}