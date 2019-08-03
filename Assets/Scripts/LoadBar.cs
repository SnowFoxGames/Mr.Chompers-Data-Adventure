using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoadBar : MonoBehaviour {

    [SerializeField] Slider loadbar;
    [SerializeField] string levelName;
    [SerializeField] bool playBossMusic;
    bool isloaded;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        loadbar.value += .01f;
        if(loadbar.value >= 1)
        {
            if(isloaded == false)
            {
                isloaded = true;
                StartCoroutine(StartGame());
            }

        }
	}

    IEnumerator StartGame()
    {
        if(playBossMusic)
        {
            FindObjectOfType<MusicBox>().PlayBossMusic();
        }
        else
        {
            FindObjectOfType<MusicBox>().PlayPeppyusic();
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(levelName);
    }


}
