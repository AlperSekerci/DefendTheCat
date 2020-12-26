using UnityEngine;

public class AgentTeam : MonoBehaviour
{
    public enum Gates
    {
        Identity,
        Hadamard,
        Not,
        Swap
    }

    public Gates[] gates = new Gates[2];
}