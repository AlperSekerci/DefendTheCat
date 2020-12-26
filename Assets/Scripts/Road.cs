using System.Collections.Generic;
using UnityEngine;
using Element = Warrior.Element;

public class Road : MonoBehaviour
{
    private int totalCapacity;
    public int defenseCapacity = 20;    
    private const int MIN_ATK_CAPACITY = 1;
    public float unitSpace = 30;
    public float unitOffset = -50;

    private Queue<Warrior> defenseUnits = new Queue<Warrior>();
    private Queue<Warrior> attackUnits = new Queue<Warrior>();
    private Warrior defInBattle = null;
    private Warrior atkInBattle = null;
    private bool isBattling = false;
    public GameObject defenseUnitPrefab;
    public Warrior cat;
    public bool Finished { get; private set; } = false;
    public int id;
    
    private void Start()
    {
        totalCapacity = defenseCapacity + MIN_ATK_CAPACITY;
    }

    private void Update()
    {
        if (Finished) return;
        if (WaveManager.Instance.WaveStarted && (attackUnits.Count + defenseUnits.Count) < totalCapacity)
        {
            AddAtkUnit();
        }
        if (isBattling)
        {
            BattleUpdate();
        }
        else
        {
            CheckNewBattle();
        }
    }

    private void BattleUpdate()
    {
        if (defInBattle == null || defInBattle.Dead) // when battling the cat
        {
            isBattling = false;
            GameOver();
            return;
        }

        float defDps = defInBattle.dps * Time.deltaTime;
        if (IsDefElementAdvantageous(defInBattle.element, atkInBattle.element)) defDps *= ((DefenseUnit)defInBattle).elementAdvDmgBoost;
        atkInBattle.HP -= defDps;
        if (atkInBattle.Dead)
        {
            Destroy(atkInBattle.gameObject);
            attackUnits.Dequeue();
            isBattling = false;
            if (attackUnits.Count == 0 && WaveManager.Instance.EnemyCount == 0)
            {
                Finished = true;
                Debug.Log(name + " is finished.");
            }
            return;
        }

        defInBattle.HP -= atkInBattle.dps * Time.deltaTime;
        if (defInBattle.Dead)
        {
            Destroy(defInBattle.gameObject);
            if (defenseUnits.Count > 0) defenseUnits.Dequeue(); // if not battling the cat
            isBattling = false;
            return;
        }
    }

    private void GameOver()
    {
        Finished = true;
        Debug.Log("Cat is dead. Game over.");
        WaveManager.Instance.GameOver();
    }

    public static bool IsDefElementAdvantageous(Element defElem, Element atkElem)
    {
        return (defElem == Element.Fire && atkElem == Element.Water) ||
            (defElem == Element.Water && atkElem == Element.Fire);
    }

    private void CheckNewBattle()
    {
        if (isBattling) return;
        if (attackUnits.Count == 0)
        {
            return;
        }

        atkInBattle = attackUnits.Peek();
        if (defenseUnits.Count > 0)
        {
            defInBattle = defenseUnits.Peek();
        }
        else
        {
            defInBattle = cat;
        }

        isBattling = true;
        UpdateUnitPositions();
    }

    private void AddAtkUnit()
    {
        Enemy enemy = WaveManager.Instance.RandomlyPickEnemy(id);
        if (enemy == null)
        {   
            return;
        } 

        attackUnits.Enqueue(enemy);
        UpdateUnitPositions();
        CheckNewBattle();
    }

    public void AddDefenseUnit(int element)
    {
        if (defenseUnits.Count >= defenseCapacity) return;
        if (!GoldManager.Instance.CanBuyUnit()) return;
        GameObject defUnitObj = Instantiate(defenseUnitPrefab, transform);
        DefenseUnit unit = defUnitObj.GetComponent<DefenseUnit>();
        unit.SetElement((Element)element);
        defenseUnits.Enqueue(unit);
        UpdateUnitPositions();
        GoldManager.Instance.PurchaseUnit();
    }

    public void UpdateUnitPositions()
    {
        float leftmostDefX = transform.position.x - defenseUnits.Count * unitSpace + unitOffset;
        float x = leftmostDefX;
        foreach (Warrior unit in defenseUnits)
        {
            unit.SetTargetPos(new Vector3(x, transform.position.y, transform.position.z));
            x += unitSpace;
        }

        x = leftmostDefX - unitSpace;
        foreach (Warrior unit in attackUnits)
        {
            unit.SetTargetPos(new Vector3(x, transform.position.y, transform.position.z));
            x -= unitSpace;
        }
    }
}