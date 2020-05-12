public class TreeGeneratorInstant : ITreeGeneratorType
{
    private readonly TreeGeneratorBase _gen;
    private readonly IBranchCreator _branchCreator;
    private IBranchUpdator _branchUpdator;
    public TreeGeneratorInstant(TreeGeneratorBase gen)
    {
        _gen = gen;
        _branchUpdator = new BranchUpdator(gen);
        _branchCreator = new BranchCreator(gen, _branchUpdator);
    }


    public void Generate()
    {
        var obj = UnityEngine.Object.Instantiate(_gen.BranchPrefab, _gen.transform.position, _gen.transform.rotation, _gen.transform);
        _gen.Branch = new Branch
        {
            GameObject = obj,
            Branches = _branchCreator.CreateSection(obj),
        };
    }

    public void Update()
    {
        _branchUpdator.Update();
    }

    
}