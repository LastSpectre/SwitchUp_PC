using BA_Praxis_Library;
using TMPro;

// contains all requests the player can send to the server
public static class UserRequestLibrary
{
    // creates a request to log the user in
    public static ServerResponse LoginRequest(TMP_Text _name, TMP_Text _password)
    {
        // create new client
        Client client = new Client();

        UserRequest currentRequest = new UserRequest()
        {
            RequestType = EUserRequestType.USER_LOGIN,
            Username = _name.text.Remove(_name.text.Length - 1),
            Password = _password.text.Remove(_password.text.Length - 1)
        };

        // get response from server with new request
        ServerResponse response = client.RunRequest(PlayerData.IP, PlayerData.Port, currentRequest);

        return response;
    }

    // creates a request to create a new user
    public static ServerResponse CreateAccountRequest(TMP_Text _name, TMP_Text _password)
    {
        // create new client
        Client client = new Client();

        UserRequest currentRequest = new UserRequest()
        {
            RequestType = EUserRequestType.USER_CREATE,
            Username = "",
            Password = "",
            // Username and password must be trimmed, so last char doesnt get send to server
            PayloadUser = new User()
            {
                Name = _name.text.Remove(_name.text.Length - 1),
                Password = _password.text.Remove(_password.text.Length - 1)
            }
        };

        // get response from server with new request
        ServerResponse response = client.RunRequest(PlayerData.IP, PlayerData.Port, currentRequest);

        return response;
    }

    // get value from a character
    public static ServerResponse GetCurrentCharStats(string _charName)
    {
        // create new client
        Client client = new Client();

        UserRequest currentRequest = new UserRequest()
        {
            RequestType = EUserRequestType.CHAR_GET,
            Username = PlayerData.playerName,
            Password = PlayerData.playerPassword,
            PayloadChar = new BA_Praxis_Library.Character() { Name = _charName }
        };

        // get response from server with new request
        ServerResponse response = client.RunRequest(PlayerData.IP, PlayerData.Port, currentRequest);

        return response;
    }

    // unlock character for player
    public static ServerResponse UnlockCharacterForUser(string _charName)
    {
        // create new client
        Client client = new Client();

        UserRequest currentRequest = new UserRequest()
        {
            RequestType = EUserRequestType.CHAR_UNLOCK,
            Username = PlayerData.playerName,
            Password = PlayerData.playerPassword,
            PayloadChar = new BA_Praxis_Library.Character() { Name = _charName }
        };

        // get response from server with new request
        ServerResponse response = client.RunRequest(PlayerData.IP, PlayerData.Port, currentRequest);

        return response;
    }

    // get the unlock status of the player level
    public static ServerResponse GetLevelStatusForUser(int _levelID)
    {
        // create new client
        Client client = new Client();

        UserRequest currentRequest = new UserRequest()
        {
            RequestType = EUserRequestType.LEVEL_STATUS,
            Username = PlayerData.playerName,
            Password = PlayerData.playerPassword,
            PayloadLevelStatus = new LevelStatus() { LevelID = _levelID }
        };

        // get response from server with new request
        ServerResponse response = client.RunRequest(PlayerData.IP, PlayerData.Port, currentRequest);
        
        return response;
    }

    // get the level data of requested level
    public static ServerResponse GetLevelData(int _levelID)
    {
        // create new client
        Client client = new Client();

        UserRequest currentRequest = new UserRequest()
        {
            RequestType = EUserRequestType.LEVEL_DATA,
            Username = PlayerData.playerName,
            Password = PlayerData.playerPassword,
            PayloadLevelData = new LevelData() { LevelID = _levelID }
        };

        // get response from server with new request
        ServerResponse response = client.RunRequest(PlayerData.IP, PlayerData.Port, currentRequest);

        return response;
    }

    // unlock level for player
    public static ServerResponse UnlockLevelForUser(int _levelID)
    {
        // create new client
        Client client = new Client();

        UserRequest currentRequest = new UserRequest()
        {
            RequestType = EUserRequestType.LEVEL_UNLOCK,
            Username = PlayerData.playerName,
            Password = PlayerData.playerPassword,
            PayloadLevelStatus = new LevelStatus { LevelID = _levelID}
        };

        // get response from server with new request
        ServerResponse response = client.RunRequest(PlayerData.IP, PlayerData.Port, currentRequest);

        return response;
    }
}