using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerToken : MonoBehaviour {

    public Waypoint current;
    public float speed;
    public float bouncesPerSecond;
    public float bounceHeight;
    public Transform model;

    [HideInInspector]
    public bool returnInteract;
    bool _pause;
    bool _midBounce;

    Stack<BaseNode> _pathToPoint = new Stack<BaseNode>();

    void Update() {
        if (_pathToPoint.Count != 0) {

            MoveToTarget();
        } else if (!_midBounce && !_pause)
            StartCoroutine(BounceModel());
    }

    public void GoToNode(Waypoint point) {
        _pathToPoint.Clear();
        _pathToPoint.Push(point);
        current = point;
    }

    public void FindPathToWaypoint(Waypoint target) {
        BaseNode root = (_pathToPoint.Count == 0) ? current as BaseNode : _pathToPoint.Pop();
        _pathToPoint.Clear();

        Dictionary<BaseNode, int> distances = new Dictionary<BaseNode, int>();
        Dictionary<BaseNode, BaseNode> parents = new Dictionary<BaseNode, BaseNode>();
        Queue<BaseNode> bfsQueue = new Queue<BaseNode>();

        distances.Add(root, 0);
        bfsQueue.Enqueue(root);
        parents.Add(root, root);

        while (bfsQueue.Count != 0) {
            BaseNode node = bfsQueue.Dequeue();
            BaseNode[] adjacents = node.adjacentNodes.ToArray();

            for (int i = 0; i < adjacents.Length; i++) {
                if (!parents.ContainsKey(adjacents[i])) {
                    parents.Add(adjacents[i], node);

                    if (adjacents[i] == target as BaseNode) {
                        bfsQueue.Clear();
                        break;
                    }

                    if (adjacents[i].canPass) {
                        distances.Add(adjacents[i], distances[node] + 1);
                        bfsQueue.Enqueue(adjacents[i]);
                    }
                }
            }
        }


        BaseNode next = target;
        if (parents.ContainsKey(target)) {
            current = target;
            while (parents.ContainsKey(next)) {
                _pathToPoint.Push(next);
                if (next != parents[next])
                    next = parents[next];
                else
                    parents.Clear();
            }
        }
    }

    IEnumerator BounceModel() {
        _midBounce = true;
        float PICollector = 0;
        while (PICollector <= 1) {
            PICollector += bouncesPerSecond * Time.deltaTime;
            model.localPosition = Vector3.up * bounceHeight * Mathf.Sin(Mathf.PI * PICollector);
            yield return new WaitForEndOfFrame();
        }
        model.localPosition = Vector3.zero;
        _midBounce = false;
    }

    void MoveToTarget() {
        Vector3 target = _pathToPoint.Peek().GetWalkPoint();

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(target, transform.position) <= speed * Time.deltaTime) {
            transform.position = target;
            _pathToPoint.Pop();

            if (_pathToPoint.Count == 0) {
                if (returnInteract) {
                    LevelManager.ToggleInput(true);
                    returnInteract = false;
                }
                current.TokenReached();
            }
        }
    }


    public void Pause(bool b) {
        _pause = b;
    }
}
