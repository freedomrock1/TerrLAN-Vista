using UnityEditor;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("Assets/Resources", BuildAssetBundleOptions.CollectDependencies, BuildTarget.StandaloneWindows64);
    }
}