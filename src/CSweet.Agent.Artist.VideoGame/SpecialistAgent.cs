using CrosswiredStudios.VideoGame.AgentKit;
using CSweet.Agent.SDK;
using Microsoft.Extensions.AI;

namespace CSweet.Agent.Artist.VideoGame;

public sealed class SpecialistAgent : VideoGameSpecialistAgentBase
{
    internal const int DefaultContextWindowTokens = 128_000;
    internal const int DefaultOutputTokens = 16_000;
    private const int MinimumOutputTokens = 1_000;
    private const int MaximumOutputTokens = 200_000;
    public override string AgentId => "com.csweet.video-game-artist";
    public override string Version => "2.3.1";
    protected override AgentConfigurationBuilder Configure(AgentConfigurationBuilder builder) =>
        base.Configure(builder)
            .Number("maxContextWindowTokens", "Maximum context-window tokens", required: true,
                description: "Planning ceiling for Artist model requests; set this no higher than the selected model's real context window.",
                minimum: 16_000, maximum: 2_000_000, step: 1_000,
                defaultValue: DefaultContextWindowTokens)
            .Number("maxOutputTokens", "Maximum output tokens", required: true,
                description: "Budget for each Artist model response, including reasoning. The provider may impose a lower ceiling.",
                minimum: MinimumOutputTokens, maximum: MaximumOutputTokens, step: 1_000,
                defaultValue: DefaultOutputTokens,
                lessThanFieldKey: "maxContextWindowTokens");

    protected override ChatOptions? ResponseOptions() =>
        new() { MaxOutputTokens = ResolveOutputTokens(Settings) };

    internal static int ResolveOutputTokens(AgentSettings settings)
    {
        var contextWindow = Math.Max(settings.GetInt32("maxContextWindowTokens", DefaultContextWindowTokens),
            MinimumOutputTokens + 1);
        var output = Math.Clamp(settings.GetInt32("maxOutputTokens", DefaultOutputTokens),
            MinimumOutputTokens, MaximumOutputTokens);
        return Math.Min(output, contextWindow - 1);
    }
    protected override string RoleKey => "game-artist";
    protected override string ArtifactTypeKey => "video-game.art-asset-delivery.v1";
    protected override string RolePrompt => "Create and curate production assets under the approved provided, procedural, generative, or hybrid asset strategy. Record rights, provider or procedural provenance, hashes, variants, and engine constraints.";
    protected override IReadOnlyList<string> RequiredSections => ["Asset Brief", "Asset Strategy", "Production Details", "Rights and Provenance", "Variants", "Engine Delivery", "Review Evidence"];
}
