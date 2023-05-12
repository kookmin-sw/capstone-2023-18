using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerItem : MonoBehaviour
{
    public PresetItem[] Karts;
    public PresetItem[] Characters;

    public int MaxKart;
    public int MaxCharacter;

    public int KartIndex;
    public int CharacterIndex;
    void Start()
    {
        Karts = transform.Find("Kart").gameObject.GetComponentsInChildren<PresetItem>();
        MaxKart = Karts.Length;
        Characters = transform.Find("Player").gameObject.GetComponentsInChildren<PresetItem>();
        MaxCharacter = Characters.Length;

        KartIndex = 0;
        CharacterIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i< MaxKart; i++)
        {
            if (Karts[i].gameObject == null) break;
            Karts[i].gameObject.SetActive(i == KartIndex ? true : false);
        }

        for (int i = 0; i < MaxCharacter; i++)
        {
            if (Characters[i] == null) break;
            Characters[i].gameObject.SetActive(i == CharacterIndex ? true : false);
        }
    }
}
