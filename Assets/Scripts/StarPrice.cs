using UnityEngine;
using System.Collections;

public class StarPrice : BaseActivateable {

    public BaseActivateable secondary;
    public int price;

    void Start() {
        model.GetComponentInChildren<TextMesh>().text = price.ToString();
        if (GameManager.IsSpent(gameObject.name))
            Use();
    }

    void Update() {
        Floating();
    }

    public override bool Activate() {

        if (!base.Activate()) {
            return false;
        }

        if (!LevelManager.SpendStars(price)) {
            _active = false;
            return false;
        }

        Use();

        return true;
    }

    void Use() {
        if (_wayPoint != null) {
            if (secondary) {
                _wayPoint.activateable = secondary;
                secondary.SetWaypoint(_wayPoint);
                secondary.SetActivateable(true);
            } else
                _wayPoint.MarkActivated();
        }
        Destroy(this.gameObject);
    }
}
