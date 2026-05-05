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

    private AudioSource audioSource;

    public bool isGrabbed = false;
    public XRGrabInteractable grabComp;

    void Start()
    {
        // Prends une référence à la composante Nav Mesh Agent.
        agent = GetComponent<NavMeshAgent>();
        joueur = GameObject.Find("Locomotion System");
        controleur = GameObject.Find("Controleur Souris");
        trous = controleur.GetComponent<trou_souris>().trous;
        int randomIndex = Random.Range(0, trous.Length-1);
        objetCible = trous[randomIndex];
        grabComp = GetComponent<XRGrabInteractable>();
        grabComp.selectEntered.AddListener(OnGrab);
        grabComp.selectExited.AddListener(OnRelease);
        // À chaque 2 seconds, la route est recalculée.

        InvokeRepeating("RecalculerRoute", 1f, 2f);
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
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
        agent.SetDestination(objetCible.transform.position);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("attaque_souris"))
        {
            GameObject fromage = other.gameObject.transform.parent.gameObject;
            agent.speed = 0f;
            fromage.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            fromageHealth--;
            if (fromageHealth <= 0)
            {
                Destroy(fromage);
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
            agent.speed = 5f;
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