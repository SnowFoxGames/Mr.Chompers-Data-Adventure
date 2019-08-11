using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour {
    [SerializeField] TextMeshProUGUI percentTracker;
    [SerializeField] float goalNumber = 8.0f;
    [SerializeField] bool inDataStream = true;
    [SerializeField] string nextSceneName;
    public List<GameObject> lives = new List<GameObject>();
    public float totalPercentage = 0.0f;
    public float percentPerCapita;
    public bool stopSpawning = false;
    bool invulnerable = false;
    float invulnerableTime = 2f;

    [Header("Sounds")]
    [SerializeField] AudioClip damageSound;

    [Header("Tutorial")]
    [SerializeField] GameObject tutorial1;
    [SerializeField] GameObject tutorial2;

    [Header("Win and Lose Text")]
    [SerializeField] GameObject winText;
    [SerializeField] GameObject loseScreen;
    [SerializeField] AudioClip winSound;
    bool soundPlayed = false;
    [Header("Final Boss Stuff")]
    [SerializeField] bool finalFight = false;
    [SerializeField] GameObject boss;
    [SerializeField] GameObject bossDuplicate;
	// Use this for initialization

	void Start () {
        stopSpawning = true;
        Debug.Log(totalPercentage);

        percentPerCapita = 100.0f / goalNumber;
        if(inDataStream)
        {
            StartCoroutine(Tutorial());
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(inDataStream)
        {
            
            CheckPercentage();
        }
        else
        {
            CountFiles();
        }


    }

    private void CheckPercentage()
    {
        if (totalPercentage >= 100)
        {
            totalPercentage = 100;
            if(soundPlayed == false)
            {
                soundPlayed = true;
                AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position, .7f); 
            }

            percentTracker.text = "ACCESS GRANTED!";
            stopSpawning = true;
            DestroyRemainingData();
            StartCoroutine(GoToNextScene());

        }
        else
        {
            percentTracker.text = totalPercentage.ToString("F2") + " %";
        }
    }

    public void IncreasePercentage()
    {
        totalPercentage += percentPerCapita;
        Debug.Log(totalPercentage);
    }

    void DestroyRemainingData()
    {
        Object[] extraData = FindObjectsOfType(typeof(Data));
        foreach (Data data in extraData)
        {
            Destroy(data.gameObject);
        }
    }

    void CountFiles()
    {
        goalNumber = FindObjectsOfType<Files>().Length;
        if(goalNumber <= 0)
        {
            if(finalFight == false)
            {
                ShowWin();
            }
            else
            {
                FindObjectOfType<MusicBox>().PlayPeppyusic();
                GameObject clone = Instantiate(bossDuplicate, boss.transform.position, Quaternion.identity) as GameObject;
                var laserCount = FindObjectsOfType<Laser>();
                foreach (Laser laser in laserCount)
                {
                    Destroy(laser.gameObject);
                }
                Destroy(boss.gameObject);
                finalFight = false;
                CountFiles();
            }
                


        }
    }

    private IEnumerator GoToNextScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(nextSceneName);
        if(nextSceneName == "Boss Screen")
        {
            FindObjectOfType<MusicBox>().PlayBossMusic();
        }
    }
    private IEnumerator Tutorial()
    {
        yield return new WaitForSeconds(1);
        tutorial1.SetActive(true);
        yield return new WaitForSeconds(3);
        tutorial1.SetActive(false);
        yield return new WaitForSeconds(.5f);
        tutorial2.SetActive(true);
        yield return new WaitForSeconds(3);
        tutorial2.SetActive(false);
        yield return new WaitForSeconds(1);
        stopSpawning = false;
    }
    private void ShowWin()
    {
        if (soundPlayed == false)
        {
            soundPlayed = true;
            AudioSource.PlayClipAtPoint(winSound, Camera.main.transform.position, .7f);
        }
        winText.SetActive(true);
        StartCoroutine(GoToNextScene());
    }
    public IEnumerator GameOver()
    {
        yield return new WaitForSeconds(2);
        loseScreen.SetActive(true);
    }
    public void Restart()
    {
        Scene level = SceneManager.GetActiveScene();
        SceneManager.LoadScene(level.buildIndex);
    }

    public void TakeLives()
    {
        if(invulnerable == true)
        {
            return;
        }
        int remainingLives = lives.Count;
        if (remainingLives <= 1)
        {
            DestroyHearts();
            stopSpawning = true;
            FindObjectOfType<Player>().Die();
            Debug.Log("GameOver");
        }
        else
        {
            DestroyHearts();

        }
    }

    private void DestroyHearts()
    {
        AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position, 1f);
        var liveNumber = lives.Count - 1;
        Destroy(lives[liveNumber].gameObject);
        lives.Remove(lives[liveNumber]);
        StartCoroutine(Invulnerable());

    }
    public IEnumerator Invulnerable()
    {
        if(invulnerable == false)
        {
            var player = FindObjectOfType<Player>().GetComponent<SpriteRenderer>();

            invulnerable = true;
            Color hurt = player.color;
            hurt.a = .5f;
            player.color = hurt;
            yield return new WaitForSeconds(invulnerableTime);
            hurt.a = 1f;
            player.color = hurt;
            invulnerable = false;
        }

    }

  

}
