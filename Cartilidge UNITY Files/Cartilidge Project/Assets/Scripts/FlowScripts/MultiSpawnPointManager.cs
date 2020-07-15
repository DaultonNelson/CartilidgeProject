using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpawnPointManager : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The player controller, Cartilidge.
    /// </summary>
    public CartilidgeController playerController;
    /// <summary>
    /// A list of spawn points in the scene.
    /// </summary>
    public List<SpawnPointData> spawnPoints = new List<SpawnPointData>();

    /// <summary>
    /// The current game instance's information.
    /// </summary>
    private GameInstanceInformation instanceInformation;
    #endregion

    public void SceneInitialization()
    {
        StartCoroutine(GetInstanceInformation());
    }

    IEnumerator GetInstanceInformation()
    {
        yield return null;
        instanceInformation = FindObjectOfType<GameInstanceInformation>();
        SpawnInPlayer(instanceInformation.exitedSceneName);

    }

    private void SpawnInPlayer(string previousScene)
    {
        Debug.Log(previousScene);
        List<string> bases = new List<string>();
        foreach (SpawnPointData pointData in spawnPoints)
        {
            bases.Add(pointData.sceneBaseName);
        }

        if (bases.Contains(previousScene))
        {
            playerController.gameObject.transform.position = spawnPoints[bases.IndexOf(previousScene)].spawnPointLocation.position;
        }
        else
        {
            Debug.LogWarning("MultiManager did not read valid scene name from Instance", gameObject);
            playerController.gameObject.transform.position = spawnPoints[0].spawnPointLocation.position;
        }
    }
}

[System.Serializable]
public class SpawnPointData 
{
    #region Variables
    /// <summary>
    /// The name of the scene that the player must be coming from to use this spawn point.
    /// </summary>
    public string sceneBaseName;
    /// <summary>
    /// The spawn point the player should spawn at based on the scene base.
    /// </summary>
    public Transform spawnPointLocation;
    #endregion
}