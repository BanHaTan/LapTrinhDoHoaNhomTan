using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    [Header("Floating Settings")]
    [Tooltip("Độ cao lơ lửng so với vị trí ban đầu")]
    public float floatHeight = 0.5f;

    [Tooltip("Tốc độ dao động lên xuống")]
    public float floatSpeed = 2f;

    [Tooltip("Thời gian delay ngẫu nhiên để mỗi food có pha khác nhau")]
    public bool randomPhase = true;

    [Header("Rotation Settings")]
    [Tooltip("Tốc độ xoay tròn")]
    public float rotationSpeed = 50f;

    [Tooltip("Có xoay theo trục Y không")]
    public bool rotateY = true;

    // Vị trí ban đầu khi spawn
    private Vector3 startPosition;
    private float phaseOffset;
    private float elapsedTime;

    void Start()
    {
        // Lưu vị trí ban đầu (vị trí tile)
        startPosition = transform.position;

        // Random phase để mỗi food dao động khác nhau
        phaseOffset = randomPhase ? Random.Range(0f, Mathf.PI * 2) : 0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        // Hiệu ứng lơ lửng lên xuống theo hình sin
        float yOffset = Mathf.Sin((elapsedTime * floatSpeed) + phaseOffset) * floatHeight;
        transform.position = startPosition + new Vector3(0f, yOffset, 0f);

        // Hiệu ứng xoay nhẹ (tùy chọn)
        if (rotateY)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
        }
    }
}