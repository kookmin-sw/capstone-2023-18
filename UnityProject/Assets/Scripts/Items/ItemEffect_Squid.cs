using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemEffect_Squid : MonoBehaviour
{
    public Transform[] Effects;
    public Image[] Effects_Images;

    Sequence EffectSequence;

    private void OnEnable()
    {
        EffectSequence = DOTween.Sequence().SetAutoKill(true)
           .Append(Effects[0].DOScale(0f, 0))
           .Join(Effects[1].DOScale(0f, 0f))
           .Join(Effects[2].DOScale(0f, 0f))
           .Join(Effects_Images[0].DOFade(1, 0))
           .Join(Effects_Images[1].DOFade(1, 0))
           .Join(Effects_Images[2].DOFade(1, 0))
           .Append(Effects[0].DOScale(0.8f, 0.5f))
           .Join(Effects[1].DOScale(0.6f, 0.4f))
           .Join(Effects[2].DOScale(0.5f, 0.3f))
           .Insert(2f, Effects_Images[0].DOFade(0, 0.5f))
           .Insert(2f, Effects_Images[1].DOFade(0, 0.5f))
           .Insert(2f, Effects_Images[2].DOFade(0, 0.5f));
    }
}
