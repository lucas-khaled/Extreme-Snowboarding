using System.Collections;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;

namespace ExtremeSnowboarding
{
#if UNITY_EDITOR
    public class PathClamper : MonoBehaviour
    {
        [SerializeField] private PathCreator pathCreator;
        [SerializeField] [OnValueChanged("ClampChildrenOnPath")] private Vector2 YZ_offset = Vector2.zero;
        [SerializeField] [OnValueChanged("AutomaticSettings")] private bool automaticChanges = false;

        public Vector2 RealOffset => (pathCreator.GetComponent<RoadMeshCreator>() == null) 
            ? YZ_offset 
            : YZ_offset + new Vector2(pathCreator.GetComponent<RoadMeshCreator>().roadWidth, -pathCreator.GetComponent<RoadMeshCreator>().thickness*0.5f);

        public PathCreator PathCreator => pathCreator;
        
        private bool IsNotAutomatic => !automaticChanges;

        [Button("Clamp Children")]
        public void ClampChildrenOnPath()
        {
            if (pathCreator == null)
            {
                Debug.LogError("Please, assign a path");
                return;
            }

            if (transform.childCount == 0)
            {
                Debug.LogWarning("There's no children in this object");
            }

            Vector2 offset = RealOffset;
            VertexPath path = pathCreator.path;
            
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Vector3 closestPoint = path.GetClosestPointOnPath(child.position);

                float distance = Vector3.Distance(closestPoint, path.GetPoint(0));
                Quaternion rotation = path.GetRotationAtDistance(distance);

                PathClampedObject clampedObject;
                if (!child.TryGetComponent(out clampedObject))
                {
                    clampedObject = child.gameObject.AddComponent<PathClampedObject>();
                }
                
                clampedObject.ClampObject(offset, closestPoint, rotation);
            }
        }

        private void AutomaticSettings()
        {
            if (automaticChanges)
                pathCreator.pathUpdated += ClampChildrenOnPath;
            else
                pathCreator.pathUpdated -= ClampChildrenOnPath;
        }
    }
#endif
}
