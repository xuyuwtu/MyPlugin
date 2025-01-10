using Terraria;

using TerrariaApi.Server;

using TShockAPI;
using TShockAPI.Hooks;

namespace LifeArmor;
[ApiVersion(2, 1)]
public class LifeArmor : TerrariaPlugin
{
    public override string Name => GetType().Namespace!;
    public override string Author => "yu";
    public override string Description => "生命装备";
    public override Version Version => GetType().Assembly.GetName().Version!;
    private Command AddCommand;
    private Dictionary<int, LifeArmorInfo> ChangeTypeDic;
    public LifeArmor(Main game) : base(game)
    {
        AddCommand = new("lifearmor.admin", Cmd, "lac");
        ChangeTypeDic = new()
        {
            {  727, new(){ index = 0, life = 10 } },
            {  728, new(){ index = 1, life = 20 } },
            {  729, new(){ index = 2, life = 30 } }
        };
    }

    public override void Initialize()
    {
        Commands.ChatCommands.Add(AddCommand);
        GetDataHandlers.PlayerSlot.Register(OnPlayerSlot);
        GeneralHooks.ReloadEvent += OnReload;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Commands.ChatCommands.Remove(AddCommand);
            GetDataHandlers.PlayerSlot.UnRegister(OnPlayerSlot);
            GeneralHooks.ReloadEvent -= OnReload;
        }
    }

    private void OnPlayerSlot(object? sender, GetDataHandlers.PlayerSlotEventArgs e)
    {
        if (e.Handled || e.Player.Dead)
        {
            return;
        }
        if (e.Slot is not >= 59 or not <= 78)
        {
            return;
        }
        var updateIndex = e.Slot - 59;
        var item = e.Player.TPlayer.armor[updateIndex];
        //int statLifeMax = e.Player.TPlayer.statLifeMax;
        int oldItemLife = 0, newItemLife = 0;
        if (ChangeTypeDic.TryGetValue(item.type, out var oldInfo) && oldInfo.IsMatch(updateIndex, item.prefix))
        {
            oldItemLife = oldInfo.life;
        }
        if (ChangeTypeDic.TryGetValue(e.Type, out var newInfo) && newInfo.IsMatch(updateIndex, e.Prefix))
        {
            newItemLife = newInfo.life;
        }
        Console.WriteLine($"slot:{e.Slot}-{updateIndex} type:{item.type}->{e.Type} life:{oldItemLife}->{newItemLife}");
        var addLife = newItemLife - oldItemLife;
        if (addLife != 0)
        {
            //e.Player.TPlayer.statLifeMax = statLifeMax + addLife;
            e.Player.TPlayer.statLifeMax += addLife;
            TSPlayer.All.SendData(PacketTypes.PlayerHp, "", e.Player.Index);
            Console.WriteLine($"life:{e.Player.TPlayer.statLifeMax - addLife}->{e.Player.TPlayer.statLifeMax}");
        }
    }
    private void OnReload(ReloadEventArgs e)
    {
    }

    private void Cmd(CommandArgs args)
    {

    }
}