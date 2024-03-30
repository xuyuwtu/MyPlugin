using System.Runtime.InteropServices;

using Terraria.ID;
using Terraria.Utilities;

namespace VBY.GameContentModify.PacketStructs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct NeverCalled : IMessageType
{
    public readonly byte MessageType => MessageID.NeverCalled;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Hello : IMessageType
{
    public readonly byte MessageType => MessageID.Hello;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Kick : IMessageType
{
    public readonly byte MessageType => MessageID.Kick;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PlayerInfo : IMessageType
{
    public readonly byte MessageType => MessageID.PlayerInfo;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncPlayer : IMessageType
{
    public readonly byte MessageType => MessageID.SyncPlayer;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncEquipment : IMessageType
{
    public readonly byte MessageType => MessageID.SyncEquipment;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct RequestWorldData : IMessageType
{
    public readonly byte MessageType => MessageID.RequestWorldData;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct WorldData : IMessageType
{
    public readonly byte MessageType => MessageID.WorldData;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SpawnTileData : IMessageType
{
    public readonly byte MessageType => MessageID.SpawnTileData;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct StatusTextSize : IMessageType
{
    public readonly byte MessageType => MessageID.StatusTextSize;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TileSection : IMessageType
{
    public readonly byte MessageType => MessageID.TileSection;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
[Old("Deprecated. Framing happens as needed after TileSection is sent.")]
partial struct TileFrameSection : IMessageType
{
    public readonly byte MessageType => MessageID.TileFrameSection;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PlayerSpawn : IMessageType
{
    public readonly byte MessageType => MessageID.PlayerSpawn;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PlayerControls : IMessageType
{
    public readonly byte MessageType => MessageID.PlayerControls;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PlayerActive : IMessageType
{
    public readonly byte MessageType => MessageID.PlayerActive;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
[Obsolete("Deprecated.")]
partial struct Unknown15 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown15;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PlayerLifeMana : IMessageType
{
    public readonly byte MessageType => MessageID.PlayerLifeMana;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TileManipulation : IMessageType
{
    public readonly byte MessageType => MessageID.TileManipulation;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SetTime : IMessageType
{
    public readonly byte MessageType => MessageID.SetTime;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct ToggleDoorState : IMessageType
{
    public readonly byte MessageType => MessageID.ToggleDoorState;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown20 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown20;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncItem : IMessageType
{
    public readonly byte MessageType => MessageID.SyncItem;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct ItemOwner : IMessageType
{
    public readonly byte MessageType => MessageID.ItemOwner;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncNPC : IMessageType
{
    public readonly byte MessageType => MessageID.SyncNPC;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct UnusedMeleeStrike : IMessageType
{
    public readonly byte MessageType => MessageID.UnusedMeleeStrike;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
[Obsolete("Deprecated. Use NetTextModule instead.")]
partial struct Unused25 : IMessageType
{
    public readonly byte MessageType => MessageID.Unused25;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
[Obsolete("Deprecated.")]
partial struct Unused26 : IMessageType
{
    public readonly byte MessageType => MessageID.Unused26;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncProjectile : IMessageType
{
    public readonly byte MessageType => MessageID.SyncProjectile;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct DamageNPC : IMessageType
{
    public readonly byte MessageType => MessageID.DamageNPC;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct KillProjectile : IMessageType
{
    public readonly byte MessageType => MessageID.KillProjectile;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TogglePVP : IMessageType
{
    public readonly byte MessageType => MessageID.TogglePVP;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct RequestChestOpen : IMessageType
{
    public readonly byte MessageType => MessageID.RequestChestOpen;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncChestItem : IMessageType
{
    public readonly byte MessageType => MessageID.SyncChestItem;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncPlayerChest : IMessageType
{
    public readonly byte MessageType => MessageID.SyncPlayerChest;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct ChestUpdates : IMessageType
{
    public readonly byte MessageType => MessageID.ChestUpdates;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PlayerHeal : IMessageType
{
    public readonly byte MessageType => MessageID.PlayerHeal;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncPlayerZone : IMessageType
{
    public readonly byte MessageType => MessageID.SyncPlayerZone;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct RequestPassword : IMessageType
{
    public readonly byte MessageType => MessageID.RequestPassword;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SendPassword : IMessageType
{
    public readonly byte MessageType => MessageID.SendPassword;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct ReleaseItemOwnership : IMessageType
{
    public readonly byte MessageType => MessageID.ReleaseItemOwnership;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncTalkNPC : IMessageType
{
    public readonly byte MessageType => MessageID.SyncTalkNPC;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct ShotAnimationAndSound : IMessageType
{
    public readonly byte MessageType => MessageID.ShotAnimationAndSound;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown42 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown42;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown43 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown43;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
[Obsolete("Deprecated.")]
partial struct Unknown44 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown44;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown45 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown45;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown46 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown46;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown47 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown47;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
[Obsolete("Deprecated. Use NetLiquidModule instead.")]
partial struct LiquidUpdate : IMessageType
{
    public readonly byte MessageType => MessageID.LiquidUpdate;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct InitialSpawn : IMessageType
{
    public readonly byte MessageType => MessageID.InitialSpawn;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PlayerBuffs : IMessageType
{
    public readonly byte MessageType => MessageID.PlayerBuffs;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct MiscDataSync : IMessageType
{
    public readonly byte MessageType => MessageID.MiscDataSync;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct LockAndUnlock : IMessageType
{
    public readonly byte MessageType => MessageID.LockAndUnlock;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct AddNPCBuff : IMessageType
{
    public readonly byte MessageType => MessageID.AddNPCBuff;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct NPCBuffs : IMessageType
{
    public readonly byte MessageType => MessageID.NPCBuffs;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct AddPlayerBuff : IMessageType
{
    public readonly byte MessageType => MessageID.AddPlayerBuff;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct UniqueTownNPCInfoSyncRequest : IMessageType
{
    public readonly byte MessageType => MessageID.UniqueTownNPCInfoSyncRequest;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown57 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown57;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct InstrumentSound : IMessageType
{
    public readonly byte MessageType => MessageID.InstrumentSound;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct HitSwitch : IMessageType
{
    public readonly byte MessageType => MessageID.HitSwitch;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown60 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown60;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SpawnBossUseLicenseStartEvent : IMessageType
{
    public readonly byte MessageType => MessageID.SpawnBossUseLicenseStartEvent;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown62 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown62;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown63 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown63;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown64 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown64;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TeleportEntity : IMessageType
{
    public readonly byte MessageType => MessageID.TeleportEntity;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown66 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown66;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown67 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown67;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Unknown68 : IMessageType
{
    public readonly byte MessageType => MessageID.Unknown68;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct ChestName : IMessageType
{
    public readonly byte MessageType => MessageID.ChestName;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct BugCatching : IMessageType
{
    public readonly byte MessageType => MessageID.BugCatching;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct BugReleasing : IMessageType
{
    public readonly byte MessageType => MessageID.BugReleasing;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TravelMerchantItems : IMessageType
{
    public readonly byte MessageType => MessageID.TravelMerchantItems;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct RequestTeleportationByServer : IMessageType
{
    public readonly byte MessageType => MessageID.RequestTeleportationByServer;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct AnglerQuest : IMessageType
{
    public readonly byte MessageType => MessageID.AnglerQuest;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct AnglerQuestFinished : IMessageType
{
    public readonly byte MessageType => MessageID.AnglerQuestFinished;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct QuestsCountSync : IMessageType
{
    public readonly byte MessageType => MessageID.QuestsCountSync;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TemporaryAnimation : IMessageType
{
    public readonly byte MessageType => MessageID.TemporaryAnimation;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct InvasionProgressReport : IMessageType
{
    public readonly byte MessageType => MessageID.InvasionProgressReport;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PlaceObject : IMessageType
{
    public readonly byte MessageType => MessageID.PlaceObject;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncPlayerChestIndex : IMessageType
{
    public readonly byte MessageType => MessageID.SyncPlayerChestIndex;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct CombatTextInt : IMessageType
{
    public readonly byte MessageType => MessageID.CombatTextInt;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct NetModules : IMessageType
{
    public readonly byte MessageType => MessageID.NetModules;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct NPCKillCountDeathTally : IMessageType
{
    public readonly byte MessageType => MessageID.NPCKillCountDeathTally;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PlayerStealth : IMessageType
{
    public readonly byte MessageType => MessageID.PlayerStealth;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct QuickStackChests : IMessageType
{
    public readonly byte MessageType => MessageID.QuickStackChests;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TileEntitySharing : IMessageType
{
    public readonly byte MessageType => MessageID.TileEntitySharing;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TileEntityPlacement : IMessageType
{
    public readonly byte MessageType => MessageID.TileEntityPlacement;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct ItemTweaker : IMessageType
{
    public readonly byte MessageType => MessageID.ItemTweaker;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct ItemFrameTryPlacing : IMessageType
{
    public readonly byte MessageType => MessageID.ItemFrameTryPlacing;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct InstancedItem : IMessageType
{
    public readonly byte MessageType => MessageID.InstancedItem;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncEmoteBubble : IMessageType
{
    public readonly byte MessageType => MessageID.SyncEmoteBubble;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncExtraValue : IMessageType
{
    public readonly byte MessageType => MessageID.SyncExtraValue;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SocialHandshake : IMessageType
{
    public readonly byte MessageType => MessageID.SocialHandshake;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Deprecated1 : IMessageType
{
    public readonly byte MessageType => MessageID.Deprecated1;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct MurderSomeoneElsesPortal : IMessageType
{
    public readonly byte MessageType => MessageID.MurderSomeoneElsesPortal;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TeleportPlayerThroughPortal : IMessageType
{
    public readonly byte MessageType => MessageID.TeleportPlayerThroughPortal;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct AchievementMessageNPCKilled : IMessageType
{
    public readonly byte MessageType => MessageID.AchievementMessageNPCKilled;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct AchievementMessageEventHappened : IMessageType
{
    public readonly byte MessageType => MessageID.AchievementMessageEventHappened;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct MinionRestTargetUpdate : IMessageType
{
    public readonly byte MessageType => MessageID.MinionRestTargetUpdate;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TeleportNPCThroughPortal : IMessageType
{
    public readonly byte MessageType => MessageID.TeleportNPCThroughPortal;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct UpdateTowerShieldStrengths : IMessageType
{
    public readonly byte MessageType => MessageID.UpdateTowerShieldStrengths;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct NebulaLevelupRequest : IMessageType
{
    public readonly byte MessageType => MessageID.NebulaLevelupRequest;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct MoonlordHorror : IMessageType
{
    public readonly byte MessageType => MessageID.MoonlordHorror;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct ShopOverride : IMessageType
{
    public readonly byte MessageType => MessageID.ShopOverride;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct GemLockToggle : IMessageType
{
    public readonly byte MessageType => MessageID.GemLockToggle;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PoofOfSmoke : IMessageType
{
    public readonly byte MessageType => MessageID.PoofOfSmoke;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SmartTextMessage : IMessageType
{
    public readonly byte MessageType => MessageID.SmartTextMessage;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct WiredCannonShot : IMessageType
{
    public readonly byte MessageType => MessageID.WiredCannonShot;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct MassWireOperation : IMessageType
{
    public readonly byte MessageType => MessageID.MassWireOperation;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct MassWireOperationPay : IMessageType
{
    public readonly byte MessageType => MessageID.MassWireOperationPay;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct ToggleParty : IMessageType
{
    public readonly byte MessageType => MessageID.ToggleParty;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SpecialFX : IMessageType
{
    public readonly byte MessageType => MessageID.SpecialFX;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct CrystalInvasionStart : IMessageType
{
    public readonly byte MessageType => MessageID.CrystalInvasionStart;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct CrystalInvasionWipeAllTheThingsss : IMessageType
{
    public readonly byte MessageType => MessageID.CrystalInvasionWipeAllTheThingsss;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct MinionAttackTargetUpdate : IMessageType
{
    public readonly byte MessageType => MessageID.MinionAttackTargetUpdate;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct CrystalInvasionSendWaitTime : IMessageType
{
    public readonly byte MessageType => MessageID.CrystalInvasionSendWaitTime;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PlayerHurtV2 : IMessageType
{
    public readonly byte MessageType => MessageID.PlayerHurtV2;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PlayerDeathV2 : IMessageType
{
    public readonly byte MessageType => MessageID.PlayerDeathV2;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct CombatTextString : IMessageType
{
    public readonly byte MessageType => MessageID.CombatTextString;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct Emoji : IMessageType
{
    public readonly byte MessageType => MessageID.Emoji;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TEDisplayDollItemSync : IMessageType
{
    public readonly byte MessageType => MessageID.TEDisplayDollItemSync;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct RequestTileEntityInteraction : IMessageType
{
    public readonly byte MessageType => MessageID.RequestTileEntityInteraction;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct WeaponsRackTryPlacing : IMessageType
{
    public readonly byte MessageType => MessageID.WeaponsRackTryPlacing;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TEHatRackItemSync : IMessageType
{
    public readonly byte MessageType => MessageID.TEHatRackItemSync;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncTilePicking : IMessageType
{
    public readonly byte MessageType => MessageID.SyncTilePicking;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncRevengeMarker : IMessageType
{
    public readonly byte MessageType => MessageID.SyncRevengeMarker;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct RemoveRevengeMarker : IMessageType
{
    public readonly byte MessageType => MessageID.RemoveRevengeMarker;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct LandGolfBallInCup : IMessageType
{
    public readonly byte MessageType => MessageID.LandGolfBallInCup;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct FinishedConnectingToServer : IMessageType
{
    public readonly byte MessageType => MessageID.FinishedConnectingToServer;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct FishOutNPC : IMessageType
{
    public readonly byte MessageType => MessageID.FishOutNPC;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct TamperWithNPC : IMessageType
{
    public readonly byte MessageType => MessageID.TamperWithNPC;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct PlayLegacySound : IMessageType
{
    public readonly byte MessageType => MessageID.PlayLegacySound;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct FoodPlatterTryPlacing : IMessageType
{
    public readonly byte MessageType => MessageID.FoodPlatterTryPlacing;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct UpdatePlayerLuckFactors : IMessageType
{
    public readonly byte MessageType => MessageID.UpdatePlayerLuckFactors;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct DeadPlayer : IMessageType
{
    public readonly byte MessageType => MessageID.DeadPlayer;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncCavernMonsterType : IMessageType
{
    public readonly byte MessageType => MessageID.SyncCavernMonsterType;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct RequestNPCBuffRemoval : IMessageType
{
    public readonly byte MessageType => MessageID.RequestNPCBuffRemoval;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct ClientSyncedInventory : IMessageType
{
    public readonly byte MessageType => MessageID.ClientSyncedInventory;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SetCountsAsHostForGameplay : IMessageType
{
    public readonly byte MessageType => MessageID.SetCountsAsHostForGameplay;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SetMiscEventValues : IMessageType
{
    public readonly byte MessageType => MessageID.SetMiscEventValues;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct RequestLucyPopup : IMessageType
{
    public readonly byte MessageType => MessageID.RequestLucyPopup;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncProjectileTrackers : IMessageType
{
    public readonly byte MessageType => MessageID.SyncProjectileTrackers;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct CrystalInvasionRequestedToSkipWaitTime : IMessageType
{
    public readonly byte MessageType => MessageID.CrystalInvasionRequestedToSkipWaitTime;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct RequestQuestEffect : IMessageType
{
    public readonly byte MessageType => MessageID.RequestQuestEffect;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncItemsWithShimmer : IMessageType
{
    public readonly byte MessageType => MessageID.SyncItemsWithShimmer;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct ShimmerActions : IMessageType
{
    public readonly byte MessageType => MessageID.ShimmerActions;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncLoadout : IMessageType
{
    public readonly byte MessageType => MessageID.SyncLoadout;
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
partial struct SyncItemCannotBeTakenByEnemies : IMessageType
{
    public readonly byte MessageType => MessageID.SyncItemCannotBeTakenByEnemies;
}
