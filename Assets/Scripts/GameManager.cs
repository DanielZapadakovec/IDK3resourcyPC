using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class GameManager : MonoBehaviour
{
    public PlayableDirector playableDirector1;
    public PlayableDirector playableDirector2;
    public GameObject MenuButtons;

    private bool isSkipped;
    private CanvasGroup menuButtonsCanvasGroup;
    private float fadeSpeed = 3f;

    public void Start()
    {
        isSkipped = false;
        menuButtonsCanvasGroup = MenuButtons.GetComponent<CanvasGroup>();

        menuButtonsCanvasGroup.alpha = 0f;
        menuButtonsCanvasGroup.interactable = false;
        menuButtonsCanvasGroup.blocksRaycasts = false;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isSkipped)
        {
            playableDirector1.Stop();
            playableDirector2.Play();
            StartCoroutine(FadeInMenuButtons());
            isSkipped = true;
        }
    }

    private IEnumerator FadeInMenuButtons()
    {
        while (menuButtonsCanvasGroup.alpha < 1f)
        {
            menuButtonsCanvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        menuButtonsCanvasGroup.alpha = 1f;
        menuButtonsCanvasGroup.interactable = true;
        menuButtonsCanvasGroup.blocksRaycasts = true;
    }
}

