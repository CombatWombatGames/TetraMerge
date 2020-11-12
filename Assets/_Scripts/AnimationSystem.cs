using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Plays animations
public class AnimationSystem
{
    static List<Sequence> rotatePieceSequences = new List<Sequence>();
    public static void RotatePiece(Transform[] transforms, Vector3[] targetPositions)
    {
        float duration = 0.2f;
        for (int i = 0; i < transforms.Length; i++)
        {
            rotatePieceSequences.Add(DOTween.Sequence()
            .Join(transforms[i].DOMove(targetPositions[i], duration))
            .Join(transforms[i].DOBlendableScaleBy(0.1f * Vector3.one, duration / 2).SetLoops(2, LoopType.Yoyo)));
        }
    }
    public static void FinishPieceRotation()
    {
        foreach (var sequence in rotatePieceSequences)
        {
            sequence.Complete();
        }
        rotatePieceSequences.Clear();
    }

    public static void ShakeField(Transform field, int scale)
    {
        if (scale > 9)
        {
            field.DOShakePosition(0.4f, Vector3.one * 0.4f * scale, 2000, 90f, false, false);
            field.DOPunchScale(- Vector3.one * 0.001f * scale, 0.2f, 1000, 1f);
        }
    }

    public static Sequence DestroyTile(Image image)
    {
        float duration = 0.3f;
        return DOTween.Sequence()
            .Join(image.DOFade(0f, duration))
            .Join(image.transform.DOScale(0f, duration))
            .AppendCallback(() =>
            {
                image.enabled = false;
                image.DOFade(1f, 0f);
                image.transform.DOScale(1f, 0f);
            });
    }
}