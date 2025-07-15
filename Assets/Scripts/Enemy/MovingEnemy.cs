using UnityEngine;

public class MovingEnemy : Enemy
{
    public float moveSpeed = 3f;
    public float moveDistance = 5f;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
    }

    void Update()
    {
        //if (!isActive) return;

       
        if (movingRight)
        {
            targetPosition = startPosition + Vector3.right * moveDistance;
        }
        else
        {
            targetPosition = startPosition - Vector3.right * moveDistance;
        }

        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            movingRight = !movingRight;
        }
    }
}