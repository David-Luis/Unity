using UnityEngine;
using DG.Tweening;

public class ShakeScaleAnimationUI : MonoBehaviour
{
    [SerializeField]
    private float duration = 1;

    [SerializeField]
    float strength = 1;

    public void Animate()
    {
        transform.DOComplete();
        transform.DOShakeScale(duration, strength);
    }
}
