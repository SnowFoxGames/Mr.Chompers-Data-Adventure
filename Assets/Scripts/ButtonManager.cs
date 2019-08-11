using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    [SerializeField] GameObject loadingScreen;
    [SerializeField] Button activeButton;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenLoadScreen()
    {
        activeButton.gameObject.SetActive(false);
        loadingScreen.SetActive(true);
    }
    public void ReturnToStart()
    {
        Destroy(FindObjectOfType<MusicBox>().gameObject);
        SceneManager.LoadScene(0);
    }

  
}
