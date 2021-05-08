using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Script.UI.Menu
{
    public class MenuCameraPointController : MonoBehaviour
    {
        [SerializeField]
        private MenuCameraPoint[] points;
        [SerializeField]
        private int startIndex = 0;
        [SerializeField]
        private bool isCycle = true;
        [Header("Cinemachine")]
        [SerializeField]
        private CinemachineVirtualCamera menuCamera;
        [SerializeField]
        private CinemachineSmoothPath path;
    

        private MenuCameraPoint actualPoint;
        private int pointIndex;

        private CinemachineTrackedDolly trackedDolly;

        private void Awake()
        {
            SetCinemachineTrack();
        }

        void SetCinemachineTrack()
        {
            path.m_Looped = isCycle;
            path.m_Waypoints = new CinemachineSmoothPath.Waypoint[points.Length];
            int index = 0;

            foreach(MenuCameraPoint point in points)
            {
                path.m_Waypoints[index] = new CinemachineSmoothPath.Waypoint();
                path.m_Waypoints[index].position = point.transform.position;
                index++;
            }

            trackedDolly = menuCamera.GetCinemachineComponent<CinemachineTrackedDolly>();
            trackedDolly.m_PathPosition = pointIndex = startIndex;
        }

        private void Start()
        {
            actualPoint = points[pointIndex];
            actualPoint.Open();
        }

        public void NextPoint()
        {
            if (pointIndex == points.Length - 1)
            {
                if (isCycle)
                    pointIndex = 0;
                else
                    return;
            }
            else
                pointIndex++;

            //StartCoroutine(ChangePoint(actualPoint, points[pointIndex]));
            //StartCoroutine(ChangeRotation(actualPoint, points[pointIndex]));
            StartCoroutine(GoToPoint(actualPoint,points[pointIndex]));
            actualPoint = points[pointIndex]; 
        }

        public void PreviousPoint()
        {
            if (pointIndex == 0)
            {
                if (isCycle)
                    pointIndex = points.Length-1;
                else
                    return;
            }
            else
                pointIndex--;

            // StartCoroutine(ChangePoint(actualPoint, points[pointIndex]));
            //StartCoroutine(ChangeRotation(actualPoint, points[pointIndex]));
            StartCoroutine(GoToPoint(actualPoint, points[pointIndex]));
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

            if(index >= points.Length)
            {
                NextPoint();
                return;
            }

            pointIndex = index;
            StartCoroutine(GoToPoint(actualPoint, points[pointIndex]));
            actualPoint = points[pointIndex];
        }

        IEnumerator GoToPoint(MenuCameraPoint fromPoint, MenuCameraPoint toPoint)
        {
            fromPoint.StartClosing();
            toPoint.StartOpening();
            float actualCinePoint = trackedDolly.m_PathPosition;

            float timeStep = 0.05f;
            float step = (pointIndex - actualCinePoint) * timeStep/toPoint.transitionTime;

            while(Mathf.Abs((float)pointIndex - trackedDolly.m_PathPosition) >0.1f)
            {
                trackedDolly.m_PathPosition += step;
                yield return new WaitForSecondsRealtime(timeStep);
            }

            trackedDolly.m_PathPosition = pointIndex;
            fromPoint.Close();
            toPoint.Open();
        }

        /*IEnumerator ChangeRotation(MenuCameraPoint fromPoint, MenuCameraPoint toPoint)
    {
        float xDifference = (toPoint.transform.eulerAngles.x % 360) - (fromPoint.transform.eulerAngles.x % 360);
        float yDifference = (toPoint.transform.eulerAngles.y % 360) - (fromPoint.transform.eulerAngles.y % 360);
        float zDifference = (toPoint.transform.eulerAngles.z % 360) - (fromPoint.transform.eulerAngles.z % 360);

        float transitionTime = toPoint.transitionTime;
        float timeStep = 0.05f;

        Vector3 rotationStep = new Vector3(xDifference, yDifference , zDifference) * timeStep / transitionTime;

        Debug.Log(xDifference + " - " + yDifference + " - " + zDifference + "\n Step: "+rotationStep);
        
        while (Quaternion.Angle(menuCamera.transform.rotation, toPoint.transform.rotation) > 30)
        {
            menuCamera.transform.rotation = Quaternion.RotateTowards(fromPoint.transform.rotation, toPoint.transform.rotation, 360*timeStep/transitionTime);
            //menuCamera.transform.eulerAngles += rotationStep;
            //menuCamera.transform.Rotate(rotationStep);

            Debug.Log(Quaternion.Angle(menuCamera.transform.rotation, toPoint.transform.rotation));
            yield return new WaitForSecondsRealtime(timeStep);
        }

        menuCamera.transform.rotation = toPoint.transform.rotation;
    }

    IEnumerator ChangePoint(MenuCameraPoint fromPoint, MenuCameraPoint toPoint)
    {
        Vector3 direction = toPoint.transform.position - fromPoint.transform.position;
        Vector3 steps = direction * toPoint.transitionTime/direction.magnitude;  

        while(Vector3.Distance(menuCamera.transform.position, toPoint.transform.position) > steps.magnitude)
        {
            menuCamera.transform.position += steps;

            yield return new WaitForSecondsRealtime(0.05f);
        }

        menuCamera.transform.position = toPoint.transform.position;

        fromPoint.Close();
        toPoint.Open();
    }*/
    }
}
