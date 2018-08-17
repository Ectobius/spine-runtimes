using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

public class FrameSkippingManager : MonoBehaviour
{
    public float targetFps = 30f;
    public int framesBeforeUpdate = 5;

    private readonly List<SkeletonRenderer> _renderers = new List<SkeletonRenderer>();

    private int _skippingCount = 0;

    private float _accum = 0f;
    private float _framesLeft;
    private float _fps;

    public void RegisterRenderer(SkeletonRenderer renderer)
    {
        if (!_renderers.Contains(renderer))
        {
            _renderers.Add(renderer);
        }
    }

    public void UnregisterRenderer(SkeletonRenderer renderer)
    {
        _renderers.Remove(renderer);
    }

    void Start ()
    {
        _framesLeft = framesBeforeUpdate;
    }

    void LateUpdate()
    {
        AccumFps();

        if (_framesLeft <= 0)
        {
            UpdateFps();

            CalcSkippingRenderers();
        }

        MarkSkippingRenderers();
    }

    private void AccumFps()
    {
        _framesLeft--;
        _accum += 1f / Time.unscaledDeltaTime;
    }

    private void UpdateFps()
    {
        _fps = _accum / framesBeforeUpdate;

        _framesLeft = framesBeforeUpdate;
        _accum = 0.0F;
    }

    private void CalcSkippingRenderers()
    {
        if (_renderers.Count == 0 || Mathf.Abs(targetFps - _fps) < 0.2f)
        {
            return;
        }

        int skippingChange = (int) Mathf.Abs(targetFps - _fps);

        if (_fps < targetFps)
        {
            _skippingCount += skippingChange;
            if (_skippingCount >= _renderers.Count)
            {
                _skippingCount = _renderers.Count;
            }
        }
        else
        {
            _skippingCount -= skippingChange;
            if (_skippingCount < 0)
            {
                _skippingCount = 0;
            }
        }
    }

    private void MarkSkippingRenderers()
    {
        for (int i = 0; i < _renderers.Count; i++)
        {
            _renderers[i].shouldSkipFrame = i < _skippingCount && (Time.frameCount + i) % 2 == 0;
        }
    }
}
