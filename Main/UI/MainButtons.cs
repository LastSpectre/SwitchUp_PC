using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButtons : MonoBehaviour
{
    // deactive teambuilding menu and disable the last chosen character
    public void BackButton_TeambuildingMenu()
    {
        UIController.get.ClearCharacterChosen();

        UIController.get.ToggleTeambuildingMenu();
    }

    // disable the level selector
    public void BackButton_LevelSelector()
    {
        UIController.get.ToggleLevelSelector();
    }

    // enable the team building menu and disable the clicked banner
    public void Button_CastleBanner()
    {
        UIController.get.ToggleTeambuildingMenu();

        UIController.get.DisableCastleBanner();
    }

    // enable the level selector and disable the clicked banner
    public void Button_MountainBanner()
    {
        UIController.get.ToggleLevelSelector();

        UIController.get.DisableMountainBanner();
    }

    // brings the player back to the menu
    public void Button_BackToMenu()
    {
        PlayerData.ResetPlayerData();
        SingletonReset.Reset();
        SceneManager.LoadScene(0);
    }
}