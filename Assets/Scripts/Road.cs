using System.Collections.Generic;
using UnityEngine;
using Element = Warrior.Element;

public class Road : MonoBehaviour
{
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

    private void Update()
    {
        if (Finished) return;
        if (isBattling)
        {
            BattleUpdate();
        }
        else if (WaveManager.Instance.WaveStarted)
        {
            AddAtkUnit();
        }
    }

    private void BattleUpdate()
    {
        float defDps = defInBattle.dps * Time.deltaTime;
        if (IsDefElementAdvantageous(defInBattle.element, atkInBattle.element)) defDps *= ((DefenseUnit)defInBattle).elementAdvDmgBoost;
        atkInBattle.HP -= defDps;
        if (atkInBattle.Dead)
        {
            Destroy(atkInBattle.gameObject);
            attackUnits.Dequeue();
            isBattling = false;
            return;
        }

        defInBattle.HP -= atkInBattle.dps * Time.deltaTime;
        if (defInBattle.Dead)
        {
            Destroy(defInBattle.gameObject);
            defenseUnits.Dequeue();
            isBattling = false;
            return;
        }
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
    }

    private void AddAtkUnit()
    {
        Enemy enemy = WaveManager.Instance.RandomlyPickEnemy();
        if (enemy == null)
        {
            Debug.Log(name + " is finished.");
            Finished = true;
            return;
        } 

        attackUnits.Enqueue(enemy);
        UpdateUnitPositions();
        CheckNewBattle();
    }

    public void AddDefenseUnit(int element)
    {
        if (defenseUnits.Count >= defenseCapacity) return;
        GameObject defUnitObj = Instantiate(defenseUnitPrefab, transform);
        DefenseUnit unit = defUnitObj.GetComponent<DefenseUnit>();
        unit.SetElement((Element)element);
        defenseUnits.Enqueue(unit);
        UpdateUnitPositions();
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