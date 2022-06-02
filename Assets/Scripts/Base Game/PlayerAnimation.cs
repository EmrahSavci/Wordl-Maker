using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

}
