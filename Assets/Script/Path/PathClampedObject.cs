using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using PathCreation;
using UnityEditor;
using UnityEngine;

namespace ExtremeSnowboarding
{
    public class PathClampedObject : MonoBehaviour
    {
        [SerializeField] private Vector2 objectOffset;

        [Button("Clamp Object")]
        private void ClampObject()
        {
            PathClamper clamper = transform.parent.GetComponent<PathClamper>();
            VertexPath path = clamper.PathCreator.path;
            
            Vector3 closestPoint = path.GetClosestPointOnPath(transform.position);
            
            float distance = Vector3.Distance(closestPoint, path.GetPoint(0));
            Quaternion rotation = path.GetRotationAtDistance(distance);
            
            ClampObject(clamper.RealOffset, closestPoint, rotation);
        }
        
        public void ClampObject(Vector2 generalOffset, Vector3 closestPoint, Quaternion rotation)
        {
            Undo.RecordObject(this, "ObjectClamped "+name);
            Vector2 finalOffset = generalOffset + objectOffset;
            transform.position = closestPoint + new Vector3(0, finalOffset.x, finalOffset.y);
            transform.rotation = rotation;
        }
    }
}
