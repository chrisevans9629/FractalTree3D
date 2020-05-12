using System.Collections.Generic;
using UnityEngine;

public interface IBranchCreator
{
    void Generate();
    List<Branch> CreateSection(GameObject parent);
}