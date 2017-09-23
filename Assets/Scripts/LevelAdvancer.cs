using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelAdvancer : BaseActivateable {

    [SerializeField] GameObject endParticle;
    static LevelAdvancer _levelAdvancer;
    HashSet<WordCard> faceUpCards = new HashSet<WordCard>();

    void Awake() {
        _levelAdvancer = this;
    }

    public override bool Activate() {
        if (!base.Activate())
            return false;
        
        if (faceUpCards.Count > 0) {
            foreach (WordCard wc in faceUpCards)
                wc.Shake();
            return false;
        }

        if (_wayPoint != null)
            _wayPoint.MarkActivated();
        GoNext();
        Destroy(this.gameObject);

        return true;
    }

    public static void AddFaceUp(WordCard wc) {
        if (_levelAdvancer == null)
            return;
        _levelAdvancer.faceUpCards.Add(wc);
    }

    public static void RemoveFaceUp(WordCard wc) {
        if (_levelAdvancer == null || !_levelAdvancer.faceUpCards.Contains(wc))
            return;
        _levelAdvancer.faceUpCards.Remove(wc);
       // if (_levelAdvancer.faceUpCards.Count == 0)
            _levelAdvancer.endParticle.SetActive(true);
    }


    public void GoNext() {
        Level next = Level.Menu;
        int check = 1 + (int)LevelManager.GetLevel();
        if (System.Enum.IsDefined(typeof(Level), check))
            next = (Level)check;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 28)
            next = (Level)1;
        GameManager.ChangeLevel(next, true);
    }
}
