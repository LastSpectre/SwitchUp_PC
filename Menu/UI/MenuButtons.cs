using BA_Praxis_Library;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [Header("UI")]
    // UI elements
    public TMP_Text m_PasswordText;
    public TMP_Text m_NameText;

    public Image CreateAccountBanner;
    public TMP_Text CreateAccountBannerText;

    public Image LoginBanner;
    public TMP_Text LoginBannerText;

    // button function to create a new account
    public void Button_CreateAccount()
    {
        ServerResponse response = UserRequestLibrary.CreateAccountRequest(m_NameText, m_PasswordText);
        
        // log the response to the player           
        CreateAccountBanner.gameObject.SetActive(true);
        CreateAccountBannerText.text = response.Text;
        Invoke("DisableCreateAccountBanner", 2.0f);
        return;
    }

    // button that logs player in
    public void Button_Login()
    {
        ServerResponse response = UserRequestLibrary.LoginRequest(m_NameText, m_PasswordText);

        // if response is positive, save name and password, and load next scene
        if (response.ResponseType == EServerResponseType.OK)
        {
            // save name and password
            PlayerData.playerName = m_NameText.text.Remove(m_NameText.text.Length - 1);
            PlayerData.playerPassword = m_PasswordText.text.Remove(m_PasswordText.text.Length - 1);

            LoginBanner.gameObject.SetActive(true);
            LoginBannerText.text = response.Text;
            StartCoroutine(LoadStoryScene());
            return;
        }

        // if response is error, print out error message
        if (response.ResponseType == EServerResponseType.ERROR)
        {
            LoginBanner.gameObject.SetActive(true);
            LoginBannerText.text = response.Text;
            Invoke("DisableLoginBanner", 2.0f);
            return;
        }
    }

    void DisableCreateAccountBanner()
    {
        CreateAccountBanner.gameObject.SetActive(false);
        CreateAccountBannerText.text = "";
    }

    void DisableLoginBanner()
    {
        LoginBanner.gameObject.SetActive(false);
        LoginBannerText.text = "";
    }

    // loads the main scene async
    IEnumerator LoadStoryScene()
    {
        MusicScript.get.FadeMusicOut();

        SingletonReset.Reset();
        AsyncOperation asyncSceneLoad = SceneManager.LoadSceneAsync(1);

        while(!asyncSceneLoad.isDone)
        {
            yield return null;
        }
    }
}