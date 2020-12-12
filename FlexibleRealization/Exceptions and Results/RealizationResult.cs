

namespace FlexibleRealization
{
    public enum RealizationOutcome
    {
        Success,
        FailedToTransform,
        FailedToBuildSpec
    }

    public class RealizationResult
    {
        public RealizationOutcome Outcome { get; internal set; }

        public string XML { get; internal set; }

        public string Realized { get; internal set; }
    }
}
