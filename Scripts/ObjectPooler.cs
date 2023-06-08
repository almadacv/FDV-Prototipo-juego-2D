using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{

    public GameObject projectileObject;
    public List<GameObject> projectileList;
    public int poolSize = 5;

    void Start()
    {
        AddProjectilesToPool(poolSize);
    }

    private void AddProjectilesToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject projectile = Instantiate(projectileObject);
            projectile.SetActive(false);
            projectileList.Add(projectile);
            projectile.transform.parent = transform;
        }
    }

    public GameObject RequestProjectile()
    {
        for (int i = 0; i < projectileList.Count; i++)
        {
            if (!projectileList[i].activeSelf)
            {
                projectileList[i].SetActive(true);
                return projectileList[i];
            }
        }
        AddProjectilesToPool(1);
        projectileList[projectileList.Count - 1].SetActive(true);
        return projectileList[projectileList.Count - 1];

    }
}
