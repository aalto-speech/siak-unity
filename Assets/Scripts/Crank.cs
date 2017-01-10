using UnityEngine;
using System.Collections;

public class Crank : BaseActivateable {
    public BaseActivateable secondary;
    public GameObject plank;
    public BaseNode wayPointToConnect;
    public BaseNode pathToConnectTo;
    
    PlayMakerFSM _myFSM;

    void Start() {
        _myFSM = GetComponent<PlayMakerFSM>();
        _myFSM.FsmVariables.FindFsmGameObject("spawnLocation").Value = CameraManager.GetCardLocation().gameObject;
        _myFSM.FsmVariables.FindFsmGameObject("plank").Value = plank;
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

        LevelManager.ToggleInput(true);
        if (_wayPoint != null) {
            if (secondary == null)
                _wayPoint.MarkActivated();
            else {
                _wayPoint.activateable = secondary;
                secondary.SetWaypoint(_wayPoint);
                secondary.SetActivateable(true);
            }
        }

        if (wayPointToConnect != null && pathToConnectTo != null) {
            wayPointToConnect.adjacentNodes.Add(pathToConnectTo);
            if (wayPointToConnect.canPass)
                wayPointToConnect.SpreadActive();
        }
        return true;
    }
}
