using UnityEngine;
using System.Collections;

public class LevelSelectCog : MonoBehaviour {

	public void Use() {
        GameManager.GetGameManager().ToggleLevelSelect();
    }
}
