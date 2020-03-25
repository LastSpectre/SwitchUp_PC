public class EmptyCharacter : Character
{
    private void Awake()
    {
        // sets the default values of the empty character to Zero
        m_AttackDamage = 0;
        m_CharacterName = "";
        m_Defense = 0;
        m_Health = 0;
        m_Type = ECharacterType.NONE;
        m_UnlockStatus = 0;
        m_Level = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
