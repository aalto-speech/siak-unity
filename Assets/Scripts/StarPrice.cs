using UnityEngine;
using System.Collections;

public class StarPrice : BaseActivateable {

    public BaseActivateable secondary;
    public int price;
    public string uniqueId;

    void Start() {
        if (GameManager.IsSpent(uniqueId)) {
            Destroy(this.gameObject);
            if (secondary == null)
                _wayPoint.EmptyActivateable();
            else
                SetUpSecondary();

            return;
        }
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
            LevelManager.GetLevelManager().starGUI.Shake(0.75f);
            return false;
        }

        Use();

        return true;
    }

    void Use() {
        GameManager.AddSpent(uniqueId, price);
        if (_wayPoint != null) {
            if (secondary) {
                SetUpSecondary();
                LevelManager.GetPlayerToken().FindPathToWaypoint(_wayPoint);
            } else
                _wayPoint.MarkActivated();
        }
        Destroy(this.gameObject);
    }

    void SetUpSecondary() {
        _wayPoint.activateable = secondary;
        secondary.SetWaypoint(_wayPoint);
        secondary.SetActivateable(true);
    }
}
