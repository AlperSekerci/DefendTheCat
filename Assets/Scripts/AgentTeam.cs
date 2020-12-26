using UnityEngine;
using UnityEngine.UI;

public class AgentTeam : MonoBehaviour
{
    public enum Gates
    {
        Identity,        
        Not,
        Hadamard,
        Swap
    }
    public Gates[] gates = new Gates[2];

    #region UI
    public GameObject selectBoardObj;
    public Button[] agentButtons;
    public Sprite[] gateImages;

    private int selectedAgentIdx = -1;
    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickAgent(-1);
        }
    }

    public void OnClickAgent(int idx)
    {
        if (selectedAgentIdx == idx || idx < 0)
        {
            selectBoardObj.SetActive(false);
            selectedAgentIdx = -1;
            return;
        }

        Vector3 selectBoardPos = selectBoardObj.transform.position;
        selectBoardPos.y = agentButtons[idx].transform.position.y;
        selectBoardObj.transform.position = selectBoardPos;
        selectBoardObj.SetActive(true);
        selectedAgentIdx = idx;
    }

    public void ReplaceAgent(int gate)
    {
        if (selectedAgentIdx < 0) return;
        gates[selectedAgentIdx] = (Gates)gate;
        agentButtons[selectedAgentIdx].image.sprite = gateImages[gate];
        int otherGateIdx = 1 - selectedAgentIdx; // TODO: only works for 2 qbits
        if (gate == (int)Gates.Swap && gates[otherGateIdx] != Gates.Swap)
        {
            selectedAgentIdx = otherGateIdx;
            ReplaceAgent((int)Gates.Swap);
        }
        else if (gates[otherGateIdx] == Gates.Swap && gate != (int)Gates.Swap)
        {
            selectedAgentIdx = otherGateIdx;
            ReplaceAgent((int)Gates.Identity);
        }

        OnClickAgent(-1);
    }
}