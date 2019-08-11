using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour {
    
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip startMusic;
    [SerializeField] AudioClip peppyMusic;
    [SerializeField] AudioClip bossMusic;
    private void Awake()
    {
        int numMusicBox = FindObjectsOfType<MusicBox>().Length;
        if(numMusicBox > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    // Use this for initialization
    void Start () {
        GetComponent<AudioSource>().clip = startMusic;
        audioSource.Play();
	}
	
	// Update is called once per frame
	void Update () {
        DebugPlayMusic();
		
	}
    public void PlayStartMusic()
    {
        audioSource.Stop();
        GetComponent<AudioSource>().clip = startMusic;
        
        audioSource.Play();
    }
    public void StopStartMusic()
    {
        
        audioSource.Stop();
    }
    public void PlayPeppyusic()
    {
        GetComponent<AudioSource>().clip = peppyMusic;
        audioSource.volume = .3f;
        audioSource.Play();
    }
    public void PlayBossMusic()
    {
        audioSource.Stop();
        GetComponent<AudioSource>().clip = bossMusic;
        audioSource.volume = .5f;
        audioSource.Play();
    }
    void DebugPlayMusic()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayBossMusic();
        }
    }
   
   
}
