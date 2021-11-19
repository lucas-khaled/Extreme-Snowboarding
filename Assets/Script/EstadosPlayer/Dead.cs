using ExtremeSnowboarding.Script.Controllers;
using ExtremeSnowboarding.Script.EventSystem;
using UnityEngine;

namespace ExtremeSnowboarding.Script.EstadosPlayer
{
    public class Dead : PlayerState
    {
        Player.Player playerView;

        public override void StateEnd()
        {
        
        }

        public override void StateStart(Player.Player player)
        {
            base.StateStart(player);

            Rigidbody rb = player.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;

            playerView = player;
            
            player.SharedValues.actualState = "Dead";
            
            ChangePlayerView();

            if (PlayerGeneralEvents.onPlayerDeath != null)
                PlayerGeneralEvents.onPlayerDeath.Invoke(player);

            PlayerGeneralEvents.onPlayerDeath += OnPlayerDeath;
            MonoBehaviour.Destroy(player.GetMeshGameObject()); 
            
            if(PlayerPrefs.GetString("Mesh") == "Male")
                player.GetMovimentationFeedbacks().maleDyingFeedback?.PlayFeedbacks();
            else
                player.GetMovimentationFeedbacks().femaleDyingFeedback?.PlayFeedbacks();
        }

        public override void StateUpdate()
        {
        
        }

        ///<summary> 
        ///Change the player camera visualization to a random alive player (doesn't work if there's no players alive)
        ///</summary>
        public void ChangePlayerView()
        {
            playerView = CorridaController.instance.GetOtherPlayerThan(playerView); 
            player.playerCamera.SetPlayer(playerView);
        }

        ///<summary>
        ///Change the player camera visualization to the specified player 
        ///</summary>
        public void ChangePlayerView(Player.Player player)
        {
            playerView = player;
            player.playerCamera.SetPlayer(player);
        }

        private void OnPlayerDeath(Player.Player player)
        {
            if (player == playerView)
                ChangePlayerView();
            
        }
    }
}
