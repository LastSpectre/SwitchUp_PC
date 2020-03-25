using UnityEngine.SceneManagement;
using UnityEngine;

public class BattleSceneButtons : MonoBehaviour
{
    // Loads back into main scene
    public void Back_ButtonClick()
    {
        SingletonReset.Reset();
        SceneManager.LoadScene(2);
    }

    public void Button_Attack()
    {
        BattleController.get.StartRound();
        UIControllerBattle.get.m_AttackButton.gameObject.SetActive(false);
    }

    public void Button_Cancel()
    {
        UIControllerBattle.get.m_CancelMenu.gameObject.SetActive(false);
    }

    public void Button_ToggleHelperMenu()
    {
        UIControllerBattle.get.m_HelperMenu.SetActive(!UIControllerBattle.get.m_HelperMenu.activeInHierarchy);
    }
}