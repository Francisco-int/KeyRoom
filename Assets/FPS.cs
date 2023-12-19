using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
    public float vidaPlayer;
    public Slider barraVida;
    public float rapidezDesplazamiento = 10.0f;
    public Camera camaraPrimeraPersona;
    public GameObject proyectil;
    [SerializeField] Transform spawnBalaPoint;
    controlJuego controlJuego;
    public AudioSource shot;

    void Start()
    {
        controlJuego = GameObject.Find("GameManager").GetComponent<controlJuego>();
        Cursor.lockState = CursorLockMode.Locked;

        barraVida.value = vidaPlayer;
    }

    void Update()
    {
        float movimientoAdelanteAtras = Input.GetAxis("Vertical") * rapidezDesplazamiento;
        float movimientoCostados = Input.GetAxis("Horizontal") * rapidezDesplazamiento;

        movimientoAdelanteAtras *= Time.deltaTime; movimientoCostados *= Time.deltaTime;

        transform.Translate(movimientoCostados, 0, movimientoAdelanteAtras);

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (Input.GetMouseButtonDown(0))
        {
            shot.Play();
            Ray ray = camaraPrimeraPersona.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            GameObject pro;
            pro = Instantiate(proyectil, spawnBalaPoint.position, proyectil.transform.rotation);
            Rigidbody rb = pro.GetComponent<Rigidbody>();
            rb.AddForce(camaraPrimeraPersona.transform.forward * 15, ForceMode.Impulse);
            Destroy(pro, 5);

            if ((Physics.Raycast(ray, out hit) == true) && hit.distance < 5)
            {
                Debug.Log("El rayo tocó al objeto: " + hit.collider.name);
            }
            if (hit.collider.name.Substring(0, 3) == "Bot")
            {
                GameObject objetoTocado = GameObject.Find(hit.transform.name);
                ControlBot scriptObjetoTocado = (ControlBot)objetoTocado.GetComponent(typeof(ControlBot));
                if (scriptObjetoTocado != null)
                {
                    scriptObjetoTocado.recibirDaño();
                }
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.CompareTag("RestartCollider"))
        // {
        //    SceneManager.LoadScene(0);
        //}
        if (collision.gameObject.CompareTag("Enemy"))
        {
            vidaPlayer--;
            barraVida.value = vidaPlayer;
            if(vidaPlayer <= 0)
            {
                controlJuego.GameOver();
            }
        }
        if (collision.gameObject.CompareTag("FirstKey"))
        {
            controlJuego.EndFirstKey();
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("SecondKey"))
        {
            controlJuego.WinSecondKey();
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Win"))
        {
            controlJuego.Win();
            Debug.Log("RRR");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("StartSecond"))
        {
            controlJuego.StartSecondKey();
        }
        if (other.gameObject.CompareTag("EndSecond"))
        {
            controlJuego.ClosedDoor();
        }
    }
}
