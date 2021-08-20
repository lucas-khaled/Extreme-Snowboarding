using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtremeSnowboarding.Script.Player;

namespace ExtremeSnowboarding.Script
{
    public class DronePoint : MonoBehaviour
    {

        [SerializeField] private enum ENEMY_BEHAVIOUR { SEEK, PURSUIT };
        [SerializeField] private ENEMY_BEHAVIOUR behaviour = ENEMY_BEHAVIOUR.SEEK;
        float maxSpeed = 5.0f;
        float maxSpeedConstant = 5f;
        float newtonForce = 32.0f;
        float distanceToTargetToReduceSpeed = 2.0f;
        float raycastDistance = 10.0f;
        float raycastAngleVariation = 25.0f;
        Vector3 currentVelocity = Vector3.zero;
        private Player.Player firstPlayer;
        private float minHeight = 5f;

        private Vector3 destinyPosition;
        private float maxBreakSpeed = 0.01f;

        Vector3 targetPositionGlobalToGizmo;

        public void UpdateFirstPlayer(Player.Player player)
        {
            firstPlayer = player;
        }


        private Vector3 Truncate(Vector3 dir, float maxMagnitude)
        {
            Vector3 normalizedVector = dir.normalized;

            if (dir.magnitude <= maxMagnitude)
                return dir;

            return normalizedVector * maxMagnitude;
        }

        private void UpdateVelocity(Vector3 SteeringForce)
        {
            float mass = GetComponent<Rigidbody>().mass;
            Vector3 acceleration = SteeringForce / mass;

            currentVelocity += acceleration * Time.deltaTime;
            currentVelocity = Truncate(currentVelocity, maxSpeed);
        }

        private void UpdatePosition()
        {
            Vector3 newPosition = transform.position + currentVelocity * Time.deltaTime;

            //newPosition.y = 1.0f;

            Vector3 playerPointPosition = firstPlayer.transform.position + new Vector3(3.13f, 1.9f, 3.7f);

            if (maxSpeed <= 100)
                maxSpeed = maxSpeedConstant * Vector3.Distance(transform.position, playerPointPosition);
            else
                maxSpeed = 100;
            
            RaycastHit hit;
            if (Physics.Raycast(newPosition, Vector3.down,out hit, minHeight, LayerMask.GetMask("Track")))
            {
                newPosition.y = hit.point.y + minHeight;
            }

            if (Vector3.Distance(this.transform.position, playerPointPosition) > 1f)
                transform.LookAt(newPosition);
            transform.position = newPosition;
        }

        private void ChooseBestDirection(Vector3 desiredDir, Vector3 raycastDir, ref float bestSolutionValue, ref Vector3 bestDir)
        {
            RaycastHit hit;

            float angle = Mathf.Acos(Vector3.Dot(desiredDir, raycastDir));
            float raycastDistanceReached = raycastDistance;

            if (Physics.Raycast(transform.position, raycastDir, out hit, raycastDistance))
                raycastDistanceReached = Vector3.Distance(transform.position, hit.point);

            float solutionValue = raycastDistanceReached + (1.5f - angle);

            if (solutionValue > bestSolutionValue)
            {
                bestSolutionValue = solutionValue;
                bestDir = raycastDir;
            }
        }

        private Vector3 ChooseBestDirection()
        {
            RaycastHit hit;
            Vector3 desiredDirection;
            Vector3 velocityNormalized = currentVelocity.normalized;

            float distanceToTarget = Vector3.Distance(transform.position, firstPlayer.transform.position);

            Vector3 targetPosition = firstPlayer.transform.position + new Vector3(3.13f, 1.9f, 3.7f);

            targetPositionGlobalToGizmo = targetPosition;

            if (behaviour == ENEMY_BEHAVIOUR.PURSUIT)
            {
                targetPosition += firstPlayer.groundedVelocity.normalized * distanceToTarget;
            }

            desiredDirection = (targetPosition - transform.position).normalized;

            float rayTargetDistance = Vector3.Distance(transform.position, targetPosition);

            rayTargetDistance = (rayTargetDistance < raycastDistance) ? rayTargetDistance : raycastDistance;

            if (!Physics.Raycast(transform.position, desiredDirection, out hit, rayTargetDistance))
                return desiredDirection;

            float bestSolutionValue = Vector3.Distance(transform.position, hit.point);
            Vector3 bestDir = desiredDirection;

            Vector3 leftDiagonal = (Quaternion.AngleAxis(-raycastAngleVariation, Vector3.up) * currentVelocity).normalized;
            Vector3 rightDiagonal = (Quaternion.AngleAxis(raycastAngleVariation, Vector3.up) * currentVelocity).normalized;
            Vector3 left = (Quaternion.AngleAxis(raycastAngleVariation, Vector3.up) * currentVelocity).normalized;
            Vector3 right = (Quaternion.AngleAxis(90.0f, Vector3.up) * currentVelocity).normalized;

            ChooseBestDirection(desiredDirection, velocityNormalized, ref bestSolutionValue, ref bestDir);
            ChooseBestDirection(desiredDirection, leftDiagonal, ref bestSolutionValue, ref bestDir);
            ChooseBestDirection(desiredDirection, left, ref bestSolutionValue, ref bestDir);
            ChooseBestDirection(desiredDirection, right, ref bestSolutionValue, ref bestDir);

            return bestDir;
        }

        private void DoBehaviour()
        {
            float distance = Vector3.Distance(firstPlayer.transform.position, transform.position);

            Vector3 directionToTarget = ChooseBestDirection();

            UpdateVelocity(directionToTarget * newtonForce);

            if (distance <= distanceToTargetToReduceSpeed)
            {
                float newSpeed = (maxBreakSpeed * distance) / distanceToTargetToReduceSpeed;
                newSpeed = (newSpeed > 0.5f) ? newSpeed : 0.5f;

                currentVelocity = currentVelocity.normalized * newSpeed;
            }
            else
                maxBreakSpeed = currentVelocity.magnitude;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Aula2");
            }
        }

        private void Update()
        {
            DoBehaviour();

            UpdatePosition();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(targetPositionGlobalToGizmo, 1f);
        }
    }
}
