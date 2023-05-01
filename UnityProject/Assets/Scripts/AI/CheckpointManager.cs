using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;

    [SerializeField] private GameObject[] List;

    public static CheckpointManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CheckpointManager>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    
    public int totalCheckPoint()
    {
        return List.Length;
    }

    public GameObject nextcheckPoint(int pos)
    {
        return List[pos];
    }
}




