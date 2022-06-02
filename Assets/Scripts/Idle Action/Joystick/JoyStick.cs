using UnityEngine;
using UnityEngine.EventSystems;
using Lean.Touch;
using Lean.Common;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.AspectRatioFitter))]
public class JoyStick : MonoBehaviour
{
    public RectTransform Background;
    private Image[] _images;
    public RectTransform Knob;
    [Header("Input Values")]
    public float Horizontal = 0;
    public float Vertical = 0;
    public float offset;
    Vector2 PointPosition;
    public static JoyStick Instance;

    private void Awake()
    {
        Instance = this;
        _images = GetComponentsInChildren<Image>();
    }

    public void OnDrag(Vector2 eventData)
    {
        PointPosition = new Vector2((eventData.x - Background.position.x) / ((Background.rect.size.x - Knob.rect.size.x) / 2), (eventData.y - Background.position.y) / ((Background.rect.size.y - Knob.rect.size.y) / 2));

        PointPosition = (PointPosition.magnitude > 1.0f) ? PointPosition.normalized : PointPosition;

        Knob.transform.position = new Vector2((PointPosition.x * ((Background.rect.size.x - Knob.rect.size.x) / 2) * offset) + Background.position.x, (PointPosition.y * ((Background.rect.size.y - Knob.rect.size.y) / 2) * offset) + Background.position.y);

        Horizontal = PointPosition.x;
        Vertical = PointPosition.y;
    }

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUp += OnFingerUp;
        LeanTouch.OnFingerUpdate += OnFingerUpdate;
    }

    internal Vector3 GetVector()
    {
       return new Vector3(Horizontal, 0, Vertical);
    }

    private void OnFingerUpdate(LeanFinger obj)
    {
        OnDrag(obj.ScreenPosition);
    }

    private void OnFingerUp(LeanFinger obj)
    {
        foreach (var item in _images)
        {
            item.enabled = false;
        }
        Horizontal = 0;
        Vertical = 0;
    }


    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUp -= OnFingerUp;
        LeanTouch.OnFingerUpdate -= OnFingerUpdate;
    }

    private void OnFingerDown(LeanFinger obj)
    {
        if(obj.IsOverGui) return;
        
        foreach (var item in _images)
        {
            item.enabled = true;
        }

        transform.position = obj.ScreenPosition;
    }

}