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

    public static void ShakeField(Transform field, int scale, ParticleSystem dustParticles, ParticleSystem shardsParticles, ParticleSystem leafParticles, ParticleSystem leafParticlesBurst, float delay = 0f)
    {
        DOVirtual.DelayedCall(delay, () =>
        {
            if (scale > 16)
            {
                dustParticles.Play();
            }
            if (scale > 9)
            {
                field.DOShakePosition(0.4f, Vector3.one * 0.4f * scale, 2000, 90f, false, false);
                field.DOPunchScale(-Vector3.one * 0.001f * scale, 0.2f, 1000, 1f);
                shardsParticles.Play();
                scale = 12;
            }
            leafParticlesBurst.emission.SetBurst(0, new Burst(0f, scale / 4, 10, 0.01f));
            leafParticlesBurst.Play();
            leafParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            leafParticles.Play();
        });
    }

    public static float FallDelay { get; set; }
    public static void SpawnTile(Image image)
    {
        float scale = 1.2f;
        image.transform.localScale = Vector2.one * scale;
        var position = image.GetComponentInParent<Transform>().position;
        image.GetComponentInParent<Transform>().position = image.transform.position * scale;
        image.enabled = true;
        float duration = 0.5f;
        DOTween.Sequence()
            .AppendInterval(FallDelay)
            .Append(image.transform.DOScale(1f, duration).SetEase(Ease.OutBounce))
            .Join(image.transform.DOLocalMove(position, duration).SetEase(Ease.OutBounce))
            //DOTween doesn't return exact position by itself
            .AppendCallback(() => image.GetComponentInParent<Transform>().position = position);
        FallDelay += 0.01f;
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
    public static void ChangeProgress(Slider slider, float value)
    {
        progressSequence?.Complete();
        if (value < slider.value)
        {
            progressSequence = DOTween.Sequence()
            .Join(slider.DOValue(1f, 0.2f))
            .AppendInterval(0.3f)
            .Append(slider.DOValue(0f, 1.6f).SetEase(Ease.Linear));
        }
        else
        {
            slider.DOValue(value, 0.2f);
        }
    }

    public static void ShowMessage(Message message, float duration)
    {
        Sequence showMessage = DOTween.Sequence()
            .Join(message.Text.transform.DOScale(1f, 0.2f).SetLoops(2, LoopType.Yoyo))
            .Join(message.Background.DOFade(0.8f, 0.4f))
            .AppendInterval(duration)
            .Append(message.Text.transform.DOScale(0f, 0.2f))
            .Join(message.Background.DOFade(0f, 0.2f))
            .AppendCallback(() => Object.Destroy(message.gameObject))
            .SetTarget(message);
    }

    public static void HideMessage(Message message)
    {
        message.DOComplete(true);
    }

    public static void GrowVines(Image[] vines)
    {
        float duration;
        float size;
        foreach (var vine in vines)
        {
            duration = Random.Range(10f, 20f);
            size = Random.Range(80f, 100f);
            vine.rectTransform.DOSizeDelta(new Vector2(vine.rectTransform.sizeDelta.x, size), duration).SetEase(Ease.InOutQuad);
        }
    }

    public static void HideVines(Image[] vines)
    {
        float durtion = 0.3f;
        foreach (var vine in vines)
        {
            vine.rectTransform.DOKill();
            vine.rectTransform.DOSizeDelta(new Vector2(vine.rectTransform.sizeDelta.x, 30f), durtion);
        }
        DOVirtual.DelayedCall(durtion + 0.1f, () => GrowVines(vines));
    }

    public static void OpenMenu(GameObject canvas, Image background, Transform panel, Transform ravenEye)
    {
        float duration = 0.4f;
        var position = panel.localPosition;
        panel.localPosition = new Vector2(panel.localPosition.x, panel.localPosition.y + 1000f);
        background.DOFade(0f, 0f);
        canvas.SetActive(true);
        DOTween.Sequence()
            .Join(background.DOFade(0.66f, duration))
            .Join(panel.DOLocalMove(position, duration).SetEase(Ease.OutBack))
            .Join(ravenEye.DOScaleY(0f, duration))
            .Append(ravenEye.DOScaleY(1f, 0.1f));
    }

    public static void RavenBlink(Transform eye)
    {
        eye.DOKill(true);
        eye.DOScaleY(0f, 0.05f).SetLoops(2, LoopType.Yoyo);
    }
}