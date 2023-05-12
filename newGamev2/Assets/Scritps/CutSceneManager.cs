using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour

  
{
    public GameObject spawnPoint;
    public GameObject cutscenePrefab;

    public void TriggerCutscene()
    {
        GameObject cutscene = Instantiate(cutscenePrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        cutscene.SetActive(true);
    }
}

