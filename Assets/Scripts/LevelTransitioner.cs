using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelTransitioner : MonoBehaviour {

    Image back;
    Text text;
    float _dotTime = 0.5f;
    int _dots = 0;
    Scene _next;
    float _fadeTime = 0.3f;
    float _minimumWait = 2f;

    void Start() {
        GameManager.CanLevelSelect(false);
        back = transform.GetComponentInChildren<Image>();
        text = transform.GetComponentInChildren<Text>();
        CrossFade(0, 0);
        CrossFade(1, _fadeTime);
        StartCoroutine(Dots());
        StartCoroutine(LoadScene());
        DontDestroyOnLoad(this.gameObject);
    }

    IEnumerator Dots() {
        float timer = 0;
        while (_dots <= 3) {
            timer += Time.unscaledDeltaTime;
            if (timer > _dotTime) {
                timer -= _dotTime;
                _dots++;
                text.text = "loading";
                if (_dots != 4) {
                    for (int i = 0; i < _dots; i++)
                        text.text += ".";
                } else {
                    _dots = 0;
                }
            }
            yield return null;
        }
    }

    IEnumerator LoadScene() {
        yield return new WaitForSecondsRealtime(_fadeTime);
        Time.timeScale = 0;
        AsyncOperation async = SceneManager.LoadSceneAsync(GameManager.NextLevel());
        while (!async.isDone) {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSecondsRealtime(_minimumWait/2);
        StopCoroutine(Dots());
        _dots = 4;
        text.text = "loaded";
        GameManager.GetLoginScreen().SetUser();
        yield return new WaitForSecondsRealtime(_minimumWait/2);
        CrossFade(0, _fadeTime);
        Time.timeScale = 1;
        yield return new WaitForSecondsRealtime(_fadeTime);
        GameManager.CanLevelSelect(true);
        Destroy(this.gameObject);
    }

    void CrossFade(float alpha, float duration) {
        back.CrossFadeAlpha(alpha, duration, false);
        text.CrossFadeAlpha(alpha, duration, false);

    }
}
