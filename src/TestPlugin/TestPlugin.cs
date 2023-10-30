using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.NetModules;
using Terraria.ID;
using Terraria.Net;
using System.Diagnostics;
using TerrariaApi.Server;
using TShockAPI;
using Terraria.DataStructures;

namespace TestPlugin;
[ApiVersion(2, 1)]
public class TestPlugin : TerrariaPlugin
{
    public int count;
    public TestPlugin(Main game) : base(game)
    {
        Commands.ChatCommands.Add(new(args =>
        {
            //NetManager.Instance.Broadcast(NetParticlesModule.Serialize(ParticleOrchestraType.ChlorophyteLeafCrystalPassive,
            //    new ParticleOrchestraSettings() { 
            //        PositionInWorld = args.Player.TPlayer.position,
            //     }));
            var plr = Main.player.FirstOrDefault(x => x.active);
            if (plr != null)
            {
                //int x = 34;
                //int y = 12;
                //var leftPoint = plr.position - new Vector2(16 * x / 2, 16 * y);
                //kotia
                //foreach (var point in new Point[] {
                //    new(0, 0), new(0, 1), new(0, 2), new(0, 3), new(0, 4), new(0, 5), new(0, 6), new(0, 7), new(0, 8), new(0, 9), new(0, 10), new(0, 11),
                //    new(1, 0), new(1, 1), new(1, 2), new(1, 3), new(1, 4), new(1, 5), new(1, 6), new(1, 7), new(1, 8), new(1, 9), new(1, 10), new(1, 11),
                //    new(2, 7), new(2, 8), new(3, 6), new(3, 9), new(4, 5), new(4, 10), new(5, 4), new(5, 11), new(8, 6), new(8, 7), new(8, 8), new(8, 9),
                //    new(9, 5), new(9, 10), new(10, 4), new(10, 11), new(11, 4), new(11, 11), new(12, 4), new(12, 11), new(13, 4), new(13, 11), new(14, 5),
                //    new(14, 10), new(15, 6), new(15, 7), new(15, 8), new(15, 9), new(17, 4), new(18, 4), new(19, 2), new(19, 3), new(19, 4), new(19, 5),
                //    new(19, 6), new(19, 7), new(19, 8), new(19, 9), new(19, 10), new(19, 11), new(20, 4), new(20, 11), new(21, 4), new(21, 11), new(24, 0),
                //    new(24, 1), new(24, 4), new(24, 5), new(24, 6), new(24, 7), new(24, 8), new(24, 9), new(24, 10), new(24, 11), new(25, 0), new(25, 1),
                //    new(25, 4), new(25, 5), new(25, 6), new(25, 7), new(25, 8), new(25, 9), new(25, 10), new(25, 11), new(28, 5), new(28, 9), new(28, 10),
                //    new(29, 4), new(29, 8), new(29, 11), new(30, 4), new(30, 7), new(30, 11), new(31, 4), new(31, 7), new(31, 11), new(32, 4), new(32, 7),
                //    new(32, 10), new(33, 5), new(33, 6), new(33, 7), new(33, 8), new(33, 9), new(33, 10), new(33, 11) })
                //{
                //    NetManager.Instance.Broadcast(NetParticlesModule.Serialize(ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings() { PositionInWorld = leftPoint + point.ToWorldCoordinates() }));
                //}

                //thtrs
                //                foreach(var point in new Point[]
                //                {
                //                    new(1, 4), new(2, 2), new(2, 3), new(2, 4), new(2, 5), new(2, 6), new(2, 7), new(2, 8), new(2, 9), new(2, 10), new(3, 4), new(3, 11),
                //new(4, 4), new(4, 11), new(8, 0), new(8, 1), new(8, 2), new(8, 3), new(8, 4), new(8, 5), new(8, 6), new(8, 7), new(8, 8), new(8, 9),
                //new(8, 10), new(8, 11), new(9, 5), new(10, 4), new(11, 4), new(12, 4), new(13, 5), new(13, 6), new(13, 7), new(13, 8), new(13, 9), new(13, 10),
                //new(13, 11), new(16, 4), new(17, 2), new(17, 3), new(17, 4), new(17, 5), new(17, 6), new(17, 7), new(17, 8), new(17, 9), new(17, 10), new(18, 4),
                //new(18, 11), new(19, 4), new(19, 11), new(23, 4), new(23, 5), new(23, 6), new(23, 7), new(23, 8), new(23, 9), new(23, 10), new(23, 11), new(24, 5),
                //new(25, 4), new(26, 4), new(27, 10), new(28, 5), new(28, 6), new(28, 7), new(28, 11), new(29, 4), new(29, 7), new(29, 11), new(30, 4), new(30, 7),
                //new(30, 8), new(30, 11), new(31, 4), new(31, 8), new(31, 11), new(32, 5), new(32, 9), new(32, 10)
                //                })
                //                {
                //                    NetManager.Instance.Broadcast(NetParticlesModule.Serialize(ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings() { PositionInWorld = leftPoint + point.ToWorldCoordinates() }));
                //                }

                int x = 79;
                int y = 61;
                var leftPoint = plr.position - new Vector2(16 * x / 2, 16 * y / 2);
                foreach (var point in new Point[]
                {
                    new(0, 41), new(0, 42), new(1, 13), new(1, 20), new(1, 40), new(1, 41), new(1, 42), new(2, 16), new(2, 17), new(2, 39), new(2, 40), new(2, 41),
new(3, 16), new(3, 17), new(3, 19), new(3, 37), new(3, 38), new(3, 39), new(3, 40), new(3, 42), new(4, 15), new(4, 16), new(4, 17), new(4, 34),
new(4, 35), new(4, 36), new(4, 37), new(4, 38), new(4, 39), new(4, 40), new(4, 41), new(5, 15), new(5, 16), new(5, 17), new(5, 18), new(5, 19),
new(5, 20), new(5, 21), new(5, 22), new(5, 23), new(5, 24), new(5, 25), new(5, 26), new(5, 27), new(5, 28), new(5, 29), new(5, 30), new(5, 31),
new(5, 32), new(5, 33), new(5, 34), new(5, 35), new(5, 36), new(5, 37), new(5, 38), new(5, 39), new(6, 14), new(6, 15), new(6, 16), new(6, 17),
new(6, 18), new(6, 19), new(6, 20), new(6, 21), new(6, 22), new(6, 23), new(6, 24), new(6, 25), new(6, 26), new(6, 27), new(6, 28), new(6, 29),
new(6, 30), new(6, 31), new(6, 32), new(6, 33), new(6, 34), new(6, 35), new(6, 36), new(6, 37), new(6, 38), new(7, 11), new(7, 12), new(7, 13),
new(7, 14), new(7, 15), new(7, 16), new(7, 17), new(7, 18), new(7, 19), new(7, 20), new(7, 21), new(7, 22), new(7, 23), new(7, 24), new(7, 25),
new(7, 26), new(7, 27), new(7, 28), new(7, 29), new(7, 30), new(7, 31), new(7, 32), new(7, 33), new(7, 34), new(7, 35), new(7, 36), new(7, 37),
new(7, 40), new(8, 10), new(8, 11), new(8, 12), new(8, 13), new(8, 14), new(8, 15), new(8, 16), new(8, 17), new(8, 18), new(8, 19), new(8, 20),
new(8, 21), new(8, 22), new(8, 23), new(8, 24), new(8, 25), new(8, 26), new(8, 27), new(8, 28), new(8, 29), new(8, 30), new(8, 31), new(8, 32),
new(8, 33), new(8, 34), new(8, 35), new(8, 36), new(8, 38), new(8, 39), new(9, 12), new(9, 13), new(9, 14), new(9, 15), new(9, 16), new(9, 17),
new(9, 18), new(9, 19), new(9, 20), new(9, 21), new(9, 22), new(9, 23), new(9, 24), new(9, 25), new(9, 26), new(9, 27), new(9, 28), new(9, 29),
new(9, 30), new(9, 31), new(9, 32), new(9, 33), new(9, 34), new(9, 35), new(9, 37), new(9, 38), new(9, 39), new(9, 42), new(10, 14), new(10, 15),
new(10, 16), new(10, 17), new(10, 18), new(10, 19), new(10, 20), new(10, 21), new(10, 22), new(10, 23), new(10, 24), new(10, 25), new(10, 26), new(10, 27),
new(10, 28), new(10, 29), new(10, 30), new(10, 31), new(10, 32), new(10, 33), new(10, 36), new(10, 37), new(10, 38), new(10, 39), new(10, 40), new(11, 15),
new(11, 16), new(11, 17), new(11, 18), new(11, 19), new(11, 20), new(11, 21), new(11, 22), new(11, 23), new(11, 24), new(11, 25), new(11, 26), new(11, 27),
new(11, 28), new(11, 30), new(11, 36), new(11, 37), new(11, 38), new(11, 39), new(11, 40), new(11, 41), new(11, 43), new(12, 15), new(12, 16), new(12, 17),
new(12, 21), new(12, 22), new(12, 23), new(12, 24), new(12, 25), new(12, 26), new(12, 27), new(12, 28), new(12, 29), new(12, 30), new(12, 31), new(12, 32),
new(12, 33), new(12, 35), new(12, 36), new(12, 37), new(12, 38), new(12, 39), new(12, 40), new(12, 41), new(13, 15), new(13, 16), new(13, 17), new(13, 20),
new(13, 21), new(13, 22), new(13, 23), new(13, 24), new(13, 25), new(13, 26), new(13, 27), new(13, 28), new(13, 29), new(13, 30), new(13, 31), new(13, 32),
new(13, 33), new(13, 35), new(13, 36), new(13, 37), new(13, 38), new(13, 39), new(13, 40), new(13, 41), new(14, 14), new(14, 15), new(14, 16), new(14, 17),
new(14, 20), new(14, 21), new(14, 22), new(14, 23), new(14, 24), new(14, 25), new(14, 26), new(14, 27), new(14, 28), new(14, 29), new(14, 30), new(14, 31),
new(14, 32), new(14, 33), new(14, 35), new(14, 36), new(14, 37), new(14, 38), new(14, 39), new(14, 41), new(14, 42), new(14, 43), new(15, 15), new(15, 16),
new(15, 17), new(15, 20), new(15, 21), new(15, 22), new(15, 23), new(15, 24), new(15, 25), new(15, 26), new(15, 27), new(15, 28), new(15, 29), new(15, 30),
new(15, 31), new(15, 32), new(15, 34), new(15, 35), new(15, 36), new(15, 37), new(15, 38), new(15, 41), new(15, 42), new(16, 15), new(16, 16), new(16, 17),
new(16, 20), new(16, 21), new(16, 22), new(16, 23), new(16, 24), new(16, 25), new(16, 26), new(16, 27), new(16, 28), new(16, 29), new(16, 30), new(16, 31),
new(16, 32), new(16, 34), new(16, 35), new(16, 36), new(16, 37), new(16, 38), new(16, 41), new(16, 42), new(17, 15), new(17, 16), new(17, 17), new(17, 20),
new(17, 21), new(17, 22), new(17, 23), new(17, 24), new(17, 25), new(17, 26), new(17, 27), new(17, 28), new(17, 29), new(17, 30), new(17, 31), new(17, 34),
new(17, 35), new(17, 36), new(17, 37), new(17, 38), new(17, 41), new(17, 42), new(18, 15), new(18, 16), new(18, 17), new(18, 21), new(18, 22), new(18, 23),
new(18, 24), new(18, 25), new(18, 26), new(18, 27), new(18, 28), new(18, 29), new(18, 30), new(18, 31), new(18, 34), new(18, 35), new(18, 36), new(18, 37),
new(18, 38), new(19, 15), new(19, 16), new(19, 17), new(19, 21), new(19, 22), new(19, 25), new(19, 26), new(19, 27), new(19, 30), new(19, 31), new(19, 36),
new(19, 37), new(19, 38), new(20, 15), new(20, 16), new(20, 17), new(20, 21), new(20, 22), new(20, 25), new(20, 26), new(20, 27), new(20, 30), new(20, 31),
new(20, 37), new(21, 15), new(21, 16), new(21, 18), new(21, 19), new(21, 21), new(21, 22), new(21, 25), new(21, 26), new(21, 27), new(21, 30), new(21, 31),
new(21, 32), new(21, 33), new(21, 34), new(21, 35), new(21, 36), new(21, 37), new(21, 38), new(21, 39), new(21, 40), new(21, 41), new(21, 42), new(21, 43),
new(21, 44), new(21, 45), new(21, 46), new(22, 13), new(22, 15), new(22, 16), new(22, 17), new(22, 18), new(22, 19), new(22, 20), new(22, 21), new(22, 22),
new(22, 25), new(22, 26), new(22, 27), new(22, 30), new(22, 31), new(22, 32), new(22, 33), new(22, 34), new(22, 35), new(22, 36), new(22, 37), new(22, 38),
new(22, 39), new(22, 40), new(22, 41), new(22, 42), new(22, 43), new(22, 44), new(22, 45), new(23, 14), new(23, 15), new(23, 16), new(23, 17), new(23, 18),
new(23, 19), new(23, 20), new(23, 21), new(23, 22), new(23, 25), new(23, 26), new(23, 27), new(23, 30), new(23, 31), new(23, 32), new(23, 33), new(23, 34),
new(23, 35), new(23, 36), new(23, 37), new(23, 38), new(23, 39), new(23, 40), new(23, 41), new(23, 42), new(23, 43), new(23, 44), new(24, 15), new(24, 16),
new(24, 17), new(24, 18), new(24, 19), new(24, 20), new(24, 21), new(24, 22), new(24, 25), new(24, 26), new(24, 27), new(24, 30), new(24, 31), new(24, 32),
new(24, 33), new(24, 34), new(24, 35), new(24, 36), new(24, 37), new(24, 38), new(24, 39), new(24, 40), new(24, 41), new(24, 42), new(24, 43), new(25, 15),
new(25, 16), new(25, 17), new(25, 18), new(25, 19), new(25, 20), new(25, 21), new(25, 22), new(25, 25), new(25, 26), new(25, 27), new(25, 30), new(25, 31),
new(25, 32), new(25, 33), new(25, 34), new(25, 35), new(25, 36), new(25, 37), new(25, 38), new(25, 39), new(25, 40), new(25, 41), new(25, 42), new(26, 15),
new(26, 16), new(26, 17), new(26, 18), new(26, 19), new(26, 21), new(26, 22), new(26, 25), new(26, 26), new(26, 27), new(26, 30), new(26, 31), new(26, 32),
new(26, 33), new(26, 34), new(26, 35), new(26, 36), new(26, 37), new(26, 38), new(26, 39), new(26, 40), new(26, 41), new(27, 15), new(27, 16), new(27, 17),
new(27, 21), new(27, 22), new(27, 25), new(27, 26), new(27, 27), new(27, 30), new(27, 31), new(27, 36), new(27, 37), new(27, 43), new(28, 15), new(28, 16),
new(28, 17), new(28, 21), new(28, 22), new(28, 25), new(28, 30), new(28, 31), new(28, 36), new(28, 37), new(28, 38), new(29, 15), new(29, 16), new(29, 17),
new(29, 21), new(29, 22), new(29, 23), new(29, 24), new(29, 25), new(29, 26), new(29, 27), new(29, 28), new(29, 29), new(29, 30), new(29, 31), new(29, 32),
new(29, 33), new(29, 34), new(29, 35), new(29, 36), new(29, 37), new(29, 38), new(29, 42), new(30, 15), new(30, 16), new(30, 17), new(30, 20), new(30, 21),
new(30, 22), new(30, 23), new(30, 24), new(30, 25), new(30, 26), new(30, 27), new(30, 28), new(30, 29), new(30, 30), new(30, 31), new(30, 32), new(30, 33),
new(30, 34), new(30, 35), new(30, 36), new(30, 37), new(30, 38), new(30, 41), new(30, 42), new(31, 15), new(31, 16), new(31, 17), new(31, 19), new(31, 20),
new(31, 21), new(31, 22), new(31, 23), new(31, 24), new(31, 25), new(31, 26), new(31, 27), new(31, 28), new(31, 29), new(31, 30), new(31, 31), new(31, 32),
new(31, 34), new(31, 35), new(31, 36), new(31, 37), new(31, 38), new(31, 41), new(31, 42), new(32, 15), new(32, 16), new(32, 17), new(32, 20), new(32, 21),
new(32, 22), new(32, 23), new(32, 24), new(32, 25), new(32, 26), new(32, 27), new(32, 28), new(32, 29), new(32, 30), new(32, 31), new(32, 32), new(32, 34),
new(32, 35), new(32, 36), new(32, 37), new(32, 38), new(32, 41), new(32, 42), new(33, 15), new(33, 16), new(33, 17), new(33, 20), new(33, 21), new(33, 22),
new(33, 23), new(33, 24), new(33, 25), new(33, 26), new(33, 27), new(33, 28), new(33, 29), new(33, 30), new(33, 31), new(33, 32), new(33, 35), new(33, 36),
new(33, 37), new(33, 38), new(33, 41), new(33, 42), new(34, 15), new(34, 16), new(34, 17), new(34, 21), new(34, 22), new(34, 23), new(34, 24), new(34, 25),
new(34, 26), new(34, 27), new(34, 28), new(34, 29), new(34, 30), new(34, 31), new(34, 32), new(34, 35), new(34, 36), new(34, 37), new(34, 38), new(34, 39),
new(34, 40), new(34, 41), new(34, 42), new(35, 15), new(35, 16), new(35, 17), new(35, 21), new(35, 22), new(35, 23), new(35, 26), new(35, 28), new(35, 29),
new(35, 31), new(35, 35), new(35, 36), new(35, 37), new(35, 38), new(35, 39), new(35, 40), new(35, 41), new(35, 43), new(36, 15), new(36, 16), new(36, 17),
new(36, 22), new(36, 36), new(36, 37), new(36, 38), new(36, 39), new(36, 40), new(36, 41), new(37, 15), new(37, 16), new(37, 17), new(37, 21), new(37, 22),
new(37, 24), new(37, 25), new(37, 26), new(37, 27), new(37, 28), new(37, 29), new(37, 30), new(37, 31), new(37, 32), new(37, 33), new(37, 34), new(37, 35),
new(37, 36), new(37, 37), new(37, 38), new(37, 39), new(37, 40), new(38, 15), new(38, 16), new(38, 17), new(38, 21), new(38, 22), new(38, 23), new(38, 24),
new(38, 25), new(38, 26), new(38, 27), new(38, 28), new(38, 29), new(38, 30), new(38, 31), new(38, 32), new(38, 33), new(38, 34), new(38, 35), new(38, 36),
new(38, 37), new(38, 38), new(38, 39), new(38, 40), new(39, 15), new(39, 16), new(39, 17), new(39, 21), new(39, 22), new(39, 23), new(39, 24), new(39, 25),
new(39, 26), new(39, 27), new(39, 28), new(39, 29), new(39, 30), new(39, 31), new(39, 32), new(39, 33), new(39, 34), new(39, 35), new(39, 36), new(39, 37),
new(39, 38), new(39, 39), new(39, 42), new(40, 15), new(40, 16), new(40, 17), new(40, 21), new(40, 22), new(40, 23), new(40, 24), new(40, 25), new(40, 26),
new(40, 27), new(40, 28), new(40, 29), new(40, 30), new(40, 31), new(40, 32), new(40, 33), new(40, 34), new(40, 35), new(40, 36), new(40, 37), new(40, 38),
new(40, 40), new(41, 15), new(41, 16), new(41, 17), new(41, 21), new(41, 22), new(41, 23), new(41, 24), new(41, 25), new(41, 26), new(41, 27), new(41, 28),
new(41, 29), new(41, 30), new(41, 31), new(41, 32), new(41, 33), new(41, 34), new(41, 35), new(42, 15), new(42, 16), new(42, 17), new(42, 21), new(42, 22),
new(42, 23), new(42, 24), new(42, 25), new(42, 26), new(42, 27), new(42, 28), new(42, 29), new(42, 30), new(42, 31), new(42, 32), new(42, 33), new(42, 34),
new(42, 35), new(42, 36), new(42, 37), new(42, 38), new(42, 39), new(42, 40), new(42, 41), new(42, 42), new(42, 43), new(42, 44), new(42, 45), new(42, 46),
new(43, 13), new(43, 14), new(43, 15), new(43, 16), new(43, 17), new(43, 18), new(43, 19), new(43, 21), new(43, 22), new(43, 23), new(43, 24), new(43, 25),
new(43, 26), new(43, 27), new(43, 28), new(43, 29), new(43, 30), new(43, 31), new(43, 32), new(43, 33), new(43, 34), new(43, 35), new(43, 36), new(43, 37),
new(43, 38), new(43, 39), new(43, 40), new(43, 41), new(43, 42), new(43, 43), new(43, 44), new(43, 45), new(44, 10), new(44, 11), new(44, 12), new(44, 13),
new(44, 14), new(44, 15), new(44, 16), new(44, 17), new(44, 18), new(44, 19), new(44, 20), new(44, 21), new(44, 22), new(44, 23), new(44, 24), new(44, 25),
new(44, 26), new(44, 27), new(44, 28), new(44, 29), new(44, 30), new(44, 31), new(44, 32), new(44, 33), new(44, 34), new(44, 35), new(44, 36), new(44, 37),
new(44, 38), new(44, 39), new(44, 40), new(44, 41), new(44, 42), new(44, 43), new(44, 44), new(45, 10), new(45, 11), new(45, 12), new(45, 13), new(45, 14),
new(45, 15), new(45, 16), new(45, 17), new(45, 18), new(45, 19), new(45, 20), new(45, 21), new(45, 22), new(45, 23), new(45, 24), new(45, 25), new(45, 26),
new(45, 27), new(45, 28), new(45, 29), new(45, 30), new(45, 31), new(45, 32), new(45, 33), new(45, 34), new(45, 35), new(45, 36), new(45, 37), new(45, 38),
new(45, 39), new(45, 40), new(45, 41), new(45, 42), new(45, 43), new(46, 14), new(46, 15), new(46, 16), new(46, 17), new(46, 18), new(46, 19), new(46, 20),
new(46, 21), new(46, 22), new(46, 23), new(46, 24), new(46, 25), new(46, 26), new(46, 27), new(46, 28), new(46, 29), new(46, 30), new(46, 31), new(46, 32),
new(46, 33), new(46, 34), new(46, 35), new(46, 36), new(46, 37), new(46, 38), new(46, 39), new(46, 40), new(46, 41), new(46, 42), new(46, 43), new(46, 45),
new(47, 15), new(47, 16), new(47, 17), new(47, 20), new(47, 21), new(47, 22), new(47, 23), new(47, 24), new(47, 25), new(47, 27), new(47, 30), new(47, 31),
new(47, 35), new(47, 36), new(47, 37), new(47, 38), new(47, 41), new(48, 15), new(48, 16), new(48, 17), new(48, 19), new(48, 20), new(48, 21), new(48, 22),
new(48, 23), new(48, 24), new(48, 26), new(48, 27), new(48, 28), new(48, 29), new(48, 30), new(48, 31), new(48, 32), new(48, 33), new(48, 34), new(48, 35),
new(48, 36), new(49, 14), new(49, 15), new(49, 16), new(49, 17), new(49, 18), new(49, 19), new(49, 20), new(49, 21), new(49, 22), new(49, 23), new(49, 24),
new(49, 26), new(49, 27), new(49, 28), new(49, 29), new(49, 30), new(49, 31), new(49, 32), new(49, 33), new(49, 34), new(49, 35), new(49, 36), new(50, 16),
new(50, 20), new(50, 21), new(50, 22), new(50, 23), new(50, 24), new(50, 26), new(50, 27), new(50, 28), new(50, 29), new(50, 30), new(50, 31), new(50, 32),
new(50, 33), new(50, 34), new(51, 21), new(51, 22), new(51, 23), new(51, 24), new(51, 26), new(51, 27), new(51, 28), new(51, 29), new(51, 30), new(51, 31),
new(51, 32), new(51, 33), new(51, 34), new(52, 23), new(52, 24), new(53, 19), new(53, 20), new(53, 21), new(53, 22), new(53, 23), new(53, 24), new(53, 25),
new(53, 26), new(53, 27), new(53, 28), new(53, 29), new(53, 30), new(53, 31), new(53, 32), new(53, 33), new(53, 34), new(53, 35), new(53, 36), new(53, 37),
new(53, 38), new(54, 17), new(54, 19), new(54, 20), new(54, 21), new(54, 22), new(54, 23), new(54, 24), new(54, 25), new(54, 26), new(54, 27), new(54, 28),
new(54, 29), new(54, 30), new(54, 31), new(54, 32), new(54, 33), new(54, 34), new(54, 35), new(54, 36), new(54, 37), new(55, 18), new(55, 20), new(55, 21),
new(55, 22), new(55, 23), new(55, 24), new(55, 25), new(55, 26), new(55, 27), new(55, 28), new(55, 29), new(55, 30), new(55, 31), new(55, 32), new(55, 33),
new(55, 34), new(55, 35), new(55, 36), new(55, 37), new(56, 20), new(56, 21), new(56, 22), new(56, 23), new(56, 24), new(56, 25), new(56, 26), new(56, 27),
new(56, 28), new(56, 29), new(56, 30), new(56, 31), new(56, 32), new(56, 33), new(56, 34), new(56, 35), new(56, 36), new(56, 42), new(57, 20), new(57, 21),
new(57, 22), new(57, 23), new(57, 24), new(57, 25), new(57, 26), new(57, 27), new(57, 28), new(57, 29), new(57, 30), new(57, 31), new(57, 32), new(57, 33),
new(57, 34), new(57, 35), new(57, 37), new(57, 39), new(58, 16), new(58, 17), new(58, 21), new(58, 22), new(58, 23), new(58, 24), new(58, 25), new(58, 26),
new(58, 27), new(58, 28), new(58, 29), new(58, 30), new(58, 31), new(58, 32), new(58, 33), new(58, 34), new(58, 38), new(58, 39), new(58, 41), new(58, 42),
new(59, 16), new(59, 17), new(59, 21), new(59, 22), new(59, 23), new(59, 24), new(59, 26), new(59, 27), new(59, 28), new(59, 29), new(59, 30), new(59, 31),
new(59, 32), new(59, 33), new(59, 36), new(59, 38), new(59, 39), new(59, 44), new(60, 15), new(60, 16), new(60, 17), new(60, 19), new(60, 21), new(60, 22),
new(60, 26), new(60, 27), new(60, 28), new(60, 32), new(60, 33), new(60, 38), new(60, 39), new(60, 40), new(60, 43), new(61, 14), new(61, 15), new(61, 16),
new(61, 17), new(61, 21), new(61, 22), new(61, 24), new(61, 25), new(61, 26), new(61, 27), new(61, 28), new(61, 29), new(61, 32), new(61, 33), new(61, 35),
new(61, 37), new(61, 38), new(61, 39), new(61, 40), new(61, 41), new(61, 42), new(62, 11), new(62, 12), new(62, 13), new(62, 14), new(62, 15), new(62, 16),
new(62, 17), new(62, 18), new(62, 21), new(62, 22), new(62, 24), new(62, 25), new(62, 26), new(62, 27), new(62, 28), new(62, 29), new(62, 30), new(62, 32),
new(62, 33), new(62, 36), new(62, 37), new(62, 38), new(62, 39), new(62, 40), new(62, 41), new(62, 42), new(62, 43), new(62, 44), new(63, 7), new(63, 8),
new(63, 9), new(63, 10), new(63, 11), new(63, 12), new(63, 13), new(63, 14), new(63, 15), new(63, 16), new(63, 17), new(63, 18), new(63, 19), new(63, 21),
new(63, 22), new(63, 23), new(63, 24), new(63, 25), new(63, 26), new(63, 27), new(63, 28), new(63, 29), new(63, 30), new(63, 31), new(63, 32), new(63, 33),
new(63, 35), new(63, 36), new(63, 37), new(63, 38), new(63, 39), new(63, 40), new(63, 41), new(63, 42), new(63, 43), new(63, 44), new(63, 45), new(63, 46),
new(63, 47), new(63, 48), new(63, 49), new(63, 50), new(64, 0), new(64, 1), new(64, 2), new(64, 3), new(64, 4), new(64, 5), new(64, 6), new(64, 7),
new(64, 8), new(64, 9), new(64, 10), new(64, 11), new(64, 12), new(64, 13), new(64, 14), new(64, 15), new(64, 16), new(64, 17), new(64, 18), new(64, 19),
new(64, 20), new(64, 21), new(64, 22), new(64, 23), new(64, 24), new(64, 25), new(64, 26), new(64, 27), new(64, 28), new(64, 29), new(64, 30), new(64, 31),
new(64, 32), new(64, 33), new(64, 34), new(64, 35), new(64, 36), new(64, 37), new(64, 38), new(64, 39), new(64, 40), new(64, 41), new(64, 42), new(64, 43),
new(64, 44), new(64, 45), new(64, 46), new(64, 47), new(64, 48), new(64, 49), new(64, 50), new(64, 51), new(64, 52), new(64, 53), new(64, 54), new(64, 55),
new(64, 56), new(64, 57), new(64, 58), new(64, 59), new(64, 60), new(65, 0), new(65, 1), new(65, 2), new(65, 3), new(65, 4), new(65, 5), new(65, 6),
new(65, 7), new(65, 8), new(65, 9), new(65, 10), new(65, 11), new(65, 12), new(65, 13), new(65, 14), new(65, 15), new(65, 16), new(65, 17), new(65, 18),
new(65, 19), new(65, 20), new(65, 21), new(65, 22), new(65, 23), new(65, 24), new(65, 25), new(65, 26), new(65, 27), new(65, 28), new(65, 29), new(65, 30),
new(65, 31), new(65, 32), new(65, 33), new(65, 34), new(65, 35), new(65, 36), new(65, 37), new(65, 38), new(65, 39), new(65, 40), new(65, 41), new(65, 42),
new(65, 43), new(65, 44), new(65, 45), new(65, 46), new(65, 47), new(65, 48), new(65, 49), new(65, 50), new(65, 51), new(65, 52), new(65, 53), new(65, 54),
new(65, 55), new(65, 56), new(65, 57), new(65, 58), new(65, 59), new(65, 60), new(66, 7), new(66, 8), new(66, 9), new(66, 10), new(66, 11), new(66, 12),
new(66, 13), new(66, 14), new(66, 15), new(66, 16), new(66, 17), new(66, 18), new(66, 19), new(66, 20), new(66, 21), new(66, 22), new(66, 23), new(66, 24),
new(66, 25), new(66, 26), new(66, 27), new(66, 28), new(66, 29), new(66, 30), new(66, 31), new(66, 32), new(66, 33), new(66, 35), new(66, 36), new(66, 37),
new(66, 38), new(66, 39), new(66, 40), new(66, 41), new(66, 42), new(66, 43), new(66, 44), new(66, 45), new(66, 46), new(66, 47), new(66, 48), new(66, 49),
new(66, 50), new(67, 11), new(67, 12), new(67, 13), new(67, 14), new(67, 15), new(67, 16), new(67, 17), new(67, 18), new(67, 21), new(67, 22), new(67, 24),
new(67, 25), new(67, 26), new(67, 27), new(67, 28), new(67, 29), new(67, 30), new(67, 32), new(67, 33), new(67, 36), new(67, 37), new(67, 38), new(67, 39),
new(67, 40), new(67, 41), new(67, 42), new(67, 43), new(67, 44), new(68, 14), new(68, 15), new(68, 16), new(68, 17), new(68, 21), new(68, 22), new(68, 25),
new(68, 26), new(68, 27), new(68, 28), new(68, 29), new(68, 32), new(68, 33), new(68, 35), new(68, 37), new(68, 38), new(68, 39), new(68, 40), new(68, 41),
new(69, 15), new(69, 16), new(69, 17), new(69, 21), new(69, 22), new(69, 25), new(69, 26), new(69, 27), new(69, 28), new(69, 32), new(69, 33), new(69, 37),
new(69, 38), new(69, 39), new(69, 40), new(70, 15), new(70, 16), new(70, 17), new(70, 21), new(70, 22), new(70, 23), new(70, 24), new(70, 26), new(70, 27),
new(70, 28), new(70, 29), new(70, 30), new(70, 31), new(70, 32), new(70, 33), new(70, 38), new(70, 39), new(70, 43), new(71, 16), new(71, 17), new(71, 20),
new(71, 21), new(71, 22), new(71, 23), new(71, 24), new(71, 25), new(71, 26), new(71, 27), new(71, 28), new(71, 29), new(71, 30), new(71, 31), new(71, 32),
new(71, 33), new(71, 34), new(71, 35), new(71, 38), new(71, 39), new(72, 19), new(72, 20), new(72, 21), new(72, 22), new(72, 23), new(72, 24), new(72, 25),
new(72, 26), new(72, 27), new(72, 28), new(72, 29), new(72, 30), new(72, 31), new(72, 32), new(72, 33), new(72, 34), new(72, 35), new(73, 19), new(73, 20),
new(73, 21), new(73, 22), new(73, 23), new(73, 24), new(73, 25), new(73, 26), new(73, 27), new(73, 28), new(73, 29), new(73, 30), new(73, 31), new(73, 32),
new(73, 33), new(73, 34), new(73, 35), new(73, 36), new(74, 19), new(74, 20), new(74, 21), new(74, 22), new(74, 23), new(74, 24), new(74, 25), new(74, 26),
new(74, 27), new(74, 28), new(74, 29), new(74, 30), new(74, 31), new(74, 32), new(74, 33), new(74, 34), new(74, 35), new(74, 36), new(74, 37), new(75, 20),
new(75, 21), new(75, 22), new(75, 23), new(75, 24), new(75, 25), new(75, 26), new(75, 27), new(75, 28), new(75, 29), new(75, 30), new(75, 31), new(75, 32),
new(75, 33), new(75, 34), new(75, 35), new(75, 36), new(75, 37), new(76, 20), new(76, 21), new(76, 22), new(76, 23), new(76, 24), new(76, 25), new(76, 26),
new(76, 27), new(76, 28), new(76, 29), new(76, 30), new(76, 31), new(76, 32), new(76, 33), new(76, 34), new(76, 35), new(76, 36), new(76, 37), new(77, 18),
new(77, 21), new(77, 22), new(77, 23), new(78, 24)
                })
                {
                    NetManager.Instance.Broadcast(NetParticlesModule.Serialize(ParticleOrchestraType.Keybrand, new ParticleOrchestraSettings() { PositionInWorld = leftPoint + point.ToWorldCoordinates() }));
                }
            }
        }, "myply"));
    }

