using UnityEngine;
using System.Collections;

public class CharacterSelect : MonoBehaviour {

    static CharacterSelect select;
    CharacterSelector[] selectors;
    public Sprite availableStar;
    public Sprite unavailableStar;
    public Texture unselectedBorder;
    public Texture selectedBorder;

    void Awake() {
        if (select == null) {
            select = this;
            DontDestroyOnLoad(gameObject);
            gameObject.SetActive(false);
            selectors = GetComponentsInChildren<CharacterSelector>();
            foreach (CharacterSelector cs in selectors)
                cs.GiveHandler(this);
        } else
            Destroy(gameObject);
    }

    void OnEnable() {
        UpdateCharacterSelections();
    }

    public void UpdateCharacterSelections() {
        foreach (CharacterSelector cs in selectors) {
            cs.UpdateSettings();
        }
    }

	public void Hide() {
        GameManager.GetGameManager().ToggleLevelSelect();
        gameObject.SetActive(false);
    }
}
