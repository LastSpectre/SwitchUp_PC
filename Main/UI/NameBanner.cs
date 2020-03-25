using UnityEngine;
using TMPro;

public class NameBanner : MonoBehaviour
{
    public TMP_Text m_Username;

    // Start is called before the first frame update
    void Start()
    {
        m_Username.text = PlayerData.playerName;
    }
}
