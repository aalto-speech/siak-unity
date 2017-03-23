using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimonSays : BaseActivateable {

    public Transform clickables;
    public Transform shape;

    Camera _cam;
    List<int> _order = new List<int>();
    bool _gameOn;
    int _count = 0;

    void Start() {
        _startPos = transform.position;
        _cam = CameraManager.GetCamera();
    }

    void Update() {
        if (_gameOn) {
            for (int i = 0; i < Input.touches.Length; i++) {

                if (Input.touches[i].phase != TouchPhase.Began)
                    continue;

                Ray ray = _cam.ScreenPointToRay(Input.touches[i].position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit)) {
                    Transform click = hit.collider.transform;
                    if (click == clickables.GetChild(_order[_count]))
                        StartCoroutine(SuccesfulClick(click));
                }

                return;
            }

            if (Input.GetMouseButtonDown(0)) {
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, float.PositiveInfinity)) {
                    Transform click = hit.collider.transform;
                    if (click == clickables.GetChild(_order[_count]))
                        StartCoroutine(SuccesfulClick(click));
                }
            }
        }
        if (!_active)
            Floating();
    }

    IEnumerator SuccesfulClick(Transform t) {
        _count++;
        int check = _count;
        if (_count == clickables.childCount)
            _gameOn = false;

        float a = 0;
        Vector3 start = t.localScale;
        while (a < 1) {
            a += Time.deltaTime * 2.0f;
            t.localScale = Vector3.Lerp(start, Vector3.zero, a);
            yield return new WaitForEndOfFrame();
        }

        if (check == clickables.childCount)
            Deactivate();
    }

    public override bool Activate() {
        if (!base.Activate() || _gameOn) {
            return false;
        }

        model.transform.localPosition = Vector3.zero;
        LevelManager.ToggleInput(false);
        StartCoroutine(GoToPosition(true, CameraManager.GetCardLocation(), transform, transform));
        return true;
    }

    protected override void EndReached(bool toLocation) {
        StartCoroutine(ClicksToShape());
    }

    IEnumerator ClicksToShape() {
        float a = 0;
        Vector3[] starts = new Vector3[clickables.childCount];
        for (int i = 0; i < clickables.childCount; i++)
            starts[i] = clickables.GetChild(i).position;

        while (a < 1) {
            a += Time.deltaTime;
            for (int i = 0; i < clickables.childCount; i++) {
                clickables.GetChild(i).position = Vector3.Lerp(starts[i], shape.GetChild(i).position, a);
            }
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(BlinkClickables());
    }

    IEnumerator BlinkClickables() {
        for (int i = 0; i < clickables.childCount; i++)
            _order.Add(i);

        int n = _order.Count;
        while (n > 1) {
            n--;
            int k = Random.Range(0, n + 1);
            int value = _order[k];
            _order[k] = _order[n];
            _order[n] = value;
        }

        for (int i = 0; i < _order.Count; i++) {
            float a = 0;
            Vector3 start = clickables.GetChild(_order[i]).localScale;
            bool reverse = false;
            while ((a < 1 && !reverse) || (a > 0 && reverse)) {
                a += (reverse) ? Time.deltaTime * 6.0f * -1 : Time.deltaTime * 6.0f;
                clickables.GetChild(_order[i]).localScale = Vector3.Slerp(start, start * 1.5f, a);
                if (a >= 1) {
                    a = 1;
                    reverse = true;
                    clickables.GetChild(_order[i]).GetChild(0).gameObject.SetActive(false);
                    clickables.GetChild(_order[i]).GetChild(1).gameObject.SetActive(true);
                }
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);
        }

        _gameOn = true;
        yield return null;
    }

    public override bool Deactivate() {
        if (!base.Deactivate())
            return false;

        if (_wayPoint != null)
            _wayPoint.MarkActivated();
        Destroy(this.gameObject);
        LevelManager.ToggleInput(true);
        return true;
    }
}
