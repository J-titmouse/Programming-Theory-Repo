using UnityEngine;

public class Jukebox : MonoBehaviour
{
    private AudioSource playerAudio;
    public bool switchTracks = true;
    private int lastTrackNum = 0;
    private int trackNumber = 1;
    [SerializeField] private AudioClip[] track;


    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        NextTrack();
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
    }
    private void NextTrack()
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

    private void ChangeSong(AudioClip upNext)
    {
        playerAudio.Stop();
        playerAudio.clip = upNext;
        playerAudio.Play();
    }
    
}
