using UnityEngine;
using System.Collections;

public class LevelAdvancer : BaseActivateable {

    public Level next;
   
    public override bool Activate() {
        if (!base.Activate())
            return false;
        

        if (_wayPoint != null)
            _wayPoint.MarkActivated();
        Destroy(this.gameObject);
        GoNextProto();

        return true;
    }

    public void GoNextProto() {
        GameManager.ChangeLevel(next);
    }
}
