using ExtremeSnowboarding.Script.Controllers;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace ExtremeSnowboarding.Script.Items
{
    public enum MovementType { STRAIGHT, FOWARD, STOPPED, BACK, STRAIGHT_BACK }

    [CreateAssetMenu(fileName = "FuckFriend", menuName = "Itens/Fuck Friend", order = 1)]
    public class FuckFriend : Item
    {
        [SerializeField] [BoxGroup("Fuck Friend Values")]
        private InstanceType instanceType = InstanceType.PROJECTILE;

        [SerializeField] [BoxGroup("Fuck Friend Values")] [ShowIf("instanceType", InstanceType.PROJECTILE)]
        private ProjectileInstance projectileInstance;

        [SerializeField] [BoxGroup("Fuck Friend Values")] [ShowIf("instanceType", InstanceType.SPECIFC_PLAYER)]
        private SpecificPlayerInstance specificPlayerInstance;
        
        private IInstance instance => (instanceType == InstanceType.PROJECTILE) ? (IInstance)projectileInstance : (IInstance)specificPlayerInstance;

        public Player.Player Player { get; private set; }
        
        public override void Activate (Player.Player player)
        {
            Player = player;
            instance.ActivateInstance(this);
        }
    }
    
    
    public interface IInstance
    {
        public void ActivateInstance(FuckFriend fuck);
    }

    [System.Serializable]
    public class ProjectileInstance : IInstance
    {
        [SerializeField] [BoxGroup("Instance Values")]
        private GameObject projectile;
        
        public void ActivateInstance(FuckFriend fuck)
        {
            GameObject instantiatedProjectile = MonoBehaviour.Instantiate(projectile, fuck.Player.transform.position, Quaternion.identity /* Considerar a rota��o do cen�rio!!!! */);
            Projectile proj = instantiatedProjectile.GetComponent<Projectile>();

            proj.fuckfriend = fuck;
            proj.caster = fuck.Player;
        }
    }

    [System.Serializable]
    public class SpecificPlayerInstance : IInstance
    {
        [SerializeField] [BoxGroup("Instance Values")]
        private EspecifiedType especifiedType = EspecifiedType.BY_PLACE;

        [SerializeField] [BoxGroup("Instance Values")] [MinMaxSlider(1,4)]
        private int position;

        [SerializeField] [BoxGroup("Instance Values")] [ShowIf("especifiedType", EspecifiedType.BY_PROXIMITY)]
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
        SPECIFC_PLAYER
    }
}