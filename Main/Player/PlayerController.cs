using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Checks player input every frame
        PlayerInput();        
    }

    // handles the player input
    private void PlayerInput()
    {
        // if player presses escape, clear the current chosen character
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            UIController.get.ClearCharacterChosen();
        }
    }
}