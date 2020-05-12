using System.Collections.Generic;
using UnityEngine;

public interface IBranchCreator
{
    List<Branch> CreateSection(GameObject parent);
}