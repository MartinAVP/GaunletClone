using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NarratorAudioManager : MonoBehaviour
{
    //Instances (For Singleton)
    public static NarratorAudioManager Instance = null;

    [SerializeField] private List<soundWithID> sounds = new List<soundWithID>();

    public List<int> AudiosQueued;

    private AudioSource audioSource;
    private bool isAnnouncing = false;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void ListIteration()
    {
        if (AudiosQueued.Count != 0)
        {
            if (isAnnouncing == false)
            {
                StartCoroutine(goThroughList());
            }
        }
    }

    private IEnumerator goThroughList()
    {
        isAnnouncing = true;
        yield return new WaitForSeconds(1.4f);

        PlaySound(AudiosQueued[0]);

        isAnnouncing = false;

        AudiosQueued.RemoveAt(0);
        ListIteration();
    }

    public void addSoundQuededByID(int id)
    {
        AudiosQueued.Add(id);

        ListIteration();
    }

    public void PlaySound(int id)
    {
        audioSource.clip = sounds[id].clip;
        audioSource.Play();
    }

    [System.Serializable]
    public struct soundWithID
    {
        public AudioClip clip;
        public string description;
    }
}
