using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class Fader : MonoBehaviour
{
    public LayerMask FaderMaks;
    public float FadeTime = 1f;
    public float FadeAmount = 0.25f;
    private List<Renderer> renders = new List<Renderer>();
    private void OnTriggerEnter(Collider other)
    {
        if (ExtensionMethods.CheckLayer(other.gameObject, FaderMaks))
        {
            var renderer = other.gameObject.GetComponent<Renderer>();
            {
                Debug.Log("Fade Starting For " + other.name);

                var renderx = other.GetComponentsInChildren<Renderer>();
                renders.Clear();
                foreach (var item in renderx)
                {
                    if (item as MeshRenderer)
                    {
                        var text = item.GetComponent<TextMeshPro>();
                        if (text)
                        {
                            text.DOFade(0, FadeTime);
                            continue;
                        }
                    }

                    renders.Add(item);
                }

                for (int i = 0; i < renders.Count; i++)
                {
                    renders[i].materials = Changer(renders[i].materials);
                }


            }
        }
    }


    private Material[] Changer(Material[] a)
    {
        for (int i = 0; i < a.Length; i++)
        {
            a[i].DOFade(FadeAmount, FadeTime);
        }

        return a;
    }

}


