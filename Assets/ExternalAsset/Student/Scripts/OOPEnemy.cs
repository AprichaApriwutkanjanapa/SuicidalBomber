using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Student
{

    public class OOPEnemy : Character
    {
        public void Start()
        {
            GetRemainEnergy();
        }

        public void Attack(OOPPlayer _player)
        {
            _player.energy -= AttackPoint;
            Debug.Log("player is energy " + _player.energy);
        }
    }
}