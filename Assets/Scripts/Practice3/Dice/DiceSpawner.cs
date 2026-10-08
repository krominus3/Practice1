using UnityEngine;
using System.Collections.Generic;
using System;

public class DiceSpawner : MonoBehaviour
{
    [Header("Префаб кубика")]
    [SerializeField] private GameObject dicePrefab;

    [Header("Настройки спавна")]
    [SerializeField] private int diceCount = 2;
    [SerializeField] private float spawnRadius = 2f;
    [SerializeField] private float spawnHeight = 1f;

    private List<Dice> spawnedDice = new List<Dice>();

    public IReadOnlyList<Dice> SpawnedDice => spawnedDice;

    public event Action<Dice> OnDiceSpawned;
    public event Action OnAllDiceCleared;

    private void Start()
    {
        SpawnDice(diceCount);
    }

    public void SetDiceCount(int newCount)
    {
        if (newCount < 1) newCount = 1;
        if (newCount > 10) newCount = 10;

        diceCount = newCount;
        RespawnDice();
    }

    private void SpawnDice(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPos = GetRandomSpawnPosition();
            GameObject newDiceObj = Instantiate(dicePrefab, randomPos, UnityEngine.Random.rotation);

            Dice dice = newDiceObj.GetComponent<Dice>();
            if (dice != null)
            {
                spawnedDice.Add(dice);

                OnDiceSpawned?.Invoke(dice);
            }
        }
    }

    private void RespawnDice()
    {
        OnAllDiceCleared?.Invoke();

        foreach (var dice in spawnedDice)
        {
            if (dice != null) Destroy(dice.gameObject);
        }
        spawnedDice.Clear();

        SpawnDice(diceCount);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle * spawnRadius;
        return new Vector3(randomCircle.x, spawnHeight, randomCircle.y);
    }

    public void ThrowAllDice()
    {
        foreach (var dice in spawnedDice)
        {
            if (dice != null) dice.Throw();
        }
    }
}