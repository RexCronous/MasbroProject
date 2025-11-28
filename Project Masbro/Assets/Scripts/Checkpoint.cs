using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer altarItemRenderer; // Reference to the child's SpriteRenderer
    [SerializeField] private Sprite claimedSprite; // New sprite when checkpoint is activated
    [SerializeField] private Sprite activatedSprite; // New sprite when checkpoint is activated
    [SerializeField] private int checkpointIndex; // Index of this checkpoint
    [SerializeField] private bool hasActivated = false;
    public enum ColliderState
    {
        Unclaimed,
        Claimed,
        Active
    }
    private SpawnSystem spawnSystem;
    private AudioManager audioManager;


    [SerializeField] public ColliderState colliderState = ColliderState.Unclaimed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (audioManager != null && this.colliderState != ColliderState.Active)
            {
                audioManager.PlaySfx(audioManager.checkPoint);
            }

            spawnSystem = FindFirstObjectByType<SpawnSystem>();
            for (int i = 0; i < spawnSystem.checkpoint.Length; i++)
            {
                ColliderState isClaimed = spawnSystem.checkpoint[i].GetComponent<Checkpoint>().colliderState;
                if (isClaimed == ColliderState.Active)
                {
                    spawnSystem.checkpoint[i].GetComponent<Checkpoint>().colliderState = ColliderState.Claimed;
                }
                if (spawnSystem.checkpoint[i] == this.gameObject)
                {
                    spawnSystem.checkpoint[i].GetComponent<Checkpoint>().colliderState = ColliderState.Active;
                    GameManager.Instance.SaveCheckpoint(checkpointIndex, hasActivated);
                    hasActivated = true;
                }
            }
        }
    }

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
            for (int i = 0; i < spawnSystem.checkpoint.Length; i++)
            {
                if (spawnSystem.checkpoint[i] == this.gameObject)
                {
                    checkpointIndex = i;
                    break;
                }
            }
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