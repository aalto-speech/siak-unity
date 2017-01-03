using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseNode : MonoBehaviour {

    float spreadDelay = 0.1f; 
    public List<BaseNode> adjacentNodes;
    public bool canPass;
    public Transform walkPoint;

    public virtual Vector3 GetWalkPoint() {
        Transform t = (walkPoint == null) ? transform : walkPoint;
        return t.position;
    }

    public virtual void SpreadActive() {
        canPass = true;
        StartCoroutine(Spread());
    }

    IEnumerator Spread() {
        yield return new WaitForSeconds(spreadDelay);
        foreach (BaseNode n in adjacentNodes)
            n.SpreadActive();
    }
}