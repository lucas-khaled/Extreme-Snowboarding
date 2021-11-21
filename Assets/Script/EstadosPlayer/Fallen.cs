using System.Collections;
using ExtremeSnowboarding.Script.Player;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace ExtremeSnowboarding.Script.EstadosPlayer
{
    public class Fallen : PlayerState
    {
        float time = 0;
        float timeFall = 3;
        float timeToCorrect = 0.5f;
        float iterationTime = 0.1f;

        Quaternion newRotation = Quaternion.identity;
        bool canRotate = false;
        float rotationDifference = 0;
        private float tempoQueda;
        private bool isObstacle = false;
        private bool shouldDeaccelerateByRigidBody = false;

        private Rigidbody rb;
        private MMFeedbacks feedbackToPlay;

        public override void StateEnd()
        {
            player.StopAllCoroutines();

            player.SetOnAnimator("fallen", false);
            player.SetOnAnimator("hardFall", false);
        }

        public override void StateStart(Player.Player player)
        {
            base.StateStart(player);
            player.SharedValues.actualState = "Fallen";

            rb = player.GetComponent<Rigidbody>();

            player.SetOnAnimator("fallen", true);
            player.SetOnAnimator("hitByFuckFriend", false);
            player.SetOnAnimator("highSpeed", false);

            if (!isObstacle)
            {
                rb.isKinematic = false;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                player.groundedVelocity = Vector3.zero;
                player.StartCoroutine(CorrectPlayerPosition());
            }
            else 
            {
                tempoQueda = rb.velocity.x;
                if (tempoQueda >= 10f)
                {
                    shouldDeaccelerateByRigidBody = true;
                    string[] animation = { "Caiu-Rolando" };
                    player.ChangeAnimationTo(animation, null, true, 0.05f);
                }
                else
                {
                    shouldDeaccelerateByRigidBody = false;
                    rb.velocity = Vector3.zero;
                    player.groundedVelocity = Vector3.zero;
                    string[] animation = { "Caiu-Comum" };
                    player.ChangeAnimationTo(animation);
                }
            }

            if (feedbackToPlay == null)
            {
                feedbackToPlay = (PlayerPrefs.GetString("Mesh") == "Male")
                    ? player.GetMovimentationFeedbacks().maleNormalFallFeedback
                    : player.GetMovimentationFeedbacks().femaleNormalFallFeedback;
            }
            
            feedbackToPlay.PlayFeedbacks();
            
            player.groundedVelocity = Vector3.zero;

        }
        public override void StateUpdate()
        {
            if (shouldDeaccelerateByRigidBody)
            {
                DeAccelerateByRigidbody(); 
                ClampOnGround();
            }

            if (time <= timeToCorrect && canRotate)
                CorrectPlayerRotation();    
            if (time >= timeFall && !shouldDeaccelerateByRigidBody)
                player.ChangeState(new Grounded(1f));

            time += Time.deltaTime;
        }

        private void DeAccelerateByRigidbody()
        {

            if (rb == null || rb.velocity == Vector3.zero)
                return;

            if (rb.velocity.x > 0)
            {
                Vector3 playerGV = player.groundedVelocity;
                rb.AddForce(tempoQueda * Time.deltaTime * Vector3.left, ForceMode.VelocityChange);
                player.groundedVelocity = new Vector3(rb.velocity.x, playerGV.y, playerGV.z);
            }
            else
            {
                rb.velocity = Vector3.zero;
                player.groundedVelocity = Vector3.zero;
                player.SetOnAnimator("fallen", false);
                player.ChangeState(new Grounded(1.2f,true));
            }

        }
        private void ClampOnGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(player.transform.position, -player.transform.up, out hit, player.SharedValues.CharacterHeight, LayerMask.GetMask("Track")))
            {
                float xChange = rb.velocity.x - hit.normal.normalized.x * 2f;
                float yChange = rb.velocity.y - hit.normal.normalized.y * 2f;

                Quaternion newRotation = Quaternion.FromToRotation(player.transform.up, hit.normal) * player.transform.rotation;
                newRotation.y = newRotation.x = 0;
                player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, newRotation, 100 * Time.deltaTime);

                rb.velocity = new Vector3(xChange, yChange, rb.velocity.z);
            }
        }

        void CorrectPlayerRotation()
        {
            if (rotationDifference == 0)
                rotationDifference = Mathf.Abs((player.transform.eulerAngles.z%360) - newRotation.eulerAngles.z);

            player.transform.rotation = Quaternion.RotateTowards(player.transform.rotation, newRotation, (rotationDifference/timeToCorrect)* Time.deltaTime);
        }

        IEnumerator CorrectPlayerPosition()
        {
            RaycastHit hit;

            if(Physics.Raycast(player.transform.position, Vector3.down, out hit, 10f, LayerMask.GetMask("Track")))
            {
                float X = hit.point.x + hit.normal.x;
                float Y = hit.point.y + (player.SharedValues.CharacterHeight / 2) * hit.normal.y;
                Vector3 newPosition = new Vector3(X, Y, player.transform.position.z);

                newRotation = Quaternion.FromToRotation(player.transform.up, hit.normal) * player.transform.rotation;
                newRotation.y = newRotation.x = 0;
                canRotate = true;

                while (time <= timeToCorrect)
                {
                    player.transform.position = Vector3.Lerp(player.transform.position, newPosition, timeToCorrect);

                    yield return new WaitForSeconds(iterationTime);
                }
            }
        }

        public Fallen(float timeFall, MMFeedbacks feedbackToPlay)
        {
            this.timeFall = timeFall;
            this.feedbackToPlay = feedbackToPlay;
        }

        public Fallen(bool isObstacle = false)
        {
            this.isObstacle = isObstacle;
        }
    }
}
