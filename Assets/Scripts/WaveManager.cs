using UnityEngine;
using System.Collections.Generic;
using Element = Enemy.Element;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }
    private List<Enemy> wave = new List<Enemy>();
    public int roadCount = 2;
    public GameObject enemyPrefab;    
    public Button startWaveBtn;

    private bool _waveStarted = false;
    public bool WaveStarted
    {
        get { return _waveStarted; }
        set
        {
            _waveStarted = value;
            if (value)
            {
                startWaveBtn.interactable = false;
            }
            else
            {
                startWaveBtn.interactable = true;
            }
        }
    }

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
                enemy.SetElement(element);
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
            // Debug.LogWarning("WaveManager: There are no enemies in the wave.");
            return null;
        }

        int idx = Random.Range(0, wave.Count);
        Enemy enemy = wave[idx];
        wave.RemoveAt(idx);
        Debug.Log("Picked enemy: " + enemy);
        enemy.MakeVisible();
        return enemy;
    }

    public void StartWave()
    {
        if (WaveStarted)
        {
            Debug.LogWarning("WaveManager: Wave is already started.");
            return;
        }

        CreateWave(QiskitHandler.Instance.SampleCircuitOutputs());
        WaveStarted = true;        
    }
}