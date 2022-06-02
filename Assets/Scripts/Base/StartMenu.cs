
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class StartMenu : BaseMenu
{
    public TextMeshProUGUI StartText;
    protected override void Awake()
    {
        StartText = GetComponentInChildren<TextMeshProUGUI>();

        base.Awake();
    }

    private void Start()
    {
        StartTextAnim();
    }

    private void OnEnable()
    {
        EventManager.FirstTouch += Hide;
    }

    private void OnDisable()
    {
        EventManager.FirstTouch -= Hide;
    }

    private void StartTextAnim()
    {
        StartText.transform.DOScale(Vector3.one * 0.95F, 0.5F).
        SetLoops(-1, LoopType.Yoyo);
    }


}
