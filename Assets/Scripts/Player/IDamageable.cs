using DefaultNamespace;
using UnityEngine;

namespace Player
{
    public interface IDamageable
    {
        public void TakeDamage(float amount);
        
        public void TakeDamage(float amount,EnumTeam team);
    }
}