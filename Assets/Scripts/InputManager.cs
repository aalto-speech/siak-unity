using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    bool _sendInputs = true;
    public Camera cam;

    public bool SendInputs {
        get {
            return _sendInputs;
        }
        set {
            _sendInputs = value;
        }
    }

    void Update() {
        if (_sendInputs) {
           HandleInput();
        }
    }

    void HandleInput() {
        //if (Input.touchCount == 0)
            //return;

        for(int i = 0; i < Input.touches.Length; i++) {

            if (Input.touches[i].phase != TouchPhase.Began)
                continue;

            Ray ray = cam.ScreenPointToRay(Input.touches[i].position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1 << 10)) {
                Interactable inter = hit.collider.GetComponent<Interactable>();
                if (inter != null)
                    inter.Interact();
                else
                    Debug.Log(hit.collider.name + " isn't interactable.");
            }

            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.PositiveInfinity,LayerMask.GetMask("Interaction"))) {
                Interactable inter = hit.collider.GetComponent<Interactable>();
                if (inter != null)
                    inter.Interact();
                else
                    Debug.Log(hit.collider.name + " isn't interactable.");
            }
        }
    }
}
