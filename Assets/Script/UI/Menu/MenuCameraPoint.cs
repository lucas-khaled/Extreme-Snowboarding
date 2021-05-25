using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace ExtremeSnowboarding.Script.UI.Menu
{
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

        private CinemachineVirtualCamera virtualCamera;
        
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

        private void OnEnable()
        {
            Debug.Log("aaaaaaaaaaaa");
            if (virtualCamera == null)
            {
                GameObject go = new GameObject(name,typeof(CinemachineVirtualCamera));
                virtualCamera = go.GetComponent<CinemachineVirtualCamera>();
                go.transform.position = transform.position;
                go.transform.SetParent(FindObjectOfType<MenuCameraPointController>().MixingCamera.transform);
            }
        }

        private void OnDestroy()
        {
            if(virtualCamera != null)
                Destroy(virtualCamera.gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
    }
}
