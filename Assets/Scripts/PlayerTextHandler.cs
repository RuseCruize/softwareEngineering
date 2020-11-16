using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTextHandler : MonoBehaviour
{
    [SerializeField] GameObject indicator;
    [SerializeField] MatchManager mm;

    // Start is called before the first frame update
    void Start()
    {
        //text.transform.position = mm.currentGuy.transform.position + new Vector3(0.0f, 2.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        indicator.transform.position = mm.currentGuy.transform.position + new Vector3(0.0f, 2.0f, 0.0f);
    }
}
