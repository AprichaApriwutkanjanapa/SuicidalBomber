using UnityEngine;

public class Character : Identity
    {
        [Header("Character")]
        public int health;

        protected bool isAlive;
        protected bool isFreeze;

        // Start is called before the first frame update

        public virtual void Move(Vector2 direction)
        {
            int toX = (int)(positionX + direction.x);
            int toY = (int)(positionY + direction.y);

            if (HasPlacement(toX, toY))
            {
                if (IsBombItem(toX, toY))
                {
                    BombFragment fragment = mapGenerator.bombFragments[toX, toY];
                    if (fragment != null)
                    {
                        fragment.Hit();
                        positionX = toX;
                        positionY = toY;
                        transform.position = new Vector3(positionX, positionY, 0);
                    }
                    else
                    {
                        Debug.LogError("BombFragment at position (" + toX + ", " + toY + ") is null."); 
                    }
                    
                }
                /*else if (IsPotionBonus(toX, toY))
                {
                    mapGenerator.potions[toX, toY].Hit();
                    positionX = toX;
                    positionY = toY;
                    transform.position = new Vector3(positionX, positionY, 0);
                }*/
                if (IsExit(toX, toY))
                {
                    mapGenerator.exit.Hit();
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
        public bool IsBombItem(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.bomb;
        }
        public bool IsPotionBonus(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.potion;
        }
        public bool IsKey(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.key;
        }
        public bool IsExit(int x, int y)
        {
            int mapData = mapGenerator.GetMapData(x, y);
            return mapData == mapGenerator.exitdoor;
        }

        public virtual void TakeDamage(int Damage)
        {
            health -= Damage;
            Debug.Log(Name + " Current health : " + health);
            CheckDead();
        }
        public virtual void TakeDamage(int Damage, bool freeze)
        {
            health -= Damage;
            isFreeze = freeze;
            GetComponent<SpriteRenderer>().color = Color.blue;
            Debug.Log(Name + " Current health : " + health);
            Debug.Log("you is Freeze");
            CheckDead();
        }
        
        public void Heal(int healPoint)
        {
            // health += healPoint;
            // Debug.Log("Current health : " + health);
            // เราสามารถเรียกใช้ฟังก์ชัน Heal โดยกำหนดให้ Bonuse = false ได้ เพื่อที่จะให้ logic ในการ heal อยู่ที่ฟังก์ชัน Heal อันเดียวและไม่ต้องเขียนซ้ำ
            Heal(healPoint, false);
        }
        
        public void Heal(int healPoint, bool Bonuse)
        {
            health += healPoint * (Bonuse ? 2 : 1);
            Debug.Log("Current health : " + health);
        }

        protected virtual void CheckDead()
        {
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
