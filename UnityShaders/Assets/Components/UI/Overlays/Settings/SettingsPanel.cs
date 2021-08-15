using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanel : MonoBehaviour
{
    [SerializeField] private Animator animator = null;

    private static readonly int Show = Animator.StringToHash("Show");
    private static readonly int Hide = Animator.StringToHash("Hide");

    public void Open()
    {
        animator.SetTrigger(Show);
    }

    public void Close()
    {
        animator.SetTrigger(Hide);
    }
}
