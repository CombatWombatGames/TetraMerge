using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoSystem : MonoBehaviour
{
    [SerializeField] VideoPlayer player = default;

    void Start()
    {
        player.Prepare();
    }
}
