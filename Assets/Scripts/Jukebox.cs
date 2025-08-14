using NUnit.Framework.Internal;
using UnityEngine;

public class Jukebox : MonoBehaviour
{
    private AudioSource playerAudio;
    public AudioClip[] track;
    public int trackNumber = 1;
    public bool switchTracks = true;
    private int lastTrackNum = 0;

    //public AudioClip getBiggerSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (trackNumber == 5)
            {
                trackNumber = 1;
            }
            else
            {
                trackNumber++;
            }
            switchTracks = true;
        }
        if (switchTracks && lastTrackNum != trackNumber)
        {
            ChangeSong(track[trackNumber]);
            switchTracks = false;
            lastTrackNum = trackNumber;
        }
        {
            switchTracks = false;
        }
    }
    public void BossFight()
    {
        ChangeSong(track[0]);
        lastTrackNum = 0;
    }

    public void EndGameMusic()
    {
        ChangeSong(track[5]);
        lastTrackNum = 5;
        Debug.Log("here");
    }
    private void ChangeSong(AudioClip upNext)
    {
        playerAudio.Stop();
        playerAudio.clip = upNext;
        playerAudio.Play();
    }
    
}
