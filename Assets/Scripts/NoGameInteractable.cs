using UnityEngine;
using System.Collections;
using System;

public class NoGameInteractable : MonoBehaviour, Interactable {

    public GameObject card;
    public Vector3 glidingPoint;

    bool _hasActivated;
    BaseActivateable _activateable;
    bool _canInteract;
    float _duration = 0.8f;

    void Start() {
        if (_activateable == null) {
            SetActivateable(Instantiate(card).GetComponent<BaseActivateable>());
        }
        StartCoroutine(Glide());
    }

    public void Interact() {
        if (!_canInteract)
            return;

       _activateable.Activate();
    }

    public void SetInteractable(bool canInteract) {
        _canInteract = canInteract;
        if (_activateable != null)
            _activateable.SetActivateable(canInteract);
    }

    public void SetActivateable(BaseActivateable ba) {
        _activateable = ba;
        _activateable.model.rotation = transform.rotation;
        _activateable.transform.position = glidingPoint;
        _activateable.SetNoGameInteractable(this);
    }

    public void MarkActivated() {
        if (!_hasActivated) {
            _hasActivated = true;
            NoGameManager.UseCard();
        }
    }

    IEnumerator Glide() {
        float tween = 0;
        while (tween < 1) {
            tween += Time.deltaTime/_duration;
            _activateable.transform.position = Vector3.Slerp(glidingPoint, transform.position, tween);
            yield return new WaitForEndOfFrame();
        }
        _activateable.transform.position = transform.position;
        SetInteractable(true);
        _activateable.SetStartPosition(transform.position);
    }
}
