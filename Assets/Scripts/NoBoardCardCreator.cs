using UnityEngine;
using System.Collections;

public class NoBoardCardCreator : MonoBehaviour {

    public GameObject card;
    public float spacing;

    public void CreateCards(string input) {
        int amount = 1;
        int.TryParse(input, out amount);
        float inRow = Mathf.Ceil(Mathf.Sqrt(amount));
        float inColumn = Mathf.Ceil(amount / inRow);
        int x = 0;
        int y = 0;
        float baseX = -(inRow - 1) / 2;
        float baseY = (inColumn - 1) / 2;

        for (int i = 0; i < amount; i++) {
            GameObject go = Instantiate(card);
            go.transform.SetParent(this.transform, true);
            go.transform.localPosition = new Vector3((baseX + x) * spacing, (baseY + y) * spacing, 0);
            x++;
            if (x == inRow) {
                x = 0;
                y--;
            }
        }

        Destroy(this.gameObject);
    }
}
