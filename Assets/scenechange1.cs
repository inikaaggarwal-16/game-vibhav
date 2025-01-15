using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections; // This is needed for IEnumerator

public class SceneSwitcher : MonoBehaviour
{
    public string sceneName = "SampleScene"; // Name of the scene to load
    public float delay = 3f; // Delay before switching
    public Image fadeImage; // UI Image to use for fade effect
    public float fadeDuration = 1f; // Duration of the fade effect

    private void Start()
    {
        // Start the scene switch process
        StartCoroutine(SwitchSceneAfterDelay());
    }

    private IEnumerator SwitchSceneAfterDelay()
    {
        // Wait for the delay
        yield return new WaitForSeconds(delay);

        // Start fading out
        yield return StartCoroutine(FadeOut());

        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;

        // Ensure the fadeImage is visible and initially transparent
        fadeImage.gameObject.SetActive(true);
        Color fadeColor = fadeImage.color;
        fadeColor.a = 0f;
        fadeImage.color = fadeColor;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeColor.a = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }
    }
}
