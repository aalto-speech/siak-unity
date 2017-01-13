using UnityEngine;
using System.Collections;

public class WordGlue : MonoBehaviour {

    public Texture picture;
    public AudioClip foreignClip;
    [Range(0,1)]
    public float foreignVolume = 1.0f;
    public AudioClip localClip;
    [Range(0, 1)]
    public float localVolume = 1.0f;
}