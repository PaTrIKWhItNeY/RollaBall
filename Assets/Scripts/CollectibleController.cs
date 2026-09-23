using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private GameObject collectParticlePrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     // Finds the Game Manager in the Scene
    gameManager = FindAnyObjectByType<GameManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
      // Call with this object collides with a trigger
    private void OnTriggerEnter(Collider other)
    {
        // Only executes if the collision was with the Player
        if (other.CompareTag("Player"))
        {
            // Spawn audio at the collectible's position (auto-destroys)
            AudioSource.PlayClipAtPoint(collectSound, transform.position);

            // Spawn particles (auto-destroys if Stop Action is set to Destroy)
            Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);

            // Safely destroy the collectible immediately
            Destroy(gameObject);
            gameManager.UpdateRemaining();
        }
    }
    private GameManager gameManager;
    

}
