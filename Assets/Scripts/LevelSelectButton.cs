using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour {
    public Level level;
    public string maxStars;

    GameObject _lock;
    Text _text;
    bool _canSelect;

    void Awake() {
        _text = GetComponentInChildren<Text>();
        _lock = transform.Find("Lock").gameObject;
    }

    void OnEnable() {
        if (GameManager.GetCompleted() >= (int)level) {
            _text.gameObject.SetActive(true);
            _text.text = GameManager.GetCollectedStars(level).ToString() + " / " + maxStars;
            _lock.SetActive(false);
            _canSelect = true;
        } else {
            _lock.SetActive(true);
            _text.gameObject.SetActive(false);
            _canSelect = false;
        }
    }

    public void SelectLevel() {
        if (!_canSelect)
            return;
        Transform t = transform;
        while (t.parent != null)
            t = t.parent;
        t.gameObject.SetActive(false);
        GameManager.ChangeLevel(level);
    }
}