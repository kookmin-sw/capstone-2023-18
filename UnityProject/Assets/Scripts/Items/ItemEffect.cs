using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class ItemEffect : NetworkBehaviour
{
    public GameObject ShieldEffect;
    public GameObject RushEffect;
    public GameObject ReverseEffect;
    public GameObject BoostEffect;
    public GameObject SlowEffect;
    public GameObject ThunderEffect;
    public GameObject TeamShield;
    
    public enum effectType
    {
        shield,
        rush,
        reverse,
        boost,
        slow,
        Thunder,
        TeamShield,
    }
    

    public void EffectOn(effectType type, float time, ulong uid)
    {
        StartCoroutine(SetEffectActive(type, time));
    }
    
    
    // Update is called once per frame

    IEnumerator SetEffectActive(effectType type, float time)
    {
        GameObject effect = null;
        switch (type)
        {
            case effectType.shield :
                effect = ShieldEffect;
                break;
            case effectType.reverse :
                effect = ReverseEffect;
                break;
            case effectType.rush :
                effect = RushEffect;
                break;
            case effectType.boost :
                effect = BoostEffect;
                break;
            case effectType.slow :
                effect = SlowEffect;
                break;
            case effectType.Thunder :
                effect = ThunderEffect;
                break;
            case effectType.TeamShield :
                effect = TeamShield;
                break;
        }
        
        if (effect != null)
        {
            effect.SetActive(true);
            while (time > 0)
            {
                time -= Time.fixedDeltaTime;
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }
            effect.SetActive(false);
        }
        else yield break;
        
    }
}
