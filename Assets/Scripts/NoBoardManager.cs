using UnityEngine;
using System.Collections;

public class NoBoardManager : MonoBehaviour {
    public GameObject finalCard;
    public GameObject noGameHolder;
    public int amountOfCards;
    public BaseActivateable nextLevel;

    static NoBoardManager _ngm;

    float _radius = 7f;
    int _usedCards;

    void Awake() {
        _ngm = this;
        StartCoroutine(CreateCards(amountOfCards));
    }

    public void CreateCards(string input) {
        int amount = 1;
        int.TryParse(input, out amount);
        StartCoroutine(CreateCards(amount));
    }

    IEnumerator CreateCards(int amount) {
        amount = Mathf.Max(1, amount);

        for (int i = 0; i < amount; i++) {
            Transform t = Instantiate(noGameHolder).transform;
            t.rotation = transform.rotation;
            t.transform.position = transform.position + Quaternion.AngleAxis(360.0f * ((amount-i) / (amount*1.0f)), transform.forward) * t.right * _radius;
            t.GetComponent<NoBoardInteractable>().glidingPoint = transform.position + Quaternion.AngleAxis(360.0f * ((amount - i) / (amount * 1.0f)), transform.forward) * t.right * 18f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    public static void UseCard() {
        _ngm._usedCards++;
        if (_ngm._usedCards == _ngm.amountOfCards)
            _ngm.CreateFinal();
        else if (_ngm._usedCards > _ngm.amountOfCards) {
            _ngm.CreateNext();
        }
    }
    
    void CreateNext() {
        Transform t = Instantiate(noGameHolder).transform;
        t.rotation = nextLevel.transform.rotation;
        t.position = nextLevel.transform.position;
        nextLevel.gameObject.SetActive(true);
        t.GetComponent<NoBoardInteractable>().SetActivateable(nextLevel);
        t.GetComponent<NoBoardInteractable>().glidingPoint = nextLevel.transform.position + -t.forward * 12.0f;
    }


    void CreateFinal() {
        Transform t = Instantiate(noGameHolder).transform;
        t.rotation = transform.rotation;
        t.position = transform.position;
        t.GetComponent<NoBoardInteractable>().SetActivateable(Instantiate(finalCard).GetComponent<BaseActivateable>());
        t.GetComponent<NoBoardInteractable>().glidingPoint = transform.position + -t.forward * 12.0f;
    }
}
