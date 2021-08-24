using UnityEngine;

namespace PathCreation.Examples
{
    // Moves along a path at constant speed.
    // Depending on the end of path instruction, will either loop, reverse, or stop at the end of the path.
    public class PathFollower : MonoBehaviour
    {
        public bool auxOnce = false;
        public PathCreator pathCreator;
        public EndOfPathInstruction endOfPathInstruction;
        public float speed = 5;
        public float distanceTravelled;
        public bool shouldFollowPath = true;

        public delegate void OnPathFinished(GameObject gameObject);
        public static OnPathFinished onPathFinished;

        void Start() {
            if (pathCreator != null)
            {
                // Subscribed to the pathUpdated event so that we're notified if the path changes during the game
                pathCreator.pathUpdated += OnPathChanged;
                onPathFinished += OnPathFinishedTest;
            }
        }

        void Update()
        {
            if (pathCreator != null)
            {
                if (shouldFollowPath)
                {
                    distanceTravelled += speed * Time.deltaTime;
                    transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
                    transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);

                    //Debug.Log(distanceTravelled / pathCreator.path.length);

                    if (distanceTravelled / pathCreator.path.length >= 1 && !auxOnce)
                    {
                        if (onPathFinished != null)
                            onPathFinished.Invoke(this.gameObject);
                        auxOnce = true;
                    }
                }
            }
        }

        // If the path changes during the game, update the distance travelled so that the follower's position on the new path
        // is as close as possible to its position on the old path
        void OnPathChanged() {
            distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
        }

        private void OnPathFinishedTest(GameObject gameObject)
        {
            Debug.Log("Path finished " + gameObject.name);
        }
    }
}