using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ControlBot : MonoBehaviour
{
    public int hp;
    public GameObject jugador;
    public int rapidez;
    public controlJuego controlJuego;
    public Transform target;
    [SerializeField] int dañoPlayer;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] float timeFind;
    public Transform particuleDead;

    void Start()
    {
        hp = 100;
        controlJuego = GameObject.Find("GameManager").GetComponent<controlJuego>();
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating("FindPlayer", timeFind, timeFind);
        jugador = GameObject.Find("Player");
        target = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Transform>();
    }
    public void recibirDaño()
    {
        hp -= dañoPlayer; 
        if (hp <= 0)
        {
            Muerte();

        }


    }
    private void Update()
    {       
        //transform.LookAt(jugador.transform);
    }

    private void OnCollisionEnter(Collision collision) 
    { 
        if (collision.gameObject.CompareTag("Bala"))
        {
            recibirDaño();              
        }
        //if (collision.gameObject.CompareTag("RestartCollider"))
        //{
        //    Destroy(gameObject);
        //}
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("ff");
            Muerte();
        }
    }
    void FindPlayer()
    {
        agent.destination = jugador.transform.position;

    }
    void Muerte()
    {
        Instantiate(particuleDead, transform.position, transform.rotation);
        controlJuego.EnemyDestroy();
        Destroy(gameObject);
    }
}

