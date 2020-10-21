using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public GameObject prefabGuy;
    private GameObject Team1;
    private GameObject Team2;
    private int ActiveTeam;

    // Start is called before the first frame update
    void Start()
    {
        Team1 = CreateGuys("Team1", new Vector2(-2, 3));
        Team2 = CreateGuys("Team2", new Vector2(2, 3));
        ActiveTeam = 1;
        Team1.GetComponent<Movement>().SetActive();
        Team2.GetComponent<Movement>().SetInactive();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (ActiveTeam == 1)
            {
                ActiveTeam = 2;
                Team2.GetComponent<Movement>().SetActive();
                Team1.GetComponent<Movement>().SetInactive();
            }
            else
            {
                ActiveTeam = 1;
                Team1.GetComponent<Movement>().SetActive();
                Team2.GetComponent<Movement>().SetInactive();
            }
        }
    }

    GameObject CreateGuys(string TeamTag, Vector2 Location)
    {
        GameObject Guy = Instantiate(prefabGuy, Location, Quaternion.identity);
        Guy.tag = TeamTag;
        return Guy;
    }
}
