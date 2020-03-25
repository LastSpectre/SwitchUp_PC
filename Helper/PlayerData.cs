using System.Collections.Generic;

public static class PlayerData
{
    // player data || TODO: remove data
    public static string playerName = "";
    public static string playerPassword = "";

    // connection data
    public static string IP = "127.0.0.1";
    public static int Port = 15151;

    // current team data
    public static List<string> CharacterInSlots = new List<string>() { "Adventurer"};

    // last selected level data
    public static int LastSelectedLevel = 1;

    public static void ResetPlayerData()
    {
        CharacterInSlots.Clear();
        CharacterInSlots.Add("Adventurer");
        LastSelectedLevel = 1;
        playerName = "";
        playerPassword = "";
    }
}