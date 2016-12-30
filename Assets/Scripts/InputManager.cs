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
            Debug.Log("touch");

            if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Interaction"))) {
                hit.collider.GetComponent<Interactable>().Interact();
            }

            return;
        }

        if (Input.GetMouseButtonDown(0)) {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Debug.Log("touch");

            if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Interaction"))) {
                hit.collider.GetComponent<Interactable>().Interact();
                Debug.Log("hit");
            }
        }
    }
}
