using UnityEngine;
using System.Collections;

public class LootChest : BaseActivateable {

    public BaseActivateable secondary;
    public int starsInside;

    PlayMakerFSM _myFSM;

    void Start() {
        _myFSM = GetComponent<PlayMakerFSM>();
        _myFSM.FsmVariables.FindFsmGameObject("model").Value = model.gameObject;
    }

    public override bool Activate() {
        if (!base.Activate()) {
            return false;
        }

        LevelManager.ToggleInput(false);
        _myFSM.SendEvent("StartMiniGame");
        return true;
    }


    public void EndMinigame() {
        Deactivate();
    }

    public override bool Deactivate() {
        if (!base.Deactivate())
            return false;

        LevelManager.AddStars(starsInside);
        LevelManager.ToggleInput(true);
        if (_wayPoint != null)
            _wayPoint.MarkActivated();
        return true;
    }
}
