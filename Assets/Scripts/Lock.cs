using UnityEngine;
using System.Collections;

public class Lock : BaseActivateable {

    public BaseActivateable secondary;
    PlayMakerFSM _myFSM;

    void Start() {
        _myFSM = GetComponent<PlayMakerFSM>();
        _startPos = transform.position;
    }

    void Update() {
        Floating();
    }

    public override bool Activate() {

        if (!base.Activate()) {
            return false;
        }
        
        if (!LevelManager.HasKey()) {
            _active = false;
            LevelManager.GetLevelManager().keyGUI.Shake(0.75f);
            return false;
        }
        
        LevelManager.ToggleInput(false);
        StartCoroutine(GoToPosition(true, CameraManager.GetRightLocation(), transform, model));

        return true;
    }

    protected override void EndReached(bool toLocation) {
        if (toLocation) {
            _myFSM.FsmVariables.FindFsmGameObject("keyPos").Value = CameraManager.GetLeftLocation().gameObject; 
            _myFSM.SendEvent("StartMinigame");
        } else
            return;
    }

    public void EndMinigame() {
        GetComponent<Rigidbody>().isKinematic = false;
        LevelManager.UseKey();
        Deactivate();
        Destroy(this.gameObject, 3.0f);
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
                LevelManager.GetPlayerToken().FindPathToWaypoint(_wayPoint);
            }
        }
        return true;
    }
}
