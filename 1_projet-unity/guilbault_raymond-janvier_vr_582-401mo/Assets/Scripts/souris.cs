using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentNavigation : MonoBehaviour
{
    public GameObject objetCible;
    public GameObject controleur;
    public GameObject[] trous;

    NavMeshAgent agent;
    int fromageHealth = 5;

    void Start()
    {
        // Prends une référence à la composante Nav Mesh Agent.
        agent = GetComponent<NavMeshAgent>();
        controleur = GameObject.Find("contrôleur souris");
        trous = controleur.GetComponent<trou_souris>().trous;
        int randomIndex = Random.Range(0, trous.Length-1);
        objetCible = trous[randomIndex];
        // À chaque 2 seconds, la route est recalculée.
        InvokeRepeating("RecalculerRoute", 1f, 2f);
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

        if (other.gameObject.CompareTag(objetCible.tag))
        {
            Destroy(gameObject);
        }
    }
}