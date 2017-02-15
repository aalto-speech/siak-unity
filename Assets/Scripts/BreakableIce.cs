using UnityEngine;
using System.Collections;

public class BreakableIce : BaseActivateable {

    public BaseActivateable secondary;
    public Transform shaker;

    bool _gameOn;
    float _volume;
    float _maxVolume = 0.3f;
    int _clicks = 3;
    float _decayTime = 2.0f;

    void Start() {
        _startPos = transform.position;
    }

    void Update() {
        if (_gameOn) {

            if (Input.GetMouseButtonDown(0))
                _volume += _maxVolume / _clicks;

            shaker.position = transform.position + new Vector3(_volume * Random.Range(-1, 1), _volume * Random.Range(-1, 1), _volume * Random.Range(-1, 1));

            if (_volume >= _maxVolume)
                Explode();

            _volume = Mathf.Max(0,_volume-Time.deltaTime*_maxVolume/_decayTime);
        }
    }

    public override bool Activate() {
        if (!base.Activate() || _gameOn) {
            return false;
        }

        LevelManager.ToggleInput(false);
        StartCoroutine(GoToPosition(true, CameraManager.GetCardLocation(), transform, model));

        return true;
    }

    protected override void EndReached(bool toLocation) {
        _gameOn = true;
    }

    public override bool Deactivate() {
        if (!base.Deactivate())
            return false;
        
        if (_wayPoint != null) {
            if (secondary == null)
                _wayPoint.MarkActivated();
            else {
                _wayPoint.activateable = secondary;
                secondary.SetWaypoint(_wayPoint);
                secondary.SetActivateable(true);
                LevelManager.GetPlayerToken().FindPathToWaypoint(_wayPoint);
            }
        }
        Destroy(this.gameObject);
        LevelManager.ToggleInput(true);
        return true;
    }

    void Explode() {
        shaker.position = transform.position;
        Rigidbody[] rbs = model.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody rb in rbs) {
            rb.isKinematic = false;
            rb.AddExplosionForce(1000.0f, transform.position, 4);
            rb.AddTorque(new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)) * 300f);
            rb.transform.parent = null;
            Destroy(rb.gameObject, 5.0f);
        }
        CameraManager.Shake(1.0f, 1.0f);
        Deactivate();
    }
}
