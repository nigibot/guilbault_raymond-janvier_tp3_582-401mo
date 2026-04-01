using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grab : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    function TriggerTorche(Gameobject torche) {
        torche.SetActive(true);
    }

    function TriggerTorcheFalse(Gameobject torche) {
        torche.SetActive(false);
    }
}
