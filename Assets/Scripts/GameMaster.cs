using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using EZCameraShake;

public class GameMaster : MonoBehaviour {

    //Difficulty setting
    public enum Difficulty {Easy, Normal, Hard }
    //Add a manu to change difficulty

    //References
    public static GameMaster gm;        //Game master reference for entire project
    public Transform lemonPrefab;       //Lemon prefab reference
    public Transform obstaclePrefab;    //Obstacle prefab (rock) reference
    public Transform lifeUpPrefab;      //Life Up prefab reference
    public AudioManager audioManager;   //Audio manager reference

    public Camera cam;
    public Transform player;            //Player reference
    public Transform rightWall;
    public Transform leftWall;          //References to side walls
    public Transform ground;            //Ground reference (not sure if needed)

    public TextMeshProUGUI scoreText;      //UI references
    //public Text finalScoreText;
    public TextMeshProUGUI finalScoreText;
    public Image hpImg;

    public Transform pauseMenu;
    public Transform gameOverUI;
    public Transform gameCompletedUI;

    //Insert audio source with some lemonish 8bit music   

    //Variables
    //Walls positioning
    private Vector3 rightWallPosition;  //Positions of walls and ground
    private Vector3 leftWallPosition;
    private Vector3 groundPosition;

    //Spawning
    [SerializeField] private float spawnLocationOffsetX = 20f;      //Offset from screen edges for randomizing all spawn positions (so they don't spawn too close to the walls)   
    public float spawnRate = 1f;                                    //Rate at which the lemons spawn (use in denominator)
    public float obstacleSpawnRate = 1f;                            //Rate at which rocks spawn 
    public float lifeUpSpawnRate = 1f;                              //Rate at which life ups spawn

    private Vector3 spawnPosition = new Vector3();                  //Point at which a lemon will spawn    
    private Vector3 obstacleSpawnPosition = new Vector3();          //Point at which an obstacle will spawn  
    private Vector3 lifeUpSpawnPosition = new Vector3();            //Point at which a life up will spawn

    //Difficulty
    private Difficulty difficulty = Difficulty.Normal;

    //Player health and score
    private int _score = 0;         //Player score - number of lemons caught
    public int score
    {
        get { return _score; }
        set { _score = Mathf.Clamp(value, 0, 100); }
    }
    public static int finalScore;         //Final score achieved   
    private int _hitPoints = 5;   //Health of the player
    public int hitPoints
    {
        get { return _hitPoints; }
        set { _hitPoints = Mathf.Clamp(value, 0, 5); }
    }

    private int damage = 1;                 //Damage dealt by not catching a lemon or catching a rock
    private int healthRegen = 1;            //and the hp received for catching one

    //Game completion
    public static bool gameisOver = false;
    

    //Methods
    private void Awake()
    {
        //Initializing game master
        gm = GameMaster.FindObjectOfType<GameMaster>();
        if (gm == null)
        {
            Debug.LogError("No GameMaster object found!");
        }
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager object found!");
        }
        else
        {
            audioManager = audioManager.GetComponent<AudioManager>();
        }