    public override void Initialize()
    {
        ServerApi.Hooks.NetGetData.Register(this, OnNetGetData);
        GetDataHandlers.TileEdit += OnTileEdit;
        //On.Terraria.Item.NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool += Item_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool;
    }

    private void OnNetGetData(GetDataEventArgs args)
    {
        if (args.MsgID == PacketTypes.LoadNetModule)
        {
            using var ms = new MemoryStream(args.Msg.readBuffer, args.Index, args.Length);
            using var br = new BinaryReader(ms);
            var id = br.ReadUInt16();
            if (id == NetManager.Instance.GetId<NetParticlesModule>())
            {
                ParticleOrchestraType type = (ParticleOrchestraType)br.ReadByte();
                ParticleOrchestraSettings settings = default;
                settings.DeserializeFrom(br);
                Console.WriteLine($"type:{type} index:{settings.IndexOfPlayerWhoInvokedThis} position:{settings.PositionInWorld} move:{settings.MovementVector} piece:{settings.UniqueInfoPiece}");
            }
        }
        
    }
    //Console.WriteLine(new StackTrace().ToString());

    private int Item_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool(On.Terraria.Item.orig_NewItem_IEntitySource_int_int_int_int_int_int_bool_int_bool_bool orig, Terraria.DataStructures.IEntitySource source, int X, int Y, int Width, int Height, int Type, int Stack, bool noBroadcast, int pfix, bool noGrabDelay, bool reverseLookup)
    {
        //Console.WriteLine("on 1");
        //Console.WriteLine("pre on 1");
        var num = orig(source, X, Y, Width, Height, Type, Stack, noBroadcast, pfix, noGrabDelay, reverseLookup);
        //if(source is EntitySource_Loot)
        //{
        //    var newItems = Item.numberOfNewItems;
        //    Item.numberOfNewItems = 241;
        //    Main.item[num].TryCombiningIntoNearbyItems(num);
        //    Item.numberOfNewItems = newItems;
        //}
        //if(Type is not (71 or 72 or 73 or 74 or 23))
        //{
        //    Console.WriteLine(new StackTrace().ToString());
        //    Console.WriteLine($"source:{source}\n" +
        //        $"X:{X}\n" +
        //        $"Y:{Y}\n" +
        //        $"Width:{Width}\n" +
        //        $"Height:{Height}\n" +
        //        $"Type:{Type}\n" +
        //        $"Stack:{Stack}\n" +
        //        $"noBroadcast:{noBroadcast}\n" +
        //        $"pfix:{pfix}\n" +
        //        $"noGrabDlay:{noGrabDelay}\n" +
        //        $"reverseLookup:{reverseLookup}");
        //}
        return num;
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            ServerApi.Hooks.NetGetData.Deregister(this, OnNetGetData);
            GetDataHandlers.TileEdit -= OnTileEdit;
        }
        base.Dispose(disposing);
    }
    private void OnTileEdit(object? args, GetDataHandlers.TileEditEventArgs e)
    {
        switch (e.Action)
        {
            //case GetDataHandlers.EditAction.KillTile:
            //    if (Main.tile[e.X, e.Y].type == TileID.DemonAltar && e.Player.SelectedItem.type == ItemID.WoodenHammer)
            //    {
            //        e.Player.Heal(50);
            //        count++;
            //        if (count == 5)
            //        {
            //            WorldGen.KillTile(e.X, e.Y);
            //            foreach (var type in new int[] {
            //                ItemID.Meowmere, ItemID.StarWrath,
            //                ItemID.NebulaBlaze, ItemID.NebulaArcanum,
            //                ItemID.StardustCellStaff, ItemID.StardustDragonStaff,
            //                ItemID.VortexBeater, ItemID.Phantasm,
            //                ItemID.SolarEruption, ItemID.DayBreak
            //            })
            //            {
            //                Item.NewItem(null, e.X * 16, e.Y * 16, 16, 16, type);
            //                Item.NewItem(null, e.X * 16, e.Y * 16, 16, 16, type);
            //            }
            //            NetMessage.SendTileSquare(-1, e.X, e.Y);
            //            count = 0;
            //        }
            //    }
            //    break;
            case GetDataHandlers.EditAction.ReplaceTile:
                if (Main.tile[e.X, e.Y] is not null && Main.tile[e.X,e.Y].type != TileID.Platforms && e.EditData == TileID.Platforms)
                {
                    
                }
                break;
        }
    }
    private void OnPlaceObject(object? args, GetDataHandlers.PlaceObjectEventArgs e)
    {
        Console.WriteLine(
            $"""
            X:{e.X}
            Y:{e.Y}
            Type:{e.Type}
            Style:{e.Style}
            Alternate:{e.Alternate}
            Direction:{e.Direction}
            """);
        if (e.Type == 14)
        {
            Utils.PlaceAndSendTileSquare3x2(e.X, e.Y, TileID.DemonAltar);
            e.Handled = true;
        }
    }
    private void OnPlayerUpdate(object? args, GetDataHandlers.PlayerUpdateEventArgs e)
    {
        if (e.Control.IsUsingItem && e.Player.SelectedItem.type == ItemID.LifeCrystal || e.Player.SelectedItem.type == ItemID.LifeFruit && e.Player.TPlayer.statLifeMax >= 400)
        {
            e.Player.SelectedItem.stack -= 1;
            TSPlayer.All.SendData(PacketTypes.PlayerSlot, "", e.Player.Index, e.Player.TPlayer.selectedItem);
            e.Player.TPlayer.statLifeMax += 20;
            TSPlayer.All.SendData(PacketTypes.PlayerHp, "", e.Player.Index);
            e.Player.Heal(20);
        }
    }
}
public static class Utils
{
    public static void PlaceAndSendTileSquare3x2(int x, int y, ushort type, int style = 0)
    {
        WorldGen.Place3x2(x, y, type, style);
        NetMessage.SendTileSquare(-1, x - 1, y - 1, 3, 2);
    }
}