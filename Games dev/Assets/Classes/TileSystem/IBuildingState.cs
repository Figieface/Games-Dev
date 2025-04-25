using UnityEngine;

public interface IBuildingState //interface for building state machine
{
    void EndState();
    void OnAction(Vector3Int gridPosition);
    void UpdateState(Vector3Int gridPosition);
}