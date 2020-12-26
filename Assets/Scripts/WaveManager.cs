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
    public Text waveInfoText;
    public Text waveNumText;
    public Button[] purchaseBtns;
    public Road[] roads;
    public GameObject gameOverObj;
    public Text survivedWavesText;

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
                foreach (Button btn in purchaseBtns) btn.interactable = false;
                foreach (AgentTeam team in QiskitHandler.Instance.agentTeams) team.SetEnable(false);
            }
            else
            {
                startWaveBtn.interactable = true;
                foreach (Button btn in purchaseBtns) btn.interactable = true;
                foreach (AgentTeam team in QiskitHandler.Instance.agentTeams) team.SetEnable(true);
            }
        }
    }

    private int _waveNum = 1;
    public int WaveNumber
    {
        get { return _waveNum; }
        private set
        {
            _waveNum = value;
            waveNumText.text = "Wave " + value;
        }
    }

    public int EnemyCount => wave.Count;

    private void Start()
    {
        Instance = this;
        waveInfoText.text = "";
    }

    private void Update()
    {
        if (WaveStarted)
        {
            CheckRoads();
        }
    }

    private void LateUpdate()
    {
        if (!WaveStarted)
        {
            waveInfoText.text = "preparing";
        }
        else if (wave.Count == 0)
        {
            waveInfoText.text = "";
        }
        else
        {
            waveInfoText.text = wave.Count + " enemies left.";
        }
    }

    private void CheckRoads()
    {
        bool finished = true;
        foreach (Road road in roads)
        {
            if (!road.Finished)
            {
                finished = false;
                break;
            }
        }

        if (!finished) return;
        Debug.Log("Both roads are cleared successfully.");
        PrepareNextWave();
    }

    private void PrepareNextWave()
    {
        WaveStarted = false;
        WaveNumber++;
        waveInfoText.text = "preparing";
        foreach (Road road in roads) road.Finished = false;
        GoldManager.Instance.WavePrepStart();
        foreach (BlochSphere qbit in QiskitHandler.Instance.qbits)
        {
            qbit.SetRandomPoint();
        }
    }

    public void GameOver()
    {
        survivedWavesText.text = (WaveNumber - 1).ToString();
        gameOverObj.SetActive(true);
    }

    public void RestartGame()
    {
        Debug.Log("restart");
        gameOverObj.SetActive(false);
        WaveNumber = 1;
        ClearWave();
        WaveStarted = false;
        GoldManager.Instance.Reset();
        foreach (Road road in roads) road.Reset();
        roads[0].cat.Reset();
        foreach (BlochSphere qbit in QiskitHandler.Instance.qbits)
        {
            qbit.Reset();
        }
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

    public Enemy RandomlyPickEnemy(int road)
    {
        if (wave.Count == 0)
        {
            // Debug.LogWarning("WaveManager: There are no enemies in the wave.");
            return null;
        }

        int idx = Random.Range(0, wave.Count);
        Enemy enemy = wave[idx];
        if (enemy.road != road) return null;
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