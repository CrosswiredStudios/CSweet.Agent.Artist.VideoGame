using CSweet.VideoGame.AgentKit;

namespace CSweet.Agent.Artist.VideoGame;

public sealed class SpecialistAgent : VideoGameSpecialistAgentBase
{
    public override string AgentId => "com.csweet.video-game-artist";
    public override string Version => "1.0.0";
    public override string PrimaryCapability => "video-game.artist.execute.v1";
    protected override string RoleKey => "game-artist";
    protected override string ArtifactTypeKey => "video-game.art-asset-delivery.v1";
    protected override string RolePrompt => "Create and curate production assets under the approved provided, procedural, generative, or hybrid asset strategy. Record rights, provider or procedural provenance, hashes, variants, and engine constraints.";
    protected override IReadOnlyList<string> RequiredSections => ["Asset Brief", "Asset Strategy", "Production Details", "Rights and Provenance", "Variants", "Engine Delivery", "Review Evidence"];
}

