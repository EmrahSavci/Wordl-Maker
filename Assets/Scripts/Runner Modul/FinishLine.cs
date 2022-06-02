using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using DG.Tweening;
public class FinishLine : Singleton<FinishLine>
{
    public enum FinishDirection
    {
        Z,
        Y
    }

    [SerializeField]
    [Title("Blocks")]
    private float Offset;
    public GameObject Block;
    public int BlockCount;
    [Title("Direction")]
    public FinishDirection Direction;
    [Title("Extra Object")]
    public GameObject ExtraObject;
    private List<GameObject> Blocks = new List<GameObject>();
    private float startPos, finishPos;
    public int sayac;
    private void Start()
    {
        for (int i = 0; i < BlockCount; i++)
        {
            GameObject _block = null;

            if (Direction == FinishDirection.Z)
            {
                _block = Instantiate(Block, transform);
                _block.transform.localPosition = new Vector3(0, 0, Offset * i);
                Blocks.Add(_block);
            }
            else
            {
                _block = Instantiate(Block, transform);
                _block.transform.localPosition = new Vector3(0, Offset * i, 0);
                _block.transform.localEulerAngles = new Vector3(270, 0, 0);
                Blocks.Add(_block);
            }

            _block.transform.GetChild(0).GetComponent<Renderer>().material.color = FinishLineDatas.colors[i];
        }

        var texts = GetComponentsInChildren<TextMeshPro>();

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = FinishLineDatas.multiplier[i];
        }

        if (Direction == FinishDirection.Z)
        {
            startPos = transform.position.z;
            finishPos = transform.position.z + (BlockCount * Offset);
        }
        else
        {
            startPos = transform.position.y;
            finishPos = transform.position.y + (BlockCount * Offset);
        }

    }


    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            Base.ChangeStat(GameStat.FinishLine);
            player.transform.DOMoveX(0, 1F).OnComplete(
                () => player.transform.DOMove(PlayerMove(player.transform.position), sayac * 0.5f).OnComplete(
                    () => Base.FinisGame(GameStat.Win, 1f))
            );
        }
    }

    Vector3 PlayerMove(Vector3 pos)
    {
        float progress = sayac;
        progress = progress.Remap(0, 10, startPos, finishPos);
        pos.x = 0;

        if (Direction == FinishDirection.Z)
        {
            pos.z = progress;
        }
        else
        {
            pos.y = progress;
        }

        return pos;
    }
}
public static class FinishLineDatas
{
    public static Color[] colors = new Color[]
    {
        new Color32(15, 38, 166, 255),
        new Color32(242,114,70,95),
        new Color32(46,75,242,255),
        new Color32(117,242,22,255),
        new Color32(85,166,23,65),
        new Color32(0,239,244,255),
        new Color32(245,24,179,255),
        new Color32(1,245,50,255),
        new Color32(107,24,245,255),
        new Color32(245,144,24,255),
        new Color32(1,245,50,255),
    };

    public static string[] multiplier = new string[]
    {
        "x1.0",
        "x1.1",
        "x1.2",
        "x1.3",
        "x1.4",
        "x1.5",
        "x1.6",
        "x1.7",
        "x1.8",
        "x1.9",
        "x2.0"
    };
}
