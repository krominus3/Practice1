using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DicePhysics : MonoBehaviour
{
    [Header("Настройки броска")]
    [SerializeField] private float minForce = 3f;
    [SerializeField] private float maxForce = 6f;
    [SerializeField] private float torqueForce = 10f;
    [SerializeField] private float throwHeight = 1f;

    private Rigidbody rb;

    public bool IsMoving => !rb.isKinematic && rb.linearVelocity.sqrMagnitude > 0.1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.isKinematic = true;
    }

    public void Throw()
    {
        // 1. "Будим" физику
        rb.isKinematic = false;

        // 2. Сбрасываем скорости
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 3. Генерируем случайную силу
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        float force = Random.Range(minForce, maxForce);

        Vector3 throwDirection = new Vector3(randomX, throwHeight, randomZ).normalized;

        rb.AddForce(throwDirection * force, ForceMode.Impulse);

        // 4. Добавляем случайное вращение
        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ) * torqueForce;

        rb.AddTorque(randomTorque, ForceMode.Impulse);
    }

    public bool IsStopped()
    {
        return rb.linearVelocity.magnitude < 0.1f && rb.angularVelocity.magnitude < 0.1f;
    }
}