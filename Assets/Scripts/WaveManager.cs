using UnityEngine;
using System.Collections.Generic;
using Element = Enemy.Element;

public class WaveManager : MonoBehaviour
{
    public WaveManager Instance { get; private set; }
    private List<Enemy> wave = new List<Enemy>();
    public int roadCount = 2;
    public GameObject enemyPrefab;

    private void Start()
    {
        Instance = this;
    }

    public void CreateWave(byte[] enemyCounts) // road & element combined
    {
        ClearWave();
        for (int enemyType = 0; enemyType < enemyCounts.Length; ++enemyType)
        {
            int road = enemyType / (int)Element.Count;
            Element element = (Element)(enemyType % (int)Element.Count);

            for (int i = 0; i < enemyCounts[enemyType]; ++i)
            {   
                GameObject enemyObj = Instantiate(enemyPrefab, transform);
                Enemy enemy = enemyObj.GetComponent<Enemy>();
                enemy.road = road;
                enemy.element = element;
                enemy.name = "Enemy[Road_" + (road + 1) + "][Element_" + element + "]";
                wave.Add(enemy);
            }
        }
    }

    public void ClearWave()
    {
        int childCt = transform.childCount;
        for (int i = 0; i < childCt; ++i)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        wave.Clear();
    }

    public Enemy RandomlyPickEnemy()
    {
        if (wave.Count == 0)
        {
            Debug.LogWarning("WaveManager: There are no enemies in the wave.");
            return null;
        }

        int idx = Random.Range(0, wave.Count);
        Enemy enemy = wave[idx];
        wave.RemoveAt(idx);
        Debug.Log("Picked enemy: " + enemy);
        return enemy;
    }

    public void StartWave()
    {
        CreateWave(QiskitHandler.Instance.SampleCircuitOutputs());
        int waveCt = wave.Count;
        for (int i = 0; i < waveCt; ++i)
        {
            RandomlyPickEnemy();
        }
    }
}