        if (lemonPrefab == null)
        {
           Debug.LogError("No Lemon Prefab found!");
        }
        //Initializing main camera
        cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("No Main Camera found!");
        }       

        if (SceneManager.GetActiveScene().name == "Level01")
        {
            difficulty = Difficulty.Easy;
        }
        else if(SceneManager.GetActiveScene().name == "Level02")
        {
            difficulty = Difficulty.Normal;
        }
        else if(SceneManager.GetActiveScene().name == "Level03")
        {
            difficulty = Difficulty.Hard;
        }

    }

    private void Start()
    {
        Debug.Log(difficulty);
        gameisOver = false;
        //Place left and right walls at the side edges of the screen, and ground on the bottom edge (+ any offsets if needed)
        leftWallPosition = cam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        leftWall.position = leftWallPosition;

        rightWallPosition = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f));
        rightWall.position = rightWallPosition;

        groundPosition = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2, 0f));
        ground.position = groundPosition;

        //Setting up the spawn position height and correct depth for all spawn points 
        spawnPosition.y = Screen.height + 20;
        spawnPosition.z = transform.position.z;
        obstacleSpawnPosition = spawnPosition;
        lifeUpSpawnPosition = spawnPosition;        

        //Play background music
        
        if(SceneManager.GetActiveScene().name == "Level01")
        {
            FindObjectOfType<AudioManager>().Play("BGMusic", false);
        }
        else if ((SceneManager.GetActiveScene().name == "Level02"))
        {
            FindObjectOfType<AudioManager>().Play("BGMusic2", false);
        }
        else if ((SceneManager.GetActiveScene().name == "Level03"))
        {
            FindObjectOfType<AudioManager>().Play("BGMusic3", false);
        }
        

        //Initiating text variables
        UpdateUI();

        //Starting lemon spawning
        StartCoroutine(SpawnLemons());
        StartCoroutine(SpawnObstacles());
        StartCoroutine(SpawnLifeUps());
    }

    //Refreshes score and hp values
    public void UpdateUI()
    {
        scoreText.text = "WYNIK: " + score +" <sub>/ 100</sub>";
        hpImg.GetComponent<DamageUI>().HPIndicator(hitPoints);
    }

    //Damages player
    public void DamagePlayer()
    {                                
        hitPoints -= damage;        
        audioManager.Play("DamageSound", true);
        cam.GetComponent<CameraShaker>().ShakeOnce(2f, 4f, 0.1f, 1f);        
        UpdateUI();        
    }

    public void LifeUp()
    {
        hitPoints += healthRegen;
        audioManager.Play("LifeUpSound", false);
        UpdateUI();
    }

    //Adds point to score 
    public void LemonCatch()
    {
        score += 2;
        audioManager.Play("CatchSound", true);
        UpdateUI();       
    }

    private float SpawnRate(float rate, Difficulty dif)
    {
        //Formula for spawn rate
        rate = rate + 0.1f * (Mathf.Sqrt((0.01f * rate) / Mathf.Exp(rate)));

        //Clamp spawn rate depending on the difficulty
        if (dif == Difficulty.Easy)
        {
            rate = Mathf.Clamp(rate, 1f, 1.2f);
        }
        else if (dif == Difficulty.Normal)
        {
            rate = Mathf.Clamp(rate, 1.2f, 1.65f);
        }
        else if (dif == Difficulty.Hard)
        {
            rate = Mathf.Clamp(rate, 1.7f, 2.0f);
        }
        else
        {
            Debug.LogError("No difficulty specified!");           
        }
        
        return rate;
    }

    private IEnumerator SpawnLemons()
    {
        yield return new WaitForSeconds(3f);   

        //Start spawning lemons
        while(!gameisOver) 
        {
            //Randomize x position of the lemon and create a vector with its position
            spawnPosition.x = Random.Range(spawnLocationOffsetX, Screen.width - spawnLocationOffsetX);
            Vector3 spawnPoint = new Vector3(cam.ScreenToWorldPoint(spawnPosition).x, cam.ScreenToWorldPoint(spawnPosition).y, spawnPosition.z);
            
            //Spawn a lemon at a spawn point with random rotation
            Instantiate(lemonPrefab, spawnPoint, transform.rotation);
            spawnRate = SpawnRate(spawnRate, difficulty);
                                      
            yield return new WaitForSeconds(1 / spawnRate);
        }        
    }

    private IEnumerator SpawnObstacles()
    {
        yield return new WaitForSeconds(7f);
        
        //Start spawning obstacles
        while (!gameisOver)
        {
            //Randomize x position of the obstacle and create a vector with its position
            obstacleSpawnPosition.x = Random.Range(spawnLocationOffsetX, Screen.width - spawnLocationOffsetX);
            Vector3 obstacleSpawnPoint = new Vector3(cam.ScreenToWorldPoint(obstacleSpawnPosition).x, cam.ScreenToWorldPoint(obstacleSpawnPosition).y, obstacleSpawnPosition.z);
            //Generate a random number, if it's less then 0.3, then spawn obstacle
            float rand = Random.Range(0f, 1f);
            if (rand < 0.4f)
            {
                //Spawn an obstacle at a spawn point with random rotation
                Instantiate(obstaclePrefab, obstacleSpawnPoint, transform.rotation);
            }
            obstacleSpawnRate = SpawnRate(obstacleSpawnRate, difficulty);
            //TODO: Add clamping for spawn rate depending on lvl
            yield return new WaitForSeconds(1 / obstacleSpawnRate);
        }
    }

    private IEnumerator SpawnLifeUps()
    {
        yield return new WaitForSeconds(10f);

        //Start spawning obstacles
        while (!gameisOver)
        {
            //Randomize x position of the life up and create a vector with its position
            lifeUpSpawnPosition.x = Random.Range(spawnLocationOffsetX, Screen.width - spawnLocationOffsetX);
            Vector3 lifeUpSpawnPoint = new Vector3(cam.ScreenToWorldPoint(lifeUpSpawnPosition).x, cam.ScreenToWorldPoint(lifeUpSpawnPosition).y, lifeUpSpawnPosition.z);
            //Generate a random number, if it's less then 0.3, then spawn life up
            float rand = Random.Range(0f, 1f);
            if (rand < 0.33f)
            {
                //Spawn an obstacle at a spawn point with random rotation
                Instantiate(lifeUpPrefab, lifeUpSpawnPoint, transform.rotation);                
            }
            lifeUpSpawnRate = SpawnRate(lifeUpSpawnRate, difficulty);
            //TODO: Add clamping for spawn rate depending on lvl
            yield return new WaitForSeconds(1 / lifeUpSpawnRate);
        }
    }

    private void Update()
    {
        //Game over if player health gets below 0
        if(hitPoints <= 0)
        {
            if(!gameisOver)
            {
                gameisOver = true;
                GameOver();
            }               
        }

        //Win if score is greater or equal than 100
        if (score >= 100)
        {
            if (!gameisOver)
            {
                gameisOver = true;
                GameComplete();
            }            
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            pauseMenu.gameObject.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        //Screen.SetResolution(460, 690, false);
    }

    void CleanScreen()
    {
        //Destroys all remaining lemons, rocks and life ups
        GameObject[] lemons = GameObject.FindGameObjectsWithTag("Lemon");
        foreach (GameObject g in lemons)
        {
            Destroy(g);
        }
        GameObject[] rocks = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (GameObject g in rocks)
        {
            Destroy(g);
        }
        GameObject[] lifeUps = GameObject.FindGameObjectsWithTag("LifeUp");
        foreach (GameObject g in lifeUps)
        {
            Destroy(g);
        }
    }

    //Executed when player dies
    void GameOver()
    {
        Debug.Log("Game over");      
        //Stop spawning lemons
        StopCoroutine(SpawnLemons());

        //Destroy all active lemons and rocks   
        CleanScreen();

        //Hide UI
        scoreText.gameObject.SetActive(false);
        hpImg.gameObject.SetActive(false);

        //Setup and show GameOverUI
        finalScore = score; 
        if (score == 1)
        {
            finalScoreText.text = "ZDOBYŁES <color=#DACB5AFF>" + finalScore + "</color> PUNKT!";
        }
        else if(score > 1 && score <= 4)
        {
            finalScoreText.text = "ZDOBYŁES <color=#DACB5AFF>" + finalScore + "</color> PUNKTY!";
        }
        else
        {
            finalScoreText.text = "ZDOBYŁES <color=#DACB5AFF>"+ finalScore + "</color> PUNKTÓW!";
        }
        gameOverUI.gameObject.SetActive(true);        
    }

    //Executed when player gathers enough points
    void GameComplete()
    {
        Debug.Log("Game complete");        
        StopCoroutine(SpawnLemons());
        CleanScreen();
        scoreText.gameObject.SetActive(false);
        hpImg.gameObject.SetActive(false);
        gameCompletedUI.gameObject.SetActive(true);
    }

    public void Retry()
    {
        gameisOver = false;
        hitPoints = 5;
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //Debug.Log("Retrying");
    }
}
