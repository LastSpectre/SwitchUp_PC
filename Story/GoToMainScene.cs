using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMainScene : MonoBehaviour
{
    public GameObject m_TopText;
    public GameObject m_MiddleText;
    public GameObject m_BottomText;
    public GameObject m_ContinueText;
    public float m_FadingSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadTextElements());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(LoadMainSceneAsync());
        }
    }

    IEnumerator LoadTextElements()
    {
        m_TopText.SetActive(true);
        yield return new WaitForSeconds(m_FadingSpeed);

        m_MiddleText.SetActive(true);
        yield return new WaitForSeconds(m_FadingSpeed);

        m_BottomText.SetActive(true);
        yield return new WaitForSeconds(m_FadingSpeed);

        m_ContinueText.SetActive(true);

        yield break;
    }

    IEnumerator LoadMainSceneAsync()
    {
        SingletonReset.Reset();

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(2);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }
}
