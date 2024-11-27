using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
    {
        [Header("Set MapGenerator")]
        public int X;
        public int Y;

        [Header("Set Player")]
        public Player player;
        public Vector2Int playerStartPos;

        [Header("Set Exit")]
        public Exit exit;

        [Header("Set Prefab")]
        public GameObject[] floorsPrefab;
        public GameObject[] wallsPrefab;
        public GameObject[] demonWallsPrefab;
        public GameObject[] itemsPrefab;

        [Header("Set Transform")]
        public Transform floorParent;
        public Transform wallParent;
        public Transform itemPotionParent;
        public Transform bombParent;

        [Header("Set object Count")]
        public int obstacleCount;

        public int[,] mapdata;

        [Header("Set Wall")]
        public Wall[,] walls;
    
        [Header("Set Item")]
        public BombFragment[,] bombFragments;

        // block types ...
        public int empty;
        public int demonWall = 1;
        public int potion = 2;
        public int bomb = 3;
        public int exitdoor = 4;
        public int key = 5;
        
        //Depth First Search
        private bool[,] visited;
        private readonly int[,] directions = new int[,] { { -1, 0 }, { 1, 0 }, { 0, -1 }, { 0, 1 } };
        private int playerSafeRadius = 1;

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
                        obj.layer = LayerMask.NameToLayer("Wall");
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
            mapdata[playerStartPos.x, playerStartPos.y] = 0;

            walls = new Wall[X, Y];
            int count = 0;
            int maxAttempts = obstacleCount * 2; // Prevent infinite loops
            int attempts = 0;

            while (count < obstacleCount && attempts < maxAttempts)
            {
                int x = Random.Range(0, X);
                int y = Random.Range(0, Y);
            
                if (mapdata[x, y] == empty && !IsInPlayerSafeZone(x, y))
                {
                    PlaceWall(x, y);
                    count++;
                }
                attempts++;
            }

            bombFragments = new BombFragment[X, Y];

            mapdata[X - 1, Y - 1] = exitdoor;
            exit.transform.position = new Vector3(X - 1, Y - 1, 0);
        }

        public int GetMapData(float x, float y)
        {
            if (x >= X || x < 0 || y >= Y || y < 0) 
                return -1;
            return mapdata[(int)x, (int)y];
        }
        
        private bool IsInPlayerSafeZone(int x, int y)
        {
            int playerDistance = Mathf.Abs(x - playerStartPos.x) + Mathf.Abs(y - playerStartPos.y);
            return playerDistance <= playerSafeRadius;
        }
        
        private bool CheckValidPath()
        {
            visited = new bool[X, Y];
            return DFS(playerStartPos.x, playerStartPos.y);
        }
        
        private bool DFS(int x, int y)
        {
            // Check if we reached the exit
            if (x == X - 1 && y == Y - 1)
                return true;

            // Mark current position as visited
            visited[x, y] = true;

            // Check all four directions
            for (int i = 0; i < 4; i++)
            {
                int newX = x + directions[i, 0];
                int newY = y + directions[i, 1];

                // Check if the new position is valid
                if (IsValidPosition(newX, newY) && !visited[newX, newY])
                {
                    // If there's a path through this position, return true
                    if (DFS(newX, newY))
                        return true;
                }
            }

            return false;
        }
        
        private bool IsValidPosition(int x, int y)
        {
            // Check if position is within bounds
            if (x < 0 || x >= X || y < 0 || y >= Y)
                return false;

            // Check if position is empty, exit, or has a breakable wall
            Wall currentWall = walls[x, y];
            return mapdata[x, y] == empty || 
                   mapdata[x, y] == exitdoor || 
                   (mapdata[x, y] == demonWall && currentWall != null && !currentWall.IsGodWall);
        }

        // Modify your wall placement code in Start()
        public void PlaceWall(int x, int y)
        {
            // Don't place walls in player safe zone
            if (IsInPlayerSafeZone(x, y))
                return;

            int r = Random.Range(0, demonWallsPrefab.Length);
            GameObject obj = Instantiate(demonWallsPrefab[r], new Vector3(x, y, 0), Quaternion.identity);
            obj.transform.parent = wallParent;
            obj.layer = LayerMask.NameToLayer("Destructible Wall");
            mapdata[x, y] = demonWall;
            walls[x, y] = obj.GetComponent<Wall>();
            walls[x, y].positionX = x;
            walls[x, y].positionY = y;
            walls[x, y].mapGenerator = this;
            obj.name = $"ObstacleWall_{walls[x, y].Name} {x}, {y}";

            Wall wallComponent = obj.GetComponent<Wall>();
        
            // Only check path blocking for GodWalls
            if (wallComponent.IsGodWall)
            {
                // If this GodWall blocks the path, convert it to a normal wall
                if (!CheckValidPath())
                {
                    obj.layer = LayerMask.NameToLayer("Destructible Wall");
                    wallComponent.IsGodWall = false;
                    wallComponent.GetComponent<SpriteRenderer>().color = Color.white; // Reset color to normal
                }
            }
        }

        public void PlaceItem(int x, int y)
        {
            int r = Random.Range(0, itemsPrefab.Length);
            GameObject droppedItem = Instantiate(itemsPrefab[r], new Vector3(x,y, 0), Quaternion.identity);
            droppedItem.transform.parent = itemPotionParent;
            mapdata[x, y] = bomb;
            bombFragments[x, y] = droppedItem.GetComponent<BombFragment>();
            bombFragments[x, y].positionX = x;
            bombFragments[x, y].positionY = y;
            bombFragments[x, y].mapGenerator = this;
            droppedItem.name = $"Item_{name}";
        }
}


