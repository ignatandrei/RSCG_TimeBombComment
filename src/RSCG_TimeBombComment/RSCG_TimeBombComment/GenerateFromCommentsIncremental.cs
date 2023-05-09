namespace RSCG_TimeBombComment;
[Generator]
public class GenerateFromCommentsIncremental : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        GenerateFromObsoleteAttribute.Initialize(context);
        GenerateCommentsAsError.Initialize(context);
        
    }

}
