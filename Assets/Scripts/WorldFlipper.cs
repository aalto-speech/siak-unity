using UnityEngine;
using System.Collections;

public class WorldFlipper : BaseActivateable {

    public Transform flipper;
    public Waypoint offscreenNode;
    public Waypoint nextNode;
    float _flipDuration = 5.0f;

    public override bool Activate() {
        if (!base.Activate()) {
            return false;
        }

        LevelManager.ToggleInput(false);
        StartCoroutine(WorldFlip());

        return true;
    }

    IEnumerator WorldFlip() {
        PlayerToken token = LevelManager.GetPlayerToken();
        token.GoToNode(offscreenNode,2.0f);
        float dur = Vector3.Distance(token.transform.position,offscreenNode.transform.position)/token.speed;
        yield return new WaitForSeconds(dur);
        nextNode.canPass = true;
        nextNode.SetInteractable(true);
        float l = 0;
        Quaternion from = flipper.rotation;
        Quaternion to = flipper.rotation * Quaternion.Euler(180, 0, 0);
        while (l < 1) {
            l += Time.deltaTime/_flipDuration;
            flipper.rotation = Quaternion.Lerp(from,to,l);
            yield return null;
        }
        token.GoToNode(nextNode);
        dur = Vector3.Distance(token.transform.position, nextNode.transform.position) / token.speed;
        yield return new WaitForSeconds(dur);
        token.FindPathToWaypoint(nextNode);
        Deactivate();
        LevelManager.ToggleInput(true);
    }
}
