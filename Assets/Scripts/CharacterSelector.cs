using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterSelector : MonoBehaviour {

    CharacterSelect characterSelect;
    Image myImage;

    public int myIndex;
    public int starCount;
    public Image starImage;
    public RawImage borderImage;
    public GameObject lockImage;
    public Sprite availableSprite;
    public Sprite unavailableSprite;

    void Awake() {
        myImage = GetComponent<Image>();
        if (starImage != null)
            starImage.GetComponentInChildren<Text>().text = starCount.ToString();
    }

    public void UpdateSettings() {
        Awake();
        if (GameManager.TotalStars() < starCount) {
            myImage.sprite = unavailableSprite;
            if (starImage != null) {
                starImage.sprite = characterSelect.unavailableStar;
            }
            borderImage.texture = characterSelect.unselectedBorder;
            lockImage.SetActive(true);
            transform.GetChild(0).GetComponent<Text>().text = " ? ? ? ? ";
        } else {
            myImage.sprite = availableSprite;
            if (starImage != null)
                starImage.sprite = characterSelect.availableStar;
            borderImage.texture = (ModelManager.GetIndex() == myIndex) ? characterSelect.selectedBorder : characterSelect.unselectedBorder;
            lockImage.SetActive(false);
            transform.GetChild(0).GetComponent<Text>().text = name;
        }
    }

    public void GiveHandler(CharacterSelect cs) {
        characterSelect = cs;
    }

    public void Select() {
        if (GameManager.TotalStars() < starCount)
            return;
        ModelManager.ChangeIndex(myIndex);
        characterSelect.UpdateCharacterSelections();
    }
}
