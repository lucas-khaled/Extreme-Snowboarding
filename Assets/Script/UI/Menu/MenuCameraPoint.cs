using System;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace ExtremeSnowboarding.Script.UI.Menu
{
    public class MenuCameraPoint : MonoBehaviour
    {
        public CinemachineVirtualCamera virtualCamera;
        
        [SerializeField] [OnValueChanged("ChangedTag")]
        private string pointTag;

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

        public void CreateCamera()
        {
            if (virtualCamera == null)
            {
                GameObject go = new GameObject(name,typeof(CinemachineVirtualCamera));
                virtualCamera = go.GetComponent<CinemachineVirtualCamera>();
                
                go.transform.position = transform.position;
                go.transform.SetParent(transform);

                virtualCamera.Priority = 0;
            }
        }

        private void Awake()
        {
            CreateCamera();
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }

        private void ChangedTag()
        {
            name = pointTag + " Point";

            if (virtualCamera != null)
                virtualCamera.name = pointTag + " virtual camera";
        }
    }
}
