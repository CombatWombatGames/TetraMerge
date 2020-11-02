using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem
{
    static List<Sequence> sequences = new List<Sequence>();
    public static void RotatePiece(Transform[] transforms, Vector3[] targetPositions)
    {
        float duration = 0.2f;
        for (int i = 0; i < transforms.Length; i++)
        {
            sequences.Add(DOTween.Sequence()
            .Join(transforms[i].DOMove(targetPositions[i], duration))
            .Join(transforms[i].DOBlendableScaleBy(0.1f * Vector3.one, duration / 2).SetLoops(2, LoopType.Yoyo)));
        }
    }
    public static void FinishPieceRotation()
    {
        foreach (var sequence in sequences)
        {
            sequence.Complete();
        }
        sequences.Clear();
    }
}