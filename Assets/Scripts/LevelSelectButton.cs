using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour {
    public Level level;
    public string maxStars;
    
    GameObject _lock;
    Text _text;
    bool _canSelect;
    int _compare;

    void Awake() {
        _text = GetComponentInChildren<Text>();
        Transform l = transform.Find("Lock");
        if (l != null)
            _lock = l.gameObject;
    }

    void Stars() {
        int stars = int.MaxValue;
        int.TryParse(maxStars, out stars);
        if (GameManager.GetCollectedStars(level) > stars)
            GameManager.SetLevelStars(level, 0);
    }

    void OnEnable() {
        _canSelect = false;
        _compare = (maxStars != "" && maxStars != "0") ? (int)level : (int)level + 1;
        if (_text != null && _lock != null) {
            if (GameManager.GetCompleted() >= _compare || true) { //temporarily unlock all
                _text.gameObject.SetActive(true);
                if (maxStars != "" && maxStars != "0")
                    _text.text = GameManager.GetCollectedStars(level).ToString() + " / " + maxStars;
                _lock.SetActive(false);
                StartCoroutine(SetClickable());
            } else {
                _lock.SetActive(true);
                _text.gameObject.SetActive(false);
            }
        } else
            StartCoroutine(SetClickable());
    }

    IEnumerator SetClickable() {
        yield return new WaitForSecondsRealtime(0.3f); //prevent instant levelselecting;
        _canSelect = true;
    }

    public void SelectLevel() {
        if (!_canSelect)
            return;
        Transform t = transform;
        while (t.parent != null)
            t = t.parent;
        t.gameObject.SetActive(false);
        GameManager.ChangeLevel(level, false);
    }
}