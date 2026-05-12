using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;

public class AgentNavigation : MonoBehaviour
{
    public GameObject objetCible;
    public GameObject controleur;
    public GameObject[] trous;
    public GameObject joueur;

    public AudioClip sons_arrivee;

    NavMeshAgent agent;
    int fromageHealth = 5;

    private AudioSource audioSource1;
    private AudioSource audioSource2;

    public bool isGrabbed = false;
    public XRGrabInteractable grabComp;

    public bool assomage = false;
    private int assomage_timer = 5;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        joueur = GameObject.Find("Locomotion System");
        controleur = GameObject.Find("Controleur Souris");

        grabComp = GetComponent<XRGrabInteractable>();
        grabComp.selectEntered.AddListener(OnGrab);
        grabComp.selectExited.AddListener(OnRelease);

        // Prends une référence à la composante Nav Mesh Agent.
        trous = controleur.GetComponent<trou_souris>().trous;
        int randomIndex = Random.Range(0, trous.Length-1);
        objetCible = trous[randomIndex];
        
        // À chaque 2 seconds, la route est recalculée.
        InvokeRepeating("RecalculerRoute", 1f, 2f);
        audioSource1 = GetComponent<AudioSource>();
        audioSource2 = GetComponent<AudioSource>();
        audioSource1.Play();
    }

    void Update() {
        if (assomage == true) {
            agent.speed = 0;
            float rotateSpeed = 100.0f;
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            InvokeRepeating("assomageTime", 1f, 1f);
        }
    }

    private void assomageTime()
    {
        assomage_timer--;
        if (assomage_timer <= 0) {
            assomage = false;
            RecalculerRoute();
            agent.speed = 10f;
            assomage_timer = 5;
        }
    }

    private void OnDestroy()
    {
        grabComp.selectEntered.RemoveListener(OnGrab);
        grabComp.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    public void RecalculerRoute()
    {
        // Calcule une nouvelle route et commence à se déplacer
        // vers la position de la destination.
        agent.SetDestination(objetCible.transform.parent.position);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("attaque_souris"))
        {
            GameObject fromage = other.gameObject.transform.parent.gameObject;
            int fromageHealthRn = fromageHealth;
            if (fromage.GetComponent<Rigidbody>().velocity.magnitude <= 0) {
                agent.speed = 0f;
                fromage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                if (fromageHealth == fromageHealthRn) {
                    fromageHealth--;
                }
                if (fromageHealth <= 0)
                {
                    Destroy(fromage);
                    int randomIndex = Random.Range(0, trous.Length-1);
                    objetCible = trous[randomIndex];
                    RecalculerRoute();
                    agent.speed = 10f;
                }
            }
            if (fromage.GetComponent<Rigidbody>().velocity.magnitude > 0) {
                assomage = true;
                assomage_timer = 5;
            }
        }

        if (other.gameObject.CompareTag("assommage"))
        {
            if (other.gameObject.GetComponent<Rigidbody>().velocity.magnitude > 0) {
                assomage = true;
                assomage_timer = 5;
            }
        }

        if (other.gameObject.CompareTag("souris_fromage"))
        {
            GameObject newObjetCible = other.gameObject.transform.parent.gameObject;
            objetCible = newObjetCible;
            RecalculerRoute();
        }

        if (other.gameObject.CompareTag("souris_joueur"))
        {
            int randomIndex = Random.Range(0, trous.Length-1);
            objetCible = trous[randomIndex];
            agent.speed = 10f;
            RecalculerRoute();
        }
        
        if (other.gameObject.CompareTag("bouche") && isGrabbed == true)
        {
            joueur.GetComponent<joueur>().mouseCounter -= 1;
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag(objetCible.tag))
        {
            Destroy(gameObject);
        }
    }
}