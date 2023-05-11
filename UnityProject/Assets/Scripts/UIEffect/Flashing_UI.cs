using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashing_UI : MonoBehaviour
{
    Sequence sequenceFlashing;
    Image TargetImage;
    public float Time;

    private void Awake()
    {
        TargetImage = gameObject.GetComponent<Image>();
    }
    // Start is called before the first frame update
    void OnEnable()
    {
        Color _color = TargetImage.color;
        _color.a = 1;
        TargetImage.color = _color;

        sequenceFlashing = DOTween.Sequence()
            .SetAutoKill(false)
            .Append(TargetImage.DOFade(0.0f, Time))
            .Append(TargetImage.DOFade(1.0f, Time))
            .SetLoops(-1);
          
    }
}
