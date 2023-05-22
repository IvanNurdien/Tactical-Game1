using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArea : MonoBehaviour
{
    public List<GameObject> enemiesInRange;
    

    private void OnTriggerEnter(Collider other)
    {
        if (!enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemiesInRange.Contains(other.gameObject))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }
}
