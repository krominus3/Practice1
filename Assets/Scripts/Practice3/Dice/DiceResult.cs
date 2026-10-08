using UnityEngine;
using System;

public class DiceResult : MonoBehaviour
{
    [Header("Настройки граней")]
    // В каком порядке проверять грани (Up, Down, Forward, Back, Right, Left)
    private int[] faceValues = { 1, 6, 2, 5, 4, 3 };

    public event Action<int> OnResultCalculated;

    private bool isChecking = false;
    private float checkTimer = 0f;
    private float checkDelay = 0.5f;

    public void StartChecking()
    {
        isChecking = true;
        checkTimer = 0f;
    }

    private void Update()
    {
        if (!isChecking) return;

        // Ждем, пока кубик остановится (нужна ссылка на физику или проверка через Rigidbody)
        Rigidbody rb = GetComponent<Rigidbody>();

        // Если кубик кинематический (спит) или скорость очень маленькая
        if (rb.isKinematic || (rb.linearVelocity.magnitude < 0.1f && rb.angularVelocity.magnitude < 0.1f))
        {
            checkTimer += Time.deltaTime;

            // Если прошло достаточно времени покоя
            if (checkTimer >= checkDelay)
            {
                CalculateResult();
                isChecking = false;
            }
        }
        else
        {
            checkTimer = 0f;
        }
    }

    private void CalculateResult()
    {
        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

        float maxDot = -1f;
        int result = 0;

        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 worldDir = transform.TransformDirection(directions[i]);

            float dot = Vector3.Dot(worldDir, Vector3.up);
            if (dot > maxDot)
            {
                maxDot = dot;
                result = faceValues[i];
            }
        }

        Debug.Log($"<color=green>Выпало: {result}</color>");

        OnResultCalculated?.Invoke(result);
    }
}