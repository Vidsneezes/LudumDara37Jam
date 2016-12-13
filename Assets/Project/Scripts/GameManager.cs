using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public enum GameState
{
    Waiting,
    InGame,
    GameOver
}

public class GameManager : MonoBehaviour {


    public GameObject spawnSpot;
    public GameObject playerSpawnSpot;
    public float baseSpeed;
    
    public float spawnRate;
    public Text scoreDisplay;
    public List<GameObject> backgrounds;
    public AudioSource mainMusic;
    public Image deathScreen;

    private GameState gameState;
    private PageManager pageManager;
    private float timer;
    private float speedMultiplier;
    private int buildingCount;
    private int currentScore;


    public float CurrentSpeed
    {
        get
        {
            return baseSpeed * speedMultiplier;
        }
    }

    public PlayerController Player
    {
        get
        {
            return playerController;
        }
    }
    private PlayerController playerController;

	// Use this for initialization
	private void Start () {
        pageManager = GameObject.FindGameObjectWithTag("PageManager").GetComponent<PageManager>();
        buildingCount = 0;
        gameState = GameState.Waiting; //being not playing
        speedMultiplier = 1;
        timer = 0;
    }
	
	// Update is called once per frame
	private void Update () {
        if (gameState == GameState.Waiting)
        {
            timer += Time.deltaTime;
            if(timer > 0.5f)
            {

                playerController = (PlayerController)Instantiate(Resources.Load<PlayerController>("Player"), playerSpawnSpot.transform.position, Quaternion.identity);
                playerController.onDie += GameOver;
                gameState = GameState.InGame;
            }
        }
        else if (gameState == GameState.InGame)
        {
            timer += Time.deltaTime;// increase to determine when to spawnbuildings
            if (timer > spawnRate)
            {
                SpawnBuilding();
                if (buildingCount > 3)
                {
                    SpawnCollectable();
                    if (Random.Range(0f, 1f) > 0.78f)
                    {
                        SpawnCollectable();
                        buildingCount += 1;
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Time.timeScale = Time.timeScale == 0 ? 1 : 0; // pause logic
            }

            if(buildingCount > 4) // buildings spawned determine what else gets spawned
            {
                SpawnDynamic(); 
            }

            for (int i = 0; i < backgrounds.Count; i++)
            {
                backgrounds[i].transform.position += -Vector3.right * CurrentSpeed * 0.5f * Time.deltaTime;
            }

            if(backgrounds[0].transform.position.x < -26.8f)
            {
                backgrounds[0].transform.position = backgrounds[backgrounds.Count - 1].transform.position + Vector3.right * 26f;
                GameObject temp = backgrounds[0];
                backgrounds.RemoveAt(0);
                backgrounds.Add(temp);
            }

        }
	}

    public void SpawnCollectable()
    {
        Debug.Log("Try Spawn");
        SimpleMovingObject smo = (SimpleMovingObject)Instantiate(Resources.Load<SimpleMovingObject>("Obstacles/Collectables/Collectable"));
        smo.transform.position = new Vector3(spawnSpot.transform.position.x, spawnSpot.transform.position.y + Random.Range(smo.spawnOffset.x, smo.spawnOffset.y), 0); //position is relative to spawnspot with an additional offset
        smo.gameManager = this;
    }

    public void SpawnDynamic()
    {
        if (Random.Range(0f, 1f) > (0.95f - (float)buildingCount / 20))
        {
            int buildingNumber = Random.Range(1, 4);
            SimpleMovingObject smo = (SimpleMovingObject)Instantiate(Resources.Load<SimpleMovingObject>("Obstacles/Dynamic/HeliCroco_"+buildingNumber));
            smo.transform.position = new Vector3(spawnSpot.transform.position.x, spawnSpot.transform.position.y + Random.Range(smo.spawnOffset.x, smo.spawnOffset.y), 0); //position is relative to spawnspot with an additional offset
            smo.gameManager = this;
            buildingCount = 0;
        }
    }

    public void SpawnBuilding()
    {
        int buildingNumber = Random.Range(1, 6);
        SimpleMovingObject smo = (SimpleMovingObject)Instantiate(Resources.Load<SimpleMovingObject>("Obstacles/Buildings/Building_" + buildingNumber));
        smo.transform.position = new Vector3(spawnSpot.transform.position.x, smo.transform.position.y, 0);
        smo.gameManager = this;
        buildingCount += 1;
        timer = 0;
    }

    private void GameOver()
    {
        gameState = GameState.GameOver;
        speedMultiplier = 0;
        StartCoroutine(LoadScoreScreen());
    }

    private IEnumerator LoadScoreScreen()
    {

        PlayerPrefs.SetInt(PageManager.lastScore, currentScore);
        int lastHighScore = PlayerPrefs.GetInt(PageManager.highScore);
        if(currentScore > lastHighScore)
        {
            PlayerPrefs.SetInt(PageManager.highScore, currentScore);
        }
        ResourceRequest rr = Resources.LoadAsync<AudioClip>("Audio/Death");
        mainMusic.Stop();
        while (!rr.isDone)
        {
            Debug.Log("not done");
            yield return new WaitForEndOfFrame();
        }
        mainMusic.clip = (AudioClip)rr.asset;
        yield return new WaitForSeconds(0.2f);
        mainMusic.Play();
        deathScreen.gameObject.SetActive(true);
        mainMusic.loop = false;
        yield return new WaitForSeconds(4f);
        pageManager.LoadScene("ScoreResultScene");
    }

    public void Score(int amount)
    {
        currentScore += amount;
        scoreDisplay.text = currentScore.ToString();
    }
}
