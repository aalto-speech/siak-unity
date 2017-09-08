using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModelManager : MonoBehaviour {

    static ModelManager _mm;
    public int chosenModelIndex;
    [SerializeField] List<GameObject> modelPrefabs;

    void Awake() {
        if (_mm != null) {
            Destroy(this.gameObject);
            return;
        }

        _mm = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public static void CreateModel(Transform target) {
        Transform model = Instantiate(_mm.modelPrefabs[_mm.chosenModelIndex]).transform;
        for(int i = target.childCount; i > 0; --i) {
            Destroy(target.GetChild(i - 1).gameObject);
        }
        model.SetParent(target);
        model.rotation = Quaternion.identity;
        model.localPosition = Vector3.zero;
    }
}
