using System.Collections.Generic;
using UnityEngine;

public class Branch
{
    public GameObject GameObject;
    public List<Branch> Branches { get; set; } = new List<Branch>();
    public int BranchSection { get; set; }
    public int BranchCount { get; set; }
}