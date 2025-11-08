using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer altarItemRenderer; // Reference to the child's SpriteRenderer
    [SerializeField] private Sprite claimedSprite; // New sprite when checkpoint is activated
    [SerializeField] private Sprite activatedSprite; // New sprite when checkpoint is activated
    [SerializeField] private int checkpointIndex; // Index of this checkpoint
    private enum ColliderState
    {
        Unclaimed,
        Claimed,
        Active
    }
    private SpawnSystem spawnSystem;
    private AudioManager audioManager;


    [SerializeField] private ColliderState colliderState = ColliderState.Unclaimed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && colliderState != ColliderState.Active)
        {
            if (audioManager != null)
                audioManager.PlaySfx(audioManager.checkPoint);

            spawnSystem = FindFirstObjectByType<SpawnSystem>();

            if (spawnSystem.previousIndex >= 0)
            {
                var previousCheckpoint = spawnSystem.checkpoint[spawnSystem.previousIndex].GetComponent<Checkpoint>();
                previousCheckpoint.colliderState = ColliderState.Claimed;
            }

            // Aktifkan checkpoint ini
            colliderState = ColliderState.Active;
            GameManager.Instance.SaveCheckpoint(checkpointIndex);
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     // print("colliderState: " + this.colliderState);
    //     if (audioManager != null && this.colliderState != ColliderState.Active)
    //     {
    //         audioManager.PlaySfx(audioManager.checkPoint);
    //     }
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         print("Checkpoint reached at index: " + checkpointIndex);
    //         spawnSystem = FindFirstObjectByType<SpawnSystem>();
    //         for (int i = 0; i < spawnSystem.checkpoint.Length; i++)
    //         {
    //             spawnSystem.checkpoint[i].GetComponent<Checkpoint>().colliderState = ColliderState.Claimed;
    //             if (spawnSystem.checkpoint[i] == this.gameObject)
    //             {
    //                 spawnSystem.checkpoint[i].GetComponent<Checkpoint>().colliderState = ColliderState.Active;
    //                 GameManager.Instance.SaveCheckpoint(checkpointIndex);
    //             }
    //         }
    //     }
    // }

    private ColliderState lastColliderState;

    private void Awake()
    {
        lastColliderState = colliderState;
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        altarItemRenderer = transform.Find("altarItem").GetComponent<SpriteRenderer>();
        // Get the SpriteRenderer component from the child named "altarItem"
        if (spawnSystem == null)
        {
            spawnSystem = FindFirstObjectByType<SpawnSystem>();
            // for (int i = 0; i < spawnSystem.checkpoint.Length; i++)
            // {
            //     if (spawnSystem.checkpoint[i] == this.gameObject)
            //     {
            //         checkpointIndex = i;
            //         // print("Checkpoint Index: " + checkpointIndex);
            //         break;
            //     }
            // }
        }
    }

    private void Update()
    {
        if (lastColliderState != colliderState)
        {
            var previous = lastColliderState;
            lastColliderState = colliderState;
            OnColliderStateChanged(previous, colliderState);
        }
    }

    private void OnColliderStateChanged(ColliderState fromState, ColliderState toState)
    {
        Debug.Log($"Checkpoint state changed from {fromState} to {toState}");

        switch (toState)
        {
            case ColliderState.Unclaimed:
                // handle unclaimed state
                break;
            case ColliderState.Claimed:
                // handle claimed state
                altarItemRenderer.sprite = claimedSprite;
                break;
            case ColliderState.Active:
                // handle active state
                altarItemRenderer.sprite = activatedSprite;
                break;
        }
    }

    public void SetIndex(int i)
    {
        checkpointIndex = i;
    }
}