using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelAdvancer : BaseActivateable {

    [SerializeField] GameObject endParticle;
    static LevelAdvancer _levelAdvancer;
    HashSet<WordCard> faceUpCards = new HashSet<WordCard>();

    void Awake() {
        _levelAdvancer = this;
        if (GameManager.GetCompleted() > UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex && model != null) {
            model.gameObject.SetActive(false);
            model = null;
        }
    }

    void Update() {
        if (model != null)
            Floating();
    }

    public override bool Activate() {
        if (faceUpCards.Count > 0) {
            foreach (WordCard wc in faceUpCards)
                wc.Shake();
            return false;
        }
        if (!base.Activate())
            return false;
        
        if (model != null) {
            StartCoroutine(FinishRoutine());
        } else {
            FinishActivation();
        }
        return true;
    }

    void FinishActivation() {
        if (_wayPoint != null)
            _wayPoint.MarkActivated();
        GoNext();
        Destroy(this.gameObject);
    }

    IEnumerator FinishRoutine() {
        AudioSource audio = model.parent.GetComponent<AudioSource>();
        LevelManager.AddTicket();
        model.gameObject.SetActive(false);
        CameraManager.Shake(0.75f, 1.0f);
        yield return new WaitForSeconds(1.0f);
        audio.Play();
        yield return new WaitForSeconds(audio.clip.length + 0.5f);
        FinishActivation();
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
       if (_levelAdvancer.faceUpCards.Count == 0)
            _levelAdvancer.endParticle.SetActive(true);
    }


    public void GoNext() {
        Level next = Level.Menu;
        int check = 1 + (int)LevelManager.GetLevel();
        if (System.Enum.IsDefined(typeof(Level), check))
            next = (Level)check;
        /*(if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 27)
            next = (Level)99;
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 28)
            next = (Level)1;*/

        GameManager.ChangeLevel(next, true);
    }
}
