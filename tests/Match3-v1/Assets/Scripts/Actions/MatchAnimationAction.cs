using UnityEngine;
using System.Collections.Generic;

using DG.Tweening;

public class MatchAnimationAction : Action
{
    private float MatchAnimationDuration = 0.2f;
    HashSet<GameObject> m_gameObject = new HashSet<GameObject>();

    public MatchAnimationAction(HashSet<GameObject> gameObjects)
    {
        foreach (GameObject gameObject in gameObjects)
        {
            m_gameObject.Add(gameObject);
        }
    }

    public override void OnStart()
    {
        foreach(GameObject gameObject in m_gameObject)
        {
            gameObject.transform.DOScale(new Vector3(1.2f, 1.2f, 1.0f), MatchAnimationDuration);
        }

        DOTween.Sequence().AppendInterval(MatchAnimationDuration).AppendCallback(Complete);
    }
}
