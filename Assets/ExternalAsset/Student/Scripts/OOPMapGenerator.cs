using UnityEngine;

namespace Student
{

    public class OOPMapGenerator : MonoBehaviour
    {
        [Header("Set MapGenerator")]
        public int X;
        public int Y;

        [Header("Set Player")]
        public OOPPlayer player;
        public Vector2Int playerStartPos;

        [Header("Set Exit")]
        public OOPExit Exit;

        [Header("Set Prefab")]
        public GameObject[] floorsPrefab;
        public GameObject[] wallsPrefab;
        public GameObject[] demonWallsPrefab;
        public GameObject[] itemsPrefab;
        public GameObject[] keysPrefab;

        [Header("Set Transform")]
        public Transform floorParent;
        public Transform wallParent;
        public Transform itemPotionParent;

        [Header("Set object Count")]
        public int obsatcleCount;
        public int itemPotionCount;
        public int itemKeyCount;

        public int[,] mapdata;

        public OOPWall[,] walls;
        public OOPItemPotion[,] potions;
        public OOPItemKey[,] keys;

        // block types ...
        public int empty = 0;
        public int demonWall = 1;
        public int potion = 2;
        public int bonuesPotion = 3;
        public int exit = 4;
        public int key = 5;

        // Start is called before the first frame update
        void Start()
        {
            mapdata = new int[X, Y];
            for (int x = -1; x < X + 1; x++)
            {
                for (int y = -1; y < Y + 1; y++)
                {
                    if (x == -1 || x == X || y == -1 || y == Y)
                    {
                        int r = Random.Range(0, wallsPrefab.Length);
                        GameObject obj = Instantiate(wallsPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
                        obj.transform.parent = wallParent;
                        obj.name = "Wall_" + x + ", " + y;
                    }
                    else
                    {
                        int r = Random.Range(0, floorsPrefab.Length);
                        GameObject obj = Instantiate(floorsPrefab[r], new Vector3(x, y, 1), Quaternion.identity);
                        obj.transform.parent = floorParent;
                        obj.name = "floor_" + x + ", " + y;
                    }
                }
            }

            player.mapGenerator = this;
            player.positionX = playerStartPos.x;
            player.positionY = playerStartPos.y;
            player.transform.position = new Vector3(playerStartPos.x, playerStartPos.y, -0.1f);
            mapdata[playerStartPos.x, playerStartPos.y] = -1;

            walls = new OOPWall[X, Y];
            int count = 0;
            while (count < obsatcleCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == 0)
                {
                    PlaceWall(x, y);
                    count++;
                }
            }

            potions = new OOPItemPotion[X, Y];
            count = 0;
            while (count < itemPotionCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceItem(x, y);
                    count++;
                }
            }

            /*keys = new OOPItemKey[X, Y];
            count = 0;
            while (count < itemKeyCount)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
                if (mapdata[x, y] == empty)
                {
                    PlaceKey(x, y);
                    count++;
                }
            }*/

            mapdata[X - 1, Y - 1] = exit;
            Exit.transform.position = new Vector3(X - 1, Y - 1, 0);
        }

        public int GetMapData(float x, float y)
        {
            if (x >= X || x < 0 || y >= Y || y < 0) return -1;
            return mapdata[(int)x, (int)y];
        }

        public void PlaceItem(int x, int y)
        {
            int r = Random.Range(0, itemsPrefab.Length);
            GameObject obj = Instantiate(itemsPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = itemPotionParent;
            mapdata[x, y] = potion;
            potions[x, y] = obj.GetComponent<OOPItemPotion>();
            potions[x, y].positionX = x;
            potions[x, y].positionY = y;
            potions[x, y].mapGenerator = this;
            obj.name = $"Item_{potions[x, y].Name} {x}, {y}";
        }

        /*public void PlaceKey(int x, int y)
        {
            int r = Random.Range(0, keysPrefab.Length);
            GameObject obj = Instantiate(keysPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = itemPotionParent;
            mapdata[x, y] = key;
            keys[x, y] = obj.GetComponent<OOPItemKey>();
            keys[x, y].positionX = x;
            keys[x, y].positionY = y;
            keys[x, y].mapGenerator = this;
            obj.name = $"Item_{keys[x, y].Name} {x}, {y}";
        }*/

        public void PlaceWall(int x, int y)
        {
            int r = Random.Range(0, demonWallsPrefab.Length);
            GameObject obj = Instantiate(demonWallsPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = wallParent;
            mapdata[x, y] = demonWall;
            walls[x, y] = obj.GetComponent<OOPWall>();
            walls[x, y].positionX = x;
            walls[x, y].positionY = y;
            walls[x, y].mapGenerator = this;
            obj.name = $"DemonWall_{walls[x, y].Name} {x}, {y}";
        }
    }
}