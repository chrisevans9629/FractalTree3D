using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;


public class TreeGeneratorAnimated : ITreeGeneratorType
{
    private readonly TreeGeneratorBase _gen;
    public float AnimationSpeed;
    public TreeGeneratorAnimated(TreeGeneratorBase gen)
    {
        _gen = gen;
    }

    public void Generate()
    {
        GameObject.Instantiate(_gen.BranchPrefab, _gen.transform.position, _gen.transform.rotation, _gen.transform);
    }

    public void Update()
    {
    }
}


public class TreeGenerator : TreeGeneratorBase
{
    private ITreeGeneratorType instant;


    // Start is called before the first frame update
    void Start()
    {
        //instant = new TreeGeneratorInstant(new BranchCreator(this, new BranchUpdator(this)), new TreeUpdator(this, new BranchUpdator(this)));
        instant = new TreeGeneratorAnimated(this);

        instant.Generate();
        PreviousRotateAngle = RotateAngle;
        //_previousBranchRatio = BranchRatio;
    }

    //private float _previousBranchRatio;
    // Update is called once per frame
    void Update()
    {
        instant.Update();
    }


}
