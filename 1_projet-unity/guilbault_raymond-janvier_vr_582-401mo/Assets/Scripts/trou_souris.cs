using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trou_souris : MonoBehaviour
{
    public GameObject[] trous;
    [SerializeField] GameObject prefabSouris;


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("CreerSouris", 1f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreerSouris()
    {
        int prob = Random.Range(0, 99);
        GameObject[] souris = GameObject.FindGameObjectsWithTag("souris");
        if (prob < 80 && souris.Length < 2)
        {
            int randomIndex = Random.Range(0, trous.Length-1);
            GameObject newMouse = Instantiate(prefabSouris, trous[randomIndex].transform.position, Quaternion.identity);
            newMouse.transform.parent = trous[randomIndex].transform;
        }
    }
}
