using System;
using UnityEngine;
namespace Player
{
    public class BombController: PlayerControllerBase
    {

        public Bomb.Bomb _bomb;
        internal void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlaceBomb();
            }
        }

        private void PlaceBomb()
        { 
            Instantiate(_bomb, new Vector3(transform.position.x,0,transform.position.z), Quaternion.identity);  
        }
    }
}