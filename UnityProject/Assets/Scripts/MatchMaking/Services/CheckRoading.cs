using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckRoading : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.SceneManager.OnSceneEvent -= SceneManager_OnSceneEvent;
        NetworkManager.Singleton.SceneManager.OnSceneEvent += SceneManager_OnSceneEvent;
    }

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void SceneManager_OnSceneEvent(SceneEvent sceneEvent)
    {
        Debug.Log(sceneEvent.SceneEventType);
        // Both client and server receive these notifications
        switch (sceneEvent.SceneEventType)
        {
            // Handle server to client Load Notifications
            case SceneEventType.Load:
                {
                    // This event provides you with the associated AsyncOperation
                    // AsyncOperation.progress can be used to determine scene loading progression
                    var asyncOperation = sceneEvent.AsyncOperation;
                    // Since the server "initiates" the event we can simply just check if we are the server here
                    if (IsServer)
                    {
                        var playerObj = NetworkManager.Singleton.SpawnManager.GetPlayerNetworkObject(sceneEvent.ClientId);
                        if (playerObj != null)
                        {
                            playerObj.Despawn(true);
                        }
                    }
                    else
                    {
                    }
                    break;
                }
            // Handle server to client unload notifications
            case SceneEventType.Unload:
                {
                    // You can use the same pattern above under SceneEventType.Load here
                    break;
                }
            // Handle client to server LoadComplete notifications
            case SceneEventType.LoadComplete:
                {
                    
                    // This will let you know when a load is completed
                    // Server Side: receives this notification for both itself and all clients
                    if (IsServer)
                    {
                        switch (SceneManager.GetActiveScene().name)
                        {
                            case "Kookmin_Multi":
                                
                                break;
                        }
                    }
                    else
                    {
                        switch (SceneManager.GetActiveScene().name)
                        {
                            case "Kookmin_Multi":
                                //SpawnPlayerServerRpc(sceneEvent.ClientId);
                                break;
                        }
                    }

                    // So you can use sceneEvent.ClientId to also track when clients are finished loading a scene
                    break;
                }
            // Handle Client to Server Unload Complete Notification(s)
            case SceneEventType.UnloadComplete:
                {
                    // This will let you know when an unload is completed
                    // You can follow the same pattern above as SceneEventType.LoadComplete here

                    // Server Side: receives this notification for both itself and all clients
                    // Client Side: receives this notification for itself

                    // So you can use sceneEvent.ClientId to also track when clients are finished unloading a scene
                    break;
                }
            // Handle Server to Client Load Complete (all clients finished loading notification)
            case SceneEventType.LoadEventCompleted:
                {
                    // This will let you know when all clients have finished loading a scene
                    // Received on both server and clients
                    switch (SceneManager.GetActiveScene().name)
                    {
                        case "Kookmin_Multi":
                            GameManager.Sound.BGMPlay(GameManager.Sound.BGMList[(int)BGMLIST.KOOKMIN]);
                            break;
                        case "Kookmin":
                            GameManager.Sound.BGMPlay(GameManager.Sound.BGMList[(int)BGMLIST.KOOKMIN]);
                            break;
                    }

                    // Example of parsing through the clients that completed list
                    if (IsServer)
                        {
                            Debug.Log("All Clients LOADED!");
                        NetPlayManager npm;
                            switch (SceneManager.GetActiveScene().name)
                            {
                                case "Kookmin_Multi":
                                npm = GameObject.Find("@PlayManager").GetComponent<NetPlayManager>();
                                    StartCoroutine(npm.StartCountDown());
                                    break;
                                case "Kookmin":
                                npm = GameObject.Find("@PlayManager").GetComponent<NetPlayManager>();
                                    StartCoroutine(npm.StartCountDown());
                                    break;
                        }

                            // Handle any server-side tasks here
                        }
                        else
                        {
                            // Handle any client-side tasks here
                        }
                    }
                    break;
            // Handle Server to Client unload Complete (all clients finished unloading notification)
            case SceneEventType.UnloadEventCompleted:
                {
                    // This will let you know when all clients have finished unloading a scene
                    // Received on both server and clients
                    foreach (var clientId in sceneEvent.ClientsThatCompleted)
                    {
                        // Example of parsing through the clients that completed list
                        if (IsServer)
                        {
                            

                            // Handle any server-side tasks here
                        }
                        else
                        {
                            // Handle any client-side tasks here
                        }
                    }
                    break;
                }
        }
    }
}
