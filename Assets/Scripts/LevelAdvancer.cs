using UnityEngine;
using System.Collections;

public class LevelAdvancer : BaseActivateable {
   
    public override bool Activate() {
        if (!base.Activate())
            return false;

        if (_wayPoint != null)
            _wayPoint.MarkActivated();
        GoNext();
        Destroy(this.gameObject);

        return true;
    }

    public void GoNext() {
        Level next = Level.Menu;
        int check = 1 + (int)LevelManager.GetLevel();
        if (System.Enum.IsDefined(typeof(Level), check))
            next = (Level)check;
        GameManager.ChangeLevel(next, true);
    }
}
