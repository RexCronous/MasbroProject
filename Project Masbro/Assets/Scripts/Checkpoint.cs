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


    [SerializeField] private ColliderState colliderState = ColliderState.Unclaimed;

    private void Start()
    {
        // Get the SpriteRenderer component from the child named "altarItem"
        altarItemRenderer = transform.Find("altarItem").GetComponent<SpriteRenderer>();
        if (spawnSystem == null)
        {
            spawnSystem = FindFirstObjectByType<SpawnSystem>();
            for (int i = 0; i < spawnSystem.checkpoint.Length; i++)
            {
                if (spawnSystem.checkpoint[i] == this.gameObject)
                {
                    checkpointIndex = i;
                    print("Checkpoint Index: " + checkpointIndex);
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            spawnSystem = FindFirstObjectByType<SpawnSystem>();
            for (int i = 0; i < spawnSystem.checkpoint.Length; i++)
            {
                spawnSystem.checkpoint[i].GetComponent<Checkpoint>().colliderState = ColliderState.Claimed;
                if (spawnSystem.checkpoint[i] == this.gameObject)
                {
                    spawnSystem.checkpoint[i].GetComponent<Checkpoint>().colliderState = ColliderState.Active;
                    GameManager.Instance.SaveCheckpoint(checkpointIndex);
                }
            }
        }
    }

    private ColliderState lastColliderState;

    private void Awake()
    {
        lastColliderState = colliderState;
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
}
