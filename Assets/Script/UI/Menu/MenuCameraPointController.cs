using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace ExtremeSnowboarding.Script.UI.Menu
{
    public class MenuCameraPointController : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain cinemachineBrain;
        
        [BoxGroup("Points Values")]
        [SerializeField] private int startIndex = 0;
        [BoxGroup("Points Values")]
        [SerializeField] private bool isCycle = true;
        [BoxGroup("Points Values")]
        [SerializeField] private List<MenuCameraPoint> points;

        private MenuCameraPoint actualPoint;
        private int pointIndex;

        public void NextPoint(float delayTime = 0)
        {
            if (pointIndex == points.Count - 1)
            {
                if (isCycle)
                    pointIndex = 0;
                else
                    return;
            }
            else
                pointIndex++;
            
            StartCoroutine(GoToPoint(actualPoint,points[pointIndex], delayTime));
            actualPoint = points[pointIndex]; 
        }

        public void PreviousPoint(float delayTime = 0)
        {
            if (pointIndex == 0)
            {
                if (isCycle)
                    pointIndex = points.Count-1;
                else
                    return;
            }
            else
                pointIndex--;
            
            StartCoroutine(GoToPoint(actualPoint, points[pointIndex], delayTime));
            actualPoint = points[pointIndex];
        }

        public void GoToPointByTag(string tag)
        {
            if(tag == string.Empty || tag == null)
            {
                NextPoint();
                return;
            }

            int index = 0;
            foreach(MenuCameraPoint point in points)
            {
                if (point.GetTag() == tag)
                    break;

                index++;
            }

            if(index >= points.Count)
            {
                NextPoint();
                return;
            }

            pointIndex = index;
            StartCoroutine(GoToPoint(actualPoint, points[pointIndex], 0));
            actualPoint = points[pointIndex];
        }

        [Button("Add a new Menu Camera Point")]
        public void AddPoint()
        {
            GameObject go = new GameObject("Point" + (points.Count+1), typeof(MenuCameraPoint));
            MenuCameraPoint newPoint = go.GetComponent<MenuCameraPoint>();
            newPoint.CreateCamera();
            
            go.transform.SetParent(transform);
            
            points.Add(newPoint);
        }

        IEnumerator GoToPoint(MenuCameraPoint fromPoint, MenuCameraPoint toPoint, float delayTime)
        {
            fromPoint.StartClosing();
            toPoint.StartOpening();
            yield return new WaitForSecondsRealtime(delayTime);

            fromPoint.virtualCamera.Priority = 0;
            toPoint.virtualCamera.Priority = 1;

            while (Vector3.Distance(cinemachineBrain.transform.position, toPoint.transform.position) > 2f)
            {
                yield return null;
            }

            fromPoint.Close();
            toPoint.Open();
        }
        
        private void Start()
        {
            pointIndex = startIndex;
            points[pointIndex].virtualCamera.Priority = 1;
            actualPoint = points[pointIndex];
            actualPoint.Open();
        }
    }
}
