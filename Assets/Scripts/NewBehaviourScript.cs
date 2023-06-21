using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionToNextLevel : MonoBehaviour
{
    IEnumerator FadeOut(string sceneName)
    {
        GameObject fadeScreen = new GameObject();
        fadeScreen.name = "Fade Screen";
        fadeScreen.AddComponent<Canvas>();
        Canvas fadeScreenCanvas = fadeScreen.GetComponent<Canvas>();
        fadeScreenCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadeScreenCanvas.sortingOrder = 1000;
        fadeScreen.AddComponent<CanvasScaler>();
        fadeScreen.AddComponent<GraphicRaycaster>();
        fadeScreen.AddComponent<Image>();
        Image fadeScreenImage = fadeScreen.GetComponent<Image>();
        fadeScreenImage.color = new Color(0, 0, 0, 0);
        for (float i = 0; i < 1; i += (float)0.01)
        {
            fadeScreenImage.color = new Color(0, 0, 0, i);
            Debug.Log("transparency is = " + i);
            yield return new WaitForSeconds(.01f);
        }
        yield return new WaitForSeconds(1f);
        ChangeSceneWithSceneName(sceneName); // Use parameter, which scene to change.
    }
    void ChangeSceneWithSceneName(string sceneName) // Change scene method with name (string) as a parameter.
    {
        SceneManager.LoadScene(sceneName);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Trigger triggered");
            StartCoroutine(FadeOut("1-1"));
        }
    }
}
