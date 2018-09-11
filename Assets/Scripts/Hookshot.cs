using UnityEngine;
using UnityEditor;

public class Hookshot : MonoBehaviour
{
    public float Speed = 10f;
    public LayerMask attachableLayers;

    RobotBoyCharacter character;
    LineRenderer trailRenderer;

    Vector2 velocity;
    bool attached;

    private void Awake()
    {
        character = FindObjectOfType<RobotBoyCharacter>();
        trailRenderer = GetComponent<LineRenderer>();
        UpdateTrailRendererPositions();
    }

    private void Start()
    {
        attached = false;
        velocity = transform.right * Speed;
        UpdateTrailRendererPositions();
    }

    private void Update()
    {
        if (!attached)
        {
            RaycastHit2D hit = CheckForObstacle();
            if (hit.collider != null)
            {
                SetPosition(hit.point);

                // we hit a wall! transition character to hookshot attached state
                character.AttachToHookshot(this);
                attached = true;
            }
            else
            {
                Move(velocity * Time.deltaTime);
            }
        }
        UpdateTrailRendererPositions();
    }

    private void UpdateTrailRendererPositions()
    {
        trailRenderer.SetPosition(0, character.transform.position);
        trailRenderer.SetPosition(1, transform.position);
    }

    private void Move(Vector2 delta)
    {
        SetPosition(transform.position + (Vector3)delta);
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;

    }

    public void SetRotation(Quaternion rotation)
    {
        this.transform.rotation = rotation;
    }

    RaycastHit2D CheckForObstacle()
    {
        Vector2 raycastOrigin = transform.position;
        Vector2 raycastDest = (Vector2)(transform.position + transform.right * Speed * Time.deltaTime);

        return Physics2D.Raycast(raycastOrigin, transform.forward, Speed, attachableLayers);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void OnDrawGizmos()
    {
        if (attached)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }


        Gizmos.color = Color.red;
        Gizmos.DrawSphere(trailRenderer.GetPosition(0), 0.1f);
        Gizmos.DrawSphere(trailRenderer.GetPosition(1), 0.1f);
    }
}