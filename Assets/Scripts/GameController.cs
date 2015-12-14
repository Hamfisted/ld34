using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class GameController : MonoBehaviour
{
    // Prefab type to spawn.
    [SerializeField] GameObject platformType;

    // Characters to manage.
    [SerializeField] EnemyCharacter enemy;
    [SerializeField] PlayerCharacter player;

    // Game parameters.
    [SerializeField] float startingSpeed = 30f;
    [SerializeField] float maximumSpeed = 50f;

    // Game components.
    Rigidbody2D body;

    // Constant parameters.
    float loopWidth;

    // Game state.
    bool finished;
    float scrollSpeed;

    // List of platforms.
    List<Platform> platforms;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();

        // Set to loop at the bounds of the collider.
        Vector2 size = GetComponent<BoxCollider2D>().bounds.size;
        loopWidth = size.x * 0.5f;

        // Start up the game state.
        finished = false;
        scrollSpeed = startingSpeed;
        platforms = new List<Platform>();
        StartCoroutine(SpawnPlatforms());
    }

    void Update()
    {
        // Scroll and reset when we loop.
        Vector2 scrollOffset = new Vector2(-scrollSpeed * Time.deltaTime, 0f);
        Vector2 position = transform.position;
        Vector2 newPosition = position + scrollOffset;
        if (newPosition.x <= -loopWidth)
        {
            newPosition.x += loopWidth * Mathf.Floor(newPosition.x / loopWidth);
        }
        newPosition.x = (newPosition.x % loopWidth);
        body.MovePosition(newPosition);

        // Scroll platforms.
        platforms.RemoveAll(platform => platform == null);
        foreach (Platform platform in platforms)
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

    IEnumerator SpawnPlatforms()
    {
        while (!finished)
        {
            Vector2 newPosition = new Vector2(loopWidth, 4f);
            GameObject newPlatform = Instantiate(platformType, newPosition, Quaternion.identity) as GameObject;
            Platform platform = newPlatform.GetComponent<Platform>();
            newPlatform.transform.position = newPosition;
            platforms.Add(platform);
            yield return new WaitForSeconds(1.5f);
        }
    }

}
