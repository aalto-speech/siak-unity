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
    float _multiplier = 1.0f;
    bool _entrance = true;

    Stack<BaseNode> _pathToPoint = new Stack<BaseNode>();

    void Start() {
        FindPathToWaypoint(current, 2.0f);
    }

    void Update() {
        if (_pathToPoint.Count != 0) {
            MoveToTarget();
        } else if (!_midBounce && !_pause)
            StartCoroutine(BounceModel());
    }

    public void GoToNode(Waypoint point) {
        GoToNode(point, 1);
    }

    public void GoToNode(Waypoint point, float multiplier) {
        model.rotation = Quaternion.LookRotation(transform.position - point.GetWalkPoint());
        _multiplier = multiplier;
        _pathToPoint.Clear();
        _pathToPoint.Push(point);
        current = point;
    }

    public void FindPathToWaypoint(Waypoint target) {
        FindPathToWaypoint(target, 1);
    }

    public void FindPathToWaypoint(Waypoint target, float multiplier) {
        _multiplier = multiplier;
        BaseNode root = (_pathToPoint.Count == 0) ? current as BaseNode : _pathToPoint.Pop();
        _pathToPoint.Clear();

        Dictionary<BaseNode, int> distances = new Dictionary<BaseNode, int>();
        Dictionary<BaseNode, BaseNode> parents = new Dictionary<BaseNode, BaseNode>();
        Queue<BaseNode> bfsQueue = new Queue<BaseNode>();

        distances.Add(root, 0);
        parents.Add(root, root);
        if (target != root)
            bfsQueue.Enqueue(root);

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
        model.rotation = Quaternion.Euler(0, model.rotation.eulerAngles.y, 0);
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
        Quaternion targetRot = Quaternion.LookRotation(transform.position - target,_pathToPoint.Peek().transform.up);
        model.rotation = Quaternion.RotateTowards(model.rotation, targetRot, 420 * Time.deltaTime);
        float motion = speed * _multiplier * Time.deltaTime;
        if (Vector3.Distance(target, transform.position) <= motion) {
            transform.position = target;
            _pathToPoint.Pop();

            if (_pathToPoint.Count == 0) {
                if (returnInteract) {
                    LevelManager.ToggleInput(true);
                    returnInteract = false;
                }
                current.TokenReached();

                if (_entrance) {
                    _entrance = false;
                    CameraManager.Shake(1f, 1.0f);
                }
            }
        } else 
            transform.position = Vector3.MoveTowards(transform.position, target, motion); 
    }

    public void Teleport(Waypoint target) {
        current = target;
        transform.position = target.GetWalkPoint();
        target.MarkActivated();
    }

    public void Pause(bool b) {
        _pause = b;
    }
}
