using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIcheckpoint : MonoBehaviour
{
    [SerializeField] private GameObject[] List;

    public int totalcp(){
        return List.Length;
    }
    public GameObject nextCheckpoint(int pos){
        return List[pos];
    }
}
