using UnityEngine;

public class CubeCreate : MonoBehaviour
{
    [Header("Cubes")]
    [SerializeField] private GameObject prefab;
    [SerializeField] private float rotationRadius = 2f;
    [SerializeField] private int numberOfCubes = 1;
    [SerializeField] private float angleIncrement = 3f;

    [Header("Parrent")]
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private Vector3 rotationAxis = new Vector3(0f, 1f, 0f);

    private GameObject[] cubes;

    void Awake()
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab not init");
            return;
        }

        if (numberOfCubes < 0)
        {
            Debug.LogError("Cube count < 0");
            numberOfCubes = 0;
            return;
        }

        // Len delat runtime kolichestvo kubov
        cubes = new GameObject[numberOfCubes];

        for (int i = 0; i < numberOfCubes; i++)
        {
            GameObject cube = Instantiate(prefab, transform);
            cubes[i] = cube;
        }
    }

    void Update()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            float angle = i * angleIncrement;
            Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

            cubes[i].transform.localRotation = rotation;
            cubes[i].transform.localPosition = rotation * new Vector3(0f, 0f, rotationRadius);
        }

        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}