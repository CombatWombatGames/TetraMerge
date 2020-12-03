using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

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

    public static void ShakeField(Transform field, int scale, ParticleSystem dustParticles, ParticleSystem shardsParticles, ParticleSystem leafParticles, ParticleSystem leafParticlesBurst)
    {
        if (scale > 16)
        {
            dustParticles.Play();
        }
        if (scale > 9)
        {
            field.DOShakePosition(0.4f, Vector3.one * 0.4f * scale, 2000, 90f, false, false);
            field.DOPunchScale(- Vector3.one * 0.001f * scale, 0.2f, 1000, 1f);
            shardsParticles.Play();
            scale = 12;
        }
        leafParticlesBurst.emission.SetBurst(0, new Burst(0f, scale / 4, 10, 0.01f));
        leafParticlesBurst.Play();
        leafParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        leafParticles.Play();
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

    public static void Glow(Image image)
    {
        float duaration = 3f;
        image.DOFade(0.25f, duaration).SetEase(Ease.Linear).SetDelay(Random.Range(0f, duaration)).SetLoops(-1, LoopType.Yoyo);
    }
    public static void StopGlow(Image image)
    {
        image.DOKill();
        image.color = new Color(1f, 1f, 1f, 0f);
    }

    static Sequence progressSequence;
    public static void ChangeProgress(Slider slider, float value, Text text)
    {
        progressSequence?.Complete();
        if (value < slider.value)
        {
            progressSequence = DOTween.Sequence()
            .Join(slider.DOValue(1f, 0.2f))
            .AppendInterval(0.3f)
            .Append(slider.DOValue(0f, 1.6f).SetEase(Ease.Linear))
            .Join(text.DOFade(1f, 0.8f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutExpo));
        }
        else
        {
            slider.DOValue(value, 0.2f);
        }
    }
}