using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEngine.EventSystems.EventTrigger;

namespace Student
{
    public class Character : Identity
    {
        [Header("Character")]
        public int energy;
        public int AttackPoint;

        protected bool isAlive;
        protected bool isFreeze;

        // Start is called before the first frame update
        protected void GetRemainEnergy()
        {
            Debug.Log(Name + " : " + energy);
        }

        public virtual void Move(Vector2 direction)
        {
            if (isFreeze == true)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                isFreeze = false;
                return;
            }
            int toX = (int)(positionX + direction.x);
            int toY = (int)(positionY + direction.y);

            if (HasPlacement(toX, toY))
            {
                if (IsDemonWalls(toX, toY))
                {
                    mapGenerator.walls[toX, toY].Hit();
                }
                else if (IsPotion(toX, toY))
                {
                    mapGenerator.potions[toX, toY].Hit();
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                }
                else if (IsPotionBonus(toX, toY))
                {
                    mapGenerator.potions[toX, toY].Hit();
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                }
                else if (IsExit(toX, toY))
                {
                    mapGenerator.Exit.Hit();
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                }
            }
            else
            {
                positionX = toX;
                positionY = toY;
                transform.position = new Vector3(positionX, positionY, 0);
                TakeDamage(1);
            }

        }
        // hasPlacement คืนค่า true ถ้ามีการวางอะไรไว้บน map ที่ตำแหน่ง x,y
        public bool HasPlacement(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData != mapGenerator.empty;
        }
        public bool IsDemonWalls(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.demonWall;
        }
        public bool IsPotion(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.potion;
        }
        public bool IsPotionBonus(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.potion;
        }
        // public bool IsKey(int x, int y)
        // {
        //     int mapData = mapGenerator.GetMapData(x, y);
        //     return mapData == mapGenerator.key;
        // }
        public bool IsExit(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.exit;
        }

        public virtual void TakeDamage(int Damage)
        {
            energy -= Damage;
            Debug.Log(Name + " Current Energy : " + energy);
            CheckDead();
        }
        public virtual void TakeDamage(int Damage, bool freeze)
        {
            energy -= Damage;
            isFreeze = freeze;
            GetComponent<SpriteRenderer>().color = Color.blue;
            Debug.Log(Name + " Current Energy : " + energy);
            Debug.Log("you is Freeze");
            CheckDead();
        }


        public void Heal(int healPoint)
        {
            // energy += healPoint;
            // Debug.Log("Current Energy : " + energy);
            // เราสามารถเรียกใช้ฟังก์ชัน Heal โดยกำหนดให้ Bonuse = false ได้ เพื่อที่จะให้ logic ในการ heal อยู่ที่ฟังก์ชัน Heal อันเดียวและไม่ต้องเขียนซ้ำ
            Heal(healPoint, false);
        }

        public void Heal(int healPoint, bool Bonuse)
        {
            energy += healPoint * (Bonuse ? 2 : 1);
            Debug.Log("Current Energy : " + energy);
        }

        protected virtual void CheckDead()
        {
            if (energy <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}