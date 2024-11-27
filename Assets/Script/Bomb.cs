using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb")]
    public GameObject bombPrefab1;
    public GameObject bombPrefab2;
    public GameObject bombPrefab3;
    public float bombFuseTime = 3.0f;
    public int bombAmount = 1;
    private int bombRemaining;
    public static bool IsBombCrafted;

    [Header("Explosion")]
    public Explosion explosionPrefab;
    public LayerMask explosionLayerMask;
    public LayerMask nonexplosionlayerMask;
    public LayerMask playerLayerMask;
    public float explosionDuration = 1f;
    public int explosionRadius = 1;
    private static readonly Vector2[] SurroundingDirections = new Vector2[]
    {
        new Vector2(-1, 1),  // Top-left
        new Vector2(0, 1),   // Top
        new Vector2(1, 1),   // Top-right
        new Vector2(1, 0),   // Right
        new Vector2(1, -1),  // Bottom-right
        new Vector2(0, -1),  // Bottom
        new Vector2(-1, -1), // Bottom-left
        new Vector2(-1, 0)   // Left
    };

    [Header("Player")]
    public Player player;
    public int explosionDamage = 1;

    [Header("MapData")]
    public MapGenerator mapGenerator;

    private void OnEnable()
    {
        bombRemaining = bombAmount;
    }

    public IEnumerator PlaceBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab1, position, Quaternion.identity);
        bomb.transform.parent = mapGenerator.bombParent;
        bombRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);
        explosionRadius = 1;

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        bombRemaining++;
    }
    
    public IEnumerator PlaceSurroundingBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab2, position, Quaternion.identity);
        bomb.transform.parent = mapGenerator.bombParent;
        bombRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        // Create center explosion
        Explosion centerExplosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        centerExplosion.SetActiveRenderer(centerExplosion.start);
        centerExplosion.DestroyAfter(explosionDuration);

        // Explode in all 8 directions
        foreach (Vector2 direction in SurroundingDirections)
        {
            ExplodeSurrounding(position, direction);
        }

        Destroy(bomb);
        bombRemaining++;
    }
    
    public IEnumerator PlaceDevilBomb()
    {
        Vector2 position = transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        GameObject bomb = Instantiate(bombPrefab3, position, Quaternion.identity);
        bomb.transform.parent = mapGenerator.bombParent;
        bombRemaining--;

        yield return new WaitForSeconds(bombFuseTime);

        position = bomb.transform.position;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(explosion.start);
        explosion.DestroyAfter(explosionDuration);
        explosionRadius = 8;

        Explode(position, Vector2.up, explosionRadius);
        Explode(position, Vector2.down, explosionRadius);
        Explode(position, Vector2.left, explosionRadius);
        Explode(position, Vector2.right, explosionRadius);

        Destroy(bomb);
        bombRemaining++;
    }

    private void Explode(Vector2 position, Vector2 direction, int length)
    {
        if (length <= 0)
            return;

        position += direction;
        
        //เเก้เรื่อง Detect Wall ให้ระเบิดไม่ระเบิด Boarder
        if (Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, nonexplosionlayerMask))
        {
            return;
        }
        ClearDestructible(position);
        
        //Collider สำหรับผู้เล่นตาย
        Collider2D playerCollider = Physics2D.OverlapBox(position, Vector2.one / 2f, 0f, playerLayerMask);
        if (playerCollider != null)
        {
            Player player = playerCollider.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(explosionDamage);
            }
        }
        
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.SetActiveRenderer(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDirection(direction);
        explosion.DestroyAfter(explosionDuration);

        Explode(position, direction, length - 1);
    }
    
    private void ExplodeSurrounding(Vector2 position, Vector2 direction)
    {
        Vector2 explosionPos = position + direction;
        
        // Check if the position is blocked by a non-explodable wall
        if (Physics2D.OverlapBox(explosionPos, Vector2.one / 2f, 0f, nonexplosionlayerMask))
        {
            return;
        }

        // Clear destructible objects at this position
        ClearDestructible(explosionPos);
        

        // Create explosion effect
        Explosion explosion = Instantiate(explosionPrefab, explosionPos, Quaternion.identity);
        
        // Set the appropriate sprite based on the direction
        explosion.SetActiveRenderer(explosion.end);
        
        // Calculate rotation based on direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        explosion.SetDirection(direction);
        explosion.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        explosion.DestroyAfter(explosionDuration);
    }


    private void ClearDestructible(Vector2 position)
    {
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);

        // Check if the coordinates are within the walls array bounds
        if (x >= 0 && x < mapGenerator.walls.GetLength(0) && 
            y >= 0 && y < mapGenerator.walls.GetLength(1))
        {
            if (mapGenerator.walls[x, y] != null)
            {
                mapGenerator.walls[x, y].Hit();
            }
        }
        else
        {
            Debug.LogWarning($"Attempted to clear destructible outside map bounds at position ({x}, {y})");
        }
    }

    public void AddBomb()
    {
        // Implementation for adding bombs
    }

    void Update()
    {
        if (bombRemaining > 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(PlaceBomb()); // Original bomb
            }
            else if (IsBombCrafted)
            {
                if (Input.GetKeyDown(KeyCode.LeftAlt)) // You can change this key
                {
                    StartCoroutine(PlaceSurroundingBomb()); // New surrounding explosion bomb
                }
                else if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    StartCoroutine(PlaceDevilBomb());
                }
            }
            
            if (bombRemaining <= 0)
            {
                bombRemaining = 0;
                Debug.Log("Not Enough bomb!");
            }
            else
            {
                Debug.Log("Bomb Remaining Left : " + bombRemaining);
            }
        }
    }
}