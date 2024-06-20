using Terraria;
using TShockAPI;
using TShockAPI.DB;
using VBY.Common.CommandV2;

namespace FlowerSeaRPG;

partial class FlowerSeaRPG
{
    #region Cmd
    private void CmdUpgrade(SubCmdArgs args)
    {
        if (!args.Player.GetFSPlayer(out var fsplayer))
        {
            return;
        }
        //if (args.Parameters.Any() && int.TryParse(args.Parameters[0], out var toLevel) && toLevel > 0)
        //{
        //    fsplayer.Upgrade(toLevel);
        //}
        //else
        //{b
            switch (fsplayer.CanUpgrade(args.Parameters.ElementAtOrDefault(0, ""), out var selectItemIndex))
            {
                case CanUpgradeInfo.ItemEnought:
                    fsplayer.Upgrade(selectItemIndex);
                    break;
                case CanUpgradeInfo.ItemUnEnought:
                    args.Player.SendErrorMessage("升级物品不足");
                    break;
                case CanUpgradeInfo.LevelMax:
                    args.Player.SendErrorMessage("已达最大等级");
                    break;
                case CanUpgradeInfo.SelectIndexStrIsNull:
                    args.Player.SendErrorMessage("此次升级需要输入给与的升级物品ID");
                    break;
                case CanUpgradeInfo.SelectIndexStrIsError:
                    args.Player.SendErrorMessage("给予升级物品ID输入错误");
                    break;
            }
        //
    }
    private void CmdInfo(SubCmdArgs args)
    {
        if (!args.Player.GetFSPlayer(out var fsplayer))
        {
            return;
        }
        args.Player.SendInfoMessage(
            $"等级: {fsplayer.Level}{(fsplayer.Level == MainConfig.MaxLevel ? "(Max)" : "")}\n" +
            $"伤害: {fsplayer.Damage}({fsplayer.Damage * MainConfig.AttributeAddInfo.AddDamage:P0}){(fsplayer.Damage == MainConfig.AttributeAddInfo.DamageMaxCount ? "(Max)" : "")}\n" +
            $"击退: {fsplayer.KnockBack}({fsplayer.KnockBack * MainConfig.AttributeAddInfo.AddKnockBack:P0}){(fsplayer.KnockBack == MainConfig.AttributeAddInfo.KnockBackMaxCount ? "(Max)" : "")}\n" +
            $"大小: {fsplayer.Scale}({fsplayer.Scale * MainConfig.AttributeAddInfo.AddScale:P0}){(fsplayer.Scale == MainConfig.AttributeAddInfo.ScaleMaxCount ? "(Max)" : "")}\n" +
            $"攻速: {fsplayer.Speed}({fsplayer.Speed * MainConfig.AttributeAddInfo.AddSpeed:P0}){(fsplayer.Speed == MainConfig.AttributeAddInfo.SpeedMaxCount ? "(Max)" : "")}\n" +
            $"属性点: {fsplayer.AttributePoints}\n" +
            $"升级物品:{((args.Parameters.Any() && args.Parameters[0].Equals("code", StringComparison.OrdinalIgnoreCase)) ? GradeInfos[fsplayer.Level].GetUpgradeItemsNameStringToGame() : GradeInfos[fsplayer.Level].GetUpgradeItemsIconStringToGame())}\n" +
            $"升级选择可获得物品:\n{GradeInfos[fsplayer.Level].GetGiveItemsString()}");
    }
    private void CmdShimmer(SubCmdArgs args)
    {
        if (!args.Player.GetFSPlayer(out var fsplayer))
        {
            return;
        }
        fsplayer.ShimmerAddDamage = !fsplayer.ShimmerAddDamage;
        fsplayer.Player.SendInfoMessage("微光增伤已{0}", fsplayer.ShimmerAddDamage ? "开启" : "关闭");
    }
    private static void CmdAdd(SubCmdArgs args)
    {
        if (args.Parameters.Count == 0)
        {
            args.Player.SendInfoMessage(Utils.GetMultiString("/fs add", new string[] {
                            "damage|伤害",
                            "knockback|击退",
                            "scale|大小",
                            "speed|攻速" }, "[数量 = 1]"));
            return;
        }
        if (!args.Player.GetFSPlayer(out var fsplayer))
        {
            return;
        }
        var temp = -1;
        ref int updateValue = ref temp;
        int maxCount = 0;
        switch (args.Parameters[0])
        {
            case "da":
            case "damage":
            case "伤害":
                updateValue = ref fsplayer.Damage;
                maxCount = MainConfig.AttributeAddInfo.DamageMaxCount;
                break;
            case "kn":
            case "knockback":
            case "击退":
                updateValue = ref fsplayer.KnockBack;
                maxCount = MainConfig.AttributeAddInfo.KnockBackMaxCount;
                break;
            case "sc":
            case "scale":
            case "大小":
                updateValue = ref fsplayer.Scale;
                maxCount = MainConfig.AttributeAddInfo.ScaleMaxCount;
                break;
            case "sp":
            case "speed":
            case "速度":
                updateValue = ref fsplayer.Speed;
                maxCount = MainConfig.AttributeAddInfo.SpeedMaxCount;
                break;
            default:
                args.Player.SendErrorMessage("未知参数 '{0}'", args.Parameters[1]);
                break;
        }
        if (fsplayer.AttributePoints == 0)
        {
            args.Player.SendInfoMessage("没有属性点就不要来啦");
        }
        if (updateValue == maxCount)
        {
            args.Player.SendInfoMessage("此属性已达到最大值");
            return;
        }
        if (updateValue != -1)
        {
            if (int.TryParse(args.Parameters.GetValueOrInput(1, "1"), out var needPoints) && needPoints > 0)
            {
                if (fsplayer.AttributePoints < needPoints)
                {
                    args.Player.SendInfoMessage("属性点不点{0},已分配全部属性点", needPoints, fsplayer.AttributePoints);
                    needPoints = fsplayer.AttributePoints;
                }
                if (updateValue + needPoints > maxCount)
                {
                    args.Player.SendInfoMessage("此属性最大值为{0},已调整增加点数", maxCount);
                    needPoints = maxCount - updateValue;
                }
                args.Player.SendSuccessMessage("加点成功,消耗属性点{0}", needPoints);
                updateValue += needPoints;
                fsplayer.AttributePoints -= needPoints;
            }
            else
            {
                args.Player.SendErrorMessage("错误的数量");
            }
        }
    }
    private void CmdRefresh(SubCmdArgs args)
    {
        if (!args.Player.GetFSPlayer(out var fsplayer))
        {
            return;
        }
        if (args.Parameters.Any() && args.Parameters[0].Equals("all", StringComparison.OrdinalIgnoreCase))
        {
            var inventory = args.Player.TPlayer.inventory;
            for (int i = 0; i < inventory.Length - 1; i++)
            {
                var bagItem = inventory[i];
                if (bagItem is not null && bagItem.stack > 0 && bagItem.damage != -1)
                {
                    var type = bagItem.type;
                    var stack = bagItem.stack;
                    var prefix = bagItem.prefix;
                    bagItem.SetDefaults(0);
                    TSPlayer.All.SendData(PacketTypes.PlayerSlot, "", args.Player.Index, i);
                    var newItemIndex = Item.NewItem(null, args.Player.TPlayer.Center, bagItem.width, bagItem.height, type, stack, false, prefix);
                    var newItem = Main.item[newItemIndex];
                    newItem.playerIndexTheItemIsReservedFor = args.Player.Index;
                    newItem.whoAmI = newItemIndex;
                    TSPlayer.All.SendData(PacketTypes.ItemOwner, "", newItemIndex);
                    Utils.UpdateItem(fsplayer, newItem);
                }
            }
        }
        else
        {
            var bagItem = args.Player.SelectedItem;
            if (bagItem is not null && bagItem.stack > 0 && bagItem.damage != -1)
            {
                var type = bagItem.type;
                var stack = bagItem.stack;
                var prefix = bagItem.prefix;
                bagItem.SetDefaults(0);
                TSPlayer.All.SendData(PacketTypes.PlayerSlot, "", args.Player.Index, args.Player.TPlayer.selectedItem);
                var newItemIndex = Item.NewItem(null, args.Player.TPlayer.Center, bagItem.width, bagItem.height, type, stack, false, prefix);
                var newItem = Main.item[newItemIndex];
                newItem.playerIndexTheItemIsReservedFor = args.Player.Index; 
                newItem.whoAmI = newItemIndex;
                TSPlayer.All.SendData(PacketTypes.ItemOwner, "", newItemIndex);
                Utils.UpdateItem(fsplayer, newItem);
            }
            else
            {
                args.Player.SendInfoMessage("不可刷新");
            }
        }
    }
    #endregion
    #region Ctl
    public static void CtlReload(TSPlayer player)
    {
        if (Config.Load(player))
        {
            GradeInfos = new GradeInfo[MainConfig.MaxLevel + 1];
            foreach (var configGradeInfo in MainConfig.UpgradeInfo)
            {
                if (configGradeInfo.GetGradeInfo(player, MainConfig.MaxLevel, out var gradeInfos))
                {
                    foreach (var info in gradeInfos)
                    {
                        if (GradeInfos[info.Level] is null)
                        {
                            GradeInfos[info.Level] = info;
                        }
                        else
                        {
                            GradeInfos[info.Level].Update(info);
                        }
                    }
                }
                else
                {
                    player.SendErrorMessage("检测到有等级超过最大等级，不加载");
                }
            }
            NPCSpawnLineInfos.Clear();
            NPCSpawnLineInfos.ForEachAdd(MainConfig.NPCSpawnLineInfos, x => x.Type);
            EventDoCommands.Clear();
            EventDoCommands.ForEachAdd(MainConfig.EventDoCommands, x => x.EventId);
            ChangeConfig.Load(player);
        }
    }
    private void CtlRegionSet(TSPlayer player, int setPoint, int nextPoint = 0)
    {
        player.SendInfoMessage("正在监听点{0}设置", setPoint);
        if (RegionRecord.ContainsKey(player.Index))
        {
            RegionRecord[player.Index].Point = setPoint;
            RegionRecord[player.Index].NextPoint = nextPoint;
        }
        else
        {
            RegionRecord.Add(player.Index, new TileRegionRecord() { Point = setPoint, NextPoint = nextPoint });
        }
        //OnTileEditManager.Register();
    }
    private void CtlRegionFind(SubCmdArgs args)
    {
        var left = args.Parameters.Count < 1;
        var (x, y) = Utils.FindBeachPoint(left, ChangeConfig.FindOffsetX, ChangeConfig.FindOffsetY);
        ChangeConfig.PlaceTileX = x;
        ChangeConfig.PlaceTileY = y;
        ChangeConfig.PlacePoint = left ? "rd" : "ld";
        ChangeConfig.Left = left;
        args.Player.SendInfoMessage($"x:{x} y:{y} point:{ChangeConfig.PlacePoint}");
        if (Utils.GetRegionInfo(ChangeConfig.PlaceFile[left ? 0 : 1], out var regionInfo)) 
        {
            Utils.GetRealPoint(ref x, ref y, regionInfo.Width, regionInfo.Height, ChangeConfig.PlacePoint,regionInfo);
            if(TShock.Regions.AddRegion(x, y, regionInfo.Width, regionInfo.Height, ChangeConfig.RegionName, TSPlayer.Server.Name, Main.worldID.ToString()))
            {
                args.Player.SendInfoMessage("区域 '{0}' 创建成功", ChangeConfig.RegionName);
            }
            else
            {
                args.Player.SendErrorMessage("区域 '{0}' 创建失败", ChangeConfig.RegionName);
            }
            //SwitchCommands.SwitchCommands.database.switchCommandList.Clear();
            //if (left)
            //{
            //    SwitchCommands.SwitchCommands.database.switchCommandList.Add(new SwitchCommands.SwitchPos(x + ChangeConfig.SwitchOffsetX, y + ChangeConfig.SwitchOffsetY).ToString(), new SwitchCommands.CommandInfo() { commandList = ChangeConfig.SwitchCommands });
            //}
            //else
            //{
            //    SwitchCommands.SwitchCommands.database.switchCommandList.Add(new SwitchCommands.SwitchPos(x + regionInfo.Width - 1 - ChangeConfig.SwitchOffsetX, y + ChangeConfig.SwitchOffsetY).ToString(), new SwitchCommands.CommandInfo() { commandList = ChangeConfig.SwitchCommands });
            //}
            //SwitchCommands.SwitchCommands.database.Write(SwitchCommands.Database.databasePath);
            ChangeConfig.Save();
        }
        else
        {
            args.Player.SendInfoMessage("从 '{0}' 获取区域信息失败", ChangeConfig.PlaceFile[left ? 0 : 1]);
        }
    }
    private void CtlRegionLoad(SubCmdArgs args)
    {
        if (!RegionRecord.ContainsKey(args.Player.Index))
        {
            args.Player.SendInfoMessage("没有记录点信息");
            return;
        }
        var record = RegionRecord[args.Player.Index];
        if (record.Point1 is not null)
        {
            RegionRecord.Remove(args.Player.Index);
            var (success, errorMessgae) = Utils.LoadRegionData(record.Point1.X, record.Point1.Y, args.Parameters[0], args.Parameters.ElementAtOrDefault(1, "lu").ToLower(), args.Parameters.Contains("-se"));
            if (success)
            {
                args.Player.SendInfoMessage("加载成功");
            }
            else
            {
                args.Player.SendErrorMessage(errorMessgae);
            }
        }
        else
        {
            args.Player.SendInfoMessage("point1 not set");
        }
    }
    private void CtlRegionSave(SubCmdArgs args)
    {
        if (!RegionRecord.ContainsKey(args.Player.Index))
        {
            args.Player.SendInfoMessage("没有记录点信息");
            return;
        }
        var record = RegionRecord[args.Player.Index];
        if (record.Point1 is not null && record.Point2 is not null)
        {
            RegionRecord.Remove(args.Player.Index);
            var info = Utils.SaveRegionData(Utils.GetRectangle(record.Point1, record.Point2), args.Parameters[0], record);
            args.Player.SendSuccessMessage("保存成功,到文件:{0} 宽:{1} 高:{2}", args.Parameters[0], info.Width, info.Height);
        }
        else
        {
            args.Player.SendInfoMessage("no enought point");
        }
    }
    private void CtlRegionSpecialadd(SubCmdArgs args)
    {
        if (!RegionRecord.ContainsKey(args.Player.Index))
        {
            args.Player.SendInfoMessage("没有记录点信息");
            return;
        }
        var record = RegionRecord[args.Player.Index];
        if(record.Point1 is null)
        {
            args.Player.SendInfoMessage("点1未设置");
            return;
        }
        if (record.Point2 is null)
        {
            args.Player.SendInfoMessage("点2未设置");
            return;
        }
        if (record.Point3 is null)
        {
            args.Player.SendInfoMessage("特殊点未设置");
            return;
        }
        var rectangle = Utils.GetRectangle(record.Point1, record.Point2);
        if (!rectangle.Contains(record.Point3))
        {
            args.Player.SendInfoMessage("点不在范围内");
            return;
        }
        record.PointName = args.Parameters[0];
        args.Player.SendInfoMessage($"设置点3名称已设置为{args.Parameters[0]}");
    }
    private void CtlDel(SubCmdArgs args)
    {
        var name = args.Parameters[0];
        bool del = false;
        using (var reader = DB.QueryReader(Utils.SelectStr[nameof(TableInfo.FSPlayerInfo)], name))
        {
            if (reader.Read())
            {
                List<FSPlayer> find = Players.Where(x => x?.Player.Name == name).ToList()!;
                if (find.Any())
                {
                    find[0].Clear();
                    find[0].Player.SendInfoMessage("管理员清除了你的数据");
                }
                del = true;
            }
            else
            {
                args.Player.SendInfoMessage("玩家 '{0}' 数据不存在", name);
            }
        }
        if (del)
        {
            DB.Query(Utils.UpdateStr[nameof(TableInfo.FSPlayerInfo)], name, 0, 0, 0, 0, 0, 0);
            args.Player.SendInfoMessage("删除成功");
        }

    }
    #endregion
}
