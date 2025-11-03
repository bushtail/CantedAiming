using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Models.Spt.Mod;

namespace CantedAiming;

// ReSharper disable once NotAccessedPositionalProperty.Global
public record Canted(HashSet<MongoId> filter) : NewItemFromCloneDetails
{
    private const string _itemTpl = "69069986473b8fd5e80545f3";
    
    public override MongoId? ItemTplToClone { get; set; } = ItemTpl.COLLIMATOR_WALTHER_MRS_REFLEX_SIGHT;
    public override string? NewId { get; set; } = _itemTpl;
    public override string? ParentId { get; set; } = "55818ad54bdc2ddc698b4569";
    public override string? HandbookParentId { get; set; } = "5b5f742686f774093e6cb4ff";
    public override double? FleaPriceRoubles { get; set; } = 10;
    public override double? HandbookPriceRoubles { get; set; } = 10;
    
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
        ConflictingItems = [ ItemTpl.MOUNT_NCSTAR_MPR45_BACKUP, _itemTpl ],
        EffectiveDistance = 0,
        Ergonomics = 2,
        Weight = 0.0
    };
    
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