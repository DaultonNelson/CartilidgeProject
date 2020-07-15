using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalSceneManager : MonoBehaviour
{
    #region Variables
    /// <summary>
    /// The door that goes to the item room.
    /// </summary>
    public DoorBehavior itemDoor;
    /// <summary>
    /// The door that leads to the finish of the demo.
    /// </summary>
    public DoorBehavior exitDoor;

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

        if (instanceInformation.talkerTreeLayer > 1)
        {
            //Debug.Log("Open Sesame");
            itemDoor.UpdateDoorStatus(true);
        }

        if (instanceInformation.talkerTreeLayer > 2)
        {
            exitDoor.UpdateDoorStatus(true);
        }
    }
}
