using ExtremeSnowboarding.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExtremeSnowboarding.Script.Items
{
    public enum MovementType { STRAIGHT, FOWARD, STOPPED, BACK, STRAIGHT_BACK, MOVE_TRACKING_TARGET }

    [CreateAssetMenu(fileName = "FuckFriend", menuName = "Itens/Fuck Friend", order = 1)]
    public class FuckFriend : Item
    {
        [SerializeField] [BoxGroup("Fuck Friend Values")]
        private InstanceType instanceType = InstanceType.PROJECTILE;

        [SerializeField] [BoxGroup("Fuck Friend Values")] [ShowIf("instanceType", InstanceType.PROJECTILE)]
        private ProjectileInstance projectileInstance;

        [SerializeField] [BoxGroup("Fuck Friend Values")] [ShowIf("instanceType", InstanceType.SPECIFC_PLAYER)]
        private SpecificPlayerInstance specificPlayerInstance;

        [SerializeField] [BoxGroup("Fuck Friend Values")] [ShowIf("instanceType", InstanceType.AREA_EFFECT)]
        private AreaEffectInstance areaEffectInstance;

        [SerializeField] [BoxGroup("VFX Activation")] private bool activateVFXOnCaster;
        [SerializeField] [BoxGroup("VFX Activation")] [ShowIf("activateVFXOnCaster")] private string[] VFXNamesOnCaster;

        [SerializeField] [BoxGroup("Animation")] private bool activateAnimationOnCaster;
        [SerializeField] [BoxGroup("Animation")] [ShowIf("activateAnimationOnCaster")] private string animationOnCaster;

        private IInstance instance => (instanceType == InstanceType.PROJECTILE) ? (IInstance)projectileInstance : (IInstance)specificPlayerInstance;

        public Player.Player Player { get; private set; }
        
        public override void Activate (Player.Player player)
        {
            Player = player;
            instance.ActivateInstance(this);
            ActivateVFX(player, activateVFXOnCaster, VFXNamesOnCaster);
            ActivateAnimation(player, activateAnimationOnCaster, animationOnCaster, true);
        }
    }
    
    
    public interface IInstance
    {
        public void ActivateInstance(FuckFriend fuck);
    }

    [System.Serializable]
    public class ProjectileInstance : IInstance
    {
        [SerializeField] [BoxGroup("Instance Values")] [AllowNesting]
        private GameObject projectile;
        
        public void ActivateInstance(FuckFriend fuck)
        {
            GameObject instantiatedProjectile = MonoBehaviour.Instantiate(projectile, fuck.Player.transform.position, projectile.transform.rotation); /* Considerar a rota��o do cen�rio!!!! */
            Projectile proj = instantiatedProjectile.GetComponent<Projectile>();

            Player.Player target = CorridaController.instance.GetPlayerByPlace(fuck.Player.SharedValues.qualification - 1);

            proj.fuckfriend = fuck;
            proj.target = target;
            proj.caster = fuck.Player;
        }
    }

    [System.Serializable]
    public class AreaEffectInstance : IInstance
    {
        [SerializeField] [BoxGroup("Instance Values")] [AllowNesting]
        private float radius = 5f;

        public void ActivateInstance(FuckFriend fuck)
        {
            Collider[] playersHitted = Physics.OverlapBox(fuck.Player.gameObject.transform.position, new Vector3(7.5f, 7.5f, 7.5f), Quaternion.identity, LayerMask.GetMask("Player"));

            foreach (var player in playersHitted)
            {
                fuck.StartEffects(player.GetComponent<Player.Player>());
            }

        }
    }

    [System.Serializable]
    public class SpecificPlayerInstance : IInstance
    {
        [SerializeField] [BoxGroup("Instance Values")] [AllowNesting]
        private EspecifiedType especifiedType = EspecifiedType.BY_PLACE;

        [SerializeField] [BoxGroup("Instance Values")] [MinValue(1), MaxValue(4)] [AllowNesting]
        private int position = 1;

        [SerializeField] [BoxGroup("Instance Values")] [ShowIf("especifiedType", EspecifiedType.BY_PROXIMITY)] [AllowNesting]
        private bool forceInstance = true;
        
        public void ActivateInstance(FuckFriend fuck)
        {
            switch (especifiedType)
            {
                case EspecifiedType.BY_PLACE:
                    InstanceByPlace(fuck);
                    break;
                case EspecifiedType.BY_PROXIMITY:
                    InstanceByProximity(fuck);
                    break;
            }
        }

        private void InstanceByPlace(FuckFriend fuck)
        {
            Player.Player playerAffected = CorridaController.instance.GetPlayerByPlace(position);
            fuck.StartEffects(playerAffected);
        }

        private void InstanceByProximity(FuckFriend fuck)
        {
            int actualPosition = CorridaController.instance.GetPlayerPlace(fuck.Player);
            int affectedPosition = actualPosition + position;
            int playersCount = CorridaController.instance.GetPlayersInGameCount();
            
            if (affectedPosition >= 1 && affectedPosition <= playersCount)
            {
                Player.Player playerAffected = CorridaController.instance.GetPlayerByPlace(affectedPosition);
                fuck.StartEffects(playerAffected);
            }
            else if (forceInstance)
            {
                int clampedPosition = Mathf.Clamp(affectedPosition, 1, playersCount);

                if (clampedPosition == actualPosition)
                {
                    int sign = (int)Mathf.Sign(position);

                    if (actualPosition == playersCount)
                        sign = -1;
                    else if (actualPosition == 1)
                        sign = +1;
                        
                    clampedPosition = clampedPosition + 1 * sign;
                }
                
                Player.Player playerAffected = CorridaController.instance.GetPlayerByPlace(clampedPosition);
                fuck.StartEffects(playerAffected);
            }
        }
        
        private enum EspecifiedType
        {
            BY_PLACE,
            BY_PROXIMITY
        }
    }

    public enum InstanceType
    {
        PROJECTILE,
        SPECIFC_PLAYER,
        AREA_EFFECT
    }
}