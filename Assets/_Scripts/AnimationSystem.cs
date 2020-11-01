using DG.Tweening;
using UnityEngine;

public class AnimationSystem
{
    public static void RotatePiece(Transform[] transforms, Vector3[] targetPositions)
    {
        float duration = 0.2f;
        for (int i = 0; i < transforms.Length; i++)
        {
            DOTween.Sequence()
            .Join(transforms[i].DOMove(targetPositions[i], duration))
            .Join(transforms[i].DOBlendableScaleBy(0.1f * Vector3.one, duration / 2).SetLoops(2, LoopType.Yoyo));
        }
    }
}