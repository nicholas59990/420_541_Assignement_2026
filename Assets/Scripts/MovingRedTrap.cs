using UnityEngine;

// Makes the red trap move back and forth
public class MovingRedTrap : MonoBehaviour
{

    [SerializeField] private float moveDistance = 3f;
    [SerializeField] private float moveSpeed = 2f;

    private Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Remember where the trap starts
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the trap back and forth
        float movement = Mathf.Sin(Time.time * moveSpeed) * moveDistance;

        transform.position = startPosition + Vector3.right * movement;
    }
}
