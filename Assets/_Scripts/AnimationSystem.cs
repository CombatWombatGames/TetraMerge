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
            size = Random.Range(60f, 80f);
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

    public static void OpenMenu(Window window, Transform ravenEye)
    {
        float duration = 0.4f;
        var position = window.Panel.localPosition;
        window.Panel.localPosition = new Vector2(window.Panel.localPosition.x, window.Panel.localPosition.y + 1000f);
        window.Background.DOFade(0f, 0f);
        window.Canvas.SetActive(true);
        DOTween.Sequence()
            .Join(window.Background.DOFade(0.66f, duration))
            .Join(window.Panel.DOLocalMove(position, duration).SetEase(Ease.OutBack))
            .Join(ravenEye.DOScaleY(0f, 0f))
            .Append(ravenEye.DOScaleY(1f, 0.1f));
    }

    public static void RavenBlink(Transform eye)
    {
        eye.DOKill(true);
        eye.DOScaleY(0f, 0.07f).SetLoops(2, LoopType.Yoyo);
    }

    public static void OpenScroll(Window window)
    {
        float duration = 0.4f;
        window.Background.DOFade(0f, 0f);
        window.Panel.DOScaleY(0f, 0f);
        window.Canvas.SetActive(true);
        DOTween.Sequence()
            .Join(window.Background.DOFade(0.66f, duration))
            .Join(window.Panel.DOScaleY(1f, 0.2f));
    }

    public static void ShowButtons(ButtonEnhanced leftButton, ButtonEnhanced rightButton)
    {
        float duration = 0.8f;
        var leftPosition = leftButton.transform.localPosition;
        var rightPosition = rightButton.transform.localPosition;
        leftButton.transform.localPosition = new Vector2(leftButton.transform.localPosition.x - 1000f, leftButton.transform.localPosition.y);
        rightButton.transform.localPosition = new Vector2(rightButton.transform.localPosition.x + 1000f, rightButton.transform.localPosition.y);
        leftButton.gameObject.SetActive(true);
        rightButton.gameObject.SetActive(true);
        DOTween.Sequence()
            .Join(leftButton.transform.DOLocalMove(leftPosition, duration).SetEase(Ease.OutBack))
            .Join(rightButton.transform.DOLocalMove(rightPosition, duration).SetEase(Ease.OutBack));
    }

    public static void HideBorder(LineRenderer topLine, LineRenderer middleLine, LineRenderer bottomLine, Vector2 start, Vector2 end, bool merge)
    {
        float duration = merge ? 0.25f : 0.2f;
        float startWidth = 0.06f;
        Color transparent = new Color(0f, 0f, 0f, 0f);
        Color2 white = new Color2(Color.white, Color.white);
        topLine.SetPositions(new Vector3[] { new Vector3 (start.x, start.y), new Vector3(start.x, end.y), new Vector3(end.x, end.y) });
        topLine.startWidth = startWidth;
        topLine.endWidth = startWidth;
        topLine.gameObject.SetActive(true);
        bottomLine.SetPositions(new Vector3[] { new Vector3 (start.x, start.y), new Vector3(end.x, start.y), new Vector3(end.x, end.y) });
        bottomLine.startWidth = startWidth;
        bottomLine.endWidth = startWidth;
        bottomLine.gameObject.SetActive(true);
        if (merge)
        {
            middleLine.SetPositions(new Vector3[] { new Vector3(start.x, start.y), new Vector3(end.x, end.y) });
            middleLine.startWidth = startWidth;
            middleLine.endWidth = startWidth;
            middleLine.gameObject.SetActive(true);
            DOTween.Sequence()
            .Join(middleLine.DOColor(new Color2(Color.white, Color.white), new Color2(transparent, Color.white), duration).SetEase(Ease.Linear))
            .Join(DOTween.To(() => middleLine.startWidth, x => middleLine.startWidth = x, 0f, duration).SetEase(Ease.Linear))
            .Append(DOTween.To(() => middleLine.endWidth, x => middleLine.endWidth = x, 0f, duration).SetEase(Ease.Linear));
        }
        DOTween.Sequence()
            .Join(topLine.DOColor(new Color2(Color.white, Color.white), new Color2(transparent, Color.white), duration).SetEase(Ease.Linear))
            .Join(bottomLine.DOColor(new Color2(Color.white, Color.white), new Color2(transparent, Color.white), duration).SetEase(Ease.Linear))
            .Join(DOTween.To(() => topLine.startWidth, x => topLine.startWidth = x, 0f, duration).SetEase(Ease.Linear))
            .Join(DOTween.To(() => bottomLine.startWidth, x => bottomLine.startWidth = x, 0f, duration).SetEase(Ease.Linear))
            .Append(DOTween.To(() => topLine.endWidth, x => topLine.endWidth = x, 0f, duration).SetEase(Ease.Linear))
            .Join(DOTween.To(() => bottomLine.endWidth, x => bottomLine.endWidth = x, 0f, duration).SetEase(Ease.Linear));
    }

    public static void MoveSelection(Transform selection, Vector3 position)
    {
        float duration = 0.2f;
        selection.DOMove(position, duration);
    }

    public static void Shake(Transform transform)
    {
        float duration = 0.1f;
        float amplitude = 25f;
        DOTween.Sequence()
            .Join(transform.DOBlendableRotateBy(-amplitude * Vector3.forward, duration).SetLoops(2, LoopType.Yoyo))
            .Append(transform.DOBlendableRotateBy(amplitude * Vector3.forward, duration).SetLoops(2, LoopType.Yoyo));
    }

    public static void LoopShake(Transform transform)
    {
        transform.DOKill();
        float duration = 0.2f;
        float amplitude = 10f;
        //transform.DOShakeRotation(duration, 15f * Vector3.forward, 10, 90f, false).SetLoops(-1);
        transform.localScale = 0.8f * Vector3.one;
        DOTween.Sequence()
            .Append(transform.DOScale(1f, duration))
            .Append(transform.DOBlendableRotateBy(-amplitude * Vector3.forward, duration).SetLoops(2, LoopType.Yoyo))
            .Append(transform.DOBlendableRotateBy(amplitude * Vector3.forward, duration).SetLoops(2, LoopType.Yoyo))
            .Append(transform.DOScale(0.8f, duration))
            .SetLoops(-1);
    }
}