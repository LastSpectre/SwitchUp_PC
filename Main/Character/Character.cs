using BA_Praxis_Library;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [HideInInspector]
    public string m_CharacterName;
    [HideInInspector]
    public int m_Health;
    [HideInInspector]
    public int m_AttackDamage;
    [HideInInspector]
    public int m_Defense;
    [HideInInspector]
    public int m_Level;

    [HideInInspector]
    public ECharacterType m_Type;

    [HideInInspector]
    public int m_UnlockStatus;

    // [HideInInspector]
    public string m_TeamStatus;

    [Header("Sounds")]
    public AudioClip m_HitBumpSound;

    // gets this character values from the database
    public void GetStats()
    {
        ServerResponse response = UserRequestLibrary.GetCurrentCharStats(m_CharacterName);

        m_AttackDamage = response.Characters[0].AttackDamage;
        m_Defense = response.Characters[0].Defense;
        m_Health = response.Characters[0].HP;
        m_UnlockStatus = response.Characters[0].UnlockStatus;
        m_Level = response.Characters[0].Level;
    }

    // unlocks this character in the database so it can be used ingame
    public void UnlockCharacter()
    {
        ServerResponse response = UserRequestLibrary.UnlockCharacterForUser(m_CharacterName);
    }

    // calculates the damage the player has to take
    public void CharacterTakeDamage(Character _enemy)
    {
        // if character has the chaos type, it takes 2x damage
        if (m_Type == ECharacterType.CHAOS)
        {
            m_Health -= Mathf.Clamp(_enemy.m_AttackDamage - m_Defense / 2, 0, 1000) * 2;
            return;
        }

        // if enemy has chaos type, take 2x damage
        if (_enemy.m_Type == ECharacterType.CHAOS)
        {
            m_Health -= Mathf.Clamp(_enemy.m_AttackDamage - m_Defense / 2, 0, 1000) * 2;
            return;
        }

        // if enemy is neutral type, take normal damage
        if (_enemy.m_Type == ECharacterType.NEUTRAL)
        {
            m_Health -= Mathf.Clamp(_enemy.m_AttackDamage - m_Defense / 2, 0, 1000);
            return;
        }

        // if enemy is ancient type
        if (_enemy.m_Type == ECharacterType.ANCIENTS)
        {
            // take 0.5x damage if character is nature type
            if (m_Type == ECharacterType.NATURE)
            {
                m_Health -= Mathf.Clamp(_enemy.m_AttackDamage - m_Defense / 2, 0, 1000) / 2;
            }
            // take 2x damage if character is dark type
            else if (m_Type == ECharacterType.DARK)
            {
                m_Health -= Mathf.Clamp(_enemy.m_AttackDamage - m_Defense / 2, 0, 1000) * 2;
            }
            // take neutral damage if character has other type
            else
            {
                m_Health -= Mathf.Clamp(_enemy.m_AttackDamage - m_Defense / 2, 0, 1000);
            }

            return;
        }

        // if enemy is dark type
        if (_enemy.m_Type == ECharacterType.DARK)
        {
            // take 0.5x damage if character is ancient typing
            if (m_Type == ECharacterType.ANCIENTS)
            {
                m_Health -= Mathf.Clamp(_enemy.m_AttackDamage - m_Defense / 2, 0, 1000) / 2;
            }
            // take 2x damage if character is nature type
            else if (m_Type == ECharacterType.NATURE)
            {
                m_Health -= Mathf.Clamp(_enemy.m_AttackDamage - m_Defense / 2, 0, 1000) * 2;
            }
            // take neutral damage if character has other type
            else
            {
                m_Health -= Mathf.Clamp(_enemy.m_AttackDamage - m_Defense / 2, 0, 1000);
            }

            return;
        }

        // if enemy is nature type
        if (_enemy.m_Type == ECharacterType.NATURE)
        {
            // take 0.5x damage if character is dark typing
            if (m_Type == ECharacterType.DARK)
            {
                m_Health -= Mathf.Clamp(_enemy.m_AttackDamage - m_Defense / 2, 0, 1000) / 2;
            }
            // take 2x damage if character is ancient typing
            else if (m_Type == ECharacterType.ANCIENTS)
            {
                m_Health -= Mathf.Clamp(_enemy.m_AttackDamage - m_Defense / 2, 0, 1000) * 2;
            }
            // take neutral damage if character has other typing
            else
            {
                m_Health -= Mathf.Clamp(_enemy.m_AttackDamage - m_Defense / 2, 0, 1000);
            }

            return;
        }
    }

    public void AnimEvent_AttackSound()
    {
        BattleController.get.m_AudioSource.PlayOneShot(m_HitBumpSound, 0.2f);
    }

    public void AnimEvent_AttackEnding()
    {
        if (m_TeamStatus == "Player")
        {
            BattleController.get.m_PlayerAttacked = true;
            gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);

            BattleController.get.m_LastSelectedEnemy.CharacterTakeDamage(this);
        }
        else if (m_TeamStatus == "Enemy")
        {
            BattleController.get.m_EnemyAttacked = true;
            gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);

            BattleController.get.m_enemy_ChosenTeamPlayerTarget.CharacterTakeDamage(this);
        }
    }

    public void AnimEvent_Die()
    {
        if (m_TeamStatus == "Enemy")
        {
            BattleController.get.m_Enemies.Remove(this);

            if (BattleController.get.m_Enemies.Count > 0)
                BattleController.get.m_LastSelectedEnemy = BattleController.get.m_Enemies[0];

            Destroy(gameObject);
        }
        else if (m_TeamStatus == "Player")
        {
            BattleController.get.m_Team.Remove(this);

            if (BattleController.get.m_Team.Count > 0)
            {
                if(this == BattleController.get.m_LastSelectedTeamMember)
                {
                    BattleController.get.m_LastSelectedTeamMember = BattleController.get.m_Team[0];
                }
                BattleController.get.ResetBattlePhase();
            }
            
            Destroy(gameObject);
        }
    }
}