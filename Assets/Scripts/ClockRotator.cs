using UnityEngine;
using System.Collections;

public class ClockRotator : BaseActivateable {

    public Waypoint[] points;
    public Transform[] pointers;
    public float[] rotations;

    public override bool Activate() {

        if (!base.Activate()) {
            return false;
        }


        LevelManager.ToggleInput(false);
        StartCoroutine(RotateClock());

        return true;
    }

    IEnumerator RotateClock() {
        float t = 0;
        Quaternion[] startRots = new Quaternion[pointers.Length];
        for (int i = 0; i < pointers.Length; i++) {
            startRots[i] = pointers[i].rotation;
        }
        while (t < 1) {
            float add = Time.deltaTime / 3.0f;
            t += add;
            for(int i = 0; i < pointers.Length; i++) {
                pointers[i].Rotate(0, rotations[i]*add, 0);
                //pointers[i].rotation = Quaternion.Lerp(startRots[i], startRots[i] * Quaternion.Euler(Vector3.up * rotations[i]),t);
            }
            yield return new WaitForEndOfFrame();
        }
        points[1].adjacentNodes.Clear();
        points[0].adjacentNodes.Add(points[1]);
        points[1].adjacentNodes.Add(points[0]);
        points[1].adjacentNodes.Add(points[2]);
        points[2].adjacentNodes.Add(points[1]);
        points[2].SetInteractable(true);
        Deactivate();
    }

    public override bool Deactivate() {
        if (!base.Deactivate())
            return false;

        LevelManager.ToggleInput(true);
        Destroy(this.gameObject);
        return true;
    }
}
