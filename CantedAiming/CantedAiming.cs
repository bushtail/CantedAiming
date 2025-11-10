using JetBrains.Annotations;
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Servers;
using SPTarkov.Server.Core.Services.Mod;

namespace CantedAiming;

[UsedImplicitly]
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class CantedAiming(CustomItemService customItemService, DatabaseServer databaseServer) : IOnLoad
{
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
        filters.Add(ItemTpl.RECEIVER_M4A1_556X45_UPPER);
        var canted = new Canted(filters);
        customItemService.CreateItemFromClone(canted);
        if (itemsDb.TryGetValue(ItemTpl.BARREL_SVT40_762X54R_625MM, out var barrel))
        {
            if (barrel.Properties?.Slots != null)
            {
                foreach (var slot in barrel.Properties.Slots)
                {
                    if (slot.Name != "mod_sight_rear" || slot.Properties?.Filters == null) continue;
                    foreach (var filter in slot.Properties.Filters)
                    {
                        filter.Filter?.Add(new MongoId(canted.NewId));
                    }
                }
            }
        }
        foreach (var item in itemsDb.Values.Where(item => canted.NewId == null || item.Id != canted.NewId))
        {
            if (item.Properties == null) continue;
            if (item.Properties.Slots == null) continue;
            foreach (var slot in item.Properties.Slots)
            {
                if (slot.Properties == null || (slot.Name != "mod_scope" && slot.Name != "mod_scope_000" && slot.Name != "mod_scope_001")) continue;
                if (slot.Properties.Filters == null) continue;
                foreach (var filter in slot.Properties.Filters)
                {
                    filter.Filter?.Add(new MongoId(canted.NewId));
                }
            }
        }
        return Task.CompletedTask;
    }
}