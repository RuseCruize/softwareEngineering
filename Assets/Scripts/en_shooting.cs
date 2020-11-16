/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class en_shooting : MonoBehaviour
{

    public GameObject pivot;
    public GameObject guy;
    public float lookSpeed = 50f;
    //public FindClosest nearGuy;

    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        FindClosestEnemy();
        //Vector3 direction = Guy.transform.position - transform.position;
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * lookSpeed);
    }

    void FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Guy closestEnemy = null;
        Guy[] allEnemies = GameObject.FindObjectsOfType<Guy>();
        foreach (Guy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }
        Debug.DrawLine(this.transform.position, closestEnemy.transform.position);
        
        Vector3 direction = closestEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, Time.deltaTime * lookSpeed);

    }
}
*/