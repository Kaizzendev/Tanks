using System;
using DefaultNamespace;
using UnityEngine;
namespace Player
{
    public class BombController: PlayerControllerBase
    {

        public Bomb.Bomb _bomb;
        internal void Update()
        {
            
            if (!isEnabled)
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlaceBomb();
            }
        }

        private void PlaceBomb()
        { 
            var bomb = Instantiate(_bomb, new Vector3(transform.position.x,0,transform.position.z), Quaternion.identity);
            bomb.team = EnumTeam.Player;
            bomb.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
    }
}