using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Mod;

namespace CantedAiming;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record Canted(HashSet<MongoId> filter) : NewItemFromCloneDetails
{
    public override MongoId? ItemTplToClone { get; set; } = ItemTpl.COLLIMATOR_WALTHER_MRS_REFLEX_SIGHT;
    private const string _itemTpl = "69069986473b8fd5e80545f3";

    public override TemplateItemProperties? OverrideProperties { get; set; } = new()
    {
        Prefab = new Prefab
        {
            Path = "mount_canted_aim.bundle",
            Rcid = ""
        },
        Slots = 
        [
            new Slot
            {
                Id = "6906998c473b8fd5e80545f4",
                MergeSlotWithChildren = false,
                Name = "mod_scope",
                Parent = _itemTpl,
                Properties = new SlotProperties
                {
                    Filters =
                    [
                        new SlotFilter
                        {
                            Filter = filter,
                            Shift = 0
                        }
                    ]
                },
                Prototype = "55d30c4c4bdc2db4468b457e",
                Required = false
            }
        ],
        Accuracy = 0,
        AdjustableOpticSensitivity = 0,
        AdjustableOpticSensitivityMax = 0,
        AnimationVariantsNumber = 0,
        BackgroundColor = "blue",
        BlocksCollapsible = false,
        BlocksFolding = false,
        CalibrationDistances = [[50, 100, 150, 200]],
        CanPutIntoDuringTheRaid = true,
        CanRequireOnRagfair = false,
        CanSellOnRagfair = true,
        CantRemoveFromSlotsDuringRaid = [],
        ConflictingItems = 
        [
            ItemTpl.MOUNT_NCSTAR_MPR45_BACKUP
        ],
        CustomAimPlane = "",
        DiscardLimit = -1,
        DiscardingBlock = false,
        DoubleActionAccuracyPenaltyMult = 1,
        DropSoundType = ItemDropSoundType.None,
        Durability = 100,
        EffectiveDistance = 0,
        Ergonomics = -3,
        ExamineExperience = 2,
        ExamineTime = 1,
        ExaminedByDefault = true,
        ExtraSizeDown = 0,
        ExtraSizeLeft = 0,
        ExtraSizeRight = 0,
        ExtraSizeUp = 0,
        ExtraSizeForceAdd = false,
        ForbidMissingVitalParts = false,
        ForbidNonEmptyContainers = false,
        Grids = [],
        HasShoulderContact = false,
        Height = 1,
        HideEntrails = false,
        InsuranceDisabled = false,
        IsAdjustableOptic = false,
        IsAlwaysAvailableForInsurance = false,
        IsAnimated = false,
        IsLockedAfterEquip = false,
        IsSecretExitRequirement = false,
        IsSpecialSlotOnly = false,
        IsUnbuyable = false,
        IsUndiscardable = false,
        IsUngivable = false,
        IsUnRemovable = false,
        IsUnsaleable = false,
        ItemSound = "mod",
        LootExperience = 10,
        Loudness = 0,
        MergesWithChildren = false,
        MetascoreGroup = "Firepower",
        MinMaxFov = new XYZ
        {
            X = 0,
            Y = 0,
            Z = 0
        },
        QuestItem = false,
        QuestStashMaxCount = 0,
        RagFairCommissionModifier = 1,
        RaidModdable = true,
        RarityPvE = "Common",
        Recoil = 0,
        RepairCost = 0,
        RepairSpeed = 0,
        ScopesCount = 1,
        SightingRange = 200,
        StackMaxSize = 1,
        StackObjectsCount = 1,
        ToolModdable = true,
        UniqueAnimationModID = 0,
        Unlootable = false,
        UnlootableFromSide = [],
        UnlootableFromSlot = "FirstPrimaryWeapon",
        Velocity = 0,
        Weight = 0.113,
        Width = 1,
        ZoomSensitivity = 0,
        Zooms = [[1, 1, 1, 1]],
        SightModType = "reflex"
    };

    public override string? ParentId { get; set; } = "55818ad54bdc2ddc698b4569";
    public override string? NewId { get; set; } = _itemTpl;
    public override double? FleaPriceRoubles { get; set; } = 10;
    public override double? HandbookPriceRoubles { get; set; } = 10;
    public override string? HandbookParentId { get; set; } = "5b5f742686f774093e6cb4ff";

    public override Dictionary<string, LocaleDetails>? Locales { get; set; } = new()
    {
        ["en"] = new LocaleDetails
        {
            Name = "Canted Aim",
            ShortName = "CAim",
            Description = "You can attach scopes to this to get canted aim without requiring a backup sight mount."
        }
    };
}