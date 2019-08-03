using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PasswordBox : MonoBehaviour {

    [SerializeField] string gwainPassword;
    [SerializeField] GameObject LoadbarObjectGwain;
    [SerializeField] Button button;
    TMP_InputField inputField;
	// Use this for initialization
	void Start () {
        inputField = GetComponent<TMP_InputField>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckPassword();

	}
    void CheckPassword()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(inputField.text == gwainPassword)
            {
                LoadGwainLevel();
            }
            else
            {
                Debug.Log("Error");
            }
        }
    }
    void LoadGwainLevel()
    {
        
        button.gameObject.SetActive(false);
        LoadbarObjectGwain.SetActive(true);
        gameObject.SetActive(false);
    }
}
