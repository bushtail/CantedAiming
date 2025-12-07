using JetBrains.Annotations;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Enums;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services.Mod;

namespace CantedAiming;

[Injectable(TypePriority = OnLoadOrder.TraderRegistration - 1), UsedImplicitly]
public class CantedAiming(CustomItemService customItemService, DatabaseServer databaseServer) : IOnLoad
{
    private ItemCantedAiming? _canted;

    private static readonly MongoId[] _rearSightTarget =
    [
        // Barrels
        ItemTpl.BARREL_SVT40_762X54R_625MM,
        ItemTpl.BARREL_MOSIN_CARBINE_762X54R_514MM,
        ItemTpl.BARREL_MOSIN_RIFLE_762X54R_730MM_REGULAR,
        ItemTpl.BARREL_MOSIN_RIFLE_762X54R_SAWEDOFF_200MM,
        ItemTpl.BARREL_MOSIN_RIFLE_762X54R_SAWEDOFF_220MM_THREADED,
        // Weapons
        ItemTpl.MACHINEGUN_DEGTYAREV_RPD_762X39_MACHINE_GUN,
        ItemTpl.MACHINEGUN_DEGTYAREV_RPDN_762X39_MACHINE_GUN
    ];

    private static readonly MongoId _newSchemeId = new("6934ee5f61cb9c71680a3469");
    
    public Task OnLoad()
    {
        var itemsDb = databaseServer.GetTables().Templates.Items;
        var tradersDb = databaseServer.GetTables().Traders;
        HashSet<MongoId>? filters = null;
        if (itemsDb.TryGetValue(ItemTpl.RECEIVER_M4A1_556X45_UPPER, out var upper))
        {
            foreach (var slot in upper.Properties?.Slots!)
            {
                if (slot.Name != "mod_scope") continue;
                foreach (var filter in slot.Properties?.Filters!)
                {
                    filters = filter.Filter!;
                }
            }
        }
        if (filters == null) return Task.CompletedTask;

        // Here lies the removed code adding the rear sights. Bad idea.
        
        _canted = new ItemCantedAiming(filters);
        
        if (itemsDb.TryGetValue(_canted.NewId!, out _))
        {
            _canted.NewId = "6934ece761cb9c71680a3468";
            _canted.Locales!["en"].Name = "Bush's Canted Aim";
            _canted.Locales["en"].ShortName = "BCAim";
            _canted.Locales["en"].Description = "You can attach scopes to this to get canted aim without requiring a backup sight mount. This one is made by bushtail.";
        }
        
        customItemService.CreateItemFromClone(_canted);

        foreach (var tpl in _rearSightTarget)
        {
            if (itemsDb.TryGetValue(tpl, out var item)) AddToRearSight(item);
        }
        
        var newId = new MongoId(_canted.NewId);
        
        foreach (var item in itemsDb.Values.Where(item => _canted.NewId == null || item.Id != newId))
        {
            if (item.Properties == null) continue;
            if (item.Properties.Slots == null) continue;
            foreach (var slot in item.Properties.Slots)
            {
                if (slot.Properties == null || (slot.Name != "mod_scope" && slot.Name != "mod_scope_000" && slot.Name != "mod_scope_001")) continue;
                if (slot.Properties.Filters == null) continue;
                foreach (var filter in slot.Properties.Filters)
                {
                    filter.Filter?.Add(newId);
                }
            }
        }
        
        _canted.filter.Remove(newId);

        if (tradersDb.TryGetValue(Traders.PRAPOR, out var trader)) AddToTrader(trader);
        
        return Task.CompletedTask;
    }

    private void AddToRearSight(TemplateItem input)
    {
        if (_canted == null) return;
        if (input.Properties?.Slots == null) return;
        foreach (var slot in input.Properties.Slots)
        {
            if (slot.Name != "mod_sight_rear" || slot.Properties?.Filters == null) continue;
            foreach (var filter in slot.Properties.Filters)
            {
                filter.Filter?.Add(new MongoId(_canted.NewId));
            }
        }
    }

    private void AddToTrader(Trader trader)
    {
        var assort = trader.Assort;
        var items = assort.Items;
        var barterScheme = assort.BarterScheme;
        items.Add(new Item
            {
                Id = _newSchemeId,
                Template = new MongoId(_canted!.NewId),
                Upd = new Upd
                {
                    StackObjectsCount = 9999999,
                    UnlimitedCount = true,
                    BuyRestrictionMax = 9999999,
                    BuyRestrictionCurrent = 0
                }
            }
        );
            
        barterScheme.Add(_newSchemeId, 
            [
                [ 
                    new BarterScheme
                    {
                        Count = 10,
                        Template = ItemTpl.MONEY_ROUBLES
                    }
                ]
            ]
        );
        
        assort.LoyalLevelItems.Add(_newSchemeId, 1);
    }
}