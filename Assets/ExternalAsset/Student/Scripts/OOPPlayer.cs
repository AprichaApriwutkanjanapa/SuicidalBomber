using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Student
{
    public class OOPPlayer : Character
    {
        public Inventory inventory;

        public void Start()
        {
            PrintInfo();
            GetRemainEnergy();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Move(Vector2.up);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Move(Vector2.down);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(Vector2.left);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Move(Vector2.right);
            }
        }

        public void Attack(OOPEnemy _enemy)
        {
            _enemy.energy -= AttackPoint;
            Debug.Log(_enemy.name + " is energy " + _enemy.energy);
        }

        protected override void CheckDead()
        {
            base.CheckDead();
            if (energy <= 0)
            {
                Debug.Log("Player is Dead");
            }
        }

    }

}