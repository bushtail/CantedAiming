using JetBrains.Annotations;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services.Mod;

namespace CantedAiming;

[UsedImplicitly]
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 128)]
public class CantedAiming(CustomItemService customItemService, DatabaseServer databaseServer) : IOnLoad
{
    private Canted? _canted;
    public Task OnLoad()
    {
        var itemsDb = databaseServer.GetTables().Templates.Items;
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
        filters.Add(ItemTpl.IRONSIGHT_SVT40_REAR_SIGHT);
        filters.Add(ItemTpl.IRONSIGHT_RPD_REAR_SIGHT);
        
        _canted = new Canted(filters);
        customItemService.CreateItemFromClone(_canted);
        
        if (itemsDb.TryGetValue(ItemTpl.BARREL_SVT40_762X54R_625MM, out var barrel)) AddToRearSight(barrel);
        if (itemsDb.TryGetValue(ItemTpl.MACHINEGUN_DEGTYAREV_RPD_762X39_MACHINE_GUN, out var rpd)) AddToRearSight(rpd);
        if (itemsDb.TryGetValue(ItemTpl.MACHINEGUN_DEGTYAREV_RPDN_762X39_MACHINE_GUN, out var rpdn)) AddToRearSight(rpdn);
        
        foreach (var item in itemsDb.Values.Where(item => _canted.NewId == null || item.Id != _canted.NewId))
        {
            if (item.Properties == null) continue;
            if (item.Properties.Slots == null) continue;
            foreach (var slot in item.Properties.Slots)
            {
                if (slot.Properties == null || (slot.Name != "mod_scope" && slot.Name != "mod_scope_000" && slot.Name != "mod_scope_001")) continue;
                if (slot.Properties.Filters == null) continue;
                foreach (var filter in slot.Properties.Filters)
                {
                    filter.Filter?.Add(new MongoId(_canted.NewId));
                }
            }
        }
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
}