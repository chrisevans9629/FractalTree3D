public class TreeGeneratorInstant : ITreeGeneratorType
{
    private readonly IBranchCreator _branchCreator;
    private ITreeUpdator _treeUpdator;
    public TreeGeneratorInstant(
        IBranchCreator branchCreator,
        ITreeUpdator treeUpdator)
    {
        _treeUpdator = treeUpdator;
        _branchCreator = branchCreator;
    }

    public void Generate()
    {
        _branchCreator.Generate();
        
    }

    public void Update()
    {
        _treeUpdator.Update();
    }

    
}


