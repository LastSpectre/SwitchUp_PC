using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChangeIP : MonoBehaviour
{
    public Image m_ConnectionMenu;
    public TMP_Text m_CurrentIPText;
    public TMP_Text m_NewIPText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        m_CurrentIPText.text = PlayerData.IP; 
    }

    public void SaveIP()
    {
        PlayerData.IP = m_NewIPText.text.Remove(m_NewIPText.text.Length - 1);
    }

    public void ChangeMenuStatus()
    {
        m_ConnectionMenu.gameObject.SetActive(!m_ConnectionMenu.gameObject.activeInHierarchy);
    }
}