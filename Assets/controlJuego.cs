using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class controlJuego : MonoBehaviour
{
    public GameObject jugador;
    public GameObject bot;
    public List<GameObject> listaEnemigos;
    float tiempoRestante;
    public Transform spawnerPoint;
    int cantEnemy;
    float moveInY;
    public Text timerText;
    public Text enemyCountText;
    [Header("Canva")]
    public Text gameOverWin;
    public Text restartGameText;
    public List<GameObject> keys;

    [Header("FirstKey")]
    public List<Transform> keysPositions;
    bool firstKeyScene;
    public bool firstKey;
    public Animator puertaAnimator;
    public float rateEnemy;
    [SerializeField] bool spawnAble;
    public float rateKeyPosition;
    bool changeAble;

    [Header("SecondKey")]
    public List<Transform> keysPositionsSecond;
    public bool secondKey;
    bool cronometro;
    public Collider startSecond;
    bool changeAbleSecond;
    public Animator secondDoorAnimator;

    bool gameOver;

    //public float rangeX;
    //    public float rangeY;
    //public Collider changeCollider;
    public int currentsEnemy;
    public ControlBot controlBot;
    // public GameObject[] floors;
    int floorToDestroy;
    public List<GameObject> enemyList;


    void Start()
    {
        Time.timeScale = 1;
        gameOverWin.enabled = false;
        restartGameText.enabled = false;
        timerText.enabled = false;
        spawnAble = true;
        changeAble = true;
        changeAbleSecond = true;
    }
    void Update()
    {



        if (!firstKey)
        {
            FirstKey();
        }
        if (secondKey)
        {
            firstKey = true;
            if (!cronometro)
            {
                StartCoroutine(ComenzarCronometro());
                cronometro = true;
                timerText.enabled = true;
                timerText.text = "Escapa en: "+ tiempoRestante.ToString("F0")+ " segundos";
            }

            SecondKey();

        }
        if(gameOver && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
    }

    public void EnemyDestroy()
    {
        currentsEnemy--;
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(rateEnemy);

        Instantiate(bot, spawnerPoint.transform.position, bot.transform.rotation);
        currentsEnemy++;
        enemyCountText.text = "Enemigos: " + currentsEnemy.ToString("F0");
        spawnAble = true;
    }
    IEnumerator KeyChangePosition()
    {
        yield return new WaitForSeconds(rateKeyPosition);
        int randomPosition = Random.Range(0, keysPositions.Count);
        keys[0].transform.position = keysPositions[randomPosition].position;
        changeAble = true;
    }

    void FirstKey()
    {
        if (changeAble)
        {
            changeAble = false;
            StartCoroutine(KeyChangePosition());
        }
        if (spawnAble)
        {
            spawnAble = false;
            StartCoroutine(SpawnEnemies());
        }
    }
    
    //public void LevelFloor()
    //{
    //    moveInY += 4;
    //    cantEnemy -= 2;
    //    jugador.transform.position = new Vector3(7, transform.position.y + moveInY, 9);
    //    controlBot.rapidez += 2;
    //    for (int i = 0; i < cantEnemy; i++)
    //    {
    //        Vector3 spawnerRange = new Vector3(Random.Range(rangeX, -rangeX), jugador.transform.position.y, Random.Range(rangeY, -rangeY));
    //        listaEnemigos.Add(Instantiate(bot, spawnerRange, Quaternion.identity));
    //    }
    //    currentsEnemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
    //    Destroy(floors[floorToDestroy]);
    //    floorToDestroy++;
    //    //changeCollider.enabled = false;
    //}

    public void StartSecondKey()
    {
        puertaAnimator.SetBool("OpenDoor", false);
        puertaAnimator.SetBool("ClosedDoor", true);
        enemyCountText.enabled = false;
        secondKey = true;
    }

    public void EndFirstKey()
    {
        firstKey = true;
        EnemiesDestroy();
        puertaAnimator.SetBool("OpenDoor", true);
    }
    void EnemiesDestroy()
    {
        enemyList.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        foreach (var enemy in enemyList)
        {
            Destroy(enemy.gameObject);
        }
        Debug.Log("FF");
    }

    public IEnumerator ComenzarCronometro(float valorCronometro = 60)
    {
        tiempoRestante = valorCronometro;
        while (tiempoRestante > 0)
        {
            yield return new WaitForSeconds(1.0f);
            tiempoRestante--;
            timerText.text = "Escapa en: " + tiempoRestante.ToString("F0") + " segundos";
        }
        if (tiempoRestante <= 0)
        {
            GameOver();
        }
    }
    IEnumerator KeySecondChangePosition()
    {
        yield return new WaitForSeconds(rateKeyPosition);
        int randomPosition = Random.Range(0, keysPositionsSecond.Count);
        keys[1].transform.position = keysPositionsSecond[randomPosition].position;
        changeAbleSecond = true;
    }
    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOver = true;
        gameOverWin.color = Color.red;
        gameOverWin.text = "Quedas fuera";
        gameOverWin.enabled = true;
        restartGameText.enabled = true;
    }

    public void Win()
    {
        gameOverWin.enabled = true;
        gameOverWin.color = Color.green;
        gameOverWin.text = "Pasante";
        restartGameText.enabled = true;
    }
    public void SecondKey()
    {
        if (changeAbleSecond)
        {
            changeAbleSecond = false;
            StartCoroutine(KeySecondChangePosition());
        }
     
    }
    public void WinSecondKey()
    {
        secondKey = false;
        secondDoorAnimator.SetBool("OpenDoor", true);
    }
    public void ClosedDoor()
    {
        secondDoorAnimator.SetBool("OpenDoor", false);
        secondDoorAnimator.SetBool("ClosedDoor", true);
    }
}