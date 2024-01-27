using UnityEngine;

public class LobbyCharacter : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform Edge1;
    [SerializeField] private Transform Edge2;

    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    private bool movingLeft;

    [Header("Idle Behavior")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    private Animator anim;
    private SpriteRenderer sr;

    private void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x >= Edge1.position.x)
                MoveInDirection(-1);
            else
            {
                DirectionChange();
            }
        }
        else
        {
            if (transform.position.x <= Edge2.position.x)
                MoveInDirection(1);
            else
            {
                DirectionChange();
            }
        }
    }

    private void DirectionChange()
    {
        idleTimer += Time.deltaTime;
        anim.SetFloat("lobbyDeltaX", Mathf.Abs(0));
        if (idleTimer > idleDuration)
        {
            movingLeft = !movingLeft;
            idleDuration = Random.Range(1f, 3f);
        }            
    }

    private void MoveInDirection(int _direction)
    {
        anim.SetFloat("lobbyDeltaX", Mathf.Abs(speed));

        idleTimer = 0f;
        sr.flipX = movingLeft;
        transform.position = new Vector3(transform.position.x + Time.deltaTime * _direction * speed,
            transform.position.y, transform.position.z);
    }
}
