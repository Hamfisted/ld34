using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class GameController : MonoBehaviour
{
    // Prefab type to spawn.
    [SerializeField] GameObject[] platformTypes;

    // Characters to manage.
    [SerializeField] EnemyCharacter enemy;
    [SerializeField] PlayerCharacter player;

    // Game parameters.
    [SerializeField] float startingSpeed = 10f;
    [SerializeField] float maximumSpeed = 20f;
    [SerializeField] float platformsPerJumpLength = 1.5f;
    [SerializeField] float extraPlatformGap = 2f;
    [SerializeField] float minimumPlatformY = 2f;
    [SerializeField] float maximumPlatformY = 12f;
    [SerializeField] float minimumDeltaY = 2f;
    [SerializeField] float maximumDeltaY = 4f;
    [SerializeField] float groundPlatformSpawnThreshold = 3f;
    [SerializeField] float spawnX = 40f;

    [SerializeField] Text YouWonText;
    [SerializeField] Text YouLostText;

    // Game components.
    Rigidbody2D body;

    // Constant parameters.
    float loopWidth;

    // Game state.
    bool finished;
    float scrollSpeed;

    // List of platforms.
    Vector2 spawnPoint;
    List<Platform> platforms;

    // I'm sorry
    float timeSinceHaltingBegan = 0;
    float speedWhenHaltingBegan;

    void Start()
    {
        YouWonText.enabled = false;
        YouLostText.enabled = false;

        body = GetComponent<Rigidbody2D>();

        // Set to loop at the bounds of the collider.
        Vector2 size = GetComponent<BoxCollider2D>().bounds.size;
        loopWidth = size.x * 0.5f;

        // Start up the game state.
        finished = false;
        scrollSpeed = startingSpeed;
        spawnPoint = new Vector2(spawnX, minimumPlatformY);
        platforms = new List<Platform>();
        StartCoroutine(SpawnPlatforms());
    }

    void Update()
    {
        if (finished)
        {
            HaltScrolling();
        }
        else
        {
            scrollSpeed = Mathf.Lerp(startingSpeed, maximumSpeed, enemy.angerFactor);

            if (player.isDead)
            {
                Lose();
            }
            else if (enemy.isDead)
            {
                Win();
            }
        }
    }

    void FixedUpdate()
    {
        // Scroll and reset when we loop.
        Vector2 scrollOffset = new Vector2(-scrollSpeed * Time.fixedDeltaTime, 0f);
        Vector2 position = transform.position;
        Vector2 newPosition = position + scrollOffset;
        if (newPosition.x <= -loopWidth)
        {
            newPosition.x += loopWidth * Mathf.Floor(newPosition.x / -loopWidth);
        }
        body.MovePosition(newPosition);

        // Scroll platforms.
        platforms.RemoveAll(platform => platform == null);
        foreach (Platform platform in platforms)
        {
            // For some reason, Start() is deferred in new objects, so check for null.
            if (platform.body != null)
            {
                if (platform.body.position.x < -loopWidth)
                {
                    GameObject.Destroy(platform.gameObject);
                }
                else
                {
                    platform.Move(scrollOffset);
                }
            }
        }
    }

    IEnumerator SpawnPlatforms()
    {
        while (!finished)
        {
            if (platforms.Count == 0)
            {
                int type = Random.Range(0, platformTypes.Length);
                GameObject newPlatform = Instantiate(platformTypes[type], spawnPoint, Quaternion.identity) as GameObject;
                Platform platform = newPlatform.GetComponentInChildren<Platform>();
                platforms.Add(platform);
            }
            else
            {
                // Spawn them based on jump length.
                float JumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * player.JumpHeight);
                float jumpTime = (-2f * JumpSpeed) / Physics2D.gravity.y;
                float jumpDistance = (scrollSpeed * jumpTime) / platformsPerJumpLength + extraPlatformGap;

                // Get distance from last platform to spawn point.
                Platform latest = platforms[platforms.Count - 1];
                Vector2 lastPosition = latest.body.position;
                float lastSize = latest.box.bounds.size.x;
                float platformGap = jumpDistance + (lastSize);
                float newY = (player.offPlatform ? spawnPoint.y : lastPosition.y);
                Vector2 nextPosition = new Vector2(lastPosition.x + platformGap, newY);
                float distance = spawnPoint.x - lastPosition.x;
                int spawnCount = (int)Mathf.Floor(distance / jumpDistance);
                for (int i = 0; i < spawnCount; ++i)
                {
                    float offset = minimumDeltaY + ((maximumDeltaY - minimumDeltaY) * Random.Range(-1f, 1f));
                    if ((nextPosition.y + offset > maximumPlatformY) || (nextPosition.y + offset < minimumPlatformY))
                    {
                        nextPosition.y -= offset;
                    }
                    else
                    {
                        nextPosition.y += offset;
                    }

                    int type = Random.Range(0, platformTypes.Length);
                    GameObject newPlatform = Instantiate(platformTypes[type], nextPosition, Quaternion.identity) as GameObject;
                    Platform platform = newPlatform.GetComponentInChildren<Platform>();
                    platforms.Add(platform);
                    nextPosition.x += platformGap;
                }
            }
            yield return null;
        }
    }

    void Win()
    {
        player.hasWon = true;
        GameOver();
    }

    void Lose()
    {
        GameOver();
    }

    void GameOver()
    {
        if (!finished) {
            finished = true;
            speedWhenHaltingBegan = scrollSpeed;

            if (player.hasWon)
            {
                YouWonText.enabled = true;
            }
            else
            {
                YouLostText.enabled = true;
            }
        }
    }

    void HaltScrolling()
    {
        float k = 3.0f;
        timeSinceHaltingBegan += Time.deltaTime * k;

        //cubic ease-out
        float f = timeSinceHaltingBegan - 1.0f;
        f = f * f * f + 1.0f;

        f = 1.0f - Mathf.Min(f, 1.0f);
        scrollSpeed = f * speedWhenHaltingBegan;
    }

}
