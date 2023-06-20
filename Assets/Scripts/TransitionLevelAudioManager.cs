using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionLevelAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }
}
