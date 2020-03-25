using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TeamBuildingMenu : MonoBehaviour
{
    [Header("UI Script References")]
    public ChooseCharacter[] m_UnlockableCharacters;
    public ChangeCharacter[] m_CurrentTeamSlots;

    private void OnEnable()
    {
        UpdateTeamBuildingMenuUI();
    }

    private void Start()
    {
        UpdateTeamBuildingMenuUI();
    }

    private void OnDisable()
    {
        // clear both lists before adding new characters to them
        PlayerData.CharacterInSlots.Clear();

        for (int i = 0; i < m_CurrentTeamSlots.Length; i++)
        {
            // if there is something in the slot, add it to the current team
            if (m_CurrentTeamSlots[i].m_CurrentTeamSlot.m_CharacterName != "")
            {
                PlayerData.CharacterInSlots.Add(m_CurrentTeamSlots[i].m_CurrentTeamSlot.m_CharacterName);
            }
        }

        UpdateTeamBuildingMenuUI();
    }

    // update the UI of the team building menu
    private void UpdateTeamBuildingMenuUI()
    {
        foreach (ChooseCharacter chooseChar in m_UnlockableCharacters)
        {
            chooseChar.ShowCharacterState();
        }

        foreach (ChangeCharacter changeCharacter in m_CurrentTeamSlots)
        {
            changeCharacter.ShowTeamSlotState();
        }
    }
}