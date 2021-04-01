using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuCameraPoint : MonoBehaviour
{
    [SerializeField]
    private string pointTag;
    [Range(0.5f,3f)]
    public float transitionTime = 1;

    [Header("Events")]
    [SerializeField]
    private UnityEvent onPointOpen;

    [SerializeField]
    private UnityEvent onPointClosed;

    [SerializeField]
    private UnityEvent onStartOpening;

    [SerializeField]
    private UnityEvent onStartClosing;

    public void Close()
    {
        if (onPointOpen != null)
            onPointClosed.Invoke();
    }

    public void Open()
    {
        if (onPointClosed != null)
            onPointOpen.Invoke();
    }

    public void StartOpening()
    {
        if (onStartOpening != null)
            onStartOpening.Invoke();
    }

    public void StartClosing()
    {
        if (onStartClosing != null)
            onStartClosing.Invoke();
    }

    public string GetTag()
    {
        return pointTag;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
