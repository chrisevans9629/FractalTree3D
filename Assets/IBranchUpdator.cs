using UnityEngine;

public interface IBranchUpdator
{
    GameObject UpdateBranch(GameObject parent, float angle, GameObject currentBranch);

    void Update();
}