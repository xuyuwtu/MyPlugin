using System.Reflection;

using ReLogic.Utilities;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Utilities;

namespace VBY.ProjectileAI;
public static partial class AIs
{
    public static void AI(Projectile projectile) => _ProjectileAIs[projectile.aiStyle].Invoke(projectile);
    private static readonly Type dType = typeof(Action<Projectile>);
    public static void SetMethod(MethodInfo method)
    {
        int index = int.Parse(method.Name.Substring(3, 3));
        if (_tempProjectileAIs[index] is null)
        {
            _tempProjectileAIs[index] = _ProjectileAIs[index];
            _ProjectileAIs[index] = (Action<Projectile>)Delegate.CreateDelegate(dType, method);
            //Console.WriteLine($"aiStyle:{index} add success. method:{method.Name}");
        }
    }
    public static void SetMethod(Action<Projectile> action)
    {
        int index = int.Parse(action.Method.Name.Substring(3, 3));
        if (_tempProjectileAIs[index] is null)
        {
            _tempProjectileAIs[index] = _ProjectileAIs[index];
            _ProjectileAIs[index] = action;
            //Console.WriteLine($"aiStyle:{index} add success. method:{action.Method.Name}");
        }
    }
    public static void RemoveMethod(MethodInfo method)
    {
        int index = int.Parse(method.Name.Substring(3, 3));
        if (_ProjectileAIs[index].Method == method)
        {
            _ProjectileAIs[index] = _tempProjectileAIs[index]!;
            _tempProjectileAIs[index] = null;
            //Console.WriteLine($"aiStyle:{index} remove success. method:{method.Name}");
        }
    }
    public static void RemoveMethod(Action<Projectile> action)
    {
        int index = int.Parse(action.Method.Name.Substring(3, 3));
        if (_ProjectileAIs[index].Method == action.Method)
        {
            _ProjectileAIs[index] = _tempProjectileAIs[index]!;
            _tempProjectileAIs[index] = null;
            //Console.WriteLine($"aiStyle:{index} remove success. method:{action.Method.Name}");
        }
    }
    internal static Action<Projectile>[] _ProjectileAIs = new Action<Projectile>[196];
    internal static Action<Projectile>?[] _tempProjectileAIs = new Action<Projectile>[196];
    static AIs()
    {
        foreach (var name in new string[] {  nameof(Projectile.AI_156_BatOfLight) }) 
        {
            _ProjectileAIs[int.Parse(name.Substring(3, 3))] = (Action<Projectile>)Delegate.CreateDelegate(dType, typeof(Projectile).GetMethod(name, BindingFlags.Instance | BindingFlags.Public)!);
        }
        foreach (var method in typeof(AIs).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(x => x.Name.StartsWith("AI_")))
        {
            //Console.WriteLine(method.Name);
            _ProjectileAIs[int.Parse(method.Name[3..])] = (Action<Projectile>)Delegate.CreateDelegate(dType, method);
        }
        foreach (var method in typeof(Projectile).GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(x => x.Name.StartsWith("AI_") && (x.Name.Length == 6 || !x.Name[7..].Contains('_')) && x.GetParameters().Length == 0 && x.ReturnType == typeof(void)))
        {
            //Console.WriteLine(method.Name);
            _ProjectileAIs[int.Parse(method.Name.Substring(3, 3))] ??= (Action<Projectile>)Delegate.CreateDelegate(dType, method);
        }
        //for(int i = 1; i < _ProjectileAIs.Length; i++)
        //{
        //    if (_ProjectileAIs[i] is null)
        //    {
        //        Console.WriteLine($"aiStyle:{i} is null");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"{_ProjectileAIs[i].Method.DeclaringType!.Name} {_ProjectileAIs[i].Method.Name}");
        //    }
        //}
        //Console.ReadKey();
    }
    public static void AI_002(Projectile projectile)
    {
        if (Main.windPhysics)
        {
            projectile.velocity.X += Main.windSpeedCurrent * Main.windPhysicsStrength;
        }
        if (projectile.type == 968)
        {
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = 1f;
            }
            projectile.alpha = Math.Max(0, projectile.alpha - 50);
            projectile.frame = (int)projectile.ai[1];
        }
        if (projectile.type == 304 && projectile.localAI[0] == 0f)
        {
            projectile.localAI[0] += 1f;
            projectile.alpha = 0;
        }
        if (projectile.type == 510)
        {
            projectile.rotation += Math.Abs(projectile.velocity.X) * 0.04f * (float)projectile.direction;
        }
        else
        {
            projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.03f * (float)projectile.direction;
        }
        if (projectile.type == 909)
        {
            int num3 = 38;
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= (float)num3)
            {
                projectile.velocity.Y += 0.4f;
                projectile.velocity.X *= 0.97f;
            }
            if (Main.netMode != 1 && projectile.ai[1] == 0f)
            {
                projectile.ai[1] = 1 + Main.rand.Next(6);
                projectile.netUpdate = true;
            }
            if (projectile.ai[1] > 0f)
            {
                projectile.frame = (int)projectile.ai[1] - 1;
            }
        }
        else if (projectile.type == 162)
        {
            if (projectile.ai[1] == 0f)
            {
                projectile.ai[1] = 1f;
                //SoundEngine.PlaySound(SoundID.Item14, projectile.position);
            }
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 18f)
            {
                projectile.velocity.Y += 0.28f;
                projectile.velocity.X *= 0.99f;
            }
            if (projectile.ai[0] > 2f)
            {
                projectile.alpha = 0;
            }
        }
        else if (projectile.type == 281)
        {
            if (projectile.ai[1] == 0f)
            {
                projectile.ai[1] = 1f;
                //SoundEngine.PlaySound(SoundID.Item14, projectile.position);
            }
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 18f)
            {
                projectile.velocity.Y += 0.28f;
                projectile.velocity.X *= 0.99f;
            }
            if (projectile.ai[0] > 2f)
            {
                projectile.alpha = 0;
            }
        }
        else if (projectile.type == 240)
        {
            if (projectile.ai[1] == 0f)
            {
                projectile.ai[1] = 1f;
                //SoundEngine.PlaySound(SoundID.Item14, projectile.position);
            }
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 16f)
            {
                projectile.velocity.Y += 0.18f;
                projectile.velocity.X *= 0.991f;
            }
            if (projectile.ai[0] > 2f)
            {
                projectile.alpha = 0;
            }
        }
        else if (projectile.type == 497)
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 30f)
            {
                projectile.velocity.X *= 0.99f;
                projectile.velocity.Y += 0.5f;
            }
            else
            {
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            }
        }
        else if (projectile.type == 861)
        {
            if (Main.myPlayer == projectile.owner)
            {
                projectile.localAI[0]++;
                if (projectile.localAI[0] > 30f)
                {
                    projectile.localAI[0] = 30f;
                }
                Player player = Main.player[projectile.owner];
                for (int num18 = 0; num18 < 255; num18++)
                {
                    Player player2 = Main.player[num18];
                    if (player2 != null && player2.active && !player2.dead && (player2.whoAmI != player.whoAmI || !(projectile.localAI[0] < 30f)) && projectile.Colliding(projectile.Hitbox, player2.Hitbox))
                    {
                        projectile.Kill();
                        break;
                    }
                }
            }
            if (projectile.velocity.Y == 0f)
            {
                projectile.velocity.X *= 0.95f;
            }
            else
            {
                projectile.velocity.X *= 0.995f;
            }
            if (Math.Abs(projectile.velocity.X) < 0.5f)
            {
                projectile.velocity.X = 0f;
            }
            if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
            {
                projectile.Kill();
            }
            projectile.velocity.Y += 0.1f;
            if (projectile.ai[1] == 1f)
            {
                projectile.frame = (projectile.frameCounter = 0);
                projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.03f * (float)projectile.direction;
            }
            else
            {
                if (projectile.frame == 0)
                {
                    projectile.frame = 1;
                }
                projectile.frameCounter++;
                if (projectile.frameCounter > 4)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                    if (projectile.frame >= Main.projFrames[projectile.type])
                    {
                        projectile.frame = 1;
                    }
                }
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 4f;
                projectile.spriteDirection = ((!(projectile.velocity.X < 0f)) ? 1 : (-1));
                if (projectile.spriteDirection == -1)
                {
                    projectile.rotation += (float)Math.PI / 2f;
                }
            }
        }
        else if (projectile.type == 249)
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 0f)
            {
                projectile.velocity.Y += 0.25f;
            }
        }
        else if (projectile.type == 347)
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 5f)
            {
                projectile.velocity.Y += 0.25f;
            }
        }
        else if (projectile.type == 501)
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 18f)
            {
                projectile.velocity.X *= 0.995f;
                projectile.velocity.Y += 0.2f;
            }
        }
        else if (projectile.type == 504 || projectile.type == 954 || projectile.type == 979)
        {
            projectile.alpha = 255;
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 3f)
            {
                int num19 = 100;
                if (projectile.ai[0] > 20f)
                {
                    int num20 = 40;
                    float num21 = projectile.ai[0] - 20f;
                    num19 = (int)(100f * (1f - num21 / (float)num20));
                    if (num21 >= (float)num20)
                    {
                        projectile.Kill();
                    }
                }
                if (projectile.ai[0] <= 10f)
                {
                    num19 = (int)projectile.ai[0] * 10;
                }
            }
            if (projectile.ai[0] >= 20f)
            {
                projectile.velocity.X *= 0.99f;
                projectile.velocity.Y += 0.1f;
            }
        }
        else if (projectile.type == 69 || projectile.type == 70 || projectile.type == 621)
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 10f)
            {
                projectile.velocity.Y += 0.25f;
                projectile.velocity.X *= 0.99f;
            }
        }
        else if (projectile.type == 166)
        {
            if (projectile.owner == Main.myPlayer && projectile.ai[1] == 1f)
            {
                for (int num23 = 0; num23 < 200; num23++)
                {
                    if (Main.npc[num23].active && Main.npc[num23].townNPC && projectile.Colliding(projectile.Hitbox, Main.npc[num23].Hitbox))
                    {
                        projectile.Kill();
                        return;
                    }
                }
                if (Main.netMode == 1)
                {
                    for (int num24 = 0; num24 < 255; num24++)
                    {
                        if (num24 != projectile.owner && Main.player[num24].active && !Main.player[projectile.owner].InOpposingTeam(Main.player[num24]) && projectile.Colliding(projectile.Hitbox, Main.player[num24].Hitbox))
                        {
                            projectile.Kill();
                            return;
                        }
                    }
                }
            }
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 20f)
            {
                projectile.velocity.Y += 0.3f;
                projectile.velocity.X *= 0.98f;
            }
        }
        else if (projectile.type == 300)
        {
            //if (projectile.ai[0] == 0f)
            //{
            //    SoundEngine.PlaySound(SoundID.Item1, projectile.position);
            //}
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 60f)
            {
                projectile.velocity.Y += 0.2f;
                projectile.velocity.X *= 0.99f;
            }
        }
        else if (projectile.type == 306)
        {
            projectile.alpha -= 50;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 0.785f;
        }
        else if (projectile.type == 304)
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 30f)
            {
                projectile.alpha += 10;
                projectile.damage = (int)((double)projectile.damage * 0.9);
                projectile.knockBack = (int)((double)projectile.knockBack * 0.9);
                if (projectile.alpha >= 255)
                {
                    projectile.active = false;
                }
            }
            if (projectile.ai[0] < 30f)
            {
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            }
        }
        else if (projectile.type == 370 || projectile.type == 371 || projectile.type == 936)
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= 15f)
            {
                projectile.velocity.Y += 0.3f;
                projectile.velocity.X *= 0.98f;
            }
        }
        else
        {
            int num29 = 20;
            if (projectile.type == 93)
            {
                num29 = 28 + Main.rand.Next(6);
            }
            projectile.ai[0] += 1f;
            if (projectile.ai[0] >= (float)num29)
            {
                if (projectile.type == 93)
                {
                    projectile.ai[0] = 40f;
                }
                projectile.velocity.Y += 0.4f;
                projectile.velocity.X *= 0.97f;
            }
            else if (projectile.type == 48 || projectile.type == 54 || projectile.type == 93 || projectile.type == 520 || projectile.type == 599)
            {
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            }
        }
        if (projectile.velocity.Y > 16f)
        {
            projectile.velocity.Y = 16f;
        }
    }
    public static void AI_003(Projectile projectile)
    {
        if (projectile.soundDelay == 0 && projectile.type != 383)
        {
            projectile.soundDelay = 8;
            //SoundEngine.PlaySound(SoundID.Item7, projectile.position);
        }
        if (projectile.ai[0] == 0f)
        {
            bool flag = true;
            int num38 = projectile.type;
            if (num38 == 866)
            {
                flag = false;
            }
            if (flag)
            {
                projectile.ai[1] += 1f;
            }
            if (projectile.type == 106 && projectile.ai[1] >= 45f)
            {
                projectile.ai[0] = 1f;
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }
            if (projectile.type == 320 || projectile.type == 383)
            {
                if (projectile.ai[1] >= 10f)
                {
                    projectile.velocity.Y += 0.5f;
                    if (projectile.type == 383 && projectile.velocity.Y < 0f)
                    {
                        projectile.velocity.Y += 0.35f;
                    }
                    projectile.velocity.X *= 0.95f;
                    if (projectile.velocity.Y > 16f)
                    {
                        projectile.velocity.Y = 16f;
                    }
                    if (projectile.type == 383 && Vector2.Distance(projectile.Center, Main.player[projectile.owner].Center) > 800f)
                    {
                        projectile.ai[0] = 1f;
                        projectile.netUpdate = true;
                    }
                }
            }
            else if (projectile.type == 182)
            {
                if (projectile.velocity.X > 0f)
                {
                    projectile.spriteDirection = 1;
                }
                else if (projectile.velocity.X < 0f)
                {
                    projectile.spriteDirection = -1;
                }
                float num40 = projectile.position.X;
                float num41 = projectile.position.Y;
                float num42 = 800f;
                bool flag2 = false;
                if (projectile.ai[1] > 10f && projectile.ai[1] < 360f)
                {
                    for (int num43 = 0; num43 < 200; num43++)
                    {
                        if (Main.npc[num43].CanBeChasedBy(projectile))
                        {
                            float num44 = Main.npc[num43].position.X + (float)(Main.npc[num43].width / 2);
                            float num45 = Main.npc[num43].position.Y + (float)(Main.npc[num43].height / 2);
                            float num46 = projectile.Distance(Main.npc[num43].Center);
                            if (num46 < num42 && Collision.CanHit(new Vector2(projectile.position.X + (float)(projectile.width / 2), projectile.position.Y + (float)(projectile.height / 2)), 1, 1, Main.npc[num43].position, Main.npc[num43].width, Main.npc[num43].height))
                            {
                                num42 = num46;
                                num40 = num44;
                                num41 = num45;
                                flag2 = true;
                            }
                        }
                    }
                }
                if (!flag2)
                {
                    num40 = projectile.position.X + (float)(projectile.width / 2) + projectile.velocity.X * 100f;
                    num41 = projectile.position.Y + (float)(projectile.height / 2) + projectile.velocity.Y * 100f;
                    if (projectile.ai[1] >= 30f)
                    {
                        projectile.ai[0] = 1f;
                        projectile.ai[1] = 0f;
                        projectile.netUpdate = true;
                    }
                }
                float num47 = 12f;
                float num48 = 0.25f;
                Vector2 vector3 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num49 = num40 - vector3.X;
                float num50 = num41 - vector3.Y;
                float num51 = (float)Math.Sqrt(num49 * num49 + num50 * num50);
                float num52 = num51;
                num51 = num47 / num51;
                num49 *= num51;
                num50 *= num51;
                if (projectile.velocity.X < num49)
                {
                    projectile.velocity.X += num48;
                    if (projectile.velocity.X < 0f && num49 > 0f)
                    {
                        projectile.velocity.X += num48 * 2f;
                    }
                }
                else if (projectile.velocity.X > num49)
                {
                    projectile.velocity.X -= num48;
                    if (projectile.velocity.X > 0f && num49 < 0f)
                    {
                        projectile.velocity.X -= num48 * 2f;
                    }
                }
                if (projectile.velocity.Y < num50)
                {
                    projectile.velocity.Y += num48;
                    if (projectile.velocity.Y < 0f && num50 > 0f)
                    {
                        projectile.velocity.Y += num48 * 2f;
                    }
                }
                else if (projectile.velocity.Y > num50)
                {
                    projectile.velocity.Y -= num48;
                    if (projectile.velocity.Y > 0f && num50 < 0f)
                    {
                        projectile.velocity.Y -= num48 * 2f;
                    }
                }
            }
            else if (projectile.type == 866)
            {
                if (projectile.owner == Main.myPlayer && projectile.damage > 0)
                {
                    float num53 = projectile.ai[1];
                    if (projectile.localAI[0] >= 10f && projectile.localAI[0] <= 360f)
                    {
                        int num54 = projectile.FindTargetWithLineOfSight();
                        projectile.ai[1] = num54;
                    }
                    else
                    {
                        projectile.ai[1] = -1f;
                    }
                    if (projectile.ai[1] != num53)
                    {
                        projectile.netUpdate = true;
                    }
                }
                projectile.localAI[0] += 1f;
                int num55 = (int)projectile.ai[1];
                Vector2 vector4;
                if (Main.npc.IndexInRange(num55) && Main.npc[num55].CanBeChasedBy(projectile))
                {
                    vector4 = Main.npc[num55].Center;
                }
                else
                {
                    vector4 = projectile.Center + projectile.velocity * 100f;
                    int num56 = 30;
                    if (projectile.owner != Main.myPlayer)
                    {
                        num56 = 60;
                    }
                    if (projectile.localAI[0] >= (float)num56)
                    {
                        projectile.ai[0] = 1f;
                        projectile.ai[1] = 0f;
                        projectile.netUpdate = true;
                    }
                }
                float num57 = 12f;
                float num58 = 0.25f;
                Vector2 vector5 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num59 = vector4.X - vector5.X;
                float num60 = vector4.Y - vector5.Y;
                float num61 = (float)Math.Sqrt(num59 * num59 + num60 * num60);
                float num62 = num61;
                num61 = num57 / num61;
                num59 *= num61;
                num60 *= num61;
                if (projectile.velocity.X < num59)
                {
                    projectile.velocity.X += num58;
                    if (projectile.velocity.X < 0f && num59 > 0f)
                    {
                        projectile.velocity.X += num58 * 2f;
                    }
                }
                else if (projectile.velocity.X > num59)
                {
                    projectile.velocity.X -= num58;
                    if (projectile.velocity.X > 0f && num59 < 0f)
                    {
                        projectile.velocity.X -= num58 * 2f;
                    }
                }
                if (projectile.velocity.Y < num60)
                {
                    projectile.velocity.Y += num58;
                    if (projectile.velocity.Y < 0f && num60 > 0f)
                    {
                        projectile.velocity.Y += num58 * 2f;
                    }
                }
                else if (projectile.velocity.Y > num60)
                {
                    projectile.velocity.Y -= num58;
                    if (projectile.velocity.Y > 0f && num60 < 0f)
                    {
                        projectile.velocity.Y -= num58 * 2f;
                    }
                }
            }
            else if (projectile.type == 301)
            {
                if (projectile.ai[1] >= 20f)
                {
                    projectile.ai[0] = 1f;
                    projectile.ai[1] = 0f;
                    projectile.velocity = Vector2.Zero;
                    projectile.netUpdate = true;
                }
            }
            else if (projectile.ai[1] >= 30f)
            {
                projectile.ai[0] = 1f;
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }
        }
        else
        {
            projectile.tileCollide = false;
            float num63 = 9f;
            float num64 = 0.4f;
            if (projectile.type == 1000)
            {
                num63 = 9.5f;
            }
            if (projectile.type == 19)
            {
                num63 = 20f;
                num64 = 1.5f;
            }
            else if (projectile.type == 33)
            {
                num63 = 18f;
                num64 = 1.2f;
            }
            else if (projectile.type == 182)
            {
                num63 = 16f;
                num64 = 1.2f;
            }
            else if (projectile.type == 866)
            {
                num63 = 16f;
                num64 = 1.2f;
            }
            else if (projectile.type == 106)
            {
                num63 = 16f;
                num64 = 1.2f;
            }
            else if (projectile.type == 272)
            {
                num63 = 20f;
                num64 = 1.5f;
            }
            else if (projectile.type == 333)
            {
                num63 = 12f;
                num64 = 0.6f;
            }
            else if (projectile.type == 301)
            {
                num63 = 15f;
                num64 = 3f;
            }
            else if (projectile.type == 320)
            {
                num63 = 15f;
                num64 = 3f;
            }
            else if (projectile.type == 383)
            {
                num63 = 16f;
                num64 = 4f;
            }
            Vector2 vector6 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
            float num65 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector6.X;
            float num66 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector6.Y;
            float num67 = (float)Math.Sqrt(num65 * num65 + num66 * num66);
            if (num67 > 3000f)
            {
                projectile.Kill();
            }
            num67 = num63 / num67;
            num65 *= num67;
            num66 *= num67;
            if (projectile.type == 383)
            {
                Vector2 vector7 = new Vector2(num65, num66) - projectile.velocity;
                if (vector7 != Vector2.Zero)
                {
                    Vector2 vector8 = vector7;
                    vector8.Normalize();
                    projectile.velocity += vector8 * Math.Min(num64, vector7.Length());
                }
            }
            else
            {
                if (projectile.velocity.X < num65)
                {
                    projectile.velocity.X += num64;
                    if (projectile.velocity.X < 0f && num65 > 0f)
                    {
                        projectile.velocity.X += num64;
                    }
                }
                else if (projectile.velocity.X > num65)
                {
                    projectile.velocity.X -= num64;
                    if (projectile.velocity.X > 0f && num65 < 0f)
                    {
                        projectile.velocity.X -= num64;
                    }
                }
                if (projectile.velocity.Y < num66)
                {
                    projectile.velocity.Y += num64;
                    if (projectile.velocity.Y < 0f && num66 > 0f)
                    {
                        projectile.velocity.Y += num64;
                    }
                }
                else if (projectile.velocity.Y > num66)
                {
                    projectile.velocity.Y -= num64;
                    if (projectile.velocity.Y > 0f && num66 < 0f)
                    {
                        projectile.velocity.Y -= num64;
                    }
                }
            }
            if (Main.myPlayer == projectile.owner)
            {
                Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
                Rectangle value = new Rectangle((int)Main.player[projectile.owner].position.X, (int)Main.player[projectile.owner].position.Y, Main.player[projectile.owner].width, Main.player[projectile.owner].height);
                if (rectangle.Intersects(value))
                {
                    projectile.Kill();
                }
            }
        }
        if (projectile.type == 106)
        {
            projectile.rotation += 0.3f * (float)projectile.direction;
        }
        else if (projectile.type == 866)
        {
            projectile.rotation = projectile.velocity.ToRotation();
        }
        else if (projectile.type == 383)
        {
            if (projectile.ai[0] == 0f)
            {
                Vector2 v = projectile.velocity;
                v = v.SafeNormalize(Vector2.Zero);
                projectile.rotation = (float)Math.Atan2(v.Y, v.X) + 1.57f;
            }
            else
            {
                Vector2 v2 = projectile.Center - Main.player[projectile.owner].Center;
                v2 = v2.SafeNormalize(Vector2.Zero);
                projectile.rotation = (float)Math.Atan2(v2.Y, v2.X) + 1.57f;
            }
        }
        else if (projectile.type == 301)
        {
            if (projectile.ai[0] == 0f)
            {
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 4f;
            }
            else
            {
                projectile.rotation += 0.4f * (float)projectile.direction;
            }
        }
        else
        {
            projectile.rotation += 0.4f * (float)projectile.direction;
        }
    }
    public static void AI_004(Projectile projectile)
    {
        if (Main.netMode != 2 && projectile.ai[1] == 0f && projectile.localAI[0] == 0f)
        {
            projectile.localAI[0] = 1f;
            LegacySoundStyle legacySoundStyle = SoundID.Item8;
            if (projectile.type == 494)
            {
                legacySoundStyle = SoundID.Item101;
            }
            SoundEngine.PlaySound(legacySoundStyle, projectile.Center);
        }
        projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
        if (projectile.ai[0] == 0f)
        {
            if (projectile.type >= 150 && projectile.type <= 152 && projectile.ai[1] == 0f && projectile.alpha == 255 && Main.rand.Next(2) == 0)
            {
                projectile.type++;
                projectile.netUpdate = true;
            }
            projectile.alpha -= 50;
            if (projectile.type >= 150 && projectile.type <= 152)
            {
                projectile.alpha -= 25;
            }
            else if (projectile.type == 493 || projectile.type == 494)
            {
                projectile.alpha -= 50;
            }
            if (projectile.alpha > 0)
            {
                return;
            }
            projectile.alpha = 0;
            projectile.ai[0] = 1f;
            if (projectile.ai[1] == 0f)
            {
                projectile.ai[1] += 1f;
                projectile.position += projectile.velocity * 1f;
            }
            if (projectile.type == 7 && Main.myPlayer == projectile.owner)
            {
                int num71 = projectile.type;
                if (projectile.ai[1] >= 6f)
                {
                    num71++;
                }
                int num72 = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.position.X + projectile.velocity.X + (float)(projectile.width / 2), projectile.position.Y + projectile.velocity.Y + (float)(projectile.height / 2), projectile.velocity.X, projectile.velocity.Y, num71, projectile.damage, projectile.knockBack, projectile.owner);
                Main.projectile[num72].damage = projectile.damage;
                Main.projectile[num72].ai[1] = projectile.ai[1] + 1f;
                NetMessage.SendData(27, -1, -1, null, num72);
            }
            else if (projectile.type == 494 && Main.myPlayer == projectile.owner)
            {
                int num73 = projectile.type;
                if (projectile.ai[1] >= (float)(7 + Main.rand.Next(2)))
                {
                    num73--;
                }
                int num74 = projectile.damage;
                float num75 = projectile.knockBack;
                if (num73 == 493)
                {
                    num74 = (int)((double)projectile.damage * 1.25);
                    num75 = projectile.knockBack * 1.25f;
                }
                int number = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.position.X + projectile.velocity.X + (float)(projectile.width / 2), projectile.position.Y + projectile.velocity.Y + (float)(projectile.height / 2), projectile.velocity.X, projectile.velocity.Y, num73, num74, num75, projectile.owner, 0f, projectile.ai[1] + 1f);
                NetMessage.SendData(27, -1, -1, null, number);
            }
            else if ((projectile.type == 150 || projectile.type == 151) && Main.myPlayer == projectile.owner)
            {
                int num76 = projectile.type;
                if (projectile.type == 150)
                {
                    num76 = 151;
                }
                else if (projectile.type == 151)
                {
                    num76 = 150;
                }
                if (projectile.ai[1] >= 10f && projectile.type == 151)
                {
                    num76 = 152;
                }
                int num77 = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.position.X + projectile.velocity.X + (float)(projectile.width / 2), projectile.position.Y + projectile.velocity.Y + (float)(projectile.height / 2), projectile.velocity.X, projectile.velocity.Y, num76, projectile.damage, projectile.knockBack, projectile.owner);
                Main.projectile[num77].damage = projectile.damage;
                Main.projectile[num77].ai[1] = projectile.ai[1] + 1f;
                NetMessage.SendData(27, -1, -1, null, num77);
            }
            return;
        }
        if (projectile.type >= 150 && projectile.type <= 152)
        {
            projectile.alpha += 3;
        }
        else if (projectile.type == 493 || projectile.type == 494)
        {
            projectile.alpha += 4;
        }
        else
        {
            projectile.alpha += 5;
        }
        if (projectile.alpha >= 255)
        {
            projectile.Kill();
        }
    }
    public static void AI_005(Projectile projectile)
    {
        if (!Main.remixWorld && projectile.type == 12 && Main.dayTime && projectile.damage == 1000)
        {
            projectile.Kill();
        }
        if (projectile.type == 503 || projectile.type == 723 || projectile.type == 724 || projectile.type == 725 || projectile.type == 726)
        {
            if (projectile.Center.Y > projectile.ai[1])
            {
                projectile.tileCollide = true;
            }
        }
        else if (projectile.type == 92)
        {
            if (projectile.position.Y > projectile.ai[1])
            {
                projectile.tileCollide = true;
            }
        }
        else if (projectile.type == 9)
        {
            projectile.tileCollide = projectile.Bottom.Y >= projectile.ai[1];
        }
        else
        {
            if (projectile.ai[1] == 0f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.ai[1] = 1f;
                projectile.netUpdate = true;
            }
            if (projectile.ai[1] != 0f)
            {
                projectile.tileCollide = true;
            }
        }
        if (projectile.soundDelay == 0)
        {
            projectile.soundDelay = 20 + Main.rand.Next(40);
            SoundEngine.PlaySound(SoundID.Item9, projectile.position);
        }
        if (projectile.type == 503 || projectile.type == 9)
        {
            projectile.alpha -= 15;
            int num83 = 150;
            if (projectile.Center.Y >= projectile.ai[1])
            {
                num83 = 0;
            }
            if (projectile.alpha < num83)
            {
                projectile.alpha = num83;
            }
            projectile.localAI[0] += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.01f * (float)projectile.direction;
        }
        else if (projectile.type == 723 || projectile.type == 724 || projectile.type == 725 || projectile.type == 726)
        {
            projectile.alpha -= 15;
            int num84 = 100;
            if (projectile.Center.Y >= projectile.ai[1])
            {
                num84 = 0;
            }
            if (projectile.alpha < num84)
            {
                projectile.alpha = num84;
            }
            projectile.localAI[0] += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.01f * (float)projectile.direction;
        }
        else
        {
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = 1f;
            }
            projectile.alpha += (int)(25f * projectile.localAI[0]);
            if (projectile.alpha > 200)
            {
                projectile.alpha = 200;
                projectile.localAI[0] = -1f;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
                projectile.localAI[0] = 1f;
            }
        }
        if (projectile.type == 503)
        {
            projectile.rotation = projectile.velocity.ToRotation() - (float)Math.PI / 2f;
        }
        else
        {
            projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.01f * (float)projectile.direction;
        }
        if (projectile.type == 12 || projectile.type == 955)
        {
            Vector2 vector10 = new Vector2(Main.screenWidth, Main.screenHeight);
            if (projectile.Hitbox.Intersects(Utils.CenteredRectangle(Main.screenPosition + vector10 / 2f, vector10 + new Vector2(400f))) && Main.rand.Next(6) == 0)
            {
                int num87 = Utils.SelectRandom<int>(Main.rand, 16, 17, 17, 17);
                if (Main.tenthAnniversaryWorld)
                {
                    num87 = Utils.SelectRandom<int>(Main.rand, 16, 16, 16, 17);
                }
                Gore.NewGore(projectile.position, projectile.velocity * 0.2f, num87);
            }
            projectile.light = 0.9f;
        }
        else if (projectile.ai[1] == 1f || projectile.type == 92)
        {
            projectile.light = 0.9f;
        }
    }
    public static void AI_006(Projectile projectile)
    {
        bool flag3 = projectile.type == 1019;
        projectile.velocity *= 0.95f;
        projectile.ai[0] += 1f;
        if (projectile.ai[0] == 180f)
        {
            projectile.Kill();
        }
        if (projectile.ai[1] == 0f)
        {
            projectile.ai[1] = 1f;
        }
        bool flag4 = Main.myPlayer == projectile.owner;
        if (flag3)
        {
            flag4 = Main.netMode != 1;
        }
        if (flag4 && (projectile.type == 10 || projectile.type == 11 || projectile.type == 463 || flag3))
        {
            int num92 = (int)(projectile.position.X / 16f) - 1;
            int num93 = (int)((projectile.position.X + (float)projectile.width) / 16f) + 2;
            int num94 = (int)(projectile.position.Y / 16f) - 1;
            int num95 = (int)((projectile.position.Y + (float)projectile.height) / 16f) + 2;
            if (num92 < 0)
            {
                num92 = 0;
            }
            if (num93 > Main.maxTilesX)
            {
                num93 = Main.maxTilesX;
            }
            if (num94 < 0)
            {
                num94 = 0;
            }
            if (num95 > Main.maxTilesY)
            {
                num95 = Main.maxTilesY;
            }
            Vector2 vector15 = default(Vector2);
            for (int num96 = num92; num96 < num93; num96++)
            {
                for (int num97 = num94; num97 < num95; num97++)
                {
                    vector15.X = num96 * 16;
                    vector15.Y = num97 * 16;
                    if (!(projectile.position.X + (float)projectile.width > vector15.X) || !(projectile.position.X < vector15.X + 16f) || !(projectile.position.Y + (float)projectile.height > vector15.Y) || !(projectile.position.Y < vector15.Y + 16f) || !Main.tile[num96, num97].active())
                    {
                        continue;
                    }
                    if (projectile.type == 10)
                    {
                        if (Main.tile[num96, num97].type == 23 || Main.tile[num96, num97].type == 199)
                        {
                            Main.tile[num96, num97].type = 2;
                            WorldGen.SquareTileFrame(num96, num97);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendTileSquare(-1, num96, num97);
                            }
                        }
                        if (Main.tile[num96, num97].type == 25 || Main.tile[num96, num97].type == 203)
                        {
                            Main.tile[num96, num97].type = 1;
                            WorldGen.SquareTileFrame(num96, num97);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendTileSquare(-1, num96, num97);
                            }
                        }
                        if (Main.tile[num96, num97].type == 112 || Main.tile[num96, num97].type == 234)
                        {
                            Main.tile[num96, num97].type = 53;
                            WorldGen.SquareTileFrame(num96, num97);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendTileSquare(-1, num96, num97);
                            }
                        }
                        if (Main.tile[num96, num97].type == 163 || Main.tile[num96, num97].type == 200)
                        {
                            Main.tile[num96, num97].type = 161;
                            WorldGen.SquareTileFrame(num96, num97);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendTileSquare(-1, num96, num97);
                            }
                        }
                        if (Main.tile[num96, num97].type == 400 || Main.tile[num96, num97].type == 401)
                        {
                            Main.tile[num96, num97].type = 396;
                            WorldGen.SquareTileFrame(num96, num97);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendTileSquare(-1, num96, num97);
                            }
                        }
                        if (Main.tile[num96, num97].type == 398 || Main.tile[num96, num97].type == 399)
                        {
                            Main.tile[num96, num97].type = 397;
                            WorldGen.SquareTileFrame(num96, num97);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendTileSquare(-1, num96, num97);
                            }
                        }
                        if (Main.tile[num96, num97].type == 661 || Main.tile[num96, num97].type == 662)
                        {
                            Main.tile[num96, num97].type = 60;
                            WorldGen.SquareTileFrame(num96, num97);
                            if (Main.netMode == 1)
                            {
                                NetMessage.SendTileSquare(-1, num96, num97);
                            }
                        }
                    }
                    if (projectile.type == 11 || projectile.type == 463)
                    {
                        if (projectile.type == 11)
                        {
                            WorldGen.Convert(num96, num97, 1, 1);
                        }
                        if (projectile.type == 463)
                        {
                            WorldGen.Convert(num96, num97, 4, 1);
                        }
                    }
                    if (!flag3)
                    {
                        continue;
                    }
                    ITile tile = Main.tile[num96, num97];
                    if (tile.type >= 0 && tile.type < TileID.Count && TileID.Sets.CommonSapling[tile.type])
                    {
                        if (Main.remixWorld && num97 >= (int)Main.worldSurface - 1 && num97 < Main.maxTilesY - 20)
                        {
                            WorldGen.AttemptToGrowTreeFromSapling(num96, num97, underground: false);
                        }
                        WorldGen.AttemptToGrowTreeFromSapling(num96, num97, num97 > (int)Main.worldSurface - 1);
                    }
                }
            }
        }
        if (flag3 && projectile.velocity.Length() < 0.5f)
        {
            projectile.Kill();
        }
    }
    public static void AI_008(Projectile projectile)
    {
        if (projectile.type == 258 && projectile.localAI[0] == 0f)
        {
            projectile.localAI[0] = 1f;
            SoundEngine.PlaySound(SoundID.Item20, projectile.position);
        }
        if (projectile.type == 96 && projectile.localAI[0] == 0f)
        {
            projectile.localAI[0] = 1f;
            SoundEngine.PlaySound(SoundID.Item20, projectile.position);
        }
        else if (projectile.type == 502)
        {
            float num105 = (float)Main.DiscoR / 255f;
            float num106 = (float)Main.DiscoG / 255f;
            float num107 = (float)Main.DiscoB / 255f;
            num105 = (0.5f + num105) / 2f;
            num106 = (0.5f + num106) / 2f;
            num107 = (0.5f + num107) / 2f;
            Lighting.AddLight(projectile.Center, num105, num106, num107);
        }
        if (projectile.type != 27 && projectile.type != 96 && projectile.type != 258)
        {
            projectile.ai[1] += 1f;
        }
        if (projectile.ai[1] >= 20f)
        {
            projectile.velocity.Y += 0.2f;
        }
        if (projectile.type == 502)
        {
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
            if (projectile.velocity.X != 0f)
            {
                projectile.spriteDirection = (projectile.direction = Math.Sign(projectile.velocity.X));
            }
        }
        else
        {
            projectile.rotation += 0.3f * (float)projectile.direction;
        }
        if (projectile.velocity.Y > 16f)
        {
            projectile.velocity.Y = 16f;
        }
    }
    public static void AI_011(Projectile projectile)
    {
        bool flag5 = projectile.type == 72 || projectile.type == 86 || projectile.type == 87;
        if (flag5)
        {
            if (projectile.velocity.X > 0f)
            {
                projectile.spriteDirection = -1;
            }
            else if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = 1;
            }
            projectile.rotation = projectile.velocity.X * 0.1f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 4)
            {
                projectile.frame = 0;
            }
        }
        else
        {
            projectile.rotation += 0.02f;
        }
        if (projectile.type == 72)
        {
            if (Main.player[projectile.owner].blueFairy)
            {
                projectile.timeLeft = 2;
            }
        }
        else if (projectile.type == 86)
        {
            if (Main.player[projectile.owner].redFairy)
            {
                projectile.timeLeft = 2;
            }
        }
        else if (projectile.type == 87)
        {
            if (Main.player[projectile.owner].greenFairy)
            {
                projectile.timeLeft = 2;
            }
        }
        else if (projectile.type == 18 && Main.player[projectile.owner].lightOrb)
        {
            projectile.timeLeft = 2;
        }
        if (!Main.player[projectile.owner].dead)
        {
            float num115 = 3f;
            if (flag5)
            {
                num115 = 6f;
            }
            Vector2 vector16 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
            float num116 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector16.X;
            float num117 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector16.Y;
            int num118 = 800;
            int num119 = 70;
            if (projectile.type == 18)
            {
                if (Main.player[projectile.owner].controlUp)
                {
                    num117 = Main.player[projectile.owner].position.Y - 40f - vector16.Y;
                    num116 -= 6f;
                    num119 = 4;
                }
                else if (Main.player[projectile.owner].controlDown)
                {
                    num117 = Main.player[projectile.owner].position.Y + (float)Main.player[projectile.owner].height + 40f - vector16.Y;
                    num116 -= 6f;
                    num119 = 4;
                }
            }
            if (flag5)
            {
                num119 = 50;
            }
            float num120 = (float)Math.Sqrt(num116 * num116 + num117 * num117);
            num120 = (float)Math.Sqrt(num116 * num116 + num117 * num117);
            if (num120 > (float)num118)
            {
                projectile.position.X = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - (float)(projectile.width / 2);
                projectile.position.Y = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - (float)(projectile.height / 2);
            }
            else if (num120 > (float)num119)
            {
                float num121 = num120 - (float)num119;
                num120 = num115 / num120;
                num116 *= num120;
                num117 *= num120;
                projectile.velocity.X = num116;
                projectile.velocity.Y = num117;
                if (flag5 && projectile.velocity.Length() > num121)
                {
                    projectile.velocity = projectile.velocity.SafeNormalize(Vector2.Zero) * num121;
                }
            }
            else
            {
                projectile.velocity.X = (projectile.velocity.Y = 0f);
            }
        }
        else
        {
            projectile.Kill();
        }
    }
    public static void AI_012(Projectile projectile)
    {
        if (projectile.type == 288 && projectile.localAI[0] == 0f)
        {
            projectile.localAI[0] = 1f;
            SoundEngine.PlaySound(SoundID.Item17, projectile.position);
        }
        if (projectile.type == 280 || projectile.type == 288)
        {
            projectile.scale -= 0.002f;
            if (projectile.scale <= 0f)
            {
                projectile.Kill();
            }
            if (projectile.type == 288)
            {
                projectile.ai[0] = 4f;
            }
            if (projectile.ai[0] > 3f)
            {
                projectile.velocity.Y += 0.075f;
            }
            else
            {
                projectile.ai[0] += 1f;
            }
            return;
        }
        float num129 = 0.02f;
        float num130 = 0.2f;
        if (projectile.type == 22)
        {
            num129 = 0.01f;
            num130 = 0.15f;
        }
        projectile.scale -= num129;
        if (projectile.scale <= 0f)
        {
            projectile.Kill();
        }
        if (projectile.ai[0] > 3f)
        {
            projectile.velocity.Y += num130;
        }
        else
        {
            projectile.ai[0] += 1f;
        }
    }
    public static void AI_013(Projectile projectile)
    {
        bool flag6 = Main.player[projectile.owner].dead;
        if (!flag6)
        {
            float num139 = (Main.player[projectile.owner].Center - projectile.Center).Length();
            flag6 = num139 > 2000f;
        }
        if (flag6)
        {
            projectile.Kill();
            return;
        }
        if (projectile.type != 481)
        {
            int dummyItemTime = 5;
            Main.player[projectile.owner].SetDummyItemTime(dummyItemTime);
        }
        if (projectile.alpha == 0)
        {
            if (projectile.position.X + (float)(projectile.width / 2) > Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2))
            {
                Main.player[projectile.owner].ChangeDir(1);
            }
            else
            {
                Main.player[projectile.owner].ChangeDir(-1);
            }
        }
        if (projectile.type == 481)
        {
            if (projectile.ai[0] == 0f)
            {
                projectile.extraUpdates = 1;
            }
            else
            {
                projectile.extraUpdates = 2;
            }
        }
        Vector2 vector17 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
        float num140 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector17.X;
        float num141 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector17.Y;
        float num142 = (float)Math.Sqrt(num140 * num140 + num141 * num141);
        if (projectile.ai[0] == 0f)
        {
            if (num142 > 700f)
            {
                projectile.ai[0] = 1f;
            }
            else if (projectile.type == 262 && num142 > 500f)
            {
                projectile.ai[0] = 1f;
            }
            else if (projectile.type == 271 && num142 > 200f)
            {
                projectile.ai[0] = 1f;
            }
            else if (projectile.type == 273 && (Main.remixWorld ? (num142 > 300f) : (num142 > 150f)))
            {
                projectile.ai[0] = 1f;
            }
            else if (projectile.type == 481 && num142 > 525f)
            {
                projectile.ai[0] = 1f;
            }
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            projectile.ai[1] += 1f;
            if (projectile.ai[1] > 5f)
            {
                projectile.alpha = 0;
            }
            if (projectile.type == 262 && projectile.ai[1] > 8f)
            {
                projectile.ai[1] = 8f;
            }
            if (projectile.type == 271 && projectile.ai[1] > 8f)
            {
                projectile.ai[1] = 8f;
            }
            if (projectile.type == 273 && projectile.ai[1] > 8f)
            {
                projectile.ai[1] = 8f;
            }
            if (projectile.type == 481 && projectile.ai[1] > 8f)
            {
                projectile.ai[1] = 8f;
            }
            if (projectile.type == 404 && projectile.ai[1] > 8f)
            {
                projectile.ai[1] = 0f;
            }
            if (projectile.ai[1] >= 10f)
            {
                projectile.ai[1] = 15f;
                projectile.velocity.Y += 0.3f;
            }
            if (projectile.type == 262 && projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = -1;
            }
            else if (projectile.type == 262)
            {
                projectile.spriteDirection = 1;
            }
            if (projectile.type == 271 && projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = -1;
            }
            else if (projectile.type == 271)
            {
                projectile.spriteDirection = 1;
            }
        }
        else if (projectile.ai[0] == 1f)
        {
            projectile.tileCollide = false;
            projectile.rotation = (float)Math.Atan2(num141, num140) - 1.57f;
            float num143 = 20f;
            if (projectile.type == 262)
            {
                num143 = 30f;
            }
            if (num142 < 50f)
            {
                projectile.Kill();
            }
            num142 = num143 / num142;
            num140 *= num142;
            num141 *= num142;
            projectile.velocity.X = num140;
            projectile.velocity.Y = num141;
            if (projectile.type == 262 && projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = 1;
            }
            else if (projectile.type == 262)
            {
                projectile.spriteDirection = -1;
            }
            if (projectile.type == 271 && projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = 1;
            }
            else if (projectile.type == 271)
            {
                projectile.spriteDirection = -1;
            }
        }
    }
    public static void AI_014(Projectile projectile)
    {
        if (projectile.type == 870 && projectile.ai[1] > 0f)
        {
            projectile.aiStyle = 170;
        }
        if (projectile.type == 473 && Main.netMode != 2)
        {
            projectile.localAI[0] += 1f;
            if (projectile.localAI[0] >= 10f)
            {
                projectile.localAI[0] = 0f;
                int num144 = 30;
                if ((projectile.Center - Main.player[Main.myPlayer].Center).Length() < (float)(Main.screenWidth + num144 * 16))
                {
                    Main.instance.SpelunkerProjectileHelper.AddSpotToCheck(projectile.Center);
                }
            }
        }
        if (projectile.type == 352)
        {
            if (projectile.localAI[1] == 0f)
            {
                projectile.localAI[1] = 1f;
            }
            projectile.alpha += (int)(25f * projectile.localAI[1]);
            if (projectile.alpha <= 0)
            {
                projectile.alpha = 0;
                projectile.localAI[1] = 1f;
            }
            else if (projectile.alpha >= 255)
            {
                projectile.alpha = 255;
                projectile.localAI[1] = -1f;
            }
            projectile.scale += projectile.localAI[1] * 0.01f;
        }
        if (projectile.type == 346)
        {
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = 1f;
                SoundEngine.PlaySound(SoundID.Item1, projectile.position);
            }
            projectile.frame = (int)projectile.ai[1];
            if (projectile.frame == 0)
            {
                Lighting.AddLight(projectile.Center, 0.25f, 0.2f, 0f);
            }
            else
            {
                Lighting.AddLight(projectile.Center, 0.15f, 0.15f, 0.15f);
            }
            if (projectile.owner == Main.myPlayer && projectile.timeLeft == 1)
            {
                for (int num145 = 0; num145 < 5; num145++)
                {
                    float num146 = 10f;
                    Vector2 vector18 = new Vector2(projectile.Center.X, projectile.Center.Y);
                    float num147 = Main.rand.Next(-20, 21);
                    float num148 = Main.rand.Next(-20, 0);
                    float num149 = (float)Math.Sqrt(num147 * num147 + num148 * num148);
                    num149 = num146 / num149;
                    num147 *= num149;
                    num148 *= num149;
                    num147 *= 1f + (float)Main.rand.Next(-30, 31) * 0.01f;
                    num148 *= 1f + (float)Main.rand.Next(-30, 31) * 0.01f;
                    int num150 = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), vector18.X, vector18.Y, num147, num148, 347, 40, 0f, Main.myPlayer, 0f, projectile.ai[1]);
                }
            }
        }
        if (projectile.type == 53)
        {
            try
            {
                int num154 = (int)(projectile.position.X / 16f) - 1;
                int num155 = (int)((projectile.position.X + (float)projectile.width) / 16f) + 2;
                int num156 = (int)(projectile.position.Y / 16f) - 1;
                int num157 = (int)((projectile.position.Y + (float)projectile.height) / 16f) + 2;
                if (num154 < 0)
                {
                    num154 = 0;
                }
                if (num155 > Main.maxTilesX)
                {
                    num155 = Main.maxTilesX;
                }
                if (num156 < 0)
                {
                    num156 = 0;
                }
                if (num157 > Main.maxTilesY)
                {
                    num157 = Main.maxTilesY;
                }
                Vector2 vector19 = default(Vector2);
                for (int num158 = num154; num158 < num155; num158++)
                {
                    for (int num159 = num156; num159 < num157; num159++)
                    {
                        if (Main.tile[num158, num159] != null && Main.tile[num158, num159].nactive() && Main.tileSolid[Main.tile[num158, num159].type] && !Main.tileSolidTop[Main.tile[num158, num159].type])
                        {
                            vector19.X = num158 * 16;
                            vector19.Y = num159 * 16;
                            if (projectile.position.X + (float)projectile.width > vector19.X && projectile.position.X < vector19.X + 16f && projectile.position.Y + (float)projectile.height > vector19.Y && projectile.position.Y < vector19.Y + 16f)
                            {
                                projectile.velocity.X = 0f;
                                projectile.velocity.Y = -0.2f;
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
        if (projectile.type == 277)
        {
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 30;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
            }
            if (Main.expertMode)
            {
                float num160 = 12f;
                int num161 = Player.FindClosest(projectile.Center, 1, 1);
                Vector2 vector20 = Main.player[num161].Center - projectile.Center;
                vector20.Normalize();
                vector20 *= num160;
                int num162 = 200;
                projectile.velocity.X = (projectile.velocity.X * (float)(num162 - 1) + vector20.X) / (float)num162;
                if (projectile.velocity.Length() > 16f)
                {
                    projectile.velocity.Normalize();
                    projectile.velocity *= 16f;
                }
            }
        }
        if (projectile.type == 261)
        {
            if (projectile.localAI[1] == 0f)
            {
                projectile.localAI[1] = 1f;
                projectile.localAI[0] = 80f;
            }
            projectile.rotation += projectile.velocity.X * 0.05f;
            if (projectile.velocity.Y != 0f)
            {
                projectile.rotation += (float)projectile.spriteDirection * 0.01f;
            }
            projectile.ai[0]++;
            if (projectile.ai[0] > 15f)
            {
                if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)
                {
                    projectile.velocity.X *= 0.97f;
                    Math.Abs(projectile.velocity.X);
                    if (Math.Abs(projectile.velocity.X) <= 0.01f)
                    {
                        projectile.Kill();
                    }
                }
                projectile.ai[0] = 15f;
                projectile.velocity.Y += 0.2f;
            }
            if (projectile.localAI[0] > 0f)
            {
                projectile.localAI[0]--;
                int num163 = 5;
                int maxValue = num163;
                if (projectile.localAI[0] < 20f)
                {
                    maxValue = num163 + num163;
                }
                if (projectile.localAI[0] < 10f)
                {
                    maxValue = num163 + num163 + num163;
                }
            }
        }
        else if (projectile.type == 277)
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 15f)
            {
                projectile.ai[0] = 15f;
                if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)
                {
                    projectile.velocity.X *= 0.97f;
                    if ((double)projectile.velocity.X > -0.01 && (double)projectile.velocity.X < 0.01)
                    {
                        projectile.Kill();
                    }
                }
                projectile.velocity.Y += 0.2f;
            }
            projectile.rotation += projectile.velocity.X * 0.05f;
        }
        else if (projectile.type == 378)
        {
            if (projectile.localAI[0] == 0f)
            {
                //SoundEngine.PlaySound(SoundID.Item17, projectile.position);
                projectile.localAI[0] += 1f;
            }
            Rectangle rectangle2 = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
            for (int num164 = 0; num164 < 200; num164++)
            {
                if (Main.npc[num164].CanBeChasedBy(projectile, ignoreDontTakeDamage: true))
                {
                    Rectangle value2 = new Rectangle((int)Main.npc[num164].position.X, (int)Main.npc[num164].position.Y, Main.npc[num164].width, Main.npc[num164].height);
                    if (rectangle2.Intersects(value2))
                    {
                        projectile.Kill();
                        return;
                    }
                }
            }
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 10f)
            {
                projectile.ai[0] = 90f;
                if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)
                {
                    projectile.velocity.X *= 0.96f;
                    if ((double)projectile.velocity.X > -0.01 && (double)projectile.velocity.X < 0.01)
                    {
                        projectile.Kill();
                    }
                }
                projectile.velocity.Y += 0.2f;
            }
            projectile.rotation += projectile.velocity.X * 0.1f;
        }
        else if (projectile.type == 483)
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 5f)
            {
                if (projectile.owner == Main.myPlayer && projectile.ai[0] > (float)Main.rand.Next(20, 130))
                {
                    projectile.Kill();
                }
                if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)
                {
                    projectile.velocity.X *= 0.97f;
                    if ((double)projectile.velocity.X > -0.01 && (double)projectile.velocity.X < 0.01)
                    {
                        projectile.velocity.X = 0f;
                        projectile.netUpdate = true;
                    }
                }
                projectile.velocity.Y += 0.3f;
                projectile.velocity.X *= 0.99f;
            }
            projectile.rotation += projectile.velocity.X * 0.05f;
        }
        else if (projectile.type == 538)
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 60f || projectile.velocity.Y >= 0f)
            {
                projectile.alpha += 6;
                projectile.velocity *= 0.5f;
            }
            else if (projectile.ai[0] > 5f)
            {
                projectile.velocity.Y += 0.1f;
                projectile.velocity.X *= 1.025f;
                projectile.alpha -= 23;
                projectile.scale = 0.8f * (255f - (float)projectile.alpha) / 255f;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
            }
            if (projectile.alpha >= 255 && projectile.ai[0] > 5f)
            {
                projectile.Kill();
                return;
            }
        }
        else
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 5f)
            {
                projectile.ai[0] = 5f;
                if (projectile.velocity.Y == 0f && projectile.velocity.X != 0f)
                {
                    projectile.velocity.X *= 0.97f;
                    if ((double)projectile.velocity.X > -0.01 && (double)projectile.velocity.X < 0.01)
                    {
                        projectile.velocity.X = 0f;
                        projectile.netUpdate = true;
                    }
                }
                projectile.velocity.Y += 0.2f;
            }
            projectile.rotation += projectile.velocity.X * 0.1f;
        }
        if (projectile.type == 538)
        {
            if (projectile.localAI[1] == 0f)
            {
                projectile.localAI[1] = 1f;
                SoundEngine.PlaySound(4, (int)projectile.position.X, (int)projectile.position.Y, 7);
            }
        }
        if (projectile.type == 450)
        {
            if (projectile.ai[1] == 0f)
            {
                projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item13, projectile.position);
            }
            if (++projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 5)
                {
                    projectile.frame = 0;
                }
            }
            if ((double)projectile.velocity.Y < 0.25 && (double)projectile.velocity.Y > 0.15)
            {
                projectile.velocity.X *= 0.8f;
            }
            projectile.rotation = (0f - projectile.velocity.X) * 0.05f;
        }
        if (projectile.type == 480)
        {
            projectile.alpha = 255;
        }
        if (projectile.type >= 326 && projectile.type <= 328)
        {
            if (projectile.wet)
            {
                projectile.Kill();
            }
            if (projectile.ai[1] == 0f && projectile.type >= 326 && projectile.type <= 328)
            {
                projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item13, projectile.position);
            }
            if ((double)projectile.velocity.Y < 0.25 && (double)projectile.velocity.Y > 0.15)
            {
                projectile.velocity.X *= 0.8f;
            }
            projectile.rotation = (0f - projectile.velocity.X) * 0.05f;
        }
        if (projectile.type >= 400 && projectile.type <= 402)
        {
            if (projectile.wet)
            {
                projectile.Kill();
            }
            if (projectile.ai[1] == 0f && projectile.type >= 326 && projectile.type <= 328)
            {
                projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item13, projectile.position);
            }
            if ((double)projectile.velocity.Y < 0.25 && (double)projectile.velocity.Y > 0.15)
            {
                projectile.velocity.X *= 0.8f;
            }
            projectile.rotation = (0f - projectile.velocity.X) * 0.05f;
        }
        if (projectile.velocity.Y > 16f)
        {
            projectile.velocity.Y = 16f;
        }
    }
    public static void AI_017(Projectile projectile)
    {
        if (projectile.velocity.Y == 0f)
        {
            projectile.velocity.X *= 0.98f;
        }
        projectile.rotation += projectile.velocity.X * 0.1f;
        projectile.velocity.Y += 0.2f;
        if (Main.getGoodWorld && Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) < 1f)
        {
            projectile.damage = 0;
            projectile.knockBack = 0f;
        }
        if (projectile.owner != Main.myPlayer)
        {
            return;
        }
        int num173 = (int)((projectile.position.X + (float)(projectile.width / 2)) / 16f);
        int num174 = (int)((projectile.position.Y + (float)projectile.height - 4f) / 16f);
        if (Main.tile[num173, num174] == null)
        {
            return;
        }
        int style = 0;
        if (projectile.type >= 201 && projectile.type <= 205)
        {
            style = projectile.type - 200;
        }
        if (projectile.type >= 527 && projectile.type <= 531)
        {
            style = projectile.type - 527 + 6;
        }
        bool flag7 = false;
        TileObject objectData = default(TileObject);
        if (TileObject.CanPlace(num173, num174, 85, style, projectile.direction, out objectData))
        {
            flag7 = TileObject.Place(objectData);
        }
        if (flag7)
        {
            NetMessage.SendObjectPlacement(-1, num173, num174, objectData.type, objectData.style, objectData.alternate, objectData.random, projectile.direction);
            SoundEngine.PlaySound(0, num173 * 16, num174 * 16);
            int num175 = Sign.ReadSign(num173, num174);
            if (num175 >= 0)
            {
                Sign.TextSign(num175, projectile.miscText);
                NetMessage.SendData(47, -1, -1, null, num175, 0f, (int)(byte)new BitsByte(b1: true));
            }
            projectile.Kill();
        }
    }
    public static void AI_018(Projectile projectile)
    {
        if (projectile.ai[1] == 0f && projectile.type == 44)
        {
            projectile.ai[1] = 1f;
            SoundEngine.PlaySound(SoundID.Item8, projectile.position);
        }
        if (projectile.type == 263 || projectile.type == 274)
        {
            if (projectile.type == 274 && projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = -1;
            }
            projectile.rotation += (float)projectile.direction * 0.05f;
            projectile.rotation += (float)projectile.direction * 0.5f * ((float)projectile.timeLeft / 180f);
            if (projectile.type == 274)
            {
                projectile.velocity *= 0.96f;
            }
            else
            {
                projectile.velocity *= 0.95f;
            }
            return;
        }
        projectile.rotation += (float)projectile.direction * 0.8f;
        projectile.ai[0] += 1f;
        if (!(projectile.ai[0] < 30f))
        {
            if (projectile.ai[0] < 100f)
            {
                projectile.velocity *= 1.06f;
            }
            else
            {
                projectile.ai[0] = 200f;
            }
        }
    }
    public static void AI_019(Projectile projectile)
    {
        projectile.AI_019_Spears();
    }
    public static void AI_020(Projectile projectile)
    {
        projectile.timeLeft = 60;
        if (projectile.type == 252)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
            }
            if (projectile.frame > 3)
            {
                projectile.frame = 0;
            }
        }
        if (projectile.type == 509)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter >= 2)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
            }
            if (projectile.frame > 1)
            {
                projectile.frame = 0;
            }
        }
        if (projectile.soundDelay <= 0)
        {
            SoundEngine.PlaySound(SoundID.Item22, projectile.position);
            projectile.soundDelay = 30;
        }
        Vector2 vector21 = Main.player[projectile.owner].RotatedRelativePoint(Main.player[projectile.owner].MountedCenter);
        if (Main.myPlayer == projectile.owner)
        {
            if (Main.player[projectile.owner].channel)
            {
                float num178 = Main.player[projectile.owner].inventory[Main.player[projectile.owner].selectedItem].shootSpeed * projectile.scale;
                Vector2 vector22 = vector21;
                float num179 = (float)Main.mouseX + Main.screenPosition.X - vector22.X;
                float num180 = (float)Main.mouseY + Main.screenPosition.Y - vector22.Y;
                if (Main.player[projectile.owner].gravDir == -1f)
                {
                    num180 = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y - vector22.Y;
                }
                float num181 = (float)Math.Sqrt(num179 * num179 + num180 * num180);
                num181 = (float)Math.Sqrt(num179 * num179 + num180 * num180);
                num181 = num178 / num181;
                num179 *= num181;
                num180 *= num181;
                if (num179 != projectile.velocity.X || num180 != projectile.velocity.Y)
                {
                    projectile.netUpdate = true;
                }
                projectile.velocity.X = num179;
                projectile.velocity.Y = num180;
            }
            else
            {
                projectile.Kill();
            }
        }
        if (projectile.velocity.X > 0f)
        {
            Main.player[projectile.owner].ChangeDir(1);
        }
        else if (projectile.velocity.X < 0f)
        {
            Main.player[projectile.owner].ChangeDir(-1);
        }
        projectile.spriteDirection = projectile.direction;
        Main.player[projectile.owner].ChangeDir(projectile.direction);
        Main.player[projectile.owner].heldProj = projectile.whoAmI;
        Main.player[projectile.owner].SetDummyItemTime(2);
        projectile.position.X = vector21.X - (float)(projectile.width / 2);
        projectile.position.Y = vector21.Y - (float)(projectile.height / 2);
        projectile.rotation = (float)(Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.5700000524520874);
        if (Main.player[projectile.owner].direction == 1)
        {
            Main.player[projectile.owner].itemRotation = (float)Math.Atan2(projectile.velocity.Y * (float)projectile.direction, projectile.velocity.X * (float)projectile.direction);
        }
        else
        {
            Main.player[projectile.owner].itemRotation = (float)Math.Atan2(projectile.velocity.Y * (float)projectile.direction, projectile.velocity.X * (float)projectile.direction);
        }
        projectile.velocity.X *= 1f + (float)Main.rand.Next(-3, 4) * 0.01f;
    }
    public static void AI_021(Projectile projectile)
    {
        projectile.rotation = projectile.velocity.X * 0.1f;
        projectile.spriteDirection = -projectile.direction;
        if (projectile.ai[1] == 1f)
        {
            projectile.ai[1] = 0f;
            Main.musicPitch = projectile.ai[0];
            SoundEngine.PlaySound(SoundID.Item26, projectile.position);
        }
    }
    public static void AI_022(Projectile projectile)
    {
        if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
        {
            projectile.alpha = 255;
        }
        if (projectile.ai[1] < 0f)
        {
            if (projectile.timeLeft > 60)
            {
                projectile.timeLeft = 60;
            }
            if (projectile.velocity.X > 0f)
            {
                projectile.rotation += 0.3f;
            }
            else
            {
                projectile.rotation -= 0.3f;
            }
            int num184 = (int)(projectile.position.X / 16f) - 1;
            int num185 = (int)((projectile.position.X + (float)projectile.width) / 16f) + 2;
            int num186 = (int)(projectile.position.Y / 16f) - 1;
            int num187 = (int)((projectile.position.Y + (float)projectile.height) / 16f) + 2;
            if (num184 < 0)
            {
                num184 = 0;
            }
            if (num185 > Main.maxTilesX)
            {
                num185 = Main.maxTilesX;
            }
            if (num186 < 0)
            {
                num186 = 0;
            }
            if (num187 > Main.maxTilesY)
            {
                num187 = Main.maxTilesY;
            }
            int num188 = (int)projectile.position.X + 4;
            int num189 = (int)projectile.position.Y + 4;
            Vector2 vector23 = default(Vector2);
            for (int num190 = num184; num190 < num185; num190++)
            {
                for (int num191 = num186; num191 < num187; num191++)
                {
                    if (Main.tile[num190, num191] != null && Main.tile[num190, num191].active() && Main.tile[num190, num191].type != 127 && Main.tileSolid[Main.tile[num190, num191].type] && !Main.tileSolidTop[Main.tile[num190, num191].type])
                    {
                        vector23.X = num190 * 16;
                        vector23.Y = num191 * 16;
                        if ((float)(num188 + 8) > vector23.X && (float)num188 < vector23.X + 16f && (float)(num189 + 8) > vector23.Y && (float)num189 < vector23.Y + 16f)
                        {
                            projectile.Kill();
                        }
                    }
                }
            }
            return;
        }
        if (projectile.ai[0] < 0f)
        {
            int num196 = (int)projectile.position.X / 16;
            int num197 = (int)projectile.position.Y / 16;
            if (Main.tile[num196, num197] == null || !Main.tile[num196, num197].active())
            {
                projectile.Kill();
            }
            projectile.ai[0] -= 1f;
            if (projectile.ai[0] <= -900f && (Main.myPlayer == projectile.owner || Main.netMode == 2) && Main.tile[num196, num197].active() && Main.tile[num196, num197].type == 127)
            {
                WorldGen.KillTile(num196, num197);
                if (Main.netMode == 1)
                {
                    NetMessage.SendData(17, -1, -1, null, 0, num196, num197);
                }
                projectile.Kill();
            }
            return;
        }
        int num198 = (int)(projectile.position.X / 16f) - 1;
        int num199 = (int)((projectile.position.X + (float)projectile.width) / 16f) + 2;
        int num200 = (int)(projectile.position.Y / 16f) - 1;
        int num201 = (int)((projectile.position.Y + (float)projectile.height) / 16f) + 2;
        if (num198 < 0)
        {
            num198 = 0;
        }
        if (num199 > Main.maxTilesX)
        {
            num199 = Main.maxTilesX;
        }
        if (num200 < 0)
        {
            num200 = 0;
        }
        if (num201 > Main.maxTilesY)
        {
            num201 = Main.maxTilesY;
        }
        int num202 = (int)projectile.position.X + 4;
        int num203 = (int)projectile.position.Y + 4;
        Vector2 vector24 = default(Vector2);
        for (int num204 = num198; num204 < num199; num204++)
        {
            for (int num205 = num200; num205 < num201; num205++)
            {
                if (Main.tile[num204, num205] != null && Main.tile[num204, num205].nactive() && Main.tile[num204, num205].type != 127 && Main.tileSolid[Main.tile[num204, num205].type] && !Main.tileSolidTop[Main.tile[num204, num205].type])
                {
                    vector24.X = num204 * 16;
                    vector24.Y = num205 * 16;
                    if ((float)(num202 + 8) > vector24.X && (float)num202 < vector24.X + 16f && (float)(num203 + 8) > vector24.Y && (float)num203 < vector24.Y + 16f)
                    {
                        projectile.Kill();
                    }
                }
            }
        }
        if (projectile.lavaWet)
        {
            projectile.Kill();
        }
        int x = (int)(projectile.Center.X / 16f);
        int y = (int)(projectile.Center.Y / 16f);
        if (WorldGen.InWorld(x, y) && Main.tile[x, y] != null && Main.tile[x, y].liquid > 0 && Main.tile[x, y].shimmer())
        {
            projectile.Kill();
        }
        if (!projectile.active)
        {
            return;
        }
        int num207 = (int)projectile.ai[0];
        int num208 = (int)projectile.ai[1];
        if (WorldGen.InWorld(num207, num208) && WorldGen.SolidTile(num207, num208))
        {
            if (Math.Abs(projectile.velocity.X) > Math.Abs(projectile.velocity.Y))
            {
                if (projectile.Center.Y < (float)(num208 * 16 + 8) && WorldGen.InWorld(num207, num208 - 1) && !WorldGen.SolidTile(num207, num208 - 1))
                {
                    num208--;
                }
                else if (WorldGen.InWorld(num207, num208 + 1) && !WorldGen.SolidTile(num207, num208 + 1))
                {
                    num208++;
                }
                else if (WorldGen.InWorld(num207, num208 - 1) && !WorldGen.SolidTile(num207, num208 - 1))
                {
                    num208--;
                }
                else if (projectile.Center.X < (float)(num207 * 16 + 8) && WorldGen.InWorld(num207 - 1, num208) && !WorldGen.SolidTile(num207 - 1, num208))
                {
                    num207--;
                }
                else if (WorldGen.InWorld(num207 + 1, num208) && !WorldGen.SolidTile(num207 + 1, num208))
                {
                    num207++;
                }
                else if (WorldGen.InWorld(num207 - 1, num208) && !WorldGen.SolidTile(num207 - 1, num208))
                {
                    num207--;
                }
            }
            else if (projectile.Center.X < (float)(num207 * 16 + 8) && WorldGen.InWorld(num207 - 1, num208) && !WorldGen.SolidTile(num207 - 1, num208))
            {
                num207--;
            }
            else if (WorldGen.InWorld(num207 + 1, num208) && !WorldGen.SolidTile(num207 + 1, num208))
            {
                num207++;
            }
            else if (WorldGen.InWorld(num207 - 1, num208) && !WorldGen.SolidTile(num207 - 1, num208))
            {
                num207--;
            }
            else if (projectile.Center.Y < (float)(num208 * 16 + 8) && WorldGen.InWorld(num207, num208 - 1) && !WorldGen.SolidTile(num207, num208 - 1))
            {
                num208--;
            }
            else if (WorldGen.InWorld(num207, num208 + 1) && !WorldGen.SolidTile(num207, num208 + 1))
            {
                num208++;
            }
            else if (WorldGen.InWorld(num207, num208 - 1) && !WorldGen.SolidTile(num207, num208 - 1))
            {
                num208--;
            }
        }
        if (projectile.velocity.X > 0f)
        {
            projectile.rotation += 0.3f;
        }
        else
        {
            projectile.rotation -= 0.3f;
        }
        if (Main.myPlayer != projectile.owner)
        {
            return;
        }
        int num209 = (int)((projectile.position.X + (float)(projectile.width / 2)) / 16f);
        int num210 = (int)((projectile.position.Y + (float)(projectile.height / 2)) / 16f);
        bool flag8 = false;
        if (num209 == num207 && num210 == num208)
        {
            flag8 = true;
        }
        if (((projectile.velocity.X <= 0f && num209 <= num207) || (projectile.velocity.X >= 0f && num209 >= num207)) && ((projectile.velocity.Y <= 0f && num210 <= num208) || (projectile.velocity.Y >= 0f && num210 >= num208)))
        {
            flag8 = true;
        }
        if (!flag8)
        {
            return;
        }
        if (WorldGen.PlaceTile(num207, num208, 127, mute: false, forced: false, projectile.owner))
        {
            if (Main.netMode == 1)
            {
                NetMessage.SendData(17, -1, -1, null, 1, num207, num208, 127f);
            }
            projectile.damage = 0;
            projectile.ai[0] = -1f;
            projectile.velocity *= 0f;
            projectile.alpha = 255;
            projectile.position.X = num207 * 16;
            projectile.position.Y = num208 * 16;
            projectile.netUpdate = true;
        }
        else
        {
            projectile.ai[1] = -1f;
        }
    }
    public static void AI_023(Projectile projectile)
    {
        if (projectile.type == 188)
        {
            if (projectile.ai[0] < 8f)
            {
                projectile.ai[0] = 8f;
            }
            projectile.localAI[0]++;
        }
        if (projectile.timeLeft > 60)
        {
            projectile.timeLeft = 60;
        }
        if (projectile.ai[0] > 7f)
        {
            projectile.ai[0] += 1f;
        }
        else
        {
            projectile.ai[0] += 1f;
        }
        projectile.rotation += 0.3f * (float)projectile.direction;
    }
    public static void AI_024(Projectile projectile)
    {
        projectile.light = projectile.scale * 0.5f;
        projectile.rotation += projectile.velocity.X * 0.2f;
        projectile.ai[1] += 1f;
        if (projectile.type == 94)
        {
            projectile.velocity *= 0.985f;
            if (projectile.ai[1] > 130f)
            {
                projectile.scale -= 0.05f;
                if ((double)projectile.scale <= 0.2)
                {
                    projectile.scale = 0.2f;
                    projectile.Kill();
                }
            }
            return;
        }
        projectile.velocity *= 0.96f;
        if (projectile.ai[1] > 15f)
        {
            projectile.scale -= 0.05f;
            if ((double)projectile.scale <= 0.2)
            {
                projectile.scale = 0.2f;
                projectile.Kill();
            }
        }
    }
    public static void AI_025(Projectile projectile)
    {
        if (projectile.type == 1013)
        {
            projectile.localAI[0]++;
        }
        if (projectile.type == 1014)
        {
            projectile.frame = Main.tileFrame[665];
        }
        if (projectile.ai[0] != 0f && projectile.velocity.Y <= 0f && projectile.velocity.X == 0f)
        {
            float num216 = 0.5f;
            int i2 = (int)((projectile.position.X - 8f) / 16f);
            int num217 = (int)(projectile.position.Y / 16f);
            bool flag9 = false;
            bool flag10 = false;
            if (WorldGen.SolidTile(i2, num217) || WorldGen.SolidTile(i2, num217 + 1))
            {
                flag9 = true;
            }
            i2 = (int)((projectile.position.X + (float)projectile.width + 8f) / 16f);
            if (WorldGen.SolidTile(i2, num217) || WorldGen.SolidTile(i2, num217 + 1))
            {
                flag10 = true;
            }
            if (flag9)
            {
                projectile.velocity.X = num216;
            }
            else if (flag10)
            {
                projectile.velocity.X = 0f - num216;
            }
            else
            {
                i2 = (int)((projectile.position.X - 8f - 16f) / 16f);
                num217 = (int)(projectile.position.Y / 16f);
                flag9 = false;
                flag10 = false;
                if (WorldGen.SolidTile(i2, num217) || WorldGen.SolidTile(i2, num217 + 1))
                {
                    flag9 = true;
                }
                i2 = (int)((projectile.position.X + (float)projectile.width + 8f + 16f) / 16f);
                if (WorldGen.SolidTile(i2, num217) || WorldGen.SolidTile(i2, num217 + 1))
                {
                    flag10 = true;
                }
                if (flag9)
                {
                    projectile.velocity.X = num216;
                }
                else if (flag10)
                {
                    projectile.velocity.X = 0f - num216;
                }
                else
                {
                    i2 = (int)((projectile.position.X - 8f - 32f) / 16f);
                    num217 = (int)(projectile.position.Y / 16f);
                    flag9 = false;
                    flag10 = false;
                    if (WorldGen.SolidTile(i2, num217) || WorldGen.SolidTile(i2, num217 + 1))
                    {
                        flag9 = true;
                    }
                    i2 = (int)((projectile.position.X + (float)projectile.width + 8f + 32f) / 16f);
                    if (WorldGen.SolidTile(i2, num217) || WorldGen.SolidTile(i2, num217 + 1))
                    {
                        flag10 = true;
                    }
                    if (!flag9 && !flag10)
                    {
                        if ((int)(projectile.Center.X / 16f) % 2 == 0)
                        {
                            flag9 = true;
                        }
                        else
                        {
                            flag10 = true;
                        }
                    }
                    if (flag9)
                    {
                        projectile.velocity.X = num216;
                    }
                    else if (flag10)
                    {
                        projectile.velocity.X = 0f - num216;
                    }
                }
            }
        }
        projectile.rotation += projectile.velocity.X * 0.06f;
        projectile.ai[0] = 1f;
        if (projectile.velocity.Y > 16f)
        {
            projectile.velocity.Y = 16f;
        }
        if (projectile.type == 1021)
        {
            if (Math.Abs(projectile.velocity.Y) <= 1f)
            {
                if (projectile.velocity.X > 0f && (double)projectile.velocity.X < 3.5)
                {
                    projectile.velocity.X += 0.025f;
                }
                if (projectile.velocity.X < 0f && (double)projectile.velocity.X > -3.5)
                {
                    projectile.velocity.X -= 0.025f;
                }
            }
        }
        else if (projectile.velocity.Y <= 6f)
        {
            if (projectile.velocity.X > 0f && projectile.velocity.X < 7f)
            {
                projectile.velocity.X += 0.05f;
            }
            if (projectile.velocity.X < 0f && projectile.velocity.X > -7f)
            {
                projectile.velocity.X -= 0.05f;
            }
        }
        if (projectile.type == 1021)
        {
            projectile.velocity.Y += 0.06f;
        }
        else
        {
            projectile.velocity.Y += 0.3f;
        }
        if (projectile.type == 655 && projectile.wet)
        {
            projectile.Kill();
        }
    }
    public static void AI_027(Projectile projectile)
    {
        if (projectile.type == 115)
        {
            projectile.ai[0] += 1f;
            if (projectile.ai[0] < 30f)
            {
                projectile.velocity *= 1.125f;
            }
        }
        if (projectile.type == 115 && projectile.localAI[1] < 5f)
        {
            projectile.localAI[1] = 5f;
        }
        if (projectile.localAI[1] < 15f)
        {
            projectile.localAI[1] += 1f;
        }
        else
        {
            if (projectile.localAI[0] == 0f)
            {
                projectile.scale -= 0.02f;
                projectile.alpha += 30;
                if (projectile.alpha >= 250)
                {
                    projectile.alpha = 255;
                    projectile.localAI[0] = 1f;
                }
            }
            else if (projectile.localAI[0] == 1f)
            {
                projectile.scale += 0.02f;
                projectile.alpha -= 30;
                if (projectile.alpha <= 0)
                {
                    projectile.alpha = 0;
                    projectile.localAI[0] = 0f;
                }
            }
        }
        if (projectile.ai[1] == 0f)
        {
            projectile.ai[1] = 1f;
            if (projectile.type == 132)
            {
                SoundEngine.PlaySound(SoundID.Item60, projectile.position);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.Item8, projectile.position);
            }
        }
        if (projectile.type == 157)
        {
            projectile.rotation += (float)projectile.direction * 0.4f;
            projectile.spriteDirection = projectile.direction;
        }
        else
        {
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 0.785f;
        }
        if (projectile.velocity.Y > 16f)
        {
            projectile.velocity.Y = 16f;
        }
    }
    public static void AI_028(Projectile projectile)
    {
        if (projectile.type == 967)
        {
            projectile.localAI[0]++;
            if (projectile.localAI[0] >= 5f && projectile.timeLeft % 3 == 0)
            {
                projectile.localAI[0] = 5f;
            }
            if (++projectile.frameCounter > 6)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }
        if (projectile.ai[1] != 0f)
        {
            return;
        }
        projectile.ai[1] = 1f;
    }
    public static void AI_029(Projectile projectile)
    {
        if (projectile.type == 619)
        {
        }
        else if (projectile.type == 620)
        {
            int num244 = (int)projectile.ai[0];
            projectile.ai[1] += 1f;
            float num245 = (60f - projectile.ai[1]) / 60f;
            if (projectile.ai[1] > 40f)
            {
                projectile.Kill();
            }
            projectile.velocity.Y += 0.2f;
            if (projectile.velocity.Y > 18f)
            {
                projectile.velocity.Y = 18f;
            }
            projectile.velocity.X *= 0.98f;
        }
        else if (projectile.type == 521)
        {
        }
        else if (projectile.type == 522)
        {
            projectile.ai[1] += 1f;
            float num252 = (60f - projectile.ai[1]) / 60f;
            if (projectile.ai[1] > 40f)
            {
                projectile.Kill();
            }
            projectile.velocity.Y += 0.2f;
            if (projectile.velocity.Y > 18f)
            {
                projectile.velocity.Y = 18f;
            }
            projectile.velocity.X *= 0.98f;
        }
        else if (projectile.type == 731)
        {
            if (++projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
            projectile.alpha -= 15;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            projectile.ai[0]++;
            if ((int)projectile.ai[0] % 2 != 0 && Main.rand.Next(4) == 0)
            {
                projectile.ai[0]++;
            }
            float num256 = 5f;
            switch ((int)projectile.ai[0])
            {
                case 10:
                    projectile.velocity.Y -= num256;
                    break;
                case 12:
                    projectile.velocity.Y += num256;
                    break;
                case 18:
                    projectile.velocity.Y += num256;
                    break;
                case 20:
                    projectile.velocity.Y -= num256;
                    projectile.ai[0] = 0f;
                    break;
            }
            Lighting.AddLight(projectile.Center, 0.2f, 0.5f, 0.7f);
        }
        else
        {
            int num257 = projectile.type - 121 + 86;
            if (projectile.type == 597)
            {
                num257 = 262;
            }
            if (projectile.ai[1] == 0f)
            {
                projectile.ai[1] = 1f;
                SoundEngine.PlaySound(SoundID.Item8, projectile.position);
            }
        }
    }
    public static void AI_030(Projectile projectile)
    {
        if (projectile.type == 907)
        {
            float num260 = 100f;
            float num261 = num260 - 50f;
            if (projectile.ai[0] > num261)
            {
                projectile.velocity *= 0.9f;
                projectile.rotation *= 0.9f;
            }
            else
            {
                projectile.rotation += 0.2f;
                if (projectile.rotation > (float)Math.PI * 2f)
                {
                    projectile.rotation -= (float)Math.PI * 2f;
                }
            }
            float num264 = projectile.ai[0];
            projectile.ai[0]++;
            if (Main.myPlayer == projectile.owner && projectile.ai[0] < num261 && projectile.ai[0] % 10f == 0f)
            {
                float num265 = (float)Math.PI / 2f * (float)((projectile.ai[0] % 20f != 0f) ? 1 : (-1));
                num265 *= (float)((projectile.whoAmI % 2 != 0) ? 1 : (-1));
                num265 += (float)Main.rand.Next(-5, 5) * MathHelper.Lerp(0.2f, 0.03f, projectile.ai[0] / num261);
                Vector2 v3 = projectile.velocity.RotatedBy(num265);
                v3 = v3.SafeNormalize(Vector2.Zero);
                v3 *= Math.Max(2.5f, (num261 - projectile.ai[0]) / num261 * (7f + (-2f + (float)Main.rand.Next(2) * 2f)));
                int num266 = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center, v3, 335, projectile.damage, projectile.knockBack * 0.25f, projectile.owner, 0f, Main.rand.Next(4));
            }
            if (num264 <= num261 && projectile.ai[0] > num261)
            {
                projectile.netUpdate = true;
            }
            if (projectile.ai[0] > num260)
            {
                projectile.Kill();
            }
        }
        else if (projectile.type == 335)
        {
            int num267 = (projectile.frame = (int)projectile.ai[1]);
            if (projectile.ai[0] < 0f)
            {
                projectile.velocity.Y += 0.25f;
                if (projectile.velocity.Y > 14f)
                {
                    projectile.velocity.Y = 14f;
                }
            }
            else
            {
                if (Main.rand.Next(Math.Max(4, 8 - (int)projectile.velocity.Length())) == 0)
                {
                    Color newColor = Color.White;
                    switch (num267)
                    {
                        case 0:
                            newColor = new Color(255, 100, 100);
                            break;
                        case 1:
                            newColor = new Color(100, 255, 100);
                            break;
                        case 2:
                            newColor = new Color(100, 100, 255);
                            break;
                        case 3:
                            newColor = new Color(255, 255, 100);
                            break;
                    }
                }
                projectile.velocity *= 0.95f;
            }
            if (projectile.ai[0] >= 0f && projectile.velocity.Length() < 0.25f)
            {
                if (projectile.velocity != Vector2.Zero)
                {
                    projectile.velocity = Vector2.Zero;
                    if (Main.netMode != 1)
                    {
                        projectile.ai[0] = 50f;
                        projectile.netUpdate = true;
                    }
                }
                projectile.ai[0]--;
            }
            projectile.localAI[0]++;
            projectile.rotation = (float)Math.Sin(projectile.localAI[0] / 10f);
        }
        else
        {
            projectile.velocity *= 0.8f;
            projectile.rotation += 0.2f;
            projectile.alpha += 4;
            if (projectile.alpha >= 255)
            {
                projectile.Kill();
            }
        }
    }
    public static void AI_031(Projectile projectile)
    {
        bool flag11 = projectile.ai[1] == 1f;
        int num271 = 0;
        switch (projectile.type)
        {
            default:
                num271 = 0;
                break;
            case 147:
                num271 = 1;
                break;
            case 146:
                num271 = 2;
                break;
            case 148:
                num271 = 3;
                break;
            case 149:
                num271 = 4;
                break;
            case 1015:
                num271 = 5;
                break;
            case 1016:
                num271 = 6;
                break;
            case 1017:
                num271 = 7;
                break;
        }
        if (projectile.owner == Main.myPlayer)
        {
            int size = 2;
            if (flag11)
            {
                size = 3;
            }
            Point point = projectile.Center.ToTileCoordinates();
            WorldGen.Convert(point.X, point.Y, num271, size);
        }
        if (projectile.timeLeft > 133)
        {
            projectile.timeLeft = 133;
        }
        int num272 = 7;
        if (flag11)
        {
            num272 = 3;
        }
        if (projectile.ai[0] > (float)num272)
        {
            float num273 = 1f;
            if (projectile.ai[0] == (float)(num272 + 1))
            {
                num273 = 0.2f;
            }
            else if (projectile.ai[0] == (float)(num272 + 2))
            {
                num273 = 0.4f;
            }
            else if (projectile.ai[0] == (float)(num272 + 3))
            {
                num273 = 0.6f;
            }
            else if (projectile.ai[0] == (float)(num272 + 4))
            {
                num273 = 0.8f;
            }
            int num274 = 0;
            if (flag11)
            {
                num273 *= 1.2f;
                num274 = (int)(12f * num273);
            }
            projectile.ai[0]++;
        }
        else
        {
            projectile.ai[0]++;
        }
        projectile.rotation += 0.3f * (float)projectile.direction;
    }
    public static void AI_032(Projectile projectile)
    {
        projectile.timeLeft = 10;
        projectile.ai[0] += 1f;
        if (projectile.ai[0] >= 20f)
        {
            projectile.ai[0] = 18f;
            Rectangle rectangle3 = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
            for (int num277 = 0; num277 < 255; num277++)
            {
                Entity entity = Main.player[num277];
                if (entity.active && rectangle3.Intersects(entity.Hitbox))
                {
                    projectile.ai[0] = 0f;
                    projectile.velocity.Y = -4.5f;
                    if (projectile.velocity.X > 2f)
                    {
                        projectile.velocity.X = 2f;
                    }
                    if (projectile.velocity.X < -2f)
                    {
                        projectile.velocity.X = -2f;
                    }
                    projectile.velocity.X = (projectile.velocity.X + (float)entity.direction * 1.75f) / 2f;
                    projectile.velocity.X += entity.velocity.X * 3f;
                    projectile.velocity.Y += entity.velocity.Y;
                    if (projectile.velocity.X > 6f)
                    {
                        projectile.velocity.X = 6f;
                    }
                    if (projectile.velocity.X < -6f)
                    {
                        projectile.velocity.X = -6f;
                    }
                    if (projectile.velocity.Length() > 16f)
                    {
                        projectile.velocity = projectile.velocity.SafeNormalize(Vector2.Zero) * 16f;
                    }
                    projectile.netUpdate = true;
                    projectile.ai[1] += 1f;
                }
            }
            for (int num278 = 0; num278 < 1000; num278++)
            {
                if (num278 == projectile.whoAmI)
                {
                    continue;
                }
                Entity entity = Main.projectile[num278];
                if (entity.active && rectangle3.Intersects(entity.Hitbox))
                {
                    projectile.ai[0] = 0f;
                    projectile.velocity.Y = -4.5f;
                    if (projectile.velocity.X > 2f)
                    {
                        projectile.velocity.X = 2f;
                    }
                    if (projectile.velocity.X < -2f)
                    {
                        projectile.velocity.X = -2f;
                    }
                    projectile.velocity.X = (projectile.velocity.X + (float)entity.direction * 1.75f) / 2f;
                    projectile.velocity.X += entity.velocity.X * 3f;
                    projectile.velocity.Y += entity.velocity.Y;
                    if (projectile.velocity.X > 6f)
                    {
                        projectile.velocity.X = 6f;
                    }
                    if (projectile.velocity.X < -6f)
                    {
                        projectile.velocity.X = -6f;
                    }
                    if (projectile.velocity.Length() > 16f)
                    {
                        projectile.velocity = projectile.velocity.SafeNormalize(Vector2.Zero) * 16f;
                    }
                    projectile.netUpdate = true;
                    projectile.ai[1] += 1f;
                }
            }
        }
        if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
        {
            projectile.Kill();
        }
        projectile.rotation += 0.02f * projectile.velocity.X;
        if (projectile.velocity.Y == 0f)
        {
            projectile.velocity.X *= 0.98f;
        }
        else if (projectile.wet)
        {
            projectile.velocity.X *= 0.99f;
        }
        else
        {
            projectile.velocity.X *= 0.995f;
        }
        if ((double)projectile.velocity.X > -0.03 && (double)projectile.velocity.X < 0.03)
        {
            projectile.velocity.X = 0f;
        }
        if (projectile.wet)
        {
            projectile.ai[1] = 0f;
            if (projectile.velocity.Y > 0f)
            {
                projectile.velocity.Y *= 0.95f;
            }
            projectile.velocity.Y -= 0.1f;
            if (projectile.velocity.Y < -4f)
            {
                projectile.velocity.Y = -4f;
            }
            if (projectile.velocity.X == 0f)
            {
                projectile.Kill();
            }
        }
        else
        {
            projectile.velocity.Y += 0.1f;
        }
        if (projectile.velocity.Y > 10f)
        {
            projectile.velocity.Y = 10f;
        }
    }
    public static void AI_033(Projectile projectile)
    {
        if (projectile.alpha > 0)
        {
            projectile.alpha -= 50;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
        }
        float num279 = 4f;
        float num280 = projectile.ai[0];
        float num281 = projectile.ai[1];
        if (num280 == 0f && num281 == 0f)
        {
            num280 = 1f;
        }
        float num282 = (float)Math.Sqrt(num280 * num280 + num281 * num281);
        num282 = num279 / num282;
        num280 *= num282;
        num281 *= num282;
        if (projectile.localAI[0] == 0f)
        {
            projectile.ai[0] = projectile.velocity.X;
            projectile.ai[1] = projectile.velocity.Y;
            projectile.localAI[1] += 1f;
            if (projectile.localAI[1] >= 30f)
            {
                projectile.velocity.Y += 0.09f;
                projectile.localAI[1] = 30f;
            }
        }
        else
        {
            if (!Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.localAI[0] = 0f;
                projectile.localAI[1] = 30f;
            }
            if (projectile.type == 1008 && Main.netMode != 2)
            {
                int num285 = 30;
                if ((projectile.Center - Main.player[Main.myPlayer].Center).Length() < (float)(Main.screenWidth + num285 * 16))
                {
                    Main.instance.SpelunkerProjectileHelper.AddSpotToCheck(projectile.Center);
                }
            }
            projectile.damage = 0;
        }
        if (projectile.velocity.Y > 16f)
        {
            projectile.velocity.Y = 16f;
        }
        projectile.rotation = (float)Math.Atan2(projectile.ai[1], projectile.ai[0]) + 1.57f;
    }
    public static void AI_034(Projectile projectile)
    {
        projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
        if (projectile.ai[1] == 1f)
        {
            projectile.ai[0]++;
        }
        else if (projectile.type >= 415 && projectile.type <= 418)
        {
            projectile.ai[0]++;
        }
    }
    public static void AI_035(Projectile projectile)
    {
        projectile.ai[0] += 1f;
        if (projectile.ai[0] > 30f)
        {
            projectile.velocity.Y += 0.2f;
            projectile.velocity.X *= 0.985f;
            if (projectile.velocity.Y > 14f)
            {
                projectile.velocity.Y = 14f;
            }
        }
        projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * (float)projectile.direction * 0.02f;
        if (projectile.owner != Main.myPlayer)
        {
            return;
        }
        Vector2 vector25 = Collision.TileCollision(projectile.position, projectile.velocity, projectile.width, projectile.height, fallThrough: true, fall2: true);
        bool flag12 = false;
        if (vector25 != projectile.velocity)
        {
            flag12 = true;
        }
        else
        {
            int num291 = (int)(projectile.Center.X + projectile.velocity.X) / 16;
            int num292 = (int)(projectile.Center.Y + projectile.velocity.Y) / 16;
            if (Main.tile[num291, num292] != null && Main.tile[num291, num292].active() && Main.tile[num291, num292].bottomSlope())
            {
                flag12 = true;
                projectile.position.Y = num292 * 16 + 16 + 8;
                projectile.position.X = num291 * 16 + 8;
            }
        }
        if (!flag12)
        {
            return;
        }
        int num293 = 213;
        if (projectile.type == 475)
        {
            num293 = 353;
        }
        if (projectile.type == 506)
        {
            num293 = 366;
        }
        if (projectile.type == 505)
        {
            num293 = 365;
        }
        int num294 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
        int num295 = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
        projectile.position += vector25;
        int num296 = 10;
        if (Main.tile[num294, num295] == null)
        {
            return;
        }
        for (; WorldGen.IsRope(num294, num295); num295++)
        {
        }
        bool flag13 = false;
        while (num296 > 0)
        {
            bool flag14 = false;
            if (Main.tile[num294, num295] == null)
            {
                break;
            }
            if (Main.tile[num294, num295].active())
            {
                if (Main.tile[num294, num295].type == 314 || TileID.Sets.Platforms[Main.tile[num294, num295].type])
                {
                    flag13 = ((!flag13) ? true : false);
                }
                else if (Main.tileCut[Main.tile[num294, num295].type] || Main.tile[num294, num295].type == 165)
                {
                    flag13 = false;
                    WorldGen.KillTile(num294, num295);
                    NetMessage.SendData(17, -1, -1, null, 0, num294, num295);
                }
            }
            if (!Main.tile[num294, num295].active())
            {
                flag13 = false;
                flag14 = true;
                WorldGen.PlaceTile(num294, num295, num293);
                NetMessage.SendData(17, -1, -1, null, 1, num294, num295, num293);
                projectile.ai[1] += 1f;
            }
            else if (!flag13)
            {
                num296 = 0;
            }
            if (flag14)
            {
                num296--;
            }
            num295++;
        }
        projectile.Kill();
    }
    public static void AI_036(Projectile projectile)
    {
        if (projectile.type != 307 && projectile.wet && !projectile.honeyWet && !projectile.shimmerWet)
        {
            projectile.Kill();
        }
        if (projectile.alpha > 0)
        {
            projectile.alpha -= 50;
        }
        else
        {
            projectile.extraUpdates = 0;
        }
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        if (projectile.type == 307)
        {
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) - 1.57f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 6)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 2)
            {
                projectile.frame = 0;
            }
        }
        else
        {
            if (projectile.type == 316)
            {
                if (projectile.velocity.X > 0f)
                {
                    projectile.spriteDirection = -1;
                }
                else if (projectile.velocity.X < 0f)
                {
                    projectile.spriteDirection = 1;
                }
            }
            else if (projectile.velocity.X > 0f)
            {
                projectile.spriteDirection = 1;
            }
            else if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = -1;
            }
            projectile.rotation = projectile.velocity.X * 0.1f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 3)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame >= 3)
            {
                projectile.frame = 0;
            }
        }
        float num301 = projectile.position.X;
        float num302 = projectile.position.Y;
        float num303 = 100000f;
        bool flag15 = false;
        projectile.ai[0] += 1f;
        if (projectile.ai[0] > 30f)
        {
            projectile.ai[0] = 30f;
            for (int num304 = 0; num304 < 200; num304++)
            {
                if (Main.npc[num304].CanBeChasedBy(projectile) && (!Main.npc[num304].wet || Main.npc[num304].type == 370 || projectile.type == 307))
                {
                    float num305 = Main.npc[num304].position.X + (float)(Main.npc[num304].width / 2);
                    float num306 = Main.npc[num304].position.Y + (float)(Main.npc[num304].height / 2);
                    float num307 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num305) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num306);
                    if (num307 < 800f && num307 < num303 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num304].position, Main.npc[num304].width, Main.npc[num304].height))
                    {
                        num303 = num307;
                        num301 = num305;
                        num302 = num306;
                        flag15 = true;
                    }
                }
            }
        }
        if (!flag15)
        {
            num301 = projectile.position.X + (float)(projectile.width / 2) + projectile.velocity.X * 100f;
            num302 = projectile.position.Y + (float)(projectile.height / 2) + projectile.velocity.Y * 100f;
        }
        else if (projectile.type == 307)
        {
            projectile.friendly = true;
        }
        float num308 = 6f;
        float num309 = 0.1f;
        if (projectile.type == 189)
        {
            num308 = 9f;
            num309 = 0.2f;
        }
        if (projectile.type == 307)
        {
            num308 = 13f;
            num309 = 0.35f;
        }
        if (projectile.type == 316)
        {
            if (flag15)
            {
                num308 = 13f;
                num309 = 0.325f;
            }
            else
            {
                num308 = 10f;
                num309 = 0.25f;
            }
        }
        if (projectile.type == 566)
        {
            num308 = 6.8f;
            num309 = 0.14f;
        }
        Vector2 vector26 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
        float num310 = num301 - vector26.X;
        float num311 = num302 - vector26.Y;
        float num312 = (float)Math.Sqrt(num310 * num310 + num311 * num311);
        float num313 = num312;
        num312 = num308 / num312;
        num310 *= num312;
        num311 *= num312;
        if (projectile.velocity.X < num310)
        {
            projectile.velocity.X += num309;
            if (projectile.velocity.X < 0f && num310 > 0f)
            {
                projectile.velocity.X += num309 * 2f;
            }
        }
        else if (projectile.velocity.X > num310)
        {
            projectile.velocity.X -= num309;
            if (projectile.velocity.X > 0f && num310 < 0f)
            {
                projectile.velocity.X -= num309 * 2f;
            }
        }
        if (projectile.velocity.Y < num311)
        {
            projectile.velocity.Y += num309;
            if (projectile.velocity.Y < 0f && num311 > 0f)
            {
                projectile.velocity.Y += num309 * 2f;
            }
        }
        else if (projectile.velocity.Y > num311)
        {
            projectile.velocity.Y -= num309;
            if (projectile.velocity.Y > 0f && num311 < 0f)
            {
                projectile.velocity.Y -= num309 * 2f;
            }
        }
    }
    public static void AI_037(Projectile projectile)
    {
        if (projectile.ai[1] == 0f)
        {
            projectile.ai[1] = 1f;
            projectile.localAI[0] = projectile.Center.X - projectile.velocity.X * 1.5f;
            projectile.localAI[1] = projectile.Center.Y - projectile.velocity.Y * 1.5f;
        }
        Vector2 vector27 = new Vector2(projectile.localAI[0], projectile.localAI[1]);
        projectile.rotation = (projectile.Center - vector27).ToRotation() - (float)Math.PI / 2f;
        if (projectile.ai[0] == 0f)
        {
            if (Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.velocity *= -1f;
                projectile.ai[0] += 1f;
                return;
            }
            float num314 = Vector2.Distance(projectile.Center, vector27);
            if (num314 > 300f)
            {
                projectile.velocity *= -1f;
                projectile.ai[0] += 1f;
            }
        }
        else if (Collision.SolidCollision(projectile.position, projectile.width, projectile.height) || Vector2.Distance(projectile.Center, vector27) < projectile.velocity.Length())
        {
            projectile.Kill();
        }
    }
    public static void AI_038(Projectile projectile)
    {
        projectile.ai[0] += 1f;
        if (projectile.ai[0] >= 6f)
        {
            projectile.ai[0] = 0f;
            SoundEngine.PlaySound(SoundID.Item34, projectile.position);
            if (Main.myPlayer == projectile.owner)
            {
                Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.position.X, projectile.position.Y, projectile.velocity.X, projectile.velocity.Y, 188, projectile.damage, projectile.knockBack, projectile.owner);
            }
        }
    }
    public static void AI_039(Projectile projectile)
    {
        projectile.alpha -= 50;
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        if (!projectile.active || !Main.player[projectile.owner].active || Main.player[projectile.owner].dead || Vector2.Distance(Main.player[projectile.owner].Center, projectile.Center) > 2000f)
        {
            projectile.Kill();
            return;
        }
        if (projectile.active && projectile.alpha == 0)
        {
            Main.player[projectile.owner].SetDummyItemTime(5);
            if (projectile.Center.X > Main.player[projectile.owner].Center.X)
            {
                Main.player[projectile.owner].ChangeDir(1);
            }
            else
            {
                Main.player[projectile.owner].ChangeDir(-1);
            }
        }
        Vector2 center = projectile.Center;
        float num315 = Main.player[projectile.owner].Center.X - center.X;
        float num316 = Main.player[projectile.owner].Center.Y - center.Y;
        float num317 = (float)Math.Sqrt(num315 * num315 + num316 * num316);
        if (!Main.player[projectile.owner].channel && projectile.active && projectile.alpha == 0)
        {
            projectile.ai[0] = 1f;
            projectile.ai[1] = -1f;
        }
        if (projectile.ai[1] > 0f && num317 > 1500f)
        {
            projectile.ai[1] = 0f;
            projectile.ai[0] = 1f;
        }
        if (projectile.ai[1] > 0f)
        {
            projectile.tileCollide = false;
            int num318 = (int)projectile.ai[1] - 1;
            if (Main.npc[num318].active && Main.npc[num318].life > 0)
            {
                float num319 = 16f;
                center = projectile.Center;
                num315 = Main.npc[num318].Center.X - center.X;
                num316 = Main.npc[num318].Center.Y - center.Y;
                num317 = (float)Math.Sqrt(num315 * num315 + num316 * num316);
                if (num317 < num319)
                {
                    projectile.velocity.X = num315;
                    projectile.velocity.Y = num316;
                    if (num317 > num319 / 3f)
                    {
                        if (projectile.velocity.X < 0f)
                        {
                            projectile.spriteDirection = -1;
                            projectile.rotation = (float)Math.Atan2(0f - projectile.velocity.Y, 0f - projectile.velocity.X);
                        }
                        else
                        {
                            projectile.spriteDirection = 1;
                            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
                        }
                        if (projectile.type == 190)
                        {
                            projectile.velocity.X = 0f;
                            projectile.velocity.Y = 0f;
                        }
                    }
                }
                else
                {
                    if (num317 == 0f)
                    {
                        num317 = 0.0001f;
                    }
                    num317 = num319 / num317;
                    num315 *= num317;
                    num316 *= num317;
                    projectile.velocity.X = num315;
                    projectile.velocity.Y = num316;
                    if (projectile.velocity.X < 0f)
                    {
                        projectile.spriteDirection = -1;
                        projectile.rotation = (float)Math.Atan2(0f - projectile.velocity.Y, 0f - projectile.velocity.X);
                    }
                    else
                    {
                        projectile.spriteDirection = 1;
                        projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
                    }
                }
                if (projectile.type == 190)
                {
                    projectile.position += Main.npc[num318].velocity;
                    for (int num320 = 0; num320 < 1000; num320++)
                    {
                        if (num320 != projectile.whoAmI && Main.projectile[num320].active && Main.projectile[num320].owner == projectile.owner && Main.projectile[num320].type == 190 && Vector2.Distance(projectile.Center, Main.projectile[num320].Center) < 8f)
                        {
                            if (projectile.position.X < Main.projectile[num320].position.X)
                            {
                                projectile.velocity.X -= 4f;
                            }
                            else
                            {
                                projectile.velocity.X += 4f;
                            }
                            if (projectile.position.Y < Main.projectile[num320].position.Y)
                            {
                                projectile.velocity.Y -= 4f;
                            }
                            else
                            {
                                projectile.velocity.Y += 4f;
                            }
                        }
                    }
                }
                if (Main.myPlayer == projectile.owner)
                {
                    float num321 = projectile.ai[0];
                    projectile.ai[0] = 1f;
                    if (num321 != projectile.ai[0])
                    {
                        projectile.netUpdate = true;
                    }
                }
            }
            else if (Main.myPlayer == projectile.owner)
            {
                float num322 = projectile.ai[1];
                projectile.ai[1] = 0f;
                if (num322 != projectile.ai[1])
                {
                    projectile.netUpdate = true;
                }
                float num323 = projectile.position.X;
                float num324 = projectile.position.Y;
                float num325 = 3000f;
                int num326 = -1;
                for (int num327 = 0; num327 < 200; num327++)
                {
                    if (Main.npc[num327].CanBeChasedBy(projectile))
                    {
                        float x2 = Main.npc[num327].Center.X;
                        float y2 = Main.npc[num327].Center.Y;
                        float num328 = Math.Abs(projectile.Center.X - x2) + Math.Abs(projectile.Center.Y - y2);
                        if (num328 < num325 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num327].position, Main.npc[num327].width, Main.npc[num327].height))
                        {
                            num325 = num328;
                            num323 = x2;
                            num324 = y2;
                            num326 = num327;
                        }
                    }
                }
                if (num326 >= 0)
                {
                    float num329 = 16f;
                    center = projectile.Center;
                    num315 = num323 - center.X;
                    num316 = num324 - center.Y;
                    num317 = (float)Math.Sqrt(num315 * num315 + num316 * num316);
                    if (num317 == 0f)
                    {
                        num317 = 0.0001f;
                    }
                    num317 = num329 / num317;
                    num315 *= num317;
                    num316 *= num317;
                    projectile.velocity.X = num315;
                    projectile.velocity.Y = num316;
                    projectile.ai[0] = 0f;
                    projectile.ai[1] = num326 + 1;
                    projectile.netUpdate = true;
                }
            }
        }
        else if (projectile.ai[0] == 0f)
        {
            if (Main.myPlayer == projectile.owner && num317 > 700f)
            {
                projectile.ai[0] = 1f;
                projectile.netUpdate = true;
            }
            if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = -1;
                projectile.rotation = (float)Math.Atan2(0f - projectile.velocity.Y, 0f - projectile.velocity.X);
            }
            else
            {
                projectile.spriteDirection = 1;
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
            }
        }
        else if (projectile.ai[0] == 1f)
        {
            projectile.tileCollide = false;
            if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = 1;
                projectile.rotation = (float)Math.Atan2(0f - projectile.velocity.Y, 0f - projectile.velocity.X);
            }
            else
            {
                projectile.spriteDirection = -1;
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
            }
            if (projectile.velocity.X < 0f)
            {
                projectile.spriteDirection = -1;
                projectile.rotation = (float)Math.Atan2(0f - projectile.velocity.Y, 0f - projectile.velocity.X);
            }
            else
            {
                projectile.spriteDirection = 1;
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
            }
            float num330 = 20f;
            if (Main.myPlayer == projectile.owner && num317 < 70f)
            {
                projectile.Kill();
            }
            num317 = num330 / num317;
            num315 *= num317;
            num316 *= num317;
            projectile.velocity.X = num315;
            projectile.velocity.Y = num316;
            if (projectile.type == 190)
            {
                projectile.position += Main.player[projectile.owner].velocity;
            }
        }
        projectile.frameCounter++;
        if (projectile.frameCounter >= 4)
        {
            projectile.frame++;
            projectile.frameCounter = 0;
        }
        if (projectile.frame >= 4)
        {
            projectile.frame = 0;
        }
    }
    public static void AI_040(Projectile projectile)
    {
        projectile.localAI[0] += 1f;
        if (projectile.localAI[0] > 3f)
        {
            projectile.localAI[0] = 100f;
            projectile.alpha -= 50;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
        }
        projectile.frameCounter++;
        if (projectile.frameCounter >= 3)
        {
            projectile.frame++;
            projectile.frameCounter = 0;
        }
        if (projectile.frame >= 5)
        {
            projectile.frame = 0;
        }
        projectile.velocity.X += projectile.ai[0];
        projectile.velocity.Y += projectile.ai[1];
        projectile.localAI[1] += 1f;
        if (projectile.localAI[1] == 50f)
        {
            projectile.localAI[1] = 51f;
            projectile.ai[0] = (float)Main.rand.Next(-100, 101) * 6E-05f;
            projectile.ai[1] = (float)Main.rand.Next(-100, 101) * 6E-05f;
        }
        if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) > 16f)
        {
            projectile.velocity.X *= 0.95f;
            projectile.velocity.Y *= 0.95f;
        }
        if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) < 12f)
        {
            projectile.velocity.X *= 1.05f;
            projectile.velocity.Y *= 1.05f;
        }
        projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 3.14f;
    }
    public static void AI_041(Projectile projectile)
    {
        if (projectile.localAI[0] == 0f)
        {
            projectile.localAI[0] = 1f;
            projectile.frame = Main.rand.Next(3);
        }
        projectile.rotation += projectile.velocity.X * 0.01f;
    }
    public static void AI_042(Projectile projectile)
    {
        if (!Main.player[projectile.owner].crystalLeaf)
        {
            projectile.Kill();
            return;
        }
        projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
        projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.height / 2) + Main.player[projectile.owner].gfxOffY - 60f;
        if (Main.player[projectile.owner].gravDir == -1f)
        {
            projectile.position.Y += 120f;
            projectile.rotation = 3.14f;
        }
        else
        {
            projectile.rotation = 0f;
        }
        projectile.position.X = (int)projectile.position.X;
        projectile.position.Y = (int)projectile.position.Y;
        float num331 = (float)(int)Main.mouseTextColor / 200f - 0.35f;
        num331 *= 0.2f;
        projectile.scale = num331 + 0.95f;
        if (projectile.owner != Main.myPlayer || Main.player[projectile.owner].crystalLeafCooldown != 0)
        {
            return;
        }
        float x3 = projectile.position.X;
        float y3 = projectile.position.Y;
        float num332 = 700f;
        NPC? nPC = null;
        for (int num333 = 0; num333 < 200; num333++)
        {
            if (Main.npc[num333].CanBeChasedBy(projectile))
            {
                float num334 = Main.npc[num333].position.X + (float)(Main.npc[num333].width / 2);
                float num335 = Main.npc[num333].position.Y + (float)(Main.npc[num333].height / 2);
                float num336 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num334) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num335);
                if (num336 < num332 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num333].position, Main.npc[num333].width, Main.npc[num333].height))
                {
                    num332 = num336;
                    nPC = Main.npc[num333];
                }
            }
        }
        if (nPC != null)
        {
            float num337 = 12f;
            Vector2 vector28 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
            float num338 = x3 - vector28.X;
            float num339 = y3 - vector28.Y;
            float num340 = (float)Math.Sqrt(num338 * num338 + num339 * num339);
            float num341 = num340;
            num340 = num337 / num340;
            num338 *= num340;
            num339 *= num340;
            int num342 = 180;
            Utils.ChaseResults chaseResults = Utils.GetChaseResults(projectile.Center, num337 * (float)num342, nPC.Center, nPC.velocity);
            if (chaseResults.InterceptionHappens && chaseResults.InterceptionTime <= 180f)
            {
                Vector2 vector29 = chaseResults.ChaserVelocity / num342;
                num338 = vector29.X;
                num339 = vector29.Y;
            }
            Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X - 4f, projectile.Center.Y, num338, num339, 227, Player.crystalLeafDamage, Player.crystalLeafKB, projectile.owner);
            Main.player[projectile.owner].crystalLeafCooldown = 40;
        }
    }
    public static void AI_043(Projectile projectile)
    {
        projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 3.14f;
        if (projectile.soundDelay == 0 && projectile.type == 227)
        {
            projectile.soundDelay = -1;
            //SoundEngine.PlaySound(6, (int)projectile.position.X, (int)projectile.position.Y);
        }
        float num345 = 1f - (float)projectile.timeLeft / 180f;
        float num346 = ((num345 * -6f * 0.85f + 0.33f) % 1f + 1f) % 1f;
        Color value3 = Main.hslToRgb(num346, 1f, 0.5f);
        value3 = Color.Lerp(value3, Color.Red, Utils.Remap(num346, 0.33f, 0.7f, 0f, 1f));
        value3 = Color.Lerp(value3, Color.Lerp(Color.LimeGreen, Color.Gold, 0.3f), (float)(int)value3.R / 255f * 1f);
        if (projectile.frameCounter++ >= 1)
        {
            projectile.frameCounter = 0;
            ParticleOrchestrator.RequestParticleSpawn(clientOnly: true, ParticleOrchestraType.ChlorophyteLeafCrystalShot, new ParticleOrchestraSettings
            {
                PositionInWorld = projectile.Center,
                MovementVector = projectile.velocity,
                UniqueInfoPiece = (byte)(Main.rgbToHsl(value3).X * 255f)
            });
        }
        Lighting.AddLight(projectile.Center, new Vector3(0.05f, 0.2f, 0.1f) * 1.5f);
    }
    public static void AI_044(Projectile projectile)
    {
        int num347 = 6;
        if (projectile.type == 228)
        {
            projectile.velocity *= 0.96f;
            projectile.alpha += 2;
            if (projectile.alpha > 200)
            {
                projectile.Kill();
            }
        }
        else if (projectile.type == 732)
        {
            num347 = 3;
            projectile.alpha += 20;
            if (projectile.alpha > 255)
            {
                projectile.Kill();
            }
            projectile.rotation = projectile.velocity.ToRotation();
            Lighting.AddLight(projectile.Center, 0.3f, 0.6f, 0.8f);
        }
        else if (projectile.type == 229)
        {
            //if (projectile.ai[0] == 0f)
            //{
            //    SoundEngine.PlaySound(SoundID.Item8, projectile.position);
            //}
            projectile.ai[0] += 1f;
            if (projectile.ai[0] > 20f)
            {
                projectile.velocity.Y += 0.3f;
                projectile.velocity.X *= 0.98f;
            }
        }
        if (++projectile.frameCounter >= num347)
        {
            projectile.frameCounter = 0;
            if (++projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }
    }
    public static void AI_045(Projectile projectile)
    {
        if (projectile.type == 237 || projectile.type == 243)
        {
            float num348 = projectile.ai[0];
            float num349 = projectile.ai[1];
            if (num348 != 0f && num349 != 0f)
            {
                bool flag16 = false;
                bool flag17 = false;
                if (projectile.velocity.X == 0f || (projectile.velocity.X < 0f && projectile.Center.X < num348) || (projectile.velocity.X > 0f && projectile.Center.X > num348))
                {
                    projectile.velocity.X = 0f;
                    flag16 = true;
                }
                if (projectile.velocity.Y == 0f || (projectile.velocity.Y < 0f && projectile.Center.Y < num349) || (projectile.velocity.Y > 0f && projectile.Center.Y > num349))
                {
                    projectile.velocity.Y = 0f;
                    flag17 = true;
                }
                if (projectile.owner == Main.myPlayer && flag16 && flag17)
                {
                    projectile.Kill();
                }
            }
            projectile.rotation += projectile.velocity.X * 0.02f;
            projectile.frameCounter++;
            if (projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 3)
                {
                    projectile.frame = 0;
                }
            }
        }
        else if (projectile.type == 238 || projectile.type == 244)
        {
            bool flag18 = true;
            int num350 = (int)projectile.Center.X;
            int num351 = (int)(projectile.position.Y + (float)projectile.height);
            if (Collision.SolidTiles(new Vector2(num350, num351), 2, 20))
            {
                flag18 = false;
            }
            projectile.frameCounter++;
            if (projectile.frameCounter > 8)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if ((!flag18 && projectile.frame > 2) || projectile.frame > 5)
                {
                    projectile.frame = 0;
                }
            }
            projectile.ai[1] += 1f;
            if (projectile.type == 244 && projectile.ai[1] >= 18000f)
            {
                projectile.alpha += 5;
                if (projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                    projectile.Kill();
                }
            }
            else if (projectile.type == 238 && projectile.ai[1] >= 18000f)
            {
                projectile.alpha += 5;
                if (projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                    projectile.Kill();
                }
            }
            else if (flag18)
            {
                projectile.ai[0] += 1f;
                if (projectile.type == 244)
                {
                    if (projectile.ai[0] > 10f)
                    {
                        projectile.ai[0] = 0f;
                        if (projectile.owner == Main.myPlayer)
                        {
                            num350 += Main.rand.Next(-14, 15);
                            Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), num350, num351, 0f, 5f, 245, projectile.damage, 0f, projectile.owner);
                        }
                    }
                }
                else if (projectile.ai[0] > 8f)
                {
                    projectile.ai[0] = 0f;
                    if (projectile.owner == Main.myPlayer)
                    {
                        num350 += Main.rand.Next(-14, 15);
                        Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), num350, num351, 0f, 5f, 239, projectile.damage, 0f, projectile.owner);
                    }
                }
            }
            projectile.localAI[0] += 1f;
            if (!(projectile.localAI[0] >= 10f))
            {
                return;
            }
            projectile.localAI[0] = 0f;
            int num352 = 0;
            int num353 = 0;
            float num354 = 0f;
            int num355 = projectile.type;
            for (int num356 = 0; num356 < 1000; num356++)
            {
                if (Main.projectile[num356].active && Main.projectile[num356].owner == projectile.owner && Main.projectile[num356].type == num355 && Main.projectile[num356].ai[1] < 18000f)
                {
                    num352++;
                    if (Main.projectile[num356].ai[1] > num354)
                    {
                        num353 = num356;
                        num354 = Main.projectile[num356].ai[1];
                    }
                }
            }
            if (projectile.type == 244)
            {
                if (num352 > 1)
                {
                    Main.projectile[num353].netUpdate = true;
                    Main.projectile[num353].ai[1] = 18000f;
                }
            }
            else if (num352 > 2)
            {
                Main.projectile[num353].netUpdate = true;
                Main.projectile[num353].ai[1] = 18000f;
            }
        }
        else if (projectile.type == 239 || projectile.type == 245 || projectile.type == 264)
        {
            int x4 = (int)(projectile.Center.X / 16f);
            int y4 = (int)((projectile.position.Y + (float)projectile.height) / 16f);
            if (WorldGen.InWorld(x4, y4) && Main.tile[x4, y4] != null && Main.tile[x4, y4].liquid == byte.MaxValue && Main.tile[x4, y4].shimmer() && projectile.velocity.Y > 0f)
            {
                projectile.velocity.Y *= -1f;
                projectile.netUpdate = true;
            }
            if (projectile.type == 239)
            {
                projectile.alpha = 50;
            }
            else if (projectile.type == 245)
            {
                projectile.alpha = 100;
            }
            else if (projectile.type == 264)
            {
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            }
        }
    }
    public static void AI_046(Projectile projectile)
    {
        int x5 = (int)(projectile.Center.X / 16f);
        int y5 = (int)(projectile.Center.Y / 16f);
        if (WorldGen.InWorld(x5, y5) && Main.tile[x5, y5] != null && Main.tile[x5, y5].liquid > 0 && Main.tile[x5, y5].shimmer())
        {
            projectile.Kill();
        }
        int num357 = 2400;
        if (projectile.type == 250)
        {
            Point point2 = projectile.Center.ToTileCoordinates();
            if (!WorldGen.InWorld(point2.X, point2.Y, 2) || Main.tile[point2.X, point2.Y] == null)
            {
                projectile.Kill();
                return;
            }
            if (projectile.owner == Main.myPlayer)
            {
                projectile.localAI[0] += 1f;
                if (projectile.localAI[0] > 4f)
                {
                    projectile.localAI[0] = 3f;
                    Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X, projectile.Center.Y, projectile.velocity.X * 0.001f, projectile.velocity.Y * 0.001f, 251, projectile.damage, projectile.knockBack, projectile.owner);
                }
                if (projectile.timeLeft > num357)
                {
                    projectile.timeLeft = num357;
                }
            }
            float num358 = 1f;
            if (projectile.velocity.Y < 0f)
            {
                num358 -= projectile.velocity.Y / 3f;
            }
            projectile.ai[0] += num358;
            if (projectile.ai[0] > 30f)
            {
                projectile.velocity.Y += 0.5f;
                if (projectile.velocity.Y > 0f)
                {
                    projectile.velocity.X *= 0.95f;
                }
                else
                {
                    projectile.velocity.X *= 1.05f;
                }
            }
            float x6 = projectile.velocity.X;
            float y6 = projectile.velocity.Y;
            float num359 = (float)Math.Sqrt(x6 * x6 + y6 * y6);
            num359 = 15.95f * projectile.scale / num359;
            x6 *= num359;
            y6 *= num359;
            projectile.velocity.X = x6;
            projectile.velocity.Y = y6;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) - 1.57f;
            return;
        }
        if (projectile.localAI[0] == 0f)
        {
            if (projectile.velocity.X > 0f)
            {
                projectile.spriteDirection = -1;
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) - 1.57f;
            }
            else
            {
                projectile.spriteDirection = 1;
                projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) - 1.57f;
            }
            projectile.localAI[0] = 1f;
            projectile.timeLeft = num357;
        }
        projectile.velocity.X *= 0.98f;
        projectile.velocity.Y *= 0.98f;
        if (projectile.rotation == 0f)
        {
            projectile.alpha = 255;
        }
        else if (projectile.timeLeft < 10)
        {
            projectile.alpha = 255 - (int)(255f * (float)projectile.timeLeft / 10f);
        }
        else if (projectile.timeLeft > num357 - 10)
        {
            int num360 = num357 - projectile.timeLeft;
            projectile.alpha = 255 - (int)(255f * (float)num360 / 10f);
        }
        else
        {
            projectile.alpha = 0;
        }
    }
    public static void AI_048(Projectile projectile)
    {
        if (projectile.type == 255)
        {
            return;
        }
        if (projectile.type == 433)
        {
            return;
        }
        if (projectile.type == 290)
        {
            //if (projectile.localAI[0] == 0f)
            //{
            //    SoundEngine.PlaySound(SoundID.Item8, projectile.position);
            //}
            projectile.localAI[0] += 1f;
            return;
        }
        if (projectile.type == 294)
        {
            projectile.localAI[0] += 1f;
            return;
        }
        projectile.localAI[0] += 1f;
    }
    public static void AI_049(Projectile projectile)
    {
        if (projectile.ai[0] == -2f)
        {
            projectile.hostile = true;
            projectile.Kill();
            return;
        }
        if (projectile.ai[0] == -3f)
        {
            projectile.Kill();
            return;
        }
        if (projectile.soundDelay == 0)
        {
            projectile.soundDelay = 3000;
            //SoundEngine.PlaySound(SoundID.Item14, projectile.position);
        }
        if (projectile.ai[0] >= 0f)
        {
            if (projectile.velocity.X > 0f)
            {
                projectile.direction = 1;
            }
            else if (projectile.velocity.X < 0f)
            {
                projectile.direction = -1;
            }
            projectile.spriteDirection = projectile.direction;
            projectile.ai[0] += 1f;
            projectile.rotation += projectile.velocity.X * 0.05f + (float)projectile.direction * 0.05f;
            if (projectile.ai[0] >= 18f)
            {
                projectile.velocity.Y += 0.28f;
                projectile.velocity.X *= 0.99f;
            }
            if ((double)projectile.velocity.Y > 15.9)
            {
                projectile.velocity.Y = 15.9f;
            }
            if (!(projectile.ai[0] > 2f))
            {
                return;
            }
            projectile.alpha = 0;
        }
        else if (projectile.ai[0] == -1f)
        {
            projectile.rotation = 0f;
            projectile.velocity.X *= 0.95f;
            projectile.velocity.Y += 0.2f;
        }
    }
    public static void AI_050(Projectile projectile)
    {
        if (projectile.type == 291)
        {
            if (projectile.localAI[0] == 0f)
            {
                //SoundEngine.PlaySound(SoundID.Item20, projectile.position);
                projectile.localAI[0] += 1f;
            }
            bool flag19 = false;
            bool flag20 = false;
            if (projectile.velocity.X < 0f && projectile.position.X < projectile.ai[0])
            {
                flag19 = true;
            }
            if (projectile.velocity.X > 0f && projectile.position.X > projectile.ai[0])
            {
                flag19 = true;
            }
            if (projectile.velocity.Y < 0f && projectile.position.Y < projectile.ai[1])
            {
                flag20 = true;
            }
            if (projectile.velocity.Y > 0f && projectile.position.Y > projectile.ai[1])
            {
                flag20 = true;
            }
            if (flag19 && flag20)
            {
                projectile.Kill();
            }
            return;
        }
        if (projectile.type == 295)
        {
            return;
        }
        if (projectile.localAI[0] == 0f)
        {
            //SoundEngine.PlaySound(SoundID.Item74, projectile.position);
            projectile.localAI[0] += 1f;
        }
        projectile.ai[0] += 1f;
        if (projectile.type == 296)
        {
            projectile.ai[0] += 3f;
        }
        float num381 = 25f;
        if (projectile.ai[0] > 540f)
        {
            num381 -= (projectile.ai[0] - 180f) / 2f;
        }
        if (num381 <= 0f)
        {
            num381 = 0f;
            projectile.Kill();
        }
        if (projectile.type == 296)
        {
            num381 *= 0.7f;
        }
    }
    public static void AI_051(Projectile projectile)
    {
        if (projectile.type == 297)
        {
            projectile.localAI[0] += 1f;
        }
        else
        {
            if (projectile.localAI[0] == 0f)
            {
                //SoundEngine.PlaySound(SoundID.Item8, projectile.position);
                projectile.localAI[0] += 1f;
            }
        }
        float num392 = projectile.Center.X;
        float num393 = projectile.Center.Y;
        float num394 = 400f;
        bool flag21 = false;
        int num395 = 0;
        if (projectile.type == 297)
        {
            for (int num396 = 0; num396 < 200; num396++)
            {
                if (Main.npc[num396].CanBeChasedBy(projectile) && projectile.Distance(Main.npc[num396].Center) < num394 && Collision.CanHit(projectile.Center, 1, 1, Main.npc[num396].Center, 1, 1))
                {
                    float num397 = Main.npc[num396].position.X + (float)(Main.npc[num396].width / 2);
                    float num398 = Main.npc[num396].position.Y + (float)(Main.npc[num396].height / 2);
                    float num399 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num397) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num398);
                    if (num399 < num394)
                    {
                        num394 = num399;
                        num392 = num397;
                        num393 = num398;
                        flag21 = true;
                        num395 = num396;
                    }
                }
            }
        }
        else
        {
            num394 = 200f;
            for (int num400 = 0; num400 < 255; num400++)
            {
                if (Main.player[num400].active && !Main.player[num400].dead)
                {
                    float num401 = Main.player[num400].position.X + (float)(Main.player[num400].width / 2);
                    float num402 = Main.player[num400].position.Y + (float)(Main.player[num400].height / 2);
                    float num403 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num401) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num402);
                    if (num403 < num394)
                    {
                        num394 = num403;
                        num392 = num401;
                        num393 = num402;
                        flag21 = true;
                        num395 = num400;
                    }
                }
            }
        }
        if (flag21)
        {
            float num404 = 3f;
            if (projectile.type == 297)
            {
                num404 = 6f;
            }
            Vector2 vector35 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
            float num405 = num392 - vector35.X;
            float num406 = num393 - vector35.Y;
            float num407 = (float)Math.Sqrt(num405 * num405 + num406 * num406);
            float num408 = num407;
            num407 = num404 / num407;
            num405 *= num407;
            num406 *= num407;
            if (projectile.type == 297)
            {
                projectile.velocity.X = (projectile.velocity.X * 20f + num405) / 21f;
                projectile.velocity.Y = (projectile.velocity.Y * 20f + num406) / 21f;
            }
            else
            {
                projectile.velocity.X = (projectile.velocity.X * 100f + num405) / 101f;
                projectile.velocity.Y = (projectile.velocity.Y * 100f + num406) / 101f;
            }
        }
    }
    public static void AI_052(Projectile projectile)
    {
        int num409 = (int)projectile.ai[0];
        float num410 = 4f;
        Vector2 vector36 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
        float num411 = Main.player[num409].Center.X - vector36.X;
        float num412 = Main.player[num409].Center.Y - vector36.Y;
        float num413 = (float)Math.Sqrt(num411 * num411 + num412 * num412);
        float num414 = num413;
        if (num413 < 50f && projectile.position.X < Main.player[num409].position.X + (float)Main.player[num409].width && projectile.position.X + (float)projectile.width > Main.player[num409].position.X && projectile.position.Y < Main.player[num409].position.Y + (float)Main.player[num409].height && projectile.position.Y + (float)projectile.height > Main.player[num409].position.Y)
        {
            if (projectile.owner == Main.myPlayer && !Main.player[Main.myPlayer].moonLeech)
            {
                int num415 = (int)projectile.ai[1];
                Main.player[num409].HealEffect(num415, broadcast: false);
                Player player3 = Main.player[num409];
                player3.statLife += num415;
                if (Main.player[num409].statLife > Main.player[num409].statLifeMax2)
                {
                    Main.player[num409].statLife = Main.player[num409].statLifeMax2;
                }
                NetMessage.SendData(66, -1, -1, null, num409, num415);
            }
            projectile.Kill();
        }
        num413 = num410 / num413;
        num411 *= num413;
        num412 *= num413;
        projectile.velocity.X = (projectile.velocity.X * 15f + num411) / 16f;
        projectile.velocity.Y = (projectile.velocity.Y * 15f + num412) / 16f;
    }
    public static void AI_053(Projectile projectile)
    {
        if (projectile.localAI[0] == 0f)
        {
            projectile.localAI[1] = 1f;
            projectile.localAI[0] = 1f;
            projectile.ai[0] = 120f;
            if (projectile.type == 377)
            {
                projectile.frame = 4;
            }
            if (projectile.type == 966)
            {
                projectile.ai[1] = -1f;
                projectile.frame = 0;
            }
        }
        projectile.velocity.X = 0f;
        projectile.velocity.Y += 0.2f;
        if (projectile.velocity.Y > 16f)
        {
            projectile.velocity.Y = 16f;
        }
        bool flag22 = false;
        float num433 = projectile.Center.X;
        float num434 = projectile.Center.Y;
        float num435 = 1000f;
        int num436 = -1;
        NPC ownerMinionAttackTargetNPC = projectile.OwnerMinionAttackTargetNPC;
        if (ownerMinionAttackTargetNPC != null && ownerMinionAttackTargetNPC.CanBeChasedBy(projectile))
        {
            float num437 = ownerMinionAttackTargetNPC.position.X + (float)(ownerMinionAttackTargetNPC.width / 2);
            float num438 = ownerMinionAttackTargetNPC.position.Y + (float)(ownerMinionAttackTargetNPC.height / 2);
            float num439 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num437) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num438);
            if (num439 < num435 && Collision.CanHit(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC.position, ownerMinionAttackTargetNPC.width, ownerMinionAttackTargetNPC.height))
            {
                num435 = num439;
                num433 = num437;
                num434 = num438;
                flag22 = true;
                num436 = ownerMinionAttackTargetNPC.whoAmI;
            }
        }
        if (!flag22)
        {
            for (int num440 = 0; num440 < 200; num440++)
            {
                if (Main.npc[num440].CanBeChasedBy(projectile))
                {
                    float num441 = Main.npc[num440].position.X + (float)(Main.npc[num440].width / 2);
                    float num442 = Main.npc[num440].position.Y + (float)(Main.npc[num440].height / 2);
                    float num443 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num441) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num442);
                    if (num443 < num435 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num440].position, Main.npc[num440].width, Main.npc[num440].height))
                    {
                        num435 = num443;
                        num433 = num441;
                        num434 = num442;
                        flag22 = true;
                        num436 = Main.npc[num440].whoAmI;
                    }
                }
            }
        }
        if (flag22)
        {
            if (projectile.type == 966 && projectile.ai[1] != (float)num436)
            {
                projectile.ai[1] = num436;
                projectile.netUpdate = true;
            }
            float num444 = num433;
            float num445 = num434;
            num433 -= projectile.Center.X;
            num434 -= projectile.Center.Y;
            int num446 = 0;
            if (projectile.type != 966)
            {
                if (projectile.frameCounter > 0)
                {
                    projectile.frameCounter--;
                }
                if (projectile.frameCounter <= 0)
                {
                    int num447 = projectile.spriteDirection;
                    if (num433 < 0f)
                    {
                        projectile.spriteDirection = -1;
                    }
                    else
                    {
                        projectile.spriteDirection = 1;
                    }
                    num446 = ((!(num434 > 0f)) ? ((Math.Abs(num434) > Math.Abs(num433) * 3f) ? 4 : ((Math.Abs(num434) > Math.Abs(num433) * 2f) ? 3 : ((!(Math.Abs(num433) > Math.Abs(num434) * 3f)) ? ((Math.Abs(num433) > Math.Abs(num434) * 2f) ? 1 : 2) : 0))) : 0);
                    int num448 = projectile.frame;
                    if (projectile.type == 308)
                    {
                        projectile.frame = num446 * 2;
                    }
                    else if (projectile.type == 377)
                    {
                        projectile.frame = num446;
                    }
                    if (projectile.ai[0] > 40f && projectile.localAI[1] == 0f && projectile.type == 308)
                    {
                        projectile.frame++;
                    }
                    if (num448 != projectile.frame || num447 != projectile.spriteDirection)
                    {
                        projectile.frameCounter = 8;
                        if (projectile.ai[0] <= 0f)
                        {
                            projectile.frameCounter = 4;
                        }
                    }
                }
            }
            if (projectile.ai[0] <= 0f)
            {
                float num449 = 60f;
                if (projectile.type == 966)
                {
                    num449 = 90f;
                }
                projectile.localAI[1] = 0f;
                projectile.ai[0] = num449;
                projectile.netUpdate = true;
                if (Main.myPlayer == projectile.owner)
                {
                    float num450 = 6f;
                    int num451 = 309;
                    if (projectile.type == 308)
                    {
                        num451 = 309;
                        num450 = 9f;
                    }
                    if (projectile.type == 377)
                    {
                        num451 = 378;
                        num450 = 9f;
                    }
                    if (projectile.type == 966)
                    {
                        num451 = 967;
                        num450 = 12.5f;
                    }
                    Vector2 vector37 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                    if (projectile.type == 966)
                    {
                        vector37.Y -= 16f;
                    }
                    else
                    {
                        switch (num446)
                        {
                            case 0:
                                vector37.Y += 12f;
                                vector37.X += 24 * projectile.spriteDirection;
                                break;
                            case 1:
                                vector37.Y += 0f;
                                vector37.X += 24 * projectile.spriteDirection;
                                break;
                            case 2:
                                vector37.Y -= 2f;
                                vector37.X += 24 * projectile.spriteDirection;
                                break;
                            case 3:
                                vector37.Y -= 6f;
                                vector37.X += 14 * projectile.spriteDirection;
                                break;
                            case 4:
                                vector37.Y -= 14f;
                                vector37.X += 2 * projectile.spriteDirection;
                                break;
                        }
                    }
                    if (projectile.type != 966 && projectile.spriteDirection < 0)
                    {
                        vector37.X += 10f;
                    }
                    float num452 = num444 - vector37.X;
                    float num453 = num445 - vector37.Y;
                    float num454 = (float)Math.Sqrt(num452 * num452 + num453 * num453);
                    float num455 = num454;
                    num454 = num450 / num454;
                    num452 *= num454;
                    num453 *= num454;
                    int num456 = projectile.damage;
                    int num457 = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), vector37.X, vector37.Y, num452, num453, num451, num456, projectile.knockBack, Main.myPlayer);
                }
            }
        }
        else
        {
            if (projectile.type == 966 && projectile.ai[1] != -1f)
            {
                projectile.ai[1] = -1f;
                projectile.netUpdate = true;
            }
            if (projectile.type != 966 && projectile.ai[0] <= 60f && (projectile.frame == 1 || projectile.frame == 3 || projectile.frame == 5 || projectile.frame == 7 || projectile.frame == 9))
            {
                projectile.frame--;
            }
        }
        if (projectile.ai[0] > 0f)
        {
            projectile.ai[0] -= 1f;
        }
    }
    public static void AI_054(Projectile projectile)
    {
        if (projectile.type == 317)
        {
            if (Main.player[Main.myPlayer].dead)
            {
                Main.player[Main.myPlayer].raven = false;
            }
            if (Main.player[Main.myPlayer].raven)
            {
                projectile.timeLeft = 2;
            }
        }
        for (int num458 = 0; num458 < 1000; num458++)
        {
            if (num458 != projectile.whoAmI && Main.projectile[num458].active && Main.projectile[num458].owner == projectile.owner && Main.projectile[num458].type == projectile.type && Math.Abs(projectile.position.X - Main.projectile[num458].position.X) + Math.Abs(projectile.position.Y - Main.projectile[num458].position.Y) < (float)projectile.width)
            {
                if (projectile.position.X < Main.projectile[num458].position.X)
                {
                    projectile.velocity.X -= 0.05f;
                }
                else
                {
                    projectile.velocity.X += 0.05f;
                }
                if (projectile.position.Y < Main.projectile[num458].position.Y)
                {
                    projectile.velocity.Y -= 0.05f;
                }
                else
                {
                    projectile.velocity.Y += 0.05f;
                }
            }
        }
        float num459 = projectile.position.X;
        float num460 = projectile.position.Y;
        float num461 = 900f;
        bool flag23 = false;
        int num462 = 500;
        if (projectile.ai[1] != 0f || projectile.friendly)
        {
            num462 = 1400;
        }
        if (Math.Abs(projectile.Center.X - Main.player[projectile.owner].Center.X) + Math.Abs(projectile.Center.Y - Main.player[projectile.owner].Center.Y) > (float)num462)
        {
            projectile.ai[0] = 1f;
        }
        if (projectile.ai[0] == 0f)
        {
            projectile.tileCollide = true;
            NPC ownerMinionAttackTargetNPC2 = projectile.OwnerMinionAttackTargetNPC;
            if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(projectile))
            {
                float num463 = ownerMinionAttackTargetNPC2.position.X + (float)(ownerMinionAttackTargetNPC2.width / 2);
                float num464 = ownerMinionAttackTargetNPC2.position.Y + (float)(ownerMinionAttackTargetNPC2.height / 2);
                float num465 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num463) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num464);
                if (num465 < num461 && Collision.CanHit(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
                {
                    num461 = num465;
                    num459 = num463;
                    num460 = num464;
                    flag23 = true;
                }
            }
            if (!flag23)
            {
                for (int num466 = 0; num466 < 200; num466++)
                {
                    if (Main.npc[num466].CanBeChasedBy(projectile))
                    {
                        float num467 = Main.npc[num466].position.X + (float)(Main.npc[num466].width / 2);
                        float num468 = Main.npc[num466].position.Y + (float)(Main.npc[num466].height / 2);
                        float num469 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num467) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num468);
                        if (num469 < num461 && Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[num466].position, Main.npc[num466].width, Main.npc[num466].height))
                        {
                            num461 = num469;
                            num459 = num467;
                            num460 = num468;
                            flag23 = true;
                        }
                    }
                }
            }
        }
        else
        {
            projectile.tileCollide = false;
        }
        if (!flag23)
        {
            projectile.friendly = true;
            float num470 = 8f;
            if (projectile.ai[0] == 1f)
            {
                num470 = 12f;
            }
            Vector2 vector38 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
            float num471 = Main.player[projectile.owner].Center.X - vector38.X;
            float num472 = Main.player[projectile.owner].Center.Y - vector38.Y - 60f;
            float num473 = (float)Math.Sqrt(num471 * num471 + num472 * num472);
            float num474 = num473;
            if (num473 < 100f && projectile.ai[0] == 1f && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.ai[0] = 0f;
            }
            if (num473 > 2000f)
            {
                projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
                projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.width / 2);
            }
            if (projectile.type == 317 && num473 > 100f)
            {
                num470 = 12f;
                if (projectile.ai[0] == 1f)
                {
                    num470 = 15f;
                }
            }
            if (num473 > 70f)
            {
                num473 = num470 / num473;
                num471 *= num473;
                num472 *= num473;
                projectile.velocity.X = (projectile.velocity.X * 20f + num471) / 21f;
                projectile.velocity.Y = (projectile.velocity.Y * 20f + num472) / 21f;
            }
            else
            {
                if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
                {
                    projectile.velocity.X = -0.15f;
                    projectile.velocity.Y = -0.05f;
                }
                projectile.velocity *= 1.01f;
            }
            projectile.friendly = false;
            projectile.rotation = projectile.velocity.X * 0.05f;
            projectile.frameCounter++;
            if (projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
            }
            if (projectile.frame > 3)
            {
                projectile.frame = 0;
            }
            if ((double)Math.Abs(projectile.velocity.X) > 0.2)
            {
                projectile.spriteDirection = -projectile.direction;
            }
            return;
        }
        if (projectile.ai[1] == -1f)
        {
            projectile.ai[1] = 17f;
        }
        if (projectile.ai[1] > 0f)
        {
            projectile.ai[1] -= 1f;
        }
        if (projectile.ai[1] == 0f)
        {
            projectile.friendly = true;
            float num475 = 16f;
            Vector2 vector39 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
            float num476 = num459 - vector39.X;
            float num477 = num460 - vector39.Y;
            float num478 = (float)Math.Sqrt(num476 * num476 + num477 * num477);
            float num479 = num478;
            if (num478 < 100f)
            {
                num475 = 10f;
            }
            num478 = num475 / num478;
            num476 *= num478;
            num477 *= num478;
            projectile.velocity.X = (projectile.velocity.X * 14f + num476) / 15f;
            projectile.velocity.Y = (projectile.velocity.Y * 14f + num477) / 15f;
        }
        else
        {
            projectile.friendly = false;
            if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) < 10f)
            {
                projectile.velocity *= 1.05f;
            }
        }
        projectile.rotation = projectile.velocity.X * 0.05f;
        projectile.frameCounter++;
        if (projectile.frameCounter >= 4)
        {
            projectile.frameCounter = 0;
            projectile.frame++;
        }
        if (projectile.frame < 4)
        {
            projectile.frame = 4;
        }
        if (projectile.frame > 7)
        {
            projectile.frame = 4;
        }
        if ((double)Math.Abs(projectile.velocity.X) > 0.2)
        {
            projectile.spriteDirection = -projectile.direction;
        }
    }
    public static void AI_055(Projectile projectile)
    {
        projectile.frameCounter++;
        if (projectile.frameCounter > 0)
        {
            projectile.frame++;
            projectile.frameCounter = 0;
            if (projectile.frame > 2)
            {
                projectile.frame = 0;
            }
        }
        if (projectile.velocity.X < 0f)
        {
            projectile.spriteDirection = -1;
            projectile.rotation = (float)Math.Atan2(0f - projectile.velocity.Y, 0f - projectile.velocity.X);
        }
        else
        {
            projectile.spriteDirection = 1;
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X);
        }
        if (projectile.ai[0] >= 0f && projectile.ai[0] < 200f)
        {
            int num480 = (int)projectile.ai[0];
            NPC nPC2 = Main.npc[num480];
            if (nPC2.CanBeChasedBy(projectile) && !NPCID.Sets.CountsAsCritter[nPC2.type])
            {
                float num481 = 8f;
                Vector2 center2 = projectile.Center;
                float num482 = nPC2.Center.X - center2.X;
                float num483 = nPC2.Center.Y - center2.Y;
                float num484 = (float)Math.Sqrt(num482 * num482 + num483 * num483);
                float num485 = num484;
                num484 = num481 / num484;
                num482 *= num484;
                num483 *= num484;
                projectile.velocity.X = (projectile.velocity.X * 14f + num482) / 15f;
                projectile.velocity.Y = (projectile.velocity.Y * 14f + num483) / 15f;
            }
            else
            {
                float num486 = 1000f;
                for (int num487 = 0; num487 < 200; num487++)
                {
                    NPC nPC3 = Main.npc[num487];
                    if (nPC3.CanBeChasedBy(projectile) && !NPCID.Sets.CountsAsCritter[nPC3.type])
                    {
                        float x7 = nPC3.Center.X;
                        float y7 = nPC3.Center.Y;
                        float num488 = Math.Abs(projectile.Center.X - x7) + Math.Abs(projectile.Center.Y - y7);
                        if (num488 < num486 && Collision.CanHit(projectile.position, projectile.width, projectile.height, nPC3.position, nPC3.width, nPC3.height))
                        {
                            num486 = num488;
                            projectile.ai[0] = num487;
                        }
                    }
                }
            }
        }
        else
        {
            projectile.Kill();
        }
    }
    public static void AI_056(Projectile projectile)
    {
        if (projectile.localAI[0] == 0f)
        {
            projectile.localAI[0] = 1f;
            projectile.rotation = projectile.ai[0];
            projectile.spriteDirection = -(int)projectile.ai[1];
        }
        if (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y) < 16f)
        {
            projectile.velocity *= 1.05f;
        }
        if (projectile.velocity.X < 0f)
        {
            projectile.direction = -1;
        }
        else
        {
            projectile.direction = 1;
        }
        projectile.rotation += (Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) * 0.025f * (float)projectile.direction;
    }
    public static void AI_057(Projectile projectile)
    {
        projectile.ai[0] += 1f;
        if (projectile.ai[0] > 30f)
        {
            projectile.ai[0] = 30f;
            projectile.velocity.Y += 0.25f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }
            projectile.velocity.X *= 0.995f;
        }
        projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
        projectile.alpha -= 50;
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        if (projectile.owner == Main.myPlayer)
        {
            if (projectile.localAI[1] == 0f)
            {
                projectile.localAI[1] = Main.rand.Next(7);
            }
            projectile.localAI[0]++;
            int num491 = 8;
            if (projectile.localAI[1] > 0f)
            {
                num491 += (int)projectile.localAI[1];
            }
            if (projectile.localAI[0] >= (float)num491)
            {
                projectile.localAI[0] = 0f;
                projectile.localAI[1] = -1f;
                Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X, projectile.Center.Y, 0f, 0f, 344, (int)((float)projectile.damage * 0.7f), projectile.knockBack * 0.55f, projectile.owner, 0f, Main.rand.Next(3));
            }
        }
    }
    public static void AI_058(Projectile projectile)
    {
        projectile.alpha -= 50;
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        if (projectile.ai[0] == 0f)
        {
            projectile.frame = 0;
            projectile.ai[1] += 1f;
            if (projectile.ai[1] > 30f)
            {
                projectile.velocity.Y += 0.1f;
            }
            if (projectile.velocity.Y >= 0f)
            {
                projectile.ai[0] = 1f;
            }
        }
        if (projectile.ai[0] == 1f)
        {
            projectile.frame = 1;
            projectile.velocity.Y += 0.1f;
            if (projectile.velocity.Y > 3f)
            {
                projectile.velocity.Y = 3f;
            }
            projectile.velocity.X *= 0.99f;
        }
        projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
    }
    public static void AI_059(Projectile projectile)
    {
        projectile.ai[1] += 1f;
        if (projectile.ai[1] >= 60f)
        {
            projectile.friendly = true;
            int num492 = (int)projectile.ai[0];
            if (Main.myPlayer == projectile.owner && (num492 == -1 || !Main.npc[num492].CanBeChasedBy(projectile)))
            {
                num492 = -1;
                int[] array = new int[200];
                int num493 = 0;
                for (int num494 = 0; num494 < 200; num494++)
                {
                    if (Main.npc[num494].CanBeChasedBy(projectile))
                    {
                        float num495 = Math.Abs(Main.npc[num494].position.X + (float)(Main.npc[num494].width / 2) - projectile.position.X + (float)(projectile.width / 2)) + Math.Abs(Main.npc[num494].position.Y + (float)(Main.npc[num494].height / 2) - projectile.position.Y + (float)(projectile.height / 2));
                        if (num495 < 800f)
                        {
                            array[num493] = num494;
                            num493++;
                        }
                    }
                }
                if (num493 == 0)
                {
                    projectile.Kill();
                    return;
                }
                num492 = array[Main.rand.Next(num493)];
                projectile.ai[0] = num492;
                projectile.netUpdate = true;
            }
            if (num492 != -1)
            {
                float num496 = 4f;
                Vector2 vector40 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                float num497 = Main.npc[num492].Center.X - vector40.X;
                float num498 = Main.npc[num492].Center.Y - vector40.Y;
                float num499 = (float)Math.Sqrt(num497 * num497 + num498 * num498);
                float num500 = num499;
                num499 = num496 / num499;
                num497 *= num499;
                num498 *= num499;
                int num501 = 30;
                projectile.velocity.X = (projectile.velocity.X * (float)(num501 - 1) + num497) / (float)num501;
                projectile.velocity.Y = (projectile.velocity.Y * (float)(num501 - 1) + num498) / (float)num501;
            }
        }
    }
    public static void AI_060(Projectile projectile)
    {
        projectile.scale -= 0.015f;
        if (projectile.scale <= 0f)
        {
            projectile.velocity *= 5f;
            projectile.oldVelocity = projectile.velocity;
            projectile.Kill();
        }
        if (projectile.ai[0] > 3f)
        {
            int num506 = 103;
            if (projectile.type == 406)
            {
                num506 = 137;
            }
            if (projectile.owner == Main.myPlayer)
            {
                Rectangle rectangle4 = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
                for (int num507 = 0; num507 < 200; num507++)
                {
                    if (Main.npc[num507].active && !Main.npc[num507].dontTakeDamage && Main.npc[num507].lifeMax > 1)
                    {
                        Rectangle value4 = new Rectangle((int)Main.npc[num507].position.X, (int)Main.npc[num507].position.Y, Main.npc[num507].width, Main.npc[num507].height);
                        if (rectangle4.Intersects(value4))
                        {
                            Main.npc[num507].AddBuff(num506, 1500);
                            projectile.Kill();
                        }
                    }
                }
                for (int num508 = 0; num508 < 255; num508++)
                {
                    if (num508 != projectile.owner && Main.player[num508].active && !Main.player[num508].dead)
                    {
                        Rectangle value5 = new Rectangle((int)Main.player[num508].position.X, (int)Main.player[num508].position.Y, Main.player[num508].width, Main.player[num508].height);
                        if (rectangle4.Intersects(value5))
                        {
                            Main.player[num508].AddBuff(num506, 1500, quiet: false);
                            projectile.Kill();
                        }
                    }
                }
            }
            projectile.ai[0] += projectile.ai[1];
            if (projectile.ai[0] > 30f)
            {
                projectile.velocity.Y += 0.1f;
            }
            if (projectile.type != 406)
            {
                return;
            }
        }
        else
        {
            projectile.ai[0] += 1f;
        }
    }
    public static void AI_063(Projectile projectile)
    {
        if (!Main.player[projectile.owner].active)
        {
            projectile.active = false;
            return;
        }
        Vector2 center3 = projectile.position;
        bool flag24 = false;
        float num523 = 2000f;
        for (int num524 = 0; num524 < 200; num524++)
        {
            NPC nPC4 = Main.npc[num524];
            if (nPC4.CanBeChasedBy(projectile))
            {
                float num525 = Vector2.Distance(nPC4.Center, projectile.Center);
                if (!(num525 >= num523) && Collision.CanHit(projectile.position, projectile.width, projectile.height, nPC4.position, nPC4.width, nPC4.height))
                {
                    num523 = num525;
                    center3 = nPC4.Center;
                    flag24 = true;
                }
            }
        }
        if (!flag24)
        {
            projectile.velocity.X *= 0.95f;
        }
        else
        {
            float num526 = 5f;
            float num527 = 0.08f;
            if (projectile.velocity.Y == 0f)
            {
                bool flag25 = false;
                if (projectile.Center.Y - 50f > center3.Y)
                {
                    flag25 = true;
                }
                if (flag25)
                {
                    projectile.velocity.Y = -6f;
                }
            }
            else
            {
                num526 = 8f;
                num527 = 0.12f;
            }
            projectile.velocity.X += (float)Math.Sign(center3.X - projectile.Center.X) * num527;
            if (projectile.velocity.X < 0f - num526)
            {
                projectile.velocity.X = 0f - num526;
            }
            if (projectile.velocity.X > num526)
            {
                projectile.velocity.X = num526;
            }
        }
        float num528 = 0f;
        Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref num528, ref projectile.gfxOffY);
        if (projectile.velocity.Y != 0f)
        {
            projectile.frame = 3;
        }
        else
        {
            if (Math.Abs(projectile.velocity.X) > 0.2f)
            {
                projectile.frameCounter++;
            }
            if (projectile.frameCounter >= 9)
            {
                projectile.frameCounter = 0;
            }
            if (projectile.frameCounter >= 6)
            {
                projectile.frame = 2;
            }
            else if (projectile.frameCounter >= 3)
            {
                projectile.frame = 1;
            }
            else
            {
                projectile.frame = 0;
            }
        }
        if (projectile.velocity.X != 0f)
        {
            projectile.direction = Math.Sign(projectile.velocity.X);
        }
        projectile.spriteDirection = -projectile.direction;
        projectile.velocity.Y += 0.2f;
        if (projectile.velocity.Y > 16f)
        {
            projectile.velocity.Y = 16f;
        }
    }
    public static void AI_064(Projectile projectile)
    {
        int num529 = 10;
        int num530 = 15;
        float num531 = 1f;
        int num532 = 150;
        int num533 = 42;
        if (projectile.type == 386)
        {
            num529 = 16;
            num530 = 16;
            num531 = 1.5f;
        }
        if (projectile.velocity.X != 0f)
        {
            projectile.direction = (projectile.spriteDirection = -Math.Sign(projectile.velocity.X));
        }
        projectile.frameCounter++;
        if (projectile.frameCounter > 2)
        {
            projectile.frame++;
            projectile.frameCounter = 0;
        }
        if (projectile.frame >= 6)
        {
            projectile.frame = 0;
        }
        if (projectile.localAI[0] == 0f && Main.myPlayer == projectile.owner)
        {
            projectile.localAI[0] = 1f;
            projectile.position.X += projectile.width / 2;
            projectile.position.Y += projectile.height / 2;
            projectile.scale = ((float)(num529 + num530) - projectile.ai[1]) * num531 / (float)(num530 + num529);
            projectile.width = (int)((float)num532 * projectile.scale);
            projectile.height = (int)((float)num533 * projectile.scale);
            projectile.position.X -= projectile.width / 2;
            projectile.position.Y -= projectile.height / 2;
            projectile.netUpdate = true;
        }
        if (projectile.ai[1] != -1f)
        {
            projectile.scale = ((float)(num529 + num530) - projectile.ai[1]) * num531 / (float)(num530 + num529);
            projectile.width = (int)((float)num532 * projectile.scale);
            projectile.height = (int)((float)num533 * projectile.scale);
        }
        if (!Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
        {
            projectile.alpha -= 30;
            if (projectile.alpha < 60)
            {
                projectile.alpha = 60;
            }
            if (projectile.type == 386 && projectile.alpha < 100)
            {
                projectile.alpha = 100;
            }
        }
        else
        {
            projectile.alpha += 30;
            if (projectile.alpha > 150)
            {
                projectile.alpha = 150;
            }
        }
        if (projectile.ai[0] > 0f)
        {
            projectile.ai[0]--;
        }
        if (projectile.ai[0] == 1f && projectile.ai[1] > 0f && projectile.owner == Main.myPlayer)
        {
            projectile.netUpdate = true;
            Vector2 center4 = projectile.Center;
            center4.Y -= (float)num533 * projectile.scale / 2f;
            float num534 = ((float)(num529 + num530) - projectile.ai[1] + 1f) * num531 / (float)(num530 + num529);
            center4.Y -= (float)num533 * num534 / 2f;
            center4.Y += 2f;
            Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), center4.X, center4.Y, projectile.velocity.X, projectile.velocity.Y, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, 10f, projectile.ai[1] - 1f);
            int num535 = 4;
            if (projectile.type == 386)
            {
                num535 = 2;
            }
            if ((int)projectile.ai[1] % num535 == 0 && projectile.ai[1] != 0f)
            {
                int num536 = 372;
                if (projectile.type == 386)
                {
                    num536 = 373;
                }
                int num537 = NPC.NewNPC(projectile.GetNPCSource_FromThis(), (int)center4.X, (int)center4.Y, num536);
                Main.npc[num537].velocity = projectile.velocity;
                Main.npc[num537].netUpdate = true;
                if (projectile.type == 386)
                {
                    Main.npc[num537].ai[2] = projectile.width;
                    Main.npc[num537].ai[3] = -1.5f;
                }
            }
        }
        if (projectile.ai[0] <= 0f)
        {
            float num538 = (float)Math.PI / 30f;
            float num539 = (float)projectile.width / 5f;
            if (projectile.type == 386)
            {
                num539 *= 2f;
            }
            float num540 = (float)(Math.Cos(num538 * (0f - projectile.ai[0])) - 0.5) * num539;
            projectile.position.X -= num540 * (float)(-projectile.direction);
            projectile.ai[0]--;
            num540 = (float)(Math.Cos(num538 * (0f - projectile.ai[0])) - 0.5) * num539;
            projectile.position.X += num540 * (float)(-projectile.direction);
        }
    }
    public static void AI_065(Projectile projectile)
    {
        if (projectile.ai[1] > 0f)
        {
            int num541 = (int)projectile.ai[1] - 1;
            if (num541 < 255)
            {
                projectile.localAI[0]++;
                if (projectile.localAI[0] > 10f)
                {
                    projectile.alpha -= 5;
                    if (projectile.alpha < 100)
                    {
                        projectile.alpha = 100;
                    }
                    projectile.rotation += projectile.velocity.X * 0.1f;
                    projectile.frame = (int)(projectile.localAI[0] / 3f) % 3;
                }
                Vector2 value6 = Main.player[num541].Center - projectile.Center;
                float num545 = 4f;
                if (projectile.ai[2] == 1f)
                {
                    num545 += 12f;
                }
                num545 += projectile.localAI[0] / 20f;
                projectile.velocity = Vector2.Normalize(value6) * num545;
                if (value6.Length() < 50f)
                {
                    projectile.Kill();
                }
            }
        }
        else
        {
            float num546 = (float)Math.PI / 15f;
            float num547 = 4f;
            float num548 = (float)(Math.Cos(num546 * projectile.ai[0]) - 0.5) * num547;
            projectile.velocity.Y -= num548;
            projectile.ai[0]++;
            num548 = (float)(Math.Cos(num546 * projectile.ai[0]) - 0.5) * num547;
            projectile.velocity.Y += num548;
            projectile.localAI[0]++;
            if (projectile.localAI[0] > 10f)
            {
                projectile.alpha -= 5;
                if (projectile.alpha < 100)
                {
                    projectile.alpha = 100;
                }
                projectile.rotation += projectile.velocity.X * 0.1f;
                projectile.frame = (int)(projectile.localAI[0] / 3f) % 3;
            }
        }
        if (projectile.wet)
        {
            projectile.position.Y -= 16f;
            projectile.Kill();
        }
    }
    public static void AI_066(Projectile projectile)
    {
        float num549 = 0f;
        float num550 = 0f;
        float num551 = 0f;
        float num552 = 0f;
        bool flag26 = projectile.type == 387 || projectile.type == 388;
        if (flag26)
        {
            num549 = 2000f;
            num550 = 800f;
            num551 = 1200f;
            num552 = 150f;
            if (Main.player[projectile.owner].dead)
            {
                Main.player[projectile.owner].twinsMinion = false;
            }
            if (Main.player[projectile.owner].twinsMinion)
            {
                projectile.timeLeft = 2;
            }
        }
        if (projectile.type == 533)
        {
            num549 = 2000f;
            num550 = 900f;
            num551 = 1500f;
            num552 = 450f;
            if (Main.player[projectile.owner].dead)
            {
                Main.player[projectile.owner].DeadlySphereMinion = false;
            }
            if (Main.player[projectile.owner].DeadlySphereMinion)
            {
                projectile.timeLeft = 2;
            }
            projectile.localAI[2] = Utils.Clamp(projectile.localAI[2] - 1f, 0f, 60f);
        }
        float num553 = 0.05f;
        for (int num554 = 0; num554 < 1000; num554++)
        {
            bool flag27 = (Main.projectile[num554].type == 387 || Main.projectile[num554].type == 388) && (projectile.type == 387 || projectile.type == 388);
            if (!flag27)
            {
                flag27 = projectile.type == 533 && Main.projectile[num554].type == 533;
            }
            if (num554 != projectile.whoAmI && Main.projectile[num554].active && Main.projectile[num554].owner == projectile.owner && flag27 && Math.Abs(projectile.position.X - Main.projectile[num554].position.X) + Math.Abs(projectile.position.Y - Main.projectile[num554].position.Y) < (float)projectile.width)
            {
                if (projectile.position.X < Main.projectile[num554].position.X)
                {
                    projectile.velocity.X -= num553;
                }
                else
                {
                    projectile.velocity.X += num553;
                }
                if (projectile.position.Y < Main.projectile[num554].position.Y)
                {
                    projectile.velocity.Y -= num553;
                }
                else
                {
                    projectile.velocity.Y += num553;
                }
            }
        }
        if (projectile.type == 533)
        {
            if ((int)projectile.ai[0] % 3 != 2)
            {
                Lighting.AddLight(projectile.Center, 0.8f, 0.3f, 0.1f);
            }
            else
            {
                Lighting.AddLight(projectile.Center, 0.3f, 0.5f, 0.7f);
            }
        }
        bool flag28 = false;
        if (projectile.ai[0] == 2f && projectile.type == 388)
        {
            projectile.ai[1]++;
            projectile.extraUpdates = 1;
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI;
            projectile.frameCounter++;
            if (projectile.frameCounter > 1)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame > 2)
            {
                projectile.frame = 0;
            }
            if (projectile.ai[1] > 40f)
            {
                projectile.ai[1] = 1f;
                projectile.ai[0] = 0f;
                projectile.extraUpdates = 0;
                projectile.numUpdates = 0;
                projectile.netUpdate = true;
            }
            else
            {
                flag28 = true;
            }
        }
        if (projectile.type == 533 && projectile.ai[0] >= 3f && projectile.ai[0] <= 5f)
        {
            int num555 = 2;
            flag28 = true;
            projectile.velocity *= 0.9f;
            projectile.ai[1]++;
            int num556 = (int)projectile.ai[1] / num555 + (int)(projectile.ai[0] - 3f) * 8;
            if (num556 < 4)
            {
                projectile.frame = 17 + num556;
            }
            else if (num556 < 5)
            {
                projectile.frame = 0;
            }
            else if (num556 < 8)
            {
                projectile.frame = 1 + num556 - 5;
            }
            else if (num556 < 11)
            {
                projectile.frame = 11 - num556;
            }
            else if (num556 < 12)
            {
                projectile.frame = 0;
            }
            else if (num556 < 16)
            {
                projectile.frame = num556 - 2;
            }
            else if (num556 < 20)
            {
                projectile.frame = 29 - num556;
            }
            else if (num556 < 21)
            {
                projectile.frame = 0;
            }
            else
            {
                projectile.frame = num556 - 4;
            }
            if (projectile.ai[1] > (float)(num555 * 8))
            {
                projectile.ai[0] -= 3f;
                projectile.ai[1] = 0f;
            }
        }
        if (projectile.type == 533 && projectile.ai[0] >= 6f && projectile.ai[0] <= 8f)
        {
            projectile.ai[1]++;
            projectile.MaxUpdates = 2;
            if (projectile.ai[0] == 7f)
            {
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI;
            }
            else
            {
                projectile.rotation += (float)Math.PI / 6f;
            }
            int num557 = 0;
            switch ((int)projectile.ai[0])
            {
                case 6:
                    projectile.frame = 5;
                    num557 = 40;
                    break;
                case 7:
                    projectile.frame = 13;
                    num557 = 30;
                    break;
                case 8:
                    projectile.frame = 17;
                    num557 = 30;
                    break;
            }
            if (projectile.ai[1] > (float)num557)
            {
                projectile.ai[1] = 1f;
                projectile.ai[0] -= 6f;
                projectile.localAI[0]++;
                projectile.extraUpdates = 0;
                projectile.numUpdates = 0;
                projectile.netUpdate = true;
            }
            else
            {
                flag28 = true;
            }
        }
        if (flag28)
        {
            return;
        }
        Vector2 center5 = projectile.position;
        Vector2 zero = Vector2.Zero;
        bool flag29 = false;
        if (projectile.ai[0] != 1f && flag26)
        {
            projectile.tileCollide = true;
        }
        if (projectile.type == 533 && projectile.ai[0] < 9f)
        {
            projectile.tileCollide = true;
        }
        if (projectile.tileCollide && WorldGen.SolidTile(Framing.GetTileSafely((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16)))
        {
            projectile.tileCollide = false;
        }
        NPC ownerMinionAttackTargetNPC3 = projectile.OwnerMinionAttackTargetNPC;
        if (ownerMinionAttackTargetNPC3 != null && ownerMinionAttackTargetNPC3.CanBeChasedBy(projectile))
        {
            float num561 = Vector2.Distance(ownerMinionAttackTargetNPC3.Center, projectile.Center);
            float num562 = num549 * 3f;
            if (num561 < num562 && !flag29 && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC3.position, ownerMinionAttackTargetNPC3.width, ownerMinionAttackTargetNPC3.height))
            {
                num549 = num561;
                center5 = ownerMinionAttackTargetNPC3.Center;
                flag29 = true;
            }
        }
        if (!flag29)
        {
            for (int num563 = 0; num563 < 200; num563++)
            {
                NPC nPC5 = Main.npc[num563];
                if (nPC5.CanBeChasedBy(projectile))
                {
                    float num564 = Vector2.Distance(nPC5.Center, projectile.Center);
                    if (!(num564 >= num549) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, nPC5.position, nPC5.width, nPC5.height))
                    {
                        num549 = num564;
                        center5 = nPC5.Center;
                        zero = nPC5.velocity;
                        flag29 = true;
                    }
                }
            }
        }
        float num565 = num550;
        if (flag29)
        {
            num565 = num551;
        }
        Player player4 = Main.player[projectile.owner];
        if (Vector2.Distance(player4.Center, projectile.Center) > num565)
        {
            if (flag26)
            {
                projectile.ai[0] = 1f;
            }
            if (projectile.type == 533 && projectile.ai[0] < 9f)
            {
                projectile.ai[0] += 3 * (3 - (int)(projectile.ai[0] / 3f));
            }
            projectile.tileCollide = false;
            projectile.netUpdate = true;
        }
        if (flag26 && flag29 && projectile.ai[0] == 0f)
        {
            Vector2 vector44 = center5 - projectile.Center;
            float num566 = vector44.Length();
            vector44.Normalize();
            if (num566 > 200f)
            {
                float num567 = 6f;
                if (projectile.type == 388)
                {
                    num567 = 14f;
                }
                vector44 *= num567;
                projectile.velocity = (projectile.velocity * 40f + vector44) / 41f;
            }
            else
            {
                float num568 = 4f;
                vector44 *= 0f - num568;
                projectile.velocity = (projectile.velocity * 40f + vector44) / 41f;
            }
        }
        else
        {
            bool flag30 = false;
            if (!flag30 && flag26)
            {
                flag30 = projectile.ai[0] == 1f;
            }
            if (!flag30 && projectile.type == 533)
            {
                flag30 = projectile.ai[0] >= 9f;
            }
            float num569 = 6f;
            float num570 = 40f;
            if (projectile.type == 533)
            {
                num569 = 12f;
            }
            if (flag30)
            {
                num569 = 15f;
            }
            Vector2 center6 = projectile.Center;
            Vector2 vector45 = player4.Center - center6 + new Vector2(0f, -60f);
            float num571 = vector45.Length();
            float num572 = num571;
            if (num571 > 200f && num569 < 8f)
            {
                num569 = 8f;
            }
            if (num569 < Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y))
            {
                num570 = 30f;
                num569 = Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y);
                if (num571 > 200f)
                {
                    num570 = 20f;
                    num569 += 4f;
                }
                else if (num571 > 100f)
                {
                    num569 += 3f;
                }
            }
            if (flag30 && num571 > 300f)
            {
                num569 += 6f;
                num570 -= 10f;
            }
            if (num571 < num552 && flag30 && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                if (projectile.type == 387 || projectile.type == 388)
                {
                    projectile.ai[0] = 0f;
                }
                if (projectile.type == 533)
                {
                    projectile.ai[0] -= 9f;
                }
                projectile.netUpdate = true;
            }
            if (num571 > 2000f)
            {
                projectile.position.X = Main.player[projectile.owner].Center.X - (float)(projectile.width / 2);
                projectile.position.Y = Main.player[projectile.owner].Center.Y - (float)(projectile.height / 2);
                projectile.netUpdate = true;
            }
            if (num571 > 70f)
            {
                Vector2 vector46 = vector45;
                vector45.Normalize();
                vector45 *= num569;
                projectile.velocity = (projectile.velocity * num570 + vector45) / (num570 + 1f);
            }
            else if (projectile.velocity.X == 0f && projectile.velocity.Y == 0f)
            {
                projectile.velocity.X = -0.15f;
                projectile.velocity.Y = -0.05f;
            }
            if (projectile.velocity.Length() > num569)
            {
                projectile.velocity *= 0.95f;
            }
        }
        if (projectile.type == 388)
        {
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI;
        }
        if (projectile.type == 387)
        {
            if (projectile.ai[0] != 1f && flag29)
            {
                projectile.rotation = (center5 - projectile.Center).ToRotation() + (float)Math.PI;
            }
            else
            {
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI;
            }
        }
        if (projectile.type == 533 && (projectile.ai[0] < 3f || projectile.ai[0] >= 9f))
        {
            projectile.rotation += projectile.velocity.X * 0.04f;
        }
        if (projectile.type == 388 || projectile.type == 387)
        {
            projectile.frameCounter++;
            if (projectile.frameCounter > 3)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }
            if (projectile.frame > 2)
            {
                projectile.frame = 0;
            }
        }
        else if (projectile.type == 533)
        {
            if (projectile.ai[0] < 3f || projectile.ai[0] >= 9f)
            {
                projectile.frameCounter++;
                if (projectile.frameCounter >= 24)
                {
                    projectile.frameCounter = 0;
                }
                int num573 = projectile.frameCounter / 4;
                projectile.frame = 4 + num573;
                switch ((int)projectile.ai[0])
                {
                    case 0:
                    case 9:
                        projectile.frame = 4 + num573;
                        break;
                    case 1:
                    case 10:
                        num573 = projectile.frameCounter / 8;
                        projectile.frame = 14 + num573;
                        break;
                    case 2:
                    case 11:
                        num573 = projectile.frameCounter / 3;
                        if (num573 >= 4)
                        {
                            num573 -= 4;
                        }
                        projectile.frame = 17 + num573;
                        break;
                }
            }
        }
        if (projectile.ai[1] > 0f && flag26)
        {
            projectile.ai[1] += Main.rand.Next(1, 4);
        }
        if (projectile.ai[1] > 90f && projectile.type == 387)
        {
            projectile.ai[1] = 0f;
            projectile.netUpdate = true;
        }
        if (projectile.ai[1] > 40f && projectile.type == 388)
        {
            projectile.ai[1] = 0f;
            projectile.netUpdate = true;
        }
        if (projectile.ai[1] > 0f && projectile.type == 533)
        {
            projectile.ai[1]++;
            int num577 = 10;
            if (projectile.ai[1] > (float)num577)
            {
                projectile.ai[1] = 0f;
                projectile.netUpdate = true;
            }
        }
        if (projectile.ai[0] == 0f && flag26)
        {
            if (projectile.type == 387)
            {
                float num578 = 8f;
                int num579 = 389;
                if (flag29 && projectile.ai[1] == 0f)
                {
                    projectile.ai[1]++;
                    if (Main.myPlayer == projectile.owner && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, center5, 0, 0))
                    {
                        Vector2 vector48 = center5 - projectile.Center;
                        vector48.Normalize();
                        vector48 *= num578;
                        int num580 = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X, projectile.Center.Y, vector48.X, vector48.Y, num579, (int)((float)projectile.damage * 1.15f), 0f, Main.myPlayer);
                        Main.projectile[num580].timeLeft = 300;
                        projectile.netUpdate = true;
                    }
                }
            }
            if (projectile.type == 388 && projectile.ai[1] == 0f && flag29 && num549 < 500f)
            {
                projectile.ai[1]++;
                if (Main.myPlayer == projectile.owner)
                {
                    projectile.ai[0] = 2f;
                    Vector2 v4 = center5 - projectile.Center;
                    v4 = v4.SafeNormalize(projectile.velocity);
                    float num581 = 8f;
                    projectile.velocity = v4 * num581;
                    projectile.AI_066_TryInterceptingTarget(center5, zero, num581);
                    projectile.netUpdate = true;
                }
            }
        }
        else
        {
            if (projectile.type != 533 || !(projectile.ai[0] < 9f))
            {
                return;
            }
            int num582 = 0;
            num582 = 800;
            if (!(projectile.ai[1] == 0f && flag29) || !(num549 < (float)num582))
            {
                return;
            }
            projectile.ai[1]++;
            if (Main.myPlayer != projectile.owner)
            {
                return;
            }
            if (projectile.localAI[0] >= 3f)
            {
                projectile.ai[0] += 4f;
                if (projectile.ai[0] == 6f)
                {
                    projectile.ai[0] = 3f;
                }
                projectile.localAI[0] = 0f;
            }
            else
            {
                projectile.ai[0] += 6f;
                Vector2 v5 = center5 - projectile.Center;
                v5 = v5.SafeNormalize(Vector2.Zero);
                float num583 = ((projectile.ai[0] == 8f) ? 12f : 10f);
                projectile.velocity = v5 * num583;
                projectile.AI_066_TryInterceptingTarget(center5, zero, num583);
                projectile.netUpdate = true;
            }
        }
    }
    public static void AI_068(Projectile projectile)
    {
        projectile.rotation += 0.25f * (float)projectile.direction;
        bool flag31 = projectile.type == 399;
        bool flag32 = projectile.type == 669;
        projectile.ai[0] += 1f;
        if (projectile.ai[0] >= 3f)
        {
            projectile.alpha -= 40;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
        }
        if (projectile.ai[0] >= 15f)
        {
            projectile.velocity.Y += 0.2f;
            if (projectile.velocity.Y > 16f)
            {
                projectile.velocity.Y = 16f;
            }
            projectile.velocity.X *= 0.99f;
        }
        projectile.spriteDirection = projectile.direction;
        if (projectile.timeLeft <= 3)
        {
            projectile.tileCollide = false;
            projectile.alpha = 255;
            projectile.position.X += projectile.width / 2;
            projectile.position.Y += projectile.height / 2;
            projectile.width = 80;
            projectile.height = 80;
            projectile.position.X -= projectile.width / 2;
            projectile.position.Y -= projectile.height / 2;
            projectile.knockBack = 8f;
        }
        if (projectile.wet && projectile.timeLeft > 3)
        {
            projectile.timeLeft = 3;
        }
    }
    public static void AI_069(Projectile projectile)
    {
        Vector2 vector49 = Main.player[projectile.owner].Center - projectile.Center;
        projectile.rotation = vector49.ToRotation() - 1.57f;
        if (Main.player[projectile.owner].dead)
        {
            projectile.Kill();
            return;
        }
        Main.player[projectile.owner].SetDummyItemTime(10);
        _ = vector49.X;
        _ = 0f;
        if (vector49.X < 0f)
        {
            Main.player[projectile.owner].ChangeDir(1);
            projectile.direction = 1;
        }
        else
        {
            Main.player[projectile.owner].ChangeDir(-1);
            projectile.direction = -1;
        }
        Main.player[projectile.owner].itemRotation = (vector49 * -1f * projectile.direction).ToRotation();
        projectile.spriteDirection = ((!(vector49.X > 0f)) ? 1 : (-1));
        if (projectile.ai[0] == 0f && vector49.Length() > 400f)
        {
            projectile.ai[0] = 1f;
        }
        if (projectile.ai[0] == 1f || projectile.ai[0] == 2f)
        {
            float num590 = vector49.Length();
            if (num590 > 1500f)
            {
                projectile.Kill();
                return;
            }
            if (num590 > 600f)
            {
                projectile.ai[0] = 2f;
            }
            projectile.tileCollide = false;
            float num591 = 20f;
            if (projectile.ai[0] == 2f)
            {
                num591 = 40f;
            }
            projectile.velocity = Vector2.Normalize(vector49) * num591;
            if (vector49.Length() < num591)
            {
                projectile.Kill();
                return;
            }
        }
        projectile.ai[1]++;
        if (projectile.ai[1] > 5f)
        {
            projectile.alpha = 0;
        }
        if ((int)projectile.ai[1] % 4 == 0 && projectile.owner == Main.myPlayer)
        {
            Vector2 spinningpoint4 = vector49 * -1f;
            spinningpoint4.Normalize();
            spinningpoint4 *= (float)Main.rand.Next(45, 65) * 0.1f;
            spinningpoint4 = spinningpoint4.RotatedBy((Main.rand.NextDouble() - 0.5) * 1.5707963705062866);
            Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X, projectile.Center.Y, spinningpoint4.X, spinningpoint4.Y, 405, projectile.damage, projectile.knockBack, projectile.owner, -10f);
        }
    }
    public static void AI_070(Projectile projectile)
    {
        if (projectile.ai[0] == 0f)
        {
            float num592 = 650f;
            int num593 = -1;
            for (int num594 = 0; num594 < 200; num594++)
            {
                NPC nPC6 = Main.npc[num594];
                float num595 = (nPC6.Center - projectile.Center).Length();
                if (!(num595 >= num592) && nPC6.CanBeChasedBy(projectile) && Collision.CanHit(projectile.position, projectile.width, projectile.height, nPC6.position, nPC6.width, nPC6.height))
                {
                    num593 = num594;
                    num592 = num595;
                }
            }
            projectile.ai[0] = num593 + 1;
            if (projectile.ai[0] == 0f)
            {
                projectile.ai[0] = -15f;
            }
            if (projectile.ai[0] > 0f)
            {
                float num596 = (float)Main.rand.Next(35, 75) / 30f;
                projectile.velocity = (projectile.velocity * 20f + Vector2.Normalize(Main.npc[(int)projectile.ai[0] - 1].Center - projectile.Center + new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101))) * num596) / 21f;
                projectile.netUpdate = true;
            }
        }
        else if (projectile.ai[0] > 0f)
        {
            Vector2 vector50 = Vector2.Normalize(Main.npc[(int)projectile.ai[0] - 1].Center - projectile.Center);
            projectile.velocity = (projectile.velocity * 40f + vector50 * 12f) / 41f;
        }
        else
        {
            projectile.ai[0]++;
            projectile.alpha -= 25;
            if (projectile.alpha < 50)
            {
                projectile.alpha = 50;
            }
            projectile.velocity *= 0.95f;
        }
        if (projectile.ai[1] == 0f)
        {
            projectile.ai[1] = (float)Main.rand.Next(80, 121) / 100f;
            projectile.netUpdate = true;
        }
        projectile.scale = projectile.ai[1];
    }
    public static void AI_071(Projectile projectile)
    {
        projectile.localAI[1]++;
        if (projectile.localAI[1] > 10f && Main.rand.Next(3) == 0)
        {
            projectile.alpha -= 5;
            if (projectile.alpha < 50)
            {
                projectile.alpha = 50;
            }
            projectile.rotation += projectile.velocity.X * 0.1f;
            projectile.frame = (int)(projectile.localAI[1] / 3f) % 3;
            Lighting.AddLight((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, 0.1f, 0.4f, 0.6f);
        }
        int num600 = -1;
        Vector2 vector52 = projectile.Center;
        float num601 = 500f;
        if (projectile.localAI[0] > 0f)
        {
            projectile.localAI[0]--;
        }
        if (projectile.ai[0] == 0f && projectile.localAI[0] == 0f)
        {
            for (int num602 = 0; num602 < 200; num602++)
            {
                NPC nPC7 = Main.npc[num602];
                if (nPC7.CanBeChasedBy(projectile) && (projectile.ai[0] == 0f || projectile.ai[0] == (float)(num602 + 1)))
                {
                    Vector2 center7 = nPC7.Center;
                    float num603 = Vector2.Distance(center7, vector52);
                    if (num603 < num601 && Collision.CanHit(projectile.position, projectile.width, projectile.height, nPC7.position, nPC7.width, nPC7.height))
                    {
                        num601 = num603;
                        vector52 = center7;
                        num600 = num602;
                    }
                }
            }
            if (num600 >= 0)
            {
                projectile.ai[0] = num600 + 1;
                projectile.netUpdate = true;
            }
            num600 = -1;
        }
        if (projectile.localAI[0] == 0f && projectile.ai[0] == 0f)
        {
            projectile.localAI[0] = 30f;
        }
        bool flag33 = false;
        if (projectile.ai[0] != 0f)
        {
            int num604 = (int)(projectile.ai[0] - 1f);
            if (Main.npc[num604].active && !Main.npc[num604].dontTakeDamage && Main.npc[num604].immune[projectile.owner] == 0)
            {
                float num605 = Main.npc[num604].position.X + (float)(Main.npc[num604].width / 2);
                float num606 = Main.npc[num604].position.Y + (float)(Main.npc[num604].height / 2);
                float num607 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num605) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num606);
                if (num607 < 1000f)
                {
                    flag33 = true;
                    vector52 = Main.npc[num604].Center;
                }
            }
            else
            {
                projectile.ai[0] = 0f;
                flag33 = false;
                projectile.netUpdate = true;
            }
        }
        if (flag33)
        {
            Vector2 v6 = vector52 - projectile.Center;
            float num608 = projectile.velocity.ToRotation();
            float num609 = v6.ToRotation();
            double num610 = num609 - num608;
            if (num610 > Math.PI)
            {
                num610 -= Math.PI * 2.0;
            }
            if (num610 < -Math.PI)
            {
                num610 += Math.PI * 2.0;
            }
            projectile.velocity = projectile.velocity.RotatedBy(num610 * 0.10000000149011612);
        }
        float num611 = projectile.velocity.Length();
        projectile.velocity.Normalize();
        projectile.velocity *= num611 + 0.0025f;
    }
    public static void AI_072(Projectile projectile)
    {
        projectile.localAI[0]++;
        if (projectile.localAI[0] > 3f)
        {
            projectile.alpha -= 25;
            if (projectile.alpha < 50)
            {
                projectile.alpha = 50;
            }
        }
        projectile.velocity *= 0.96f;
        if (projectile.ai[1] == 0f)
        {
            projectile.ai[1] = (float)Main.rand.Next(60, 121) / 100f;
            projectile.netUpdate = true;
        }
        projectile.scale = projectile.ai[1];
        projectile.position = projectile.Center;
        int num612 = 14;
        int num613 = 14;
        projectile.width = (int)((float)num612 * projectile.ai[1]);
        projectile.height = (int)((float)num613 * projectile.ai[1]);
        projectile.position -= new Vector2(projectile.width / 2, projectile.height / 2);
    }
    public static void AI_073(Projectile projectile)
    {
        ITile tile2 = Main.tile[(int)projectile.ai[0], (int)projectile.ai[1]];
        if (tile2 == null || !tile2.active() || tile2.type != 338)
        {
            projectile.Kill();
        }
    }
    public static void AI_074(Projectile projectile)
    {
        if (projectile.extraUpdates == 1)
        {
            projectile.localAI[0] *= projectile.localAI[1];
            projectile.localAI[1] -= 0.001f;
            if ((double)projectile.localAI[0] < 0.01)
            {
                projectile.Kill();
            }
        }
    }
    public static void AI_075(Projectile projectile)
    {
        projectile.AI_075();
    }
    public static void AI_076(Projectile projectile)
    {
        Player player5 = Main.player[projectile.owner];
        player5.heldProj = projectile.whoAmI;
        if (projectile.type == 441)
        {
            if (player5.mount.Type != 9)
            {
                projectile.Kill();
                return;
            }
        }
        else if (projectile.type == 453 && player5.mount.Type != 8)
        {
            projectile.Kill();
            return;
        }
        if (Main.myPlayer == projectile.owner)
        {
            projectile.position.X = Main.screenPosition.X + (float)Main.mouseX;
            projectile.position.Y = Main.screenPosition.Y + (float)Main.mouseY;
            if (projectile.ai[0] != projectile.position.X - player5.position.X || projectile.ai[1] != projectile.position.Y - player5.position.Y)
            {
                projectile.netUpdate = true;
            }
            projectile.ai[0] = projectile.position.X - player5.position.X;
            projectile.ai[1] = projectile.position.Y - player5.position.Y;
            player5.mount.AimAbility(player5, projectile.position);
            if (!player5.channel)
            {
                player5.mount.UseAbility(player5, projectile.position, toggleOn: false);
                projectile.Kill();
            }
            return;
        }
        projectile.position.X = player5.position.X + projectile.ai[0];
        projectile.position.Y = player5.position.Y + projectile.ai[1];
        if (projectile.type == 441)
        {
            if (!player5.mount.AbilityCharging)
            {
                player5.mount.StartAbilityCharge(player5);
            }
        }
        else if (projectile.type == 453 && !player5.mount.AbilityActive)
        {
            player5.mount.UseAbility(player5, projectile.position, toggleOn: false);
        }
        player5.mount.AimAbility(player5, projectile.position);
    }
    public static void AI_077(Projectile projectile)
    {
        ActiveSound activeSound = SoundEngine.GetActiveSound(SlotId.FromFloat(projectile.localAI[0]));
        if (activeSound != null)
        {
            if (activeSound.Volume == 0f)
            {
                activeSound.Stop();
                projectile.localAI[0] = SlotId.Invalid.ToFloat();
            }
            activeSound.Volume = Math.Max(0f, activeSound.Volume - 0.05f);
        }
        else
        {
            projectile.localAI[0] = SlotId.Invalid.ToFloat();
        }
        if (projectile.ai[1] == 1f)
        {
            projectile.friendly = false;
            if (projectile.alpha < 255)
            {
                projectile.alpha += 51;
            }
            if (projectile.alpha >= 255)
            {
                projectile.alpha = 255;
                projectile.Kill();
                return;
            }
        }
        else
        {
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 50;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
        }
        float num628 = 30f;
        float num629 = num628 * 4f;
        projectile.ai[0]++;
        if (projectile.ai[0] > num629)
        {
            projectile.ai[0] = 0f;
        }
        Vector2 vector54 = -Vector2.UnitY.RotatedBy((float)Math.PI * 2f * projectile.ai[0] / num628);
        float val = 0.75f + vector54.Y * 0.25f;
        float val2 = 0.8f - vector54.Y * 0.2f;
        float num630 = Math.Max(val, val2);
        projectile.position += new Vector2(projectile.width, projectile.height) / 2f;
        projectile.width = (projectile.height = (int)(80f * num630));
        projectile.position -= new Vector2(projectile.width, projectile.height) / 2f;
        projectile.frameCounter++;
        if (projectile.frameCounter >= 3)
        {
            projectile.frameCounter = 0;
            projectile.frame++;
            if (projectile.frame >= 4)
            {
                projectile.frame = 0;
            }
        }
    }
    public static void AI_078(Projectile projectile)
    {
        if (projectile.alpha > 0)
        {
            projectile.alpha -= 30;
        }
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        Vector2 v7 = projectile.ai[0].ToRotationVector2();
        float num636 = projectile.velocity.ToRotation();
        float num637 = v7.ToRotation();
        double num638 = num637 - num636;
        if (num638 > Math.PI)
        {
            num638 -= Math.PI * 2.0;
        }
        if (num638 < -Math.PI)
        {
            num638 += Math.PI * 2.0;
        }
        projectile.velocity = projectile.velocity.RotatedBy(num638 * 0.05000000074505806);
        projectile.velocity *= 0.96f;
        projectile.rotation = projectile.velocity.ToRotation() - (float)Math.PI / 2f;
        if (Main.myPlayer == projectile.owner && projectile.timeLeft > 60)
        {
            projectile.timeLeft = 60;
        }
    }
    public static void AI_079(Projectile projectile)
    {
        bool flag34 = true;
        int num639 = (int)projectile.ai[0] - 1;
        if (projectile.type == 447 && (projectile.ai[0] == 0f || ((!Main.npc[num639].active || Main.npc[num639].type != 392) && (!Main.npc[num639].active || Main.npc[num639].type != 395 || !(Main.npc[num639].ai[3] % 120f >= 60f) || Main.npc[num639].ai[0] != 2f))))
        {
            flag34 = false;
        }
        if (!flag34)
        {
            projectile.Kill();
            return;
        }
        NPC nPC8 = Main.npc[num639];
        float num640 = nPC8.Center.Y + 46f;
        float num641 = num640;
        if (projectile.type == 447)
        {
            int target = nPC8.target;
            if (nPC8.type == 392)
            {
                target = Main.npc[(int)nPC8.ai[0]].target;
            }
            Player player6 = Main.player[target];
            if (player6 != null && player6.active && !player6.dead)
            {
                num641 = player6.Bottom.Y;
            }
        }
        num641 /= 16f;
        int x9 = (int)nPC8.Center.X / 16;
        int num642 = (int)num640 / 16;
        int num643 = 0;
        if ((float)num642 >= num641 && Main.tile[x9, num642].nactive() && Main.tileSolid[Main.tile[x9, num642].type] && !Main.tileSolidTop[Main.tile[x9, num642].type])
        {
            num643 = 1;
        }
        else
        {
            for (; num643 < 150 && num642 + num643 < Main.maxTilesY; num643++)
            {
                int num644 = num642 + num643;
                if ((float)num644 >= num641 && Main.tile[x9, num644].nactive() && Main.tileSolid[Main.tile[x9, num644].type] && !Main.tileSolidTop[Main.tile[x9, num644].type])
                {
                    num643--;
                    break;
                }
            }
        }
        projectile.position.X = nPC8.Center.X - (float)(projectile.width / 2);
        projectile.position.Y = num640;
        projectile.height = (num643 + 1) * 16;
        int num645 = (int)projectile.position.Y + projectile.height;
        if (Main.tile[x9, num645 / 16].nactive() && Main.tileSolid[Main.tile[x9, num645 / 16].type] && !Main.tileSolidTop[Main.tile[x9, num645 / 16].type])
        {
            int num646 = num645 % 16;
            projectile.height -= num646 - 2;
        }
        if (projectile.type == 447 && ++projectile.frameCounter >= 5)
        {
            projectile.frameCounter = 0;
            if (++projectile.frame >= 4)
            {
                projectile.frame = 0;
            }
        }
    }
    public static void AI_080(Projectile projectile)
    {
        if (projectile.ai[0] == 0f && projectile.ai[1] > 0f)
        {
            projectile.ai[1]--;
        }
        else if (projectile.ai[0] == 0f && projectile.ai[1] == 0f)
        {
            projectile.ai[0] = 1f;
            projectile.ai[1] = (int)Player.FindClosest(projectile.position, projectile.width, projectile.height);
            projectile.netUpdate = true;
            float num650 = projectile.velocity.Length();
            projectile.velocity = Vector2.Normalize(projectile.velocity) * (num650 + 4f);
        }
        else if (projectile.ai[0] == 1f)
        {
            projectile.tileCollide = true;
            projectile.localAI[1]++;
            float num653 = 180f;
            float num654 = 0f;
            float num655 = 30f;
            if (projectile.localAI[1] == num653)
            {
                projectile.Kill();
                return;
            }
            if (projectile.localAI[1] >= num654 && projectile.localAI[1] < num654 + num655)
            {
                Vector2 v8 = Main.player[(int)projectile.ai[1]].Center - projectile.Center;
                float num656 = projectile.velocity.ToRotation();
                float num657 = v8.ToRotation();
                double num658 = num657 - num656;
                if (num658 > Math.PI)
                {
                    num658 -= Math.PI * 2.0;
                }
                if (num658 < -Math.PI)
                {
                    num658 += Math.PI * 2.0;
                }
                projectile.velocity = projectile.velocity.RotatedBy(num658 * 0.20000000298023224);
            }
        }
        projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
        if (++projectile.frameCounter >= 3)
        {
            projectile.frameCounter = 0;
            if (++projectile.frame >= 3)
            {
                projectile.frame = 0;
            }
        }
        for (int num663 = 0; num663 < 255; num663++)
        {
            Player player7 = Main.player[num663];
            if (player7.active && !player7.dead && Vector2.Distance(player7.Center, projectile.Center) <= 42f)
            {
                projectile.Kill();
                break;
            }
        }
    }
    public static void AI_081(Projectile projectile)
    {
        int num664 = projectile.penetrate;
        if (projectile.ai[0] == 0f)
        {
            projectile.tileCollide = true;
            projectile.localAI[0]++;
            float num669 = 0.01f;
            int num670 = 5;
            int num671 = num670 * 15;
            int num672 = 0;
            if (projectile.localAI[0] > 7f)
            {
                if (projectile.localAI[1] == 0f)
                {
                    projectile.scale -= num669;
                    projectile.alpha += num670;
                    if (projectile.alpha > num671)
                    {
                        projectile.alpha = num671;
                        projectile.localAI[1] = 1f;
                    }
                }
                else if (projectile.localAI[1] == 1f)
                {
                    projectile.scale += num669;
                    projectile.alpha -= num670;
                    if (projectile.alpha <= num672)
                    {
                        projectile.alpha = num672;
                        projectile.localAI[1] = 0f;
                    }
                }
            }
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 4f;
        }
        else if (projectile.ai[0] >= 1f && projectile.ai[0] < (float)(1 + num664))
        {
            projectile.tileCollide = false;
            projectile.alpha += 15;
            projectile.velocity *= 0.98f;
            projectile.localAI[0] = 0f;
            int num673 = -1;
            Vector2 vector56 = projectile.Center;
            float num674 = 250f;
            for (int num675 = 0; num675 < 200; num675++)
            {
                NPC nPC9 = Main.npc[num675];
                if (nPC9.CanBeChasedBy(projectile))
                {
                    Vector2 center9 = nPC9.Center;
                    float num676 = Vector2.Distance(center9, projectile.Center);
                    if (num676 < num674)
                    {
                        num674 = num676;
                        vector56 = center9;
                        num673 = num675;
                    }
                }
            }
            if (projectile.alpha >= 255)
            {
                if (projectile.ai[0] == 1f)
                {
                    projectile.Kill();
                    return;
                }
                if (num673 >= 0)
                {
                    projectile.netUpdate = true;
                    projectile.ai[0] += num664;
                    projectile.position = vector56 + ((float)Main.rand.NextDouble() * ((float)Math.PI * 2f)).ToRotationVector2() * 100f - new Vector2(projectile.width, projectile.height) / 2f;
                    projectile.velocity = Vector2.Normalize(vector56 - projectile.Center) * 15f;
                    projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 4f;
                }
                else
                {
                    projectile.Kill();
                }
            }
            if (projectile.active && num673 >= 0)
            {
                projectile.position += Main.npc[num673].velocity;
            }
        }
        else if (projectile.ai[0] >= (float)(1 + num664) && projectile.ai[0] < (float)(1 + num664 * 2))
        {
            projectile.scale = 0.9f;
            projectile.tileCollide = false;
            projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 4f;
            projectile.ai[1]++;
            if (projectile.ai[1] >= 15f)
            {
                projectile.alpha += 51;
                projectile.velocity *= 0.8f;
                if (projectile.alpha >= 255)
                {
                    projectile.Kill();
                }
            }
            else
            {
                projectile.alpha -= 125;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
                projectile.velocity *= 0.98f;
            }
            projectile.localAI[0]++;
            int num681 = -1;
            Vector2 center11 = projectile.Center;
            float num682 = 250f;
            for (int num683 = 0; num683 < 200; num683++)
            {
                NPC nPC10 = Main.npc[num683];
                if (nPC10.CanBeChasedBy(projectile))
                {
                    Vector2 center12 = nPC10.Center;
                    float num684 = Vector2.Distance(center12, projectile.Center);
                    if (num684 < num682)
                    {
                        num682 = num684;
                        center11 = center12;
                        num681 = num683;
                    }
                }
            }
            if (num681 >= 0)
            {
                projectile.position += Main.npc[num681].velocity;
            }
        }
        float num689 = (float)projectile.alpha / 255f;
        Lighting.AddLight((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, 0.3f * num689, 0.4f * num689, 1f * num689);
    }
    public static void AI_082(Projectile projectile)
    {
        projectile.alpha -= 40;
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        if (projectile.ai[0] == 0f)
        {
            projectile.localAI[0]++;
            if (projectile.localAI[0] >= 45f)
            {
                projectile.localAI[0] = 0f;
                projectile.ai[0] = 1f;
                projectile.ai[1] = 0f - projectile.ai[1];
                projectile.netUpdate = true;
            }
            projectile.velocity.X = projectile.velocity.RotatedBy(projectile.ai[1]).X;
            projectile.velocity.X = MathHelper.Clamp(projectile.velocity.X, -6f, 6f);
            projectile.velocity.Y -= 0.08f;
            if (projectile.velocity.Y > 0f)
            {
                projectile.velocity.Y -= 0.2f;
            }
            if (projectile.velocity.Y < -7f)
            {
                projectile.velocity.Y = -7f;
            }
        }
        else if (projectile.ai[0] == 1f)
        {
            projectile.localAI[0]++;
            if (projectile.localAI[0] >= 90f)
            {
                projectile.localAI[0] = 0f;
                projectile.ai[0] = 2f;
                projectile.ai[1] = (int)Player.FindClosest(projectile.position, projectile.width, projectile.height);
                projectile.netUpdate = true;
            }
            projectile.velocity.X = projectile.velocity.RotatedBy(projectile.ai[1]).X;
            projectile.velocity.X = MathHelper.Clamp(projectile.velocity.X, -6f, 6f);
            projectile.velocity.Y -= 0.08f;
            if (projectile.velocity.Y > 0f)
            {
                projectile.velocity.Y -= 0.2f;
            }
            if (projectile.velocity.Y < -7f)
            {
                projectile.velocity.Y = -7f;
            }
        }
        else if (projectile.ai[0] == 2f)
        {
            Vector2 value7 = Main.player[(int)projectile.ai[1]].Center - projectile.Center;
            if (value7.Length() < 30f)
            {
                projectile.Kill();
                return;
            }
            value7.Normalize();
            value7 *= 14f;
            value7 = Vector2.Lerp(projectile.velocity, value7, 0.6f);
            if (value7.Y < 6f)
            {
                value7.Y = 6f;
            }
            float num690 = 0.4f;
            if (projectile.velocity.X < value7.X)
            {
                projectile.velocity.X += num690;
                if (projectile.velocity.X < 0f && value7.X > 0f)
                {
                    projectile.velocity.X += num690;
                }
            }
            else if (projectile.velocity.X > value7.X)
            {
                projectile.velocity.X -= num690;
                if (projectile.velocity.X > 0f && value7.X < 0f)
                {
                    projectile.velocity.X -= num690;
                }
            }
            if (projectile.velocity.Y < value7.Y)
            {
                projectile.velocity.Y += num690;
                if (projectile.velocity.Y < 0f && value7.Y > 0f)
                {
                    projectile.velocity.Y += num690;
                }
            }
            else if (projectile.velocity.Y > value7.Y)
            {
                projectile.velocity.Y -= num690;
                if (projectile.velocity.Y > 0f && value7.Y < 0f)
                {
                    projectile.velocity.Y -= num690;
                }
            }
        }
        projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
    }
    public static void AI_083(Projectile projectile)
    {
        if (projectile.alpha > 200)
        {
            projectile.alpha = 200;
        }
        projectile.alpha -= 5;
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        float num692 = (float)projectile.alpha / 255f;
        projectile.scale = 1f - num692;
        if (projectile.ai[0] >= 0f)
        {
            projectile.ai[0]++;
        }
        if (projectile.ai[0] == -1f)
        {
            projectile.frame = 1;
            projectile.extraUpdates = 1;
        }
        else if (projectile.ai[0] < 30f)
        {
            projectile.position = Main.npc[(int)projectile.ai[1]].Center - new Vector2(projectile.width, projectile.height) / 2f - projectile.velocity;
        }
        else
        {
            projectile.velocity *= 0.96f;
            if (++projectile.frameCounter >= 6)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 2)
                {
                    projectile.frame = 0;
                }
            }
        }
        if (projectile.alpha >= 40)
        {
            return;
        }
    }
    public static void AI_084(Projectile projectile)
    {
        Vector2? vector59 = null;
        if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
        {
            projectile.velocity = -Vector2.UnitY;
        }
        if (projectile.type == 455 && Main.npc[(int)projectile.ai[1]].active && Main.npc[(int)projectile.ai[1]].type == 396)
        {
            if (Main.npc[(int)projectile.ai[1]].ai[0] == -2f)
            {
                projectile.Kill();
                return;
            }
            Vector2 vector60 = Utils.Vector2FromElipse(elipseSizes: new Vector2(27f, 59f) * Main.npc[(int)projectile.ai[1]].localAI[1], angleVector: Main.npc[(int)projectile.ai[1]].localAI[0].ToRotationVector2());
            projectile.position = Main.npc[(int)projectile.ai[1]].Center + vector60 - new Vector2(projectile.width, projectile.height) / 2f;
        }
        else if (projectile.type == 455 && Main.npc[(int)projectile.ai[1]].active && Main.npc[(int)projectile.ai[1]].type == 400)
        {
            Vector2 vector61 = Utils.Vector2FromElipse(elipseSizes: new Vector2(30f, 30f) * Main.npc[(int)projectile.ai[1]].localAI[1], angleVector: Main.npc[(int)projectile.ai[1]].localAI[0].ToRotationVector2());
            projectile.position = Main.npc[(int)projectile.ai[1]].Center + vector61 - new Vector2(projectile.width, projectile.height) / 2f;
        }
        else if (projectile.type == 537 && Main.npc[(int)projectile.ai[1]].active && Main.npc[(int)projectile.ai[1]].type == 411)
        {
            Vector2 vector62 = new Vector2(Main.npc[(int)projectile.ai[1]].direction * 6, -4f);
            projectile.position = Main.npc[(int)projectile.ai[1]].Center + vector62 - projectile.Size / 2f + new Vector2(0f, 0f - Main.npc[(int)projectile.ai[1]].gfxOffY);
        }
        else if (projectile.type == 461 && Main.projectile[(int)projectile.ai[1]].active && Main.projectile[(int)projectile.ai[1]].type == 460)
        {
            Vector2 vector63 = Vector2.Normalize(Main.projectile[(int)projectile.ai[1]].velocity);
            projectile.position = Main.projectile[(int)projectile.ai[1]].Center + vector63 * 16f - new Vector2(projectile.width, projectile.height) / 2f + new Vector2(0f, 0f - Main.projectile[(int)projectile.ai[1]].gfxOffY);
            projectile.velocity = Vector2.Normalize(Main.projectile[(int)projectile.ai[1]].velocity);
        }
        else if (projectile.type == 642 && Main.projectile[(int)projectile.ai[1]].active && Main.projectile[(int)projectile.ai[1]].type == 641)
        {
            Projectile newProj = Main.projectile[(int)projectile.ai[1]];
            projectile.Center = newProj.Center;
            Vector2 vector64 = projectile.ai[0].ToRotationVector2().RotatedBy((float)(-newProj.direction) * ((float)Math.PI / 3f) / 50f);
            projectile.ai[0] = vector64.ToRotation();
            projectile.velocity = Vector2.Normalize(vector64);
        }
        else
        {
            if (projectile.type != 632 || !Main.projectile[(int)projectile.ai[1]].active || Main.projectile[(int)projectile.ai[1]].type != 633)
            {
                projectile.Kill();
                return;
            }
            float num696 = (float)(int)projectile.ai[0] - 2.5f;
            Vector2 vector65 = Vector2.Normalize(Main.projectile[(int)projectile.ai[1]].velocity);
            Projectile projectile2 = Main.projectile[(int)projectile.ai[1]];
            float num697 = num696 * ((float)Math.PI / 6f);
            float num698 = 20f;
            Vector2 zero2 = Vector2.Zero;
            float num699 = 1f;
            float num700 = 15f;
            float num701 = -2f;
            if (projectile2.ai[0] < 180f)
            {
                num699 = 1f - projectile2.ai[0] / 180f;
                num700 = 20f - projectile2.ai[0] / 180f * 14f;
                if (projectile2.ai[0] < 120f)
                {
                    num698 = 20f - 4f * (projectile2.ai[0] / 120f);
                    projectile.Opacity = projectile2.ai[0] / 120f * 0.4f;
                }
                else
                {
                    num698 = 16f - 10f * ((projectile2.ai[0] - 120f) / 60f);
                    projectile.Opacity = 0.4f + (projectile2.ai[0] - 120f) / 60f * 0.6f;
                }
                num701 = -22f + projectile2.ai[0] / 180f * 20f;
            }
            else
            {
                num699 = 0f;
                num698 = 1.75f;
                num700 = 6f;
                projectile.Opacity = 1f;
                num701 = -2f;
            }
            float num702 = (projectile2.ai[0] + num696 * num698) / (num698 * 6f) * ((float)Math.PI * 2f);
            num697 = Vector2.UnitY.RotatedBy(num702).Y * ((float)Math.PI / 6f) * num699;
            zero2 = (Vector2.UnitY.RotatedBy(num702) * new Vector2(4f, num700)).RotatedBy(projectile2.velocity.ToRotation());
            projectile.position = projectile2.Center + vector65 * 16f - projectile.Size / 2f + new Vector2(0f, 0f - Main.projectile[(int)projectile.ai[1]].gfxOffY);
            projectile.position += projectile2.velocity.ToRotation().ToRotationVector2() * num701;
            projectile.position += zero2;
            projectile.velocity = Vector2.Normalize(projectile2.velocity).RotatedBy(num697);
            projectile.scale = 1.4f * (1f - num699);
            projectile.damage = projectile2.damage;
            if (projectile2.ai[0] >= 180f)
            {
                projectile.damage *= 3;
                vector59 = projectile2.Center;
            }
            if (!Collision.CanHitLine(Main.player[projectile.owner].Center, 0, 0, projectile2.Center, 0, 0))
            {
                vector59 = Main.player[projectile.owner].Center;
            }
            projectile.friendly = projectile2.ai[0] > 30f;
        }
        if (projectile.velocity.HasNaNs() || projectile.velocity == Vector2.Zero)
        {
            projectile.velocity = -Vector2.UnitY;
        }
        if (projectile.type == 461)
        {
            projectile.ai[0]++;
            if (projectile.ai[0] >= 300f)
            {
                projectile.Kill();
                return;
            }
            projectile.scale = (float)Math.Sin(projectile.ai[0] * (float)Math.PI / 300f) * 10f;
            if (projectile.scale > 1f)
            {
                projectile.scale = 1f;
            }
        }
        if (projectile.type == 455)
        {
            if (projectile.localAI[0] == 0f)
            {
                SoundEngine.PlaySound(29, (int)projectile.position.X, (int)projectile.position.Y, 104);
            }
            float num703 = 1f;
            if (Main.npc[(int)projectile.ai[1]].type == 400)
            {
                num703 = 0.4f;
            }
            projectile.localAI[0]++;
            if (projectile.localAI[0] >= 180f)
            {
                projectile.Kill();
                return;
            }
            projectile.scale = (float)Math.Sin(projectile.localAI[0] * (float)Math.PI / 180f) * 10f * num703;
            if (projectile.scale > num703)
            {
                projectile.scale = num703;
            }
        }
        if (projectile.type == 642)
        {
            float num704 = 1f;
            projectile.localAI[0]++;
            if (projectile.localAI[0] >= 50f)
            {
                projectile.Kill();
                return;
            }
            projectile.scale = (float)Math.Sin(projectile.localAI[0] * (float)Math.PI / 50f) * 10f * num704;
            if (projectile.scale > num704)
            {
                projectile.scale = num704;
            }
        }
        if (projectile.type == 537)
        {
            float num705 = 0.8f;
            projectile.localAI[0]++;
            if (projectile.localAI[0] >= 90f)
            {
                projectile.Kill();
                return;
            }
            projectile.scale = (float)Math.Sin(projectile.localAI[0] * (float)Math.PI / 90f) * 10f * num705;
            if (projectile.scale > num705)
            {
                projectile.scale = num705;
            }
        }
        float num706 = projectile.velocity.ToRotation();
        if (projectile.type == 455)
        {
            num706 += projectile.ai[0];
        }
        projectile.rotation = num706 - (float)Math.PI / 2f;
        projectile.velocity = num706.ToRotationVector2();
        float num707 = 0f;
        float num708 = 0f;
        Vector2 samplingPoint = projectile.Center;
        if (vector59.HasValue)
        {
            samplingPoint = vector59.Value;
        }
        if (projectile.type == 455)
        {
            num707 = 3f;
            num708 = projectile.width;
        }
        else if (projectile.type == 461)
        {
            num707 = 2f;
            num708 = 0f;
        }
        else if (projectile.type == 642)
        {
            num707 = 2f;
            num708 = 0f;
        }
        else if (projectile.type == 632)
        {
            num707 = 2f;
            num708 = 0f;
        }
        else if (projectile.type == 537)
        {
            num707 = 2f;
            num708 = 0f;
        }
        float[] array2 = new float[(int)num707];
        Collision.LaserScan(samplingPoint, projectile.velocity, num708 * projectile.scale, 2400f, array2);
        float num709 = 0f;
        for (int num710 = 0; num710 < array2.Length; num710++)
        {
            num709 += array2[num710];
        }
        num709 /= num707;
        float amount = 0.5f;
        if (projectile.type == 455)
        {
            NPC nPC11 = Main.npc[(int)projectile.ai[1]];
            if (nPC11.type == 396)
            {
                Player player8 = Main.player[nPC11.target];
                if (!Collision.CanHitLine(nPC11.position, nPC11.width, nPC11.height, player8.position, player8.width, player8.height))
                {
                    num709 = Math.Min(2400f, Vector2.Distance(nPC11.Center, player8.Center) + 150f);
                    amount = 0.75f;
                }
            }
        }
        if (projectile.type == 632)
        {
            amount = 0.75f;
        }
        projectile.localAI[1] = MathHelper.Lerp(projectile.localAI[1], num709, amount);
        if (projectile.type != 632 || !(Math.Abs(projectile.localAI[1] - num709) < 100f) || !(projectile.scale > 0.15f))
        {
            return;
        }
        float laserLuminance = 0.5f;
        float laserAlphaMultiplier = 0f;
        float lastPrismHue = projectile.GetLastPrismHue(projectile.ai[0], ref laserLuminance, ref laserAlphaMultiplier);
        Color color = Main.hslToRgb(lastPrismHue, 1f, laserLuminance);
        color.A = (byte)((float)(int)color.A * laserAlphaMultiplier);
        Color color2 = color;
        Vector2 vector78 = projectile.Center + projectile.velocity * (projectile.localAI[1] - 14.5f * projectile.scale);
        float x10 = Main.rgbToHsl(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB)).X;
        DelegateMethods.v3_1 = color.ToVector3() * 0.3f;
        float value8 = 0.1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
        Vector2 size2 = new Vector2(projectile.velocity.Length() * projectile.localAI[1], (float)projectile.width * projectile.scale);
        float num738 = projectile.velocity.ToRotation();
        if (Main.netMode != 2)
        {
            ((WaterShaderData)Filters.Scene["WaterDistortion"].GetShader()).QueueRipple(projectile.position + new Vector2(size2.X * 0.5f, 0f).RotatedBy(num738), new Color(0.5f, 0.1f * (float)Math.Sign(value8) + 0.5f, 0f, 1f) * Math.Abs(value8), size2, RippleShape.Square, num738);
        }
        Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * projectile.localAI[1], (float)projectile.width * projectile.scale, DelegateMethods.CastLight);
    }
    public static void AI_085(Projectile projectile)
    {
        Vector2 vector81 = new Vector2(0f, 216f);
        projectile.alpha -= 15;
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        int num739 = (int)Math.Abs(projectile.ai[0]) - 1;
        int num740 = (int)projectile.ai[1];
        if (!Main.npc[num739].active || Main.npc[num739].type != 396)
        {
            projectile.Kill();
            return;
        }
        projectile.localAI[0]++;
        if (projectile.localAI[0] >= 330f && projectile.ai[0] > 0f && Main.netMode != 1)
        {
            projectile.ai[0] *= -1f;
            projectile.netUpdate = true;
        }
        if (Main.netMode != 1 && projectile.ai[0] > 0f && (!Main.player[(int)projectile.ai[1]].active || Main.player[(int)projectile.ai[1]].dead))
        {
            projectile.ai[0] *= -1f;
            projectile.netUpdate = true;
        }
        projectile.rotation = (Main.npc[(int)Math.Abs(projectile.ai[0]) - 1].Center - Main.player[(int)projectile.ai[1]].Center + vector81).ToRotation() + (float)Math.PI / 2f;
        if (projectile.ai[0] > 0f)
        {
            Vector2 value9 = Main.player[(int)projectile.ai[1]].Center - projectile.Center;
            if (value9.X != 0f || value9.Y != 0f)
            {
                projectile.velocity = Vector2.Normalize(value9) * Math.Min(16f, value9.Length());
            }
            else
            {
                projectile.velocity = Vector2.Zero;
            }
            if (value9.Length() < 20f && projectile.localAI[1] == 0f)
            {
                projectile.localAI[1] = 1f;
                int timeToAdd = 840;
                if (Main.expertMode)
                {
                    timeToAdd = 960;
                }
                if (!Main.player[num740].creativeGodMode)
                {
                    Main.player[num740].AddBuff(145, timeToAdd);
                }
            }
        }
        else
        {
            Vector2 value10 = Main.npc[(int)Math.Abs(projectile.ai[0]) - 1].Center - projectile.Center + vector81;
            if (value10.X != 0f || value10.Y != 0f)
            {
                projectile.velocity = Vector2.Normalize(value10) * Math.Min(16f, value10.Length());
            }
            else
            {
                projectile.velocity = Vector2.Zero;
            }
            if (value10.Length() < 20f)
            {
                projectile.Kill();
            }
        }
    }
    public static void AI_086(Projectile projectile)
    {
        if (projectile.localAI[1] == 0f)
        {
            projectile.localAI[1] = 1f;
            SoundEngine.PlaySound(SoundID.Item120, projectile.position);
        }
        projectile.ai[0]++;
        if (projectile.ai[1] == 1f)
        {
            if (projectile.ai[0] >= 130f)
            {
                projectile.alpha += 10;
            }
            else
            {
                projectile.alpha -= 10;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (projectile.alpha > 255)
            {
                projectile.alpha = 255;
            }
            if (projectile.ai[0] >= 150f)
            {
                projectile.Kill();
                return;
            }
            if (projectile.ai[0] % 30f == 0f && Main.netMode != 1)
            {
                Vector2 vector82 = projectile.rotation.ToRotationVector2();
                Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X, projectile.Center.Y, vector82.X, vector82.Y, 464, projectile.damage, projectile.knockBack, projectile.owner);
            }
            projectile.rotation += (float)Math.PI / 30f;
            Lighting.AddLight(projectile.Center, 0.3f, 0.75f, 0.9f);
            return;
        }
        projectile.position -= projectile.velocity;
        if (projectile.ai[0] >= 40f)
        {
            projectile.alpha += 3;
        }
        else
        {
            projectile.alpha -= 40;
        }
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        if (projectile.alpha > 255)
        {
            projectile.alpha = 255;
        }
        if (projectile.ai[0] >= 45f)
        {
            projectile.Kill();
            return;
        }
    }
    public static void AI_087(Projectile projectile)
    {
        projectile.position.Y = projectile.ai[0];
        projectile.height = (int)projectile.ai[1];
        if (projectile.Center.X > Main.player[projectile.owner].Center.X)
        {
            projectile.direction = 1;
        }
        else
        {
            projectile.direction = -1;
        }
        projectile.velocity.X = (float)projectile.direction * 1E-06f;
        if (projectile.owner == Main.myPlayer)
        {
            for (int num745 = 0; num745 < 1000; num745++)
            {
                if (Main.projectile[num745].active && num745 != projectile.whoAmI && Main.projectile[num745].type == projectile.type && Main.projectile[num745].owner == projectile.owner && Main.projectile[num745].timeLeft > projectile.timeLeft)
                {
                    projectile.Kill();
                    return;
                }
            }
        }
    }
    public static void AI_088(Projectile projectile)
    {
        if (projectile.type == 465)
        {
            if (projectile.localAI[1] == 0f)
            {
                //SoundEngine.PlaySound(SoundID.Item121, projectile.position);
                projectile.localAI[1] = 1f;
            }
            if (projectile.ai[0] < 180f)
            {
                projectile.alpha -= 5;
                if (projectile.alpha < 0)
                {
                    projectile.alpha = 0;
                }
            }
            else
            {
                projectile.alpha += 5;
                if (projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                    projectile.Kill();
                    return;
                }
            }
            projectile.ai[0]++;
            if (projectile.ai[0] % 30f == 0f && projectile.ai[0] < 180f && Main.netMode != 1)
            {
                int[] array3 = new int[5];
                Vector2[] array4 = new Vector2[5];
                int num749 = 0;
                float num750 = 2000f;
                for (int num751 = 0; num751 < 255; num751++)
                {
                    if (!Main.player[num751].active || Main.player[num751].dead)
                    {
                        continue;
                    }
                    Vector2 center14 = Main.player[num751].Center;
                    float num752 = Vector2.Distance(center14, projectile.Center);
                    if (num752 < num750 && Collision.CanHit(projectile.Center, 1, 1, center14, 1, 1))
                    {
                        array3[num749] = num751;
                        array4[num749] = center14;
                        int num38 = num749 + 1;
                        num749 = num38;
                        if (num38 >= array4.Length)
                        {
                            break;
                        }
                    }
                }
                for (int num753 = 0; num753 < num749; num753++)
                {
                    Vector2 vector85 = array4[num753] - projectile.Center;
                    float ai = Main.rand.Next(100);
                    Vector2 vector86 = Vector2.Normalize(vector85.RotatedByRandom(0.7853981852531433)) * 7f;
                    Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X, projectile.Center.Y, vector86.X, vector86.Y, 466, projectile.damage, 0f, Main.myPlayer, vector85.ToRotation(), ai);
                }
            }
            Lighting.AddLight(projectile.Center, 0.4f, 0.85f, 0.9f);
            if (++projectile.frameCounter >= 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
            if (projectile.alpha >= 150 || !(projectile.ai[0] < 180f))
            {
                return;
            }
        }
        else if (projectile.type == 466)
        {
            projectile.frameCounter++;
            Lighting.AddLight(projectile.Center, 0.3f, 0.45f, 0.5f);
            if (projectile.velocity == Vector2.Zero)
            {
                if (projectile.frameCounter >= projectile.extraUpdates * 2)
                {
                    projectile.frameCounter = 0;
                    bool flag35 = true;
                    for (int num760 = 1; num760 < projectile.oldPos.Length; num760++)
                    {
                        if (projectile.oldPos[num760] != projectile.oldPos[0])
                        {
                            flag35 = false;
                        }
                    }
                    if (flag35)
                    {
                        projectile.Kill();
                        return;
                    }
                }
            }
            else
            {
                if (projectile.frameCounter < projectile.extraUpdates * 2)
                {
                    return;
                }
                projectile.frameCounter = 0;
                float num766 = projectile.velocity.Length();
                UnifiedRandom unifiedRandom = new UnifiedRandom((int)projectile.ai[1]);
                int num767 = 0;
                Vector2 spinningpoint15 = -Vector2.UnitY;
                while (true)
                {
                    int num768 = unifiedRandom.Next();
                    projectile.ai[1] = num768;
                    num768 %= 100;
                    float f = (float)num768 / 100f * ((float)Math.PI * 2f);
                    Vector2 vector91 = f.ToRotationVector2();
                    if (vector91.Y > 0f)
                    {
                        vector91.Y *= -1f;
                    }
                    bool flag36 = false;
                    if (vector91.Y > -0.02f)
                    {
                        flag36 = true;
                    }
                    if (vector91.X * (float)(projectile.extraUpdates + 1) * 2f * num766 + projectile.localAI[0] > 40f)
                    {
                        flag36 = true;
                    }
                    if (vector91.X * (float)(projectile.extraUpdates + 1) * 2f * num766 + projectile.localAI[0] < -40f)
                    {
                        flag36 = true;
                    }
                    if (flag36)
                    {
                        if (num767++ >= 100)
                        {
                            projectile.velocity = Vector2.Zero;
                            projectile.localAI[1] = 1f;
                            break;
                        }
                        continue;
                    }
                    spinningpoint15 = vector91;
                    break;
                }
                if (projectile.velocity != Vector2.Zero)
                {
                    projectile.localAI[0] += spinningpoint15.X * (float)(projectile.extraUpdates + 1) * 2f * num766;
                    projectile.velocity = spinningpoint15.RotatedBy(projectile.ai[0] + (float)Math.PI / 2f) * num766;
                    projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                }
            }
        }
        else
        {
            if (projectile.type != 580)
            {
                return;
            }
            if (projectile.localAI[1] == 0f && projectile.ai[0] >= 900f)
            {
                projectile.ai[0] -= 1000f;
                projectile.localAI[1] = -1f;
            }
            projectile.frameCounter++;
            Lighting.AddLight(projectile.Center, 0.3f, 0.45f, 0.5f);
            if (projectile.velocity == Vector2.Zero)
            {
                if (projectile.frameCounter >= projectile.extraUpdates * 2)
                {
                    projectile.frameCounter = 0;
                    bool flag37 = true;
                    for (int num769 = 1; num769 < projectile.oldPos.Length; num769++)
                    {
                        if (projectile.oldPos[num769] != projectile.oldPos[0])
                        {
                            flag37 = false;
                        }
                    }
                    if (flag37)
                    {
                        projectile.Kill();
                        return;
                    }
                }
            }
            else
            {
                if (projectile.frameCounter < projectile.extraUpdates * 2)
                {
                    return;
                }
                projectile.frameCounter = 0;
                float num775 = projectile.velocity.Length();
                UnifiedRandom unifiedRandom2 = new UnifiedRandom((int)projectile.ai[1]);
                int num776 = 0;
                Vector2 spinningpoint16 = -Vector2.UnitY;
                while (true)
                {
                    int num777 = unifiedRandom2.Next();
                    projectile.ai[1] = num777;
                    num777 %= 100;
                    float f2 = (float)num777 / 100f * ((float)Math.PI * 2f);
                    Vector2 vector94 = f2.ToRotationVector2();
                    if (vector94.Y > 0f)
                    {
                        vector94.Y *= -1f;
                    }
                    bool flag38 = false;
                    if (vector94.Y > -0.02f)
                    {
                        flag38 = true;
                    }
                    if (vector94.X * (float)(projectile.extraUpdates + 1) * 2f * num775 + projectile.localAI[0] > 40f)
                    {
                        flag38 = true;
                    }
                    if (vector94.X * (float)(projectile.extraUpdates + 1) * 2f * num775 + projectile.localAI[0] < -40f)
                    {
                        flag38 = true;
                    }
                    if (flag38)
                    {
                        if (num776++ >= 100)
                        {
                            projectile.velocity = Vector2.Zero;
                            if (projectile.localAI[1] < 1f)
                            {
                                projectile.localAI[1] += 2f;
                            }
                            break;
                        }
                        continue;
                    }
                    spinningpoint16 = vector94;
                    break;
                }
                if (!(projectile.velocity != Vector2.Zero))
                {
                    return;
                }
                projectile.localAI[0] += spinningpoint16.X * (float)(projectile.extraUpdates + 1) * 2f * num775;
                projectile.velocity = spinningpoint16.RotatedBy(projectile.ai[0] + (float)Math.PI / 2f) * num775;
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                if (Main.rand.Next(4) == 0 && Main.netMode != 1 && projectile.localAI[1] == 0f)
                {
                    float num778 = (float)Main.rand.Next(-3, 4) * ((float)Math.PI / 3f) / 3f;
                    Vector2 vector95 = projectile.ai[0].ToRotationVector2().RotatedBy(num778) * projectile.velocity.Length();
                    if (!Collision.CanHitLine(projectile.Center, 0, 0, projectile.Center + vector95 * 50f, 0, 0))
                    {
                        Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X - vector95.X, projectile.Center.Y - vector95.Y, vector95.X, vector95.Y, projectile.type, projectile.damage, projectile.knockBack, projectile.owner, vector95.ToRotation() + 1000f, projectile.ai[1]);
                    }
                }
            }
        }
    }
    public static void AI_089(Projectile projectile)
    {
        if (projectile.ai[1] == -1f)
        {
            projectile.alpha += 12;
        }
        else if (projectile.ai[0] < 300f)
        {
            projectile.alpha -= 5;
        }
        else
        {
            projectile.alpha += 12;
        }
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        if (projectile.alpha > 255)
        {
            projectile.alpha = 255;
        }
        projectile.scale = 1f - (float)projectile.alpha / 255f;
        projectile.scale *= 0.6f;
        projectile.rotation += (float)Math.PI / 210f;
        if (projectile.localAI[1] == 0f)
        {
            projectile.localAI[1] = 1f;
            //SoundEngine.PlaySound(SoundID.Item123, projectile.position);
        }
        projectile.ai[0]++;
        if (projectile.ai[0] == 300f && projectile.ai[1] != -1f && Main.netMode != 1)
        {
            if (!NPC.AnyNPCs(454))
            {
                projectile.ai[1] = NPC.NewNPC(projectile.GetNPCSource_FromThis(), (int)projectile.Center.X, (int)projectile.Center.Y, 454);
            }
            else
            {
                projectile.ai[1] = NPC.NewNPC(projectile.GetNPCSource_FromThis(), (int)projectile.Center.X, (int)projectile.Center.Y, 521);
            }
        }
        else if (projectile.ai[0] == 320f)
        {
            projectile.Kill();
            return;
        }
        bool flag39 = false;
        if (projectile.ai[1] == -1f)
        {
            if (projectile.alpha == 255)
            {
                flag39 = true;
            }
        }
        else
        {
            flag39 = !(projectile.ai[1] >= 0f) || !Main.npc[(int)projectile.ai[1]].active;
            if ((flag39 || Main.npc[(int)projectile.ai[1]].type != 439) && (flag39 || Main.npc[(int)projectile.ai[1]].type != 454) && (flag39 || Main.npc[(int)projectile.ai[1]].type != 521))
            {
                flag39 = true;
            }
        }
        if (flag39)
        {
            projectile.Kill();
        }
        else
        {
            Lighting.AddLight(projectile.Center, 1.1f, 0.9f, 0.4f);
        }
    }
    public static void AI_090(Projectile projectile)
    {
        if (Main.player[projectile.owner].dead)
        {
            projectile.Kill();
        }
        if (Main.player[projectile.owner].magicLantern)
        {
            projectile.timeLeft = 2;
        }
        if (projectile.tileCollide)
        {
            if (!Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.player[projectile.owner].Center, 1, 1))
            {
                projectile.tileCollide = false;
            }
            else if (!Collision.SolidCollision(projectile.position, projectile.width, projectile.height) && Collision.CanHitLine(projectile.position, projectile.width, projectile.height, Main.player[projectile.owner].Center, 1, 1))
            {
                projectile.tileCollide = true;
            }
        }
        projectile.direction = Main.player[projectile.owner].direction;
        projectile.spriteDirection = projectile.direction;
        Lighting.AddLight(projectile.position, 0.35f, 0.35f, 0.1f);
        projectile.localAI[0] += 1f;
        if (projectile.localAI[0] >= 10f)
        {
            projectile.localAI[0] = 0f;
        }
        Vector2 vector98 = Main.player[projectile.owner].Center - projectile.Center;
        vector98.X += 40 * projectile.direction;
        vector98.Y -= 40f;
        float num794 = vector98.Length();
        if (num794 > 1000f)
        {
            projectile.Center = Main.player[projectile.owner].Center;
        }
        float num795 = 3f;
        float num796 = 4f;
        if (num794 > 200f)
        {
            num796 += (num794 - 200f) * 0.1f;
            projectile.tileCollide = false;
        }
        if (num794 < num796)
        {
            projectile.velocity *= 0.25f;
            num796 = num794;
        }
        if (vector98.X != 0f || vector98.Y != 0f)
        {
            vector98.Normalize();
            vector98 *= num796;
        }
        projectile.velocity = (projectile.velocity * (num795 - 1f) + vector98) / num795;
        if (projectile.velocity.Length() > 6f)
        {
            float num797 = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            if ((double)Math.Abs(projectile.rotation - num797) >= 3.14)
            {
                if (num797 < projectile.rotation)
                {
                    projectile.rotation -= 6.28f;
                }
                else
                {
                    projectile.rotation += 6.28f;
                }
            }
            projectile.rotation = (projectile.rotation * 4f + num797) / 5f;
            projectile.frameCounter++;
            if (projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame > 7)
                {
                    projectile.frame = 4;
                }
            }
            if (projectile.frame < 4)
            {
                projectile.frame = 7;
            }
            return;
        }
        if ((double)projectile.rotation > 3.14)
        {
            projectile.rotation -= 6.28f;
        }
        if ((double)projectile.rotation > -0.01 && (double)projectile.rotation < 0.01)
        {
            projectile.rotation = 0f;
        }
        else
        {
            projectile.rotation *= 0.9f;
        }
        projectile.frameCounter++;
        if (projectile.frameCounter > 6)
        {
            projectile.frameCounter = 0;
            projectile.frame++;
            if (projectile.frame > 3)
            {
                projectile.frame = 0;
            }
        }
    }
    public static void AI_091(Projectile projectile)
    {
        Vector2 center15 = projectile.Center;
        projectile.scale = 1f - projectile.localAI[0];
        projectile.width = (int)(20f * projectile.scale);
        projectile.height = projectile.width;
        projectile.position.X = center15.X - (float)(projectile.width / 2);
        projectile.position.Y = center15.Y - (float)(projectile.height / 2);
        if ((double)projectile.localAI[0] < 0.1)
        {
            projectile.localAI[0] += 0.01f;
        }
        else
        {
            projectile.localAI[0] += 0.025f;
        }
        if (projectile.localAI[0] >= 0.95f)
        {
            projectile.Kill();
        }
        projectile.velocity.X += projectile.ai[0] * 1.5f;
        projectile.velocity.Y += projectile.ai[1] * 1.5f;
        if (projectile.velocity.Length() > 16f)
        {
            projectile.velocity.Normalize();
            projectile.velocity *= 16f;
        }
        projectile.ai[0] *= 1.05f;
        projectile.ai[1] *= 1.05f;
    }
    public static void AI_092(Projectile projectile)
    {
        bool flag40 = projectile.type == 1007;
        bool flag41 = projectile.type >= 511 && projectile.type <= 513;
        projectile.tileCollide = false;
        if (Main.netMode != 1 && flag40 && projectile.localAI[0] == 0f)
        {
            if (projectile.direction == 0)
            {
                projectile.direction = 1;
            }
            ParticleOrchestrator.BroadcastParticleSpawn(ParticleOrchestraType.GasTrap, new ParticleOrchestraSettings
            {
                PositionInWorld = projectile.Center,
                MovementVector = Vector2.Zero
            });
        }
        projectile.ai[0] += 1f;
        if (projectile.ai[1] >= 1f)
        {
            projectile.ai[0] += 2f;
        }
        float num800 = 260f;
        if (flag40)
        {
            num800 = 80f;
        }
        if (projectile.ai[0] > num800)
        {
            projectile.Kill();
            projectile.ai[0] = num800;
            return;
        }
        float fromValue = projectile.ai[0] / num800;
        if (flag40)
        {
            projectile.scale = Utils.Remap(fromValue, 0f, 0.95f, 1f, 6f);
            Vector2 center16 = projectile.Center;
            projectile.width = (int)(50f * projectile.scale);
            projectile.height = (int)(50f * projectile.scale);
            projectile.Center = center16;
            projectile.Opacity = MathHelper.Clamp(Utils.Remap(fromValue, 0f, 0.25f, 0f, 1f) * Utils.Remap(fromValue, 0.75f, 1f, 1f, 0f), 0f, 1f) * 0.85f;
        }
        else
        {
            projectile.Opacity = Utils.Remap(fromValue, 0f, 0.3f, 0f, 1f) * Utils.Remap(fromValue, 0.3f, 1f, 1f, 0f) * 0.7f;
        }
        projectile.localAI[0] += projectile.direction;
        projectile.rotation = (float)projectile.whoAmI * 0.4002029f + projectile.localAI[0] * ((float)Math.PI * 2f) / 480f;
        if (flag40)
        {
            projectile.velocity = Vector2.Zero;
        }
        else
        {
            projectile.velocity *= 0.96f;
        }
        if (flag41)
        {
            Rectangle rectangle5 = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
            for (int num801 = 0; num801 < 1000; num801++)
            {
                if (num801 == projectile.whoAmI)
                {
                    continue;
                }
                Projectile projectile3 = Main.projectile[num801];
                if (!projectile3.active || projectile3.type < 511 || projectile3.type > 513)
                {
                    continue;
                }
                Rectangle value11 = new Rectangle((int)projectile3.position.X, (int)projectile3.position.Y, projectile3.width, projectile3.height);
                if (!rectangle5.Intersects(value11))
                {
                    continue;
                }
                Vector2 vector99 = projectile3.Center - projectile.Center;
                if (vector99 == Vector2.Zero)
                {
                    if (num801 < projectile.whoAmI)
                    {
                        vector99.X = -1f;
                        vector99.Y = 1f;
                    }
                    else
                    {
                        vector99.X = 1f;
                        vector99.Y = -1f;
                    }
                }
                Vector2 vector100 = vector99.SafeNormalize(Vector2.UnitX) * 0.005f;
                projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.velocity - vector100, 0.6f);
                projectile3.velocity = Vector2.Lerp(projectile3.velocity, projectile3.velocity + vector100, 0.6f);
            }
        }
        Vector2 vector101 = projectile.velocity.SafeNormalize(Vector2.Zero);
        Vector2 pos = projectile.Center + vector101 * 16f;
        if (!flag40 && Collision.IsWorldPointSolid(pos, treatPlatformsAsNonSolid: true))
        {
            projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.velocity - vector101 * 1f, 0.5f);
        }
        if (!flag40)
        {
            return;
        }
        int num802 = 20;
        int num803 = 2940;
        float num804 = MathHelper.Clamp(Utils.Remap(fromValue, 0f, 0.2f, 0f, 1f), 0f, 1f) * 180f;
        if (projectile.localAI[1] > 0f)
        {
            projectile.localAI[1]--;
        }
        if (!(projectile.localAI[1] <= 0f))
        {
            return;
        }
        projectile.localAI[1] = 15f;
        if (Main.netMode != 2)
        {
            Player localPlayer = Main.LocalPlayer;
            if (localPlayer.active && !localPlayer.DeadOrGhost && localPlayer.Center.Distance(projectile.Center) <= num804)
            {
                localPlayer.AddBuff(num802, num803);
            }
        }
        if (Main.netMode == 1)
        {
            return;
        }
        for (int num805 = 0; num805 < 200; num805++)
        {
            NPC nPC12 = Main.npc[num805];
            if (nPC12.active && !nPC12.buffImmune[num802] && nPC12.Center.Distance(projectile.Center) <= num804)
            {
                nPC12.AddBuff(num802, num803);
            }
        }
    }
    public static void AI_093(Projectile projectile)
    {
        if (projectile.alpha > 0)
        {
            projectile.alpha -= 25;
            if (projectile.alpha <= 0)
            {
                projectile.alpha = 0;
            }
        }
        if (projectile.velocity.Y > 18f)
        {
            projectile.velocity.Y = 18f;
        }
        if (projectile.ai[0] == 0f)
        {
            projectile.ai[1] += 1f;
            if (projectile.ai[1] > 20f)
            {
                projectile.velocity.Y += 0.1f;
                projectile.velocity.X *= 0.992f;
            }
            projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
            return;
        }
        projectile.tileCollide = false;
        if (projectile.ai[0] == 1f)
        {
            projectile.tileCollide = false;
            projectile.velocity *= 0.6f;
        }
        else
        {
            projectile.tileCollide = false;
            int num806 = (int)(0f - projectile.ai[0]);
            num806--;
            projectile.position = Main.npc[num806].Center - projectile.velocity;
            projectile.position.X -= projectile.width / 2;
            projectile.position.Y -= projectile.height / 2;
            if (!Main.npc[num806].active || Main.npc[num806].life < 0)
            {
                projectile.tileCollide = true;
                projectile.ai[0] = 0f;
                projectile.ai[1] = 20f;
                projectile.velocity = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
                projectile.velocity.Normalize();
                projectile.velocity *= 6f;
                projectile.netUpdate = true;
            }
            else if (projectile.velocity.Length() > (float)((Main.npc[num806].width + Main.npc[num806].height) / 3))
            {
                projectile.velocity *= 0.99f;
            }
        }
        if (projectile.ai[0] != 0f)
        {
            projectile.ai[1] += 1f;
            if (projectile.ai[1] > 90f)
            {
                projectile.Kill();
            }
        }
    }
    public static void AI_094(Projectile projectile)
    {
        if (++projectile.frameCounter >= 4)
        {
            projectile.frameCounter = 0;
            if (++projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }
        projectile.ai[0]++;
        if (projectile.ai[0] <= 40f)
        {
            projectile.alpha -= 5;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            projectile.velocity *= 0.85f;
            if (projectile.ai[0] == 40f)
            {
                projectile.netUpdate = true;
                switch (Main.rand.Next(3))
                {
                    case 0:
                        projectile.ai[1] = 10f;
                        break;
                    case 1:
                        projectile.ai[1] = 15f;
                        break;
                    case 2:
                        projectile.ai[1] = 30f;
                        break;
                }
            }
        }
        else if (projectile.ai[0] <= 60f)
        {
            projectile.velocity = Vector2.Zero;
            if (projectile.ai[0] == 60f)
            {
                projectile.netUpdate = true;
            }
        }
        else if (projectile.ai[0] <= 210f)
        {
            if (Main.netMode != 1 && (projectile.localAI[0] += 1f) >= projectile.ai[1])
            {
                projectile.localAI[0] = 0f;
                int num807 = Item.NewItem(projectile.GetItemSource_FromThis(), (int)projectile.Center.X, (int)projectile.Center.Y, 0, 0, 73);
                Main.item[num807].velocity = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * new Vector2(3f, 2f) * (Main.rand.NextFloat() * 0.5f + 0.5f) - Vector2.UnitY * 1f;
            }
            if (projectile.ai[0] == 210f)
            {
                projectile.netUpdate = true;
            }
        }
        else
        {
            projectile.scale -= 1f / 30f;
            projectile.alpha += 15;
            if (projectile.ai[0] == 239f)
            {
                projectile.netUpdate = true;
            }
            if (projectile.ai[0] == 240f)
            {
                projectile.Kill();
            }
        }
        float num809 = 0.8f;
        float num810 = 0.70980394f;
        float num811 = 24f / 85f;
        Lighting.AddLight(projectile.Center, num809 * 0.3f, num810 * 0.3f, num811 * 0.3f);
    }
    public static void AI_095(Projectile projectile)
    {
        if (projectile.localAI[0] > 2f)
        {
            projectile.alpha -= 20;
            if (projectile.alpha < 100)
            {
                projectile.alpha = 100;
            }
        }
        else
        {
            projectile.localAI[0] += 1f;
        }
        if (projectile.ai[0] > 30f)
        {
            if (projectile.velocity.Y > -8f)
            {
                projectile.velocity.Y -= 0.05f;
            }
            projectile.velocity.X *= 0.98f;
        }
        else
        {
            projectile.ai[0] += 1f;
        }
        projectile.rotation = projectile.velocity.X * 0.1f;
        if (projectile.wet)
        {
            if (projectile.velocity.Y > 0f)
            {
                projectile.velocity.Y *= 0.98f;
            }
            if (projectile.velocity.Y > -8f)
            {
                projectile.velocity.Y -= 0.2f;
            }
            projectile.velocity.X *= 0.94f;
        }
    }
    public static void AI_096(Projectile projectile)
    {
        projectile.ai[0] += 0.6f;
        if (projectile.ai[0] > 500f)
        {
            projectile.Kill();
        }
        projectile.velocity.Y += 0.008f;
    }
    public static void AI_097(Projectile projectile)
    {
        projectile.frameCounter++;
        float num815 = 4f;
        if ((float)projectile.frameCounter < num815 * 1f)
        {
            projectile.frame = 0;
        }
        else if ((float)projectile.frameCounter < num815 * 2f)
        {
            projectile.frame = 1;
        }
        else if ((float)projectile.frameCounter < num815 * 3f)
        {
            projectile.frame = 2;
        }
        else if ((float)projectile.frameCounter < num815 * 4f)
        {
            projectile.frame = 3;
        }
        else if ((float)projectile.frameCounter < num815 * 5f)
        {
            projectile.frame = 4;
        }
        else if ((float)projectile.frameCounter < num815 * 6f)
        {
            projectile.frame = 3;
        }
        else if ((float)projectile.frameCounter < num815 * 7f)
        {
            projectile.frame = 2;
        }
        else if ((float)projectile.frameCounter < num815 * 8f)
        {
            projectile.frame = 1;
        }
        else
        {
            projectile.frameCounter = 0;
            projectile.frame = 0;
        }
        Main.CurrentFrameFlags.HadAnActiveInteractibleProjectile = true;
        if (projectile.owner == Main.myPlayer)
        {
            for (int num816 = 0; num816 < 1000; num816++)
            {
                if (num816 != projectile.whoAmI && Main.projectile[num816].active && Main.projectile[num816].owner == projectile.owner && Main.projectile[num816].type == projectile.type)
                {
                    if (projectile.timeLeft >= Main.projectile[num816].timeLeft)
                    {
                        Main.projectile[num816].Kill();
                    }
                    else
                    {
                        projectile.Kill();
                    }
                }
            }
        }
        if (projectile.ai[0] == 0f)
        {
            if ((double)projectile.velocity.Length() < 0.1)
            {
                projectile.velocity.X = 0f;
                projectile.velocity.Y = 0f;
                projectile.ai[0] = 1f;
                projectile.ai[1] = 45f;
                return;
            }
            projectile.velocity *= 0.94f;
            if (projectile.velocity.X < 0f)
            {
                projectile.direction = -1;
            }
            else
            {
                projectile.direction = 1;
            }
            projectile.spriteDirection = projectile.direction;
            return;
        }
        if (Main.player[projectile.owner].Center.X < projectile.Center.X)
        {
            projectile.direction = -1;
        }
        else
        {
            projectile.direction = 1;
        }
        projectile.spriteDirection = projectile.direction;
        projectile.ai[1] += 1f;
        float num817 = 0.005f;
        if (projectile.ai[1] > 0f)
        {
            projectile.velocity.Y -= num817;
        }
        else
        {
            projectile.velocity.Y += num817;
        }
        if (projectile.ai[1] >= 90f)
        {
            projectile.ai[1] *= -1f;
        }
    }
    public static void AI_098(Projectile projectile)
    {
        Vector2 vector104 = new Vector2(projectile.ai[0], projectile.ai[1]);
        Vector2 value12 = vector104 - projectile.Center;
        if (value12.Length() < projectile.velocity.Length())
        {
            projectile.Kill();
            return;
        }
        value12.Normalize();
        value12 *= 15f;
        projectile.velocity = Vector2.Lerp(projectile.velocity, value12, 0.1f);
    }
    public static void AI_099(Projectile projectile)
    {
        if (projectile.type >= 556 && projectile.type <= 561)
        {
            projectile.AI_099_1();
        }
        else
        {
            projectile.AI_099_2();
        }
    }
    public static void AI_101(Projectile projectile)
    {
        float num820 = 20f;
        projectile.localAI[0]++;
        projectile.alpha = (int)MathHelper.Lerp(0f, 255f, projectile.localAI[0] / num820);
        int num821 = (int)projectile.ai[0];
        int num822 = -1;
        int num823 = -1;
        switch (projectile.type)
        {
            case 536:
                num822 = 535;
                num823 = 0;
                break;
            case 591:
                num823 = 1;
                break;
        }
        switch (num823)
        {
            case 1:
                if (projectile.localAI[0] >= num820 || num821 < 0 || num821 > 255 || !Main.player[num821].active || Main.player[num821].dead)
                {
                    projectile.Kill();
                    return;
                }
                if (projectile.type == 591)
                {
                    projectile.position -= projectile.velocity;
                    projectile.position += Main.player[projectile.owner].position - Main.player[projectile.owner].oldPosition;
                    projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                    if (Math.Sign(projectile.velocity.X) != Math.Sign(Main.player[num821].velocity.X) && Main.player[num821].velocity.X != 0f)
                    {
                        projectile.Kill();
                        return;
                    }
                }
                else
                {
                    projectile.Center = Main.player[num821].Center - projectile.velocity;
                }
                break;
            case 0:
                if (projectile.localAI[0] >= num820 || num821 < 0 || num821 > 1000 || !Main.projectile[num821].active || Main.projectile[num821].type != num822)
                {
                    projectile.Kill();
                    return;
                }
                projectile.Center = Main.projectile[num821].Center - projectile.velocity;
                break;
        }
        projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
    }
    public static void AI_102(Projectile projectile)
    {
        int num824 = 0;
        float num825 = 0f;
        float x11 = 0f;
        float y9 = 0f;
        int num826 = -1;
        int num827 = 0;
        float num828 = 0f;
        bool flag42 = true;
        bool flag43 = false;
        bool flag44 = false;
        switch (projectile.type)
        {
            case 539:
                num824 = 407;
                num825 = 210f;
                x11 = 0.15f;
                y9 = 0.075f;
                num828 = 16f;
                break;
            case 573:
                num824 = 424;
                num825 = 90f;
                num828 = 20f;
                flag42 = false;
                flag43 = true;
                break;
            case 574:
                num824 = 420;
                num825 = 180f;
                x11 = 0.15f;
                y9 = 0.075f;
                num828 = 8f;
                flag42 = false;
                num826 = 576;
                num827 = 65;
                if (Main.expertMode)
                {
                    num827 = 50;
                }
                flag44 = true;
                break;
        }
        if (flag44)
        {
            int num829 = (int)projectile.ai[1];
            if (!Main.npc[num829].active || Main.npc[num829].type != num824)
            {
                projectile.Kill();
                return;
            }
            projectile.timeLeft = 2;
        }
        projectile.ai[0]++;
        if (projectile.ai[0] < num825)
        {
            bool flag45 = true;
            int num830 = (int)projectile.ai[1];
            if (Main.npc[num830].active && Main.npc[num830].type == num824)
            {
                if (!flag43 && Main.npc[num830].oldPos[1] != Vector2.Zero)
                {
                    projectile.position += Main.npc[num830].position - Main.npc[num830].oldPos[1];
                }
            }
            else
            {
                projectile.ai[0] = num825;
                flag45 = false;
            }
            if (flag45 && !flag43)
            {
                projectile.velocity += new Vector2(Math.Sign(Main.npc[num830].Center.X - projectile.Center.X), Math.Sign(Main.npc[num830].Center.Y - projectile.Center.Y)) * new Vector2(x11, y9);
                if (projectile.velocity.Length() > 6f)
                {
                    projectile.velocity *= 6f / projectile.velocity.Length();
                }
            }
            if (projectile.type == 539)
            {
                if (++projectile.frameCounter >= 4)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= Main.projFrames[projectile.type])
                    {
                        projectile.frame = 0;
                    }
                }
                projectile.rotation = projectile.velocity.X * 0.1f;
            }
            if (projectile.type == 573)
            {
                projectile.alpha = 255;
            }
            if (projectile.type == 574)
            {
                if (flag45)
                {
                    int target2 = Main.npc[num830].target;
                    float num834 = projectile.velocity.ToRotation();
                    if (Collision.CanHitLine(projectile.Center, 0, 0, Main.player[target2].Center, 0, 0))
                    {
                        num834 = projectile.DirectionTo(Main.player[target2].Center).ToRotation();
                    }
                    projectile.rotation = projectile.rotation.AngleLerp(num834 + (float)Math.PI / 2f, 0.2f);
                }
                projectile.frame = 1;
            }
        }
        if (projectile.ai[0] == num825)
        {
            bool flag46 = true;
            int num835 = -1;
            if (!flag42)
            {
                int num836 = (int)projectile.ai[1];
                if (Main.npc[num836].active && Main.npc[num836].type == num824)
                {
                    num835 = Main.npc[num836].target;
                }
                else
                {
                    flag46 = false;
                }
            }
            else
            {
                flag46 = false;
            }
            if (!flag46)
            {
                num835 = Player.FindClosest(projectile.position, projectile.width, projectile.height);
            }
            Vector2 value13 = Main.player[num835].Center - projectile.Center;
            value13.X += Main.rand.Next(-50, 51);
            value13.Y += Main.rand.Next(-50, 51);
            value13.X *= (float)Main.rand.Next(80, 121) * 0.01f;
            value13.Y *= (float)Main.rand.Next(80, 121) * 0.01f;
            Vector2 vector105 = Vector2.Normalize(value13);
            if (vector105.HasNaNs())
            {
                vector105 = Vector2.UnitY;
            }
            if (num826 == -1)
            {
                projectile.velocity = vector105 * num828;
                projectile.netUpdate = true;
            }
            else
            {
                if (Main.netMode != 1 && Collision.CanHitLine(projectile.Center, 0, 0, Main.player[num835].Center, 0, 0))
                {
                    Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X, projectile.Center.Y, vector105.X * num828, vector105.Y * num828, num826, num827, 1f, Main.myPlayer);
                }
                projectile.ai[0] = 0f;
            }
        }
        if (!(projectile.ai[0] >= num825))
        {
            return;
        }
        projectile.rotation = projectile.rotation.AngleLerp(projectile.velocity.ToRotation() + (float)Math.PI / 2f, 0.4f);
        if (projectile.type == 539)
        {
            if (++projectile.frameCounter >= 2)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }
        if (projectile.type == 573)
        {
            projectile.alpha = 0;
        }
    }
    public static void AI_103(Projectile projectile)
    {
        projectile.scale = projectile.ai[1];
        projectile.ai[0]++;
        if (projectile.ai[0] >= 30f)
        {
            projectile.alpha += 25;
            if (projectile.alpha >= 250)
            {
                projectile.Kill();
            }
        }
        else
        {
            if (!(projectile.ai[0] >= 0f))
            {
                return;
            }
            projectile.alpha -= 25;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
                if (projectile.localAI[1] == 0f && Main.netMode != 1 && projectile.localAI[0] != 0f)
                {
                    projectile.localAI[1] = 1f;
                    NPC.NewNPC(projectile.GetNPCSource_FromThis(), (int)projectile.Center.X, (int)projectile.Bottom.Y, (int)projectile.localAI[0]);
                }
            }
        }
    }
    public static void AI_104(Projectile projectile)
    {
        if (projectile.ai[0] == 1f)
        {
            projectile.scale *= 0.995f;
            projectile.alpha += 3;
            if (projectile.alpha >= 250)
            {
                projectile.Kill();
            }
        }
        else
        {
            projectile.scale *= 1.01f;
            projectile.alpha -= 7;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
                projectile.ai[0] = 1f;
            }
        }
        projectile.frameCounter++;
        if (projectile.frameCounter > 6)
        {
            projectile.frameCounter = 0;
            projectile.frame++;
            if (projectile.frame > 3)
            {
                projectile.frame = 0;
            }
        }
        projectile.velocity.Y -= 0.03f;
        projectile.velocity.X *= 0.97f;
    }
    public static void AI_105(Projectile projectile)
    {
        float num840 = 1f - (float)projectile.alpha / 255f;
        num840 *= projectile.scale;
        Lighting.AddLight(projectile.Center, 0.2f * num840, 0.275f * num840, 0.075f * num840);
        projectile.localAI[0] += 1f;
        if (projectile.localAI[0] >= 90f)
        {
            projectile.localAI[0] *= -1f;
        }
        if (projectile.localAI[0] >= 0f)
        {
            projectile.scale += 0.003f;
        }
        else
        {
            projectile.scale -= 0.003f;
        }
        projectile.rotation += 0.0025f * projectile.scale;
        float num841 = 1f;
        float num842 = 1f;
        if (projectile.identity % 6 == 0)
        {
            num842 *= -1f;
        }
        if (projectile.identity % 6 == 1)
        {
            num841 *= -1f;
        }
        if (projectile.identity % 6 == 2)
        {
            num842 *= -1f;
            num841 *= -1f;
        }
        if (projectile.identity % 6 == 3)
        {
            num842 = 0f;
        }
        if (projectile.identity % 6 == 4)
        {
            num841 = 0f;
        }
        projectile.localAI[1] += 1f;
        if (projectile.localAI[1] > 60f)
        {
            projectile.localAI[1] = -180f;
        }
        if (projectile.localAI[1] >= -60f)
        {
            projectile.velocity.X += 0.002f * num842;
            projectile.velocity.Y += 0.002f * num841;
        }
        else
        {
            projectile.velocity.X -= 0.002f * num842;
            projectile.velocity.Y -= 0.002f * num841;
        }
        projectile.ai[0] += 1f;
        if (projectile.ai[0] > 5400f)
        {
            projectile.damage = 0;
            projectile.ai[1] = 1f;
            if (projectile.alpha < 255)
            {
                projectile.alpha += 5;
                if (projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                }
            }
            else if (projectile.owner == Main.myPlayer)
            {
                projectile.Kill();
            }
        }
        else
        {
            float num843 = (projectile.Center - Main.player[projectile.owner].Center).Length() / 100f;
            if (num843 > 4f)
            {
                num843 *= 1.1f;
            }
            if (num843 > 5f)
            {
                num843 *= 1.2f;
            }
            if (num843 > 6f)
            {
                num843 *= 1.3f;
            }
            if (num843 > 7f)
            {
                num843 *= 1.4f;
            }
            if (num843 > 8f)
            {
                num843 *= 1.5f;
            }
            if (num843 > 9f)
            {
                num843 *= 1.6f;
            }
            if (num843 > 10f)
            {
                num843 *= 1.7f;
            }
            if (!Main.player[projectile.owner].sporeSac)
            {
                num843 += 100f;
            }
            projectile.ai[0] += num843;
            if (projectile.alpha > 50)
            {
                projectile.alpha -= 10;
                if (projectile.alpha < 50)
                {
                    projectile.alpha = 50;
                }
            }
        }
        bool flag47 = false;
        Vector2 vector106 = new Vector2(0f, 0f);
        float num844 = 340f;
        for (int num845 = 0; num845 < 200; num845++)
        {
            if (Main.npc[num845].CanBeChasedBy(projectile))
            {
                float num846 = Main.npc[num845].position.X + (float)(Main.npc[num845].width / 2);
                float num847 = Main.npc[num845].position.Y + (float)(Main.npc[num845].height / 2);
                float num848 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num846) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num847);
                if (num848 < num844)
                {
                    num844 = num848;
                    vector106 = Main.npc[num845].Center;
                    flag47 = true;
                }
            }
        }
        if (flag47)
        {
            Vector2 vector107 = vector106 - projectile.Center;
            vector107.Normalize();
            vector107 *= 4f;
            projectile.velocity = (projectile.velocity * 40f + vector107) / 41f;
        }
        else if ((double)projectile.velocity.Length() > 0.2)
        {
            projectile.velocity *= 0.98f;
        }
    }
    public static void AI_106(Projectile projectile)
    {
        projectile.rotation += projectile.velocity.X * 0.02f;
        if (projectile.velocity.X < 0f)
        {
            projectile.rotation -= Math.Abs(projectile.velocity.Y) * 0.02f;
        }
        else
        {
            projectile.rotation += Math.Abs(projectile.velocity.Y) * 0.02f;
        }
        projectile.velocity *= 0.98f;
        projectile.ai[0] += 1f;
        if (projectile.ai[0] >= 60f)
        {
            if (projectile.alpha < 255)
            {
                projectile.alpha += 5;
                if (projectile.alpha > 255)
                {
                    projectile.alpha = 255;
                }
            }
            else if (projectile.owner == Main.myPlayer)
            {
                projectile.Kill();
            }
        }
        else if (projectile.alpha > 80)
        {
            projectile.alpha -= 30;
            if (projectile.alpha < 80)
            {
                projectile.alpha = 80;
            }
        }
    }
    public static void AI_107(Projectile projectile)
    {
        float num849 = 10f;
        float num850 = 5f;
        float num851 = 40f;
        if (projectile.type == 575)
        {
            if (projectile.timeLeft > 30 && projectile.alpha > 0)
            {
                projectile.alpha -= 25;
            }
            if (projectile.timeLeft > 30 && projectile.alpha < 128 && Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.alpha = 128;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (++projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            Lighting.AddLight(projectile.Center, 0.5f, 0.1f, 0.3f);
        }
        else if (projectile.type == 596)
        {
            num849 = 10f;
            num850 = 7.5f;
            if (projectile.timeLeft > 30 && projectile.alpha > 0)
            {
                projectile.alpha -= 25;
            }
            if (projectile.timeLeft > 30 && projectile.alpha < 128 && Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.alpha = 128;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (++projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            projectile.ai[1]++;
            float num853 = projectile.ai[1] / 180f * ((float)Math.PI * 2f);
            for (float num854 = 0f; num854 < 3f; num854++)
            {
                if (Main.rand.Next(3) != 0)
                {
                    return;
                }
            }
            if (projectile.timeLeft < 4)
            {
                int num855 = 30;
                if (Main.expertMode)
                {
                    num855 = 22;
                }
                projectile.position = projectile.Center;
                projectile.width = (projectile.height = 60);
                projectile.Center = projectile.position;
                projectile.damage = num855;
            }
        }
        int num857 = (int)projectile.ai[0];
        if (num857 >= 0 && Main.player[num857].active && !Main.player[num857].dead)
        {
            if (projectile.Distance(Main.player[num857].Center) > num851)
            {
                Vector2 vector108 = projectile.DirectionTo(Main.player[num857].Center);
                if (vector108.HasNaNs())
                {
                    vector108 = Vector2.UnitY;
                }
                projectile.velocity = (projectile.velocity * (num849 - 1f) + vector108 * num850) / num849;
            }
        }
        else
        {
            if (projectile.timeLeft > 30)
            {
                projectile.timeLeft = 30;
            }
            if (projectile.ai[0] != -1f)
            {
                projectile.ai[0] = -1f;
                projectile.netUpdate = true;
            }
        }
    }
    public static void AI_108(Projectile projectile)
    {
        bool flag48 = projectile.type == 579 || projectile.type == 578;
        if (flag48 && projectile.ai[1] == 1f && Main.netMode != 2)
        {
            projectile.ai[1] = 0f;
        }
        if (flag48 && projectile.localAI[1] == 0f)
        {
            projectile.localAI[1] = 1f;
            SoundEngine.PlaySound(SoundID.Item117, projectile.position);
        }
        if (projectile.type == 578 && projectile.localAI[0] == 0f)
        {
            projectile.localAI[0] = 1f;
            int num860 = Player.FindClosest(projectile.Center, 0, 0);
            Vector2 vector109 = Main.player[num860].Center - projectile.Center;
            if (vector109 == Vector2.Zero)
            {
                vector109 = Vector2.UnitY;
            }
            projectile.ai[1] = vector109.ToRotation();
            projectile.netUpdate = true;
        }
        projectile.ai[0]++;
        if (projectile.ai[0] <= 90f)
        {
            projectile.scale = (projectile.ai[0] - 50f) / 40f;
            projectile.alpha = 255 - (int)(255f * projectile.scale);
            projectile.rotation -= (float)Math.PI / 20f;
            if (projectile.type == 578)
            {
                Vector2 vector120 = projectile.ai[1].ToRotationVector2();
                Vector2 vector121 = vector120.RotatedBy(1.5707963705062866) * (Main.rand.Next(2) == 0).ToDirectionInt() * Main.rand.Next(10, 21);
                vector120 *= (float)Main.rand.Next(-80, 81);
                Vector2 vector122 = vector120 - vector121;
                vector122 /= 10f;
                if (projectile.ai[0] == 90f && Main.netMode != 1)
                {
                    Vector2 vector123 = projectile.ai[1].ToRotationVector2() * 8f;
                    float ai2 = Main.rand.Next(80);
                    Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X - vector123.X, projectile.Center.Y - vector123.Y, vector123.X, vector123.Y, 580, 50, 1f, Main.myPlayer, projectile.ai[1], ai2);
                }
            }
            else if (projectile.type == 579)
            {
                if (projectile.ai[0] == 90f && Main.netMode != 1)
                {
                    projectile.ai[1] = 1f;
                    projectile.netUpdate = true;
                    for (int num863 = 0; num863 < 2; num863++)
                    {
                        int num864 = NPC.NewNPC(projectile.GetNPCSource_FromThis(), (int)projectile.Center.X, (int)projectile.Center.Y, 427, projectile.whoAmI);
                        Main.npc[num864].velocity = -Vector2.UnitY.RotatedByRandom(6.2831854820251465) * Main.rand.Next(4, 9) - Vector2.UnitY * 2f;
                        Main.npc[num864].netUpdate = true;
                    }
                }
            }
            else if (projectile.type == 813)
            {
                if (projectile.ai[0] == 90f && Main.netMode != 1)
                {
                    int num865 = NPC.NewNPC(projectile.GetNPCSource_FromThis(), (int)projectile.Center.X, (int)projectile.Center.Y, 619, projectile.whoAmI);
                    Main.npc[num865].netUpdate = true;
                }
            }
        }
        else if (projectile.ai[0] <= 120f)
        {
            projectile.scale = 1f;
            projectile.alpha = 0;
            projectile.rotation -= (float)Math.PI / 60f;
        }
        else
        {
            projectile.scale = 1f - (projectile.ai[0] - 120f) / 60f;
            projectile.alpha = 255 - (int)(255f * projectile.scale);
            projectile.rotation -= (float)Math.PI / 30f;
            if (projectile.alpha >= 255)
            {
                projectile.Kill();
            }
        }
    }
    public static void AI_109(Projectile projectile)
    {
        if (projectile.localAI[1] == 0f)
        {
            projectile.localAI[1] = projectile.velocity.Length();
        }
        if (projectile.ai[0] == 0f)
        {
            projectile.localAI[0]++;
            if (projectile.localAI[0] > 30f)
            {
                projectile.ai[0] = 1f;
                projectile.localAI[0] = 0f;
                return;
            }
        }
        else if (projectile.ai[0] == 1f)
        {
            Vector2 zero3 = Vector2.Zero;
            if (projectile.type != 582 || !Main.npc[(int)projectile.ai[1]].active || Main.npc[(int)projectile.ai[1]].type != 124)
            {
                projectile.Kill();
                return;
            }
            NPC.lazyNPCOwnedProjectileSearchArray[(int)projectile.ai[1]] = projectile.whoAmI;
            zero3 = Main.npc[(int)projectile.ai[1]].Center;
            projectile.tileCollide = false;
            float num868 = projectile.localAI[1];
            Vector2 value14 = zero3 - projectile.Center;
            if (value14.Length() < num868)
            {
                projectile.Kill();
                return;
            }
            value14.Normalize();
            value14 *= num868;
            projectile.velocity = Vector2.Lerp(projectile.velocity, value14, 0.04f);
        }
        projectile.rotation += (float)Math.PI / 10f;
    }
    public static void AI_110(Projectile projectile)
    {
        if (projectile.localAI[1] == 0f)
        {
            projectile.localAI[1] = projectile.velocity.Length();
        }
        Vector2 zero4 = Vector2.Zero;
        if (Main.npc[(int)projectile.ai[0]].active && Main.npc[(int)projectile.ai[0]].townNPC)
        {
            zero4 = Main.npc[(int)projectile.ai[0]].Center;
            float num869 = projectile.localAI[1];
            Vector2 value15 = zero4 - projectile.Center;
            if (value15.Length() < num869 || projectile.Hitbox.Intersects(Main.npc[(int)projectile.ai[0]].Hitbox))
            {
                projectile.Kill();
                int num870 = Main.npc[(int)projectile.ai[0]].lifeMax - Main.npc[(int)projectile.ai[0]].life;
                if (num870 > 20)
                {
                    num870 = 20;
                }
                if (num870 > 0)
                {
                    NPC nPC13 = Main.npc[(int)projectile.ai[0]];
                    nPC13.life += num870;
                    Main.npc[(int)projectile.ai[0]].HealEffect(num870);
                }
            }
            else
            {
                value15.Normalize();
                value15 *= num869;
                if (value15.Y < projectile.velocity.Y)
                {
                    value15.Y = projectile.velocity.Y;
                }
                value15.Y += 1f;
                projectile.velocity = Vector2.Lerp(projectile.velocity, value15, 0.04f);
                projectile.rotation += projectile.velocity.X * 0.05f;
            }
        }
        else
        {
            projectile.Kill();
        }
    }
    public static void AI_112(Projectile projectile)
    {
        if (projectile.type == 836)
        {
            if (projectile.localAI[0] == 0f)
            {
                projectile.localAI[0] = 1f;
            }
            if (++projectile.frameCounter >= 6)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            Player player9 = Main.player[(int)projectile.ai[1]];
            bool flag49 = player9.active && !player9.dead && Vector2.Distance(player9.Center, projectile.Center) < 800f;
            int num873 = (projectile.spriteDirection = ((Main.WindForVisuals > 0f) ? 1 : (-1)));
            projectile.direction = ((player9.Center.X > projectile.Center.X) ? 1 : (-1));
            bool flag50 = num873 != projectile.direction;
            float num874 = 2.5f;
            float num875 = 2f;
            if (flag50)
            {
                num874 = 1.5f;
                num875 = 1f;
            }
            if (flag49)
            {
                if (!flag50)
                {
                    float num876 = player9.Center.X - projectile.Center.X;
                    projectile.velocity.X += 0.05f * (float)projectile.direction * (0.6f + Math.Abs(Main.WindForVisuals));
                    if (projectile.velocity.X > num874)
                    {
                        projectile.velocity.X -= 0.1f;
                    }
                    if (projectile.velocity.X < 0f - num874)
                    {
                        projectile.velocity.X += 0.1f;
                    }
                }
                if (player9.Top.Y >= projectile.Center.Y || flag50)
                {
                    projectile.velocity.Y += 0.05f;
                    if (projectile.velocity.Y > num874)
                    {
                        projectile.velocity.Y -= 0.1f;
                    }
                }
                else if (player9.Top.Y < projectile.Center.Y)
                {
                    projectile.velocity.Y -= 0.1f;
                    if (projectile.velocity.Y < 0f - num875)
                    {
                        projectile.velocity.Y += 0.2f;
                    }
                }
            }
            else
            {
                projectile.velocity.Y += 0.2f;
                if (projectile.velocity.Y < 0f - num875)
                {
                    projectile.velocity.Y += 0.2f;
                }
                if (projectile.velocity.Y > num875)
                {
                    projectile.velocity.Y -= 0.2f;
                }
            }
            projectile.rotation = projectile.velocity.X * 0.125f;
        }
        if (projectile.type == 590)
        {
            if (++projectile.frameCounter >= 4)
            {
                int num877 = 0;
                int num878 = 3;
                if (projectile.ai[2] == 1f)
                {
                    num877 = 3;
                    num878 = 6;
                }
                projectile.frameCounter = 0;
                if (++projectile.frame >= num878)
                {
                    projectile.frame = num877;
                }
            }
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 15;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (projectile.alpha == 0)
            {
                float num879 = (float)Main.rand.Next(28, 42) * 0.005f;
                num879 += (float)(270 - Main.mouseTextColor) / 500f;
                float num880 = 0.1f;
                float num881 = 0.3f + num879 / 2f;
                float num882 = 0.6f + num879;
                float num883 = 0.35f;
                num880 *= num883;
                num881 *= num883;
                num882 *= num883;
                Lighting.AddLight(projectile.Center, num880, num881, num882);
            }
            projectile.velocity = new Vector2(0f, (float)Math.Sin((float)Math.PI * 2f * projectile.ai[0] / 180f) * 0.15f);
            projectile.ai[0]++;
            if (projectile.ai[0] >= 180f)
            {
                projectile.ai[0] = 0f;
            }
        }
        if (projectile.type != 644)
        {
            return;
        }
        Color newColor3 = Main.hslToRgb(projectile.ai[0], 1f, 0.5f);
        int num884 = (int)projectile.ai[1];
        if (num884 < 0 || num884 >= 1000 || (!Main.projectile[num884].active && Main.projectile[num884].type != 643))
        {
            projectile.ai[1] = -1f;
        }
        else
        {
            DelegateMethods.v3_1 = newColor3.ToVector3() * 0.5f;
            Utils.PlotTileLine(projectile.Center, Main.projectile[num884].Center, 8f, DelegateMethods.CastLight);
        }
        if (projectile.localAI[0] == 0f)
        {
            projectile.localAI[0] = Main.rand.NextFloat() * 0.8f + 0.8f;
            projectile.direction = ((Main.rand.Next(2) > 0) ? 1 : (-1));
        }
        projectile.rotation = projectile.localAI[1] / 40f * ((float)Math.PI * 2f) * (float)projectile.direction;
        if (projectile.alpha > 0)
        {
            projectile.alpha -= 8;
        }
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        if (projectile.alpha == 0)
        {
            Lighting.AddLight(projectile.Center, newColor3.ToVector3() * 0.5f);
        }
        if (Main.rand.Next(10) == 0)
        {
            float num887 = 1f + Main.rand.NextFloat() * 2f;
            float fadeIn = 1f + Main.rand.NextFloat();
            float num888 = 1f + Main.rand.NextFloat();
            Vector2 vector136 = Utils.RandomVector2(Main.rand, -1f, 1f);
            if (vector136 != Vector2.Zero)
            {
                vector136.Normalize();
            }
            vector136 *= 20f + Main.rand.NextFloat() * 100f;
            Vector2 vec = projectile.Center + vector136;
            Point point3 = vec.ToTileCoordinates();
            bool flag51 = true;
            if (!WorldGen.InWorld(point3.X, point3.Y))
            {
                flag51 = false;
            }
            if (flag51 && WorldGen.SolidTile(point3.X, point3.Y))
            {
                flag51 = false;
            }
        }
        projectile.scale = projectile.Opacity / 2f * projectile.localAI[0];
        projectile.velocity = Vector2.Zero;
        projectile.localAI[1]++;
        if (projectile.localAI[1] >= 60f)
        {
            projectile.Kill();
        }
        if (projectile.localAI[1] == 30f)
        {
            projectile.DoRainbowCrystalStaffExplosion();
            if (Main.myPlayer == projectile.owner)
            {
                projectile.friendly = true;
                int num889 = projectile.width;
                int num890 = projectile.height;
                int num891 = projectile.penetrate;
                projectile.position = projectile.Center;
                projectile.width = (projectile.height = 60);
                projectile.Center = projectile.position;
                projectile.penetrate = -1;
                projectile.maxPenetrate = -1;
                projectile.Damage();
                projectile.penetrate = num891;
                projectile.position = projectile.Center;
                projectile.width = num889;
                projectile.height = num890;
                projectile.Center = projectile.position;
                projectile.friendly = false;
            }
        }
    }
    public static void AI_113(Projectile projectile)
    {
        int num892 = 25;
        if (projectile.type == 614)
        {
            num892 = 63;
        }
        if (projectile.alpha > 0)
        {
            projectile.alpha -= num892;
        }
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        if (projectile.ai[0] == 0f)
        {
            if (projectile.type == 614)
            {
                int num893 = (int)projectile.ai[1];
                if (!Main.npc[num893].CanBeChasedBy(projectile))
                {
                    projectile.Kill();
                    return;
                }
                float num894 = projectile.velocity.ToRotation();
                Vector2 vector137 = Main.npc[num893].Center - projectile.Center;
                if (vector137 != Vector2.Zero)
                {
                    vector137.Normalize();
                    vector137 *= 14f;
                }
                float num895 = 5f;
                projectile.velocity = (projectile.velocity * (num895 - 1f) + vector137) / num895;
            }
            else
            {
                projectile.ai[1]++;
                if (projectile.ai[1] >= 45f)
                {
                    float num896 = 0.98f;
                    float num897 = 0.35f;
                    if (projectile.type == 636)
                    {
                        num896 = 0.995f;
                        num897 = 0.15f;
                    }
                    projectile.ai[1] = 45f;
                    projectile.velocity.X *= num896;
                    projectile.velocity.Y += num897;
                }
                projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
            }
        }
        if (projectile.ai[0] == 1f)
        {
            Vector2 center17 = projectile.Center;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            int num898 = 15;
            if (projectile.type == 636)
            {
                num898 = 5 * projectile.MaxUpdates;
            }
            if (projectile.type == 971)
            {
                num898 = 9 * projectile.MaxUpdates;
            }
            if (projectile.type == 975)
            {
                num898 = 9 * projectile.MaxUpdates;
            }
            bool flag52 = false;
            bool flag53 = false;
            projectile.localAI[0]++;
            if (projectile.localAI[0] % 30f == 0f)
            {
                flag53 = true;
            }
            int num899 = (int)projectile.ai[1];
            if (projectile.localAI[0] >= (float)(60 * num898))
            {
                flag52 = true;
            }
            else if (num899 < 0 || num899 >= 200)
            {
                flag52 = true;
            }
            else if (Main.npc[num899].active && !Main.npc[num899].dontTakeDamage)
            {
                projectile.Center = Main.npc[num899].Center - projectile.velocity * 2f;
                projectile.gfxOffY = Main.npc[num899].gfxOffY;
                if (flag53)
                {
                    Main.npc[num899].HitEffect(0, 1.0);
                }
            }
            else
            {
                flag52 = true;
            }
            if (flag52)
            {
                projectile.Kill();
            }
            if (!flag52 && projectile.type == 971)
            {
                if (projectile.localAI[1] == 0f)
                {
                    projectile.localAI[1] = 1f;
                    projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                }
            }
            if (!flag52 && projectile.type == 975)
            {
                if (projectile.localAI[1] == 0f)
                {
                    projectile.localAI[1] = 1f;
                    projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                }
            }
        }
        if (projectile.type == 614)
        {
            Lighting.AddLight(projectile.Center, 0.2f, 0.6f, 0.7f);
        }
        if (projectile.type == 636)
        {
            Lighting.AddLight(projectile.Center, 0.8f, 0.7f, 0.4f);
        }
    }
    public static void AI_114(Projectile projectile)
    {
        if (Main.netMode == 2 && projectile.localAI[0] == 0f)
        {
            PortalHelper.SyncPortalSections(projectile.Center, 1);
            projectile.localAI[0] = 1f;
        }
        projectile.timeLeft = 3;
        bool flag54 = false;
        if (projectile.owner != 255 && (!Main.player[projectile.owner].active || Main.player[projectile.owner].dead || projectile.Distance(Main.player[projectile.owner].Center) > 12800f))
        {
            flag54 = true;
        }
        if (!flag54 && !WorldGen.InWorld((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16, Lighting.OffScreenTiles))
        {
            flag54 = true;
        }
        if (!flag54 && !PortalHelper.SupportedTilesAreFine(projectile.Center, projectile.ai[0]))
        {
            flag54 = true;
        }
        if (flag54)
        {
            projectile.Kill();
            return;
        }
        Color portalColor = PortalHelper.GetPortalColor(projectile.owner, (int)projectile.ai[1]);
        projectile.alpha -= 25;
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        if (projectile.alpha == 0)
        {
            Lighting.AddLight(projectile.Center + projectile.velocity * 3f, portalColor.ToVector3() * 0.5f);
        }
        if (++projectile.frameCounter >= 6)
        {
            projectile.frameCounter = 0;
            if (++projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.frame = 0;
            }
        }
        projectile.rotation = projectile.ai[0] - (float)Math.PI / 2f;
    }
    public static void AI_115(Projectile projectile)
    {
        Lighting.AddLight(projectile.Center, new Vector3(0.075f, 0.3f, 0.15f));
        projectile.velocity *= 0.985f;
        projectile.rotation += projectile.velocity.X * 0.2f;
        if (projectile.velocity.X > 0f)
        {
            projectile.rotation += 0.08f;
        }
        else
        {
            projectile.rotation -= 0.08f;
        }
        projectile.ai[1] += 1f;
        if (projectile.ai[1] > 30f)
        {
            projectile.alpha += 10;
            if (projectile.alpha >= 255)
            {
                projectile.alpha = 255;
                projectile.Kill();
            }
        }
    }
    public static void AI_116(Projectile projectile)
    {
        if (projectile.localAI[0] == 0f)
        {
            projectile.rotation = projectile.ai[1];
            projectile.localAI[0] = 1f;
        }
        Player player10 = Main.player[projectile.owner];
        if (player10.setSolar)
        {
            projectile.timeLeft = 2;
        }
        float angle = (float)player10.miscCounter / 300f * ((float)Math.PI * 4f) + projectile.ai[1];
        angle = MathHelper.WrapAngle(angle);
        projectile.rotation = projectile.rotation.AngleLerp(angle, 0.05f);
        projectile.alpha -= 15;
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        projectile.velocity = projectile.rotation.ToRotationVector2() * 100f - player10.velocity;
        projectile.Center = player10.Center - projectile.velocity;
    }
    public static void AI_117(Projectile projectile)
    {
        projectile.ai[1] += 0.01f;
        projectile.scale = projectile.ai[1];
        projectile.ai[0]++;
        if (projectile.ai[0] >= (float)(3 * Main.projFrames[projectile.type]))
        {
            projectile.Kill();
            return;
        }
        if (++projectile.frameCounter >= 3)
        {
            projectile.frameCounter = 0;
            if (++projectile.frame >= Main.projFrames[projectile.type])
            {
                projectile.hide = true;
            }
        }
        projectile.alpha -= 63;
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        bool flag55 = projectile.type == 612 || projectile.type == 953 || projectile.type == 978;
        bool flag56 = projectile.type == 624;
        if (flag55)
        {
            Lighting.AddLight(projectile.Center, 0.9f, 0.8f, 0.6f);
        }
        if (projectile.ai[0] != 1f)
        {
            return;
        }
        projectile.position = projectile.Center;
        projectile.width = (projectile.height = (int)(52f * projectile.scale));
        projectile.Center = projectile.position;
        projectile.Damage();
        if (!flag56)
        {
            return;
        }
    }
    public static void AI_118(Projectile projectile)
    {
        projectile.ai[0]++;
        int num924 = 0;
        if (projectile.velocity.Length() <= 4f)
        {
            num924 = 1;
        }
        projectile.alpha -= 15;
        if (projectile.alpha < 0)
        {
            projectile.alpha = 0;
        }
        switch (num924)
        {
            case 0:
                projectile.rotation -= (float)Math.PI / 30f;
                if (projectile.ai[0] >= 30f)
                {
                    projectile.velocity *= 0.98f;
                    projectile.scale += 0.0074468083f;
                    if (projectile.scale > 1.3f)
                    {
                        projectile.scale = 1.3f;
                    }
                    projectile.rotation -= (float)Math.PI / 180f;
                }
                if (projectile.velocity.Length() < 4.1f)
                {
                    projectile.velocity.Normalize();
                    projectile.velocity *= 4f;
                    projectile.ai[0] = 0f;
                }
                break;
            case 1:
                {
                    projectile.rotation -= (float)Math.PI / 30f;
                    if (projectile.ai[0] % 30f == 0f && projectile.ai[0] < 241f && Main.myPlayer == projectile.owner)
                    {
                        Vector2 vector146 = Vector2.UnitY.RotatedByRandom(6.2831854820251465) * 12f;
                        Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X, projectile.Center.Y, vector146.X, vector146.Y, 618, projectile.damage / 2, 0f, projectile.owner, 0f, projectile.whoAmI);
                    }
                    Vector2 vector147 = projectile.Center;
                    float num926 = 800f;
                    bool flag57 = false;
                    int num927 = 0;
                    if (projectile.ai[1] == 0f)
                    {
                        for (int num928 = 0; num928 < 200; num928++)
                        {
                            if (Main.npc[num928].CanBeChasedBy(projectile))
                            {
                                Vector2 center18 = Main.npc[num928].Center;
                                if (projectile.Distance(center18) < num926 && Collision.CanHit(new Vector2(projectile.position.X + (float)(projectile.width / 2), projectile.position.Y + (float)(projectile.height / 2)), 1, 1, Main.npc[num928].position, Main.npc[num928].width, Main.npc[num928].height))
                                {
                                    num926 = projectile.Distance(center18);
                                    vector147 = center18;
                                    flag57 = true;
                                    num927 = num928;
                                }
                            }
                        }
                        if (flag57)
                        {
                            if (projectile.ai[1] != (float)(num927 + 1))
                            {
                                projectile.netUpdate = true;
                            }
                            projectile.ai[1] = num927 + 1;
                        }
                        flag57 = false;
                    }
                    if (projectile.ai[1] != 0f)
                    {
                        int num929 = (int)(projectile.ai[1] - 1f);
                        if (Main.npc[num929].active && Main.npc[num929].CanBeChasedBy(projectile, ignoreDontTakeDamage: true) && projectile.Distance(Main.npc[num929].Center) < 1000f)
                        {
                            flag57 = true;
                            vector147 = Main.npc[num929].Center;
                        }
                    }
                    if (!projectile.friendly)
                    {
                        flag57 = false;
                    }
                    if (flag57)
                    {
                        float num930 = 4f;
                        int num931 = 8;
                        Vector2 vector148 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
                        float num932 = vector147.X - vector148.X;
                        float num933 = vector147.Y - vector148.Y;
                        float num934 = (float)Math.Sqrt(num932 * num932 + num933 * num933);
                        float num935 = num934;
                        num934 = num930 / num934;
                        num932 *= num934;
                        num933 *= num934;
                        projectile.velocity.X = (projectile.velocity.X * (float)(num931 - 1) + num932) / (float)num931;
                        projectile.velocity.Y = (projectile.velocity.Y * (float)(num931 - 1) + num933) / (float)num931;
                    }
                    break;
                }
        }
        if (projectile.alpha < 150)
        {
            Lighting.AddLight(projectile.Center, 0.7f, 0.2f, 0.6f);
        }
        if (projectile.ai[0] >= 600f)
        {
            projectile.Kill();
        }
    }
    public static void AI_119(Projectile projectile)
    {
        int num936 = 0;
        float num937 = 0f;
        float x12 = 0f;
        float y10 = 0f;
        bool flag58 = false;
        bool flag59 = false;
        int num38 = projectile.type;
        if (num38 == 618)
        {
            num936 = 617;
            num937 = 420f;
            x12 = 0.15f;
            y10 = 0.15f;
        }
        if (flag59)
        {
            int num938 = (int)projectile.ai[1];
            if (!Main.projectile[num938].active || Main.projectile[num938].type != num936)
            {
                projectile.Kill();
                return;
            }
            projectile.timeLeft = 2;
        }
        projectile.ai[0]++;
        if (!(projectile.ai[0] < num937))
        {
            return;
        }
        bool flag60 = true;
        int num939 = (int)projectile.ai[1];
        if (Main.projectile[num939].active && Main.projectile[num939].type == num936)
        {
            if (!flag58 && Main.projectile[num939].oldPos[1] != Vector2.Zero)
            {
                projectile.position += Main.projectile[num939].position - Main.projectile[num939].oldPos[1];
            }
            if (projectile.Center.HasNaNs())
            {
                projectile.Kill();
                return;
            }
        }
        else
        {
            projectile.ai[0] = num937;
            flag60 = false;
            projectile.Kill();
        }
        if (flag60 && !flag58)
        {
            projectile.velocity += new Vector2(Math.Sign(Main.projectile[num939].Center.X - projectile.Center.X), Math.Sign(Main.projectile[num939].Center.Y - projectile.Center.Y)) * new Vector2(x12, y10);
            if (projectile.velocity.Length() > 6f)
            {
                projectile.velocity *= 6f / projectile.velocity.Length();
            }
        }
        if (projectile.type == 618)
        {
            projectile.alpha = 255;
        }
        else
        {
            projectile.Kill();
        }
    }
    public static void AI_120(Projectile projectile)
    {
        projectile.AI_120_StardustGuardian();
    }
    public static void AI_122(Projectile projectile)
    {
        int num941 = (int)projectile.ai[0];
        bool flag61 = false;
        if (num941 == -1 || !Main.npc[num941].active)
        {
            flag61 = true;
        }
        if (flag61)
        {
            if (projectile.type == 629)
            {
                projectile.Kill();
                return;
            }
            if (projectile.type == 631 && projectile.ai[0] != -1f)
            {
                projectile.ai[0] = -1f;
                projectile.netUpdate = true;
            }
        }
        if (!flag61 && projectile.Hitbox.Intersects(Main.npc[num941].Hitbox))
        {
            projectile.Kill();
            if (projectile.type == 631)
            {
                projectile.localAI[1] = 1f;
                projectile.Damage();
            }
            return;
        }
        if (projectile.type == 629)
        {
            Vector2 value17 = Main.npc[num941].Center - projectile.Center;
            projectile.velocity = Vector2.Normalize(value17) * 5f;
        }
        if (projectile.type != 631)
        {
            return;
        }
        if (projectile.ai[1] > 0f)
        {
            projectile.ai[1]--;
            projectile.velocity = Vector2.Zero;
            return;
        }
        if (flag61)
        {
            if (projectile.velocity == Vector2.Zero)
            {
                projectile.Kill();
            }
            projectile.tileCollide = true;
            projectile.alpha += 10;
            if (projectile.alpha > 255)
            {
                projectile.Kill();
            }
        }
        else
        {
            Vector2 value18 = Main.npc[num941].Center - projectile.Center;
            projectile.velocity = Vector2.Normalize(value18) * 12f;
            projectile.alpha -= 15;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
        }
        projectile.rotation = projectile.velocity.ToRotation() - (float)Math.PI / 2f;
    }
    public static void AI_123(Projectile projectile)
    {
        bool flag62 = projectile.type == 641;
        bool flag63 = projectile.type == 643;
        float num942 = 1000f;
        projectile.velocity = Vector2.Zero;
        if (flag62)
        {
            projectile.alpha -= 5;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (projectile.direction == 0)
            {
                projectile.direction = Main.player[projectile.owner].direction;
            }
            projectile.rotation -= (float)projectile.direction * ((float)Math.PI * 2f) / 120f;
            projectile.scale = projectile.Opacity;
            Lighting.AddLight(projectile.Center, new Vector3(0.3f, 0.9f, 0.7f) * projectile.Opacity);
        }
        if (flag63)
        {
            projectile.alpha -= 5;
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (projectile.direction == 0)
            {
                projectile.direction = Main.player[projectile.owner].direction;
            }
            if (++projectile.frameCounter >= 3)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
            projectile.localAI[0]++;
            if (projectile.localAI[0] >= 60f)
            {
                projectile.localAI[0] = 0f;
            }
        }
        if (projectile.ai[0] < 0f)
        {
            projectile.ai[0]++;
            if (flag62)
            {
                projectile.ai[1] -= (float)projectile.direction * ((float)Math.PI / 8f) / 50f;
            }
        }
        if (projectile.ai[0] == 0f)
        {
            int num944 = -1;
            float num945 = num942;
            NPC ownerMinionAttackTargetNPC4 = projectile.OwnerMinionAttackTargetNPC;
            if (ownerMinionAttackTargetNPC4 != null && ownerMinionAttackTargetNPC4.CanBeChasedBy(projectile))
            {
                float num946 = projectile.Distance(ownerMinionAttackTargetNPC4.Center);
                if (num946 < num945 && Collision.CanHitLine(projectile.Center, 0, 0, ownerMinionAttackTargetNPC4.Center, 0, 0))
                {
                    num945 = num946;
                    num944 = ownerMinionAttackTargetNPC4.whoAmI;
                }
            }
            if (num944 < 0)
            {
                for (int num947 = 0; num947 < 200; num947++)
                {
                    NPC nPC14 = Main.npc[num947];
                    if (nPC14.CanBeChasedBy(projectile))
                    {
                        float num948 = projectile.Distance(nPC14.Center);
                        if (num948 < num945 && Collision.CanHitLine(projectile.Center, 0, 0, nPC14.Center, 0, 0))
                        {
                            num945 = num948;
                            num944 = num947;
                        }
                    }
                }
            }
            if (num944 != -1)
            {
                projectile.ai[0] = 1f;
                projectile.ai[1] = num944;
                projectile.netUpdate = true;
                return;
            }
        }
        if (!(projectile.ai[0] > 0f))
        {
            return;
        }
        int num949 = (int)projectile.ai[1];
        if (!Main.npc[num949].CanBeChasedBy(projectile))
        {
            projectile.ai[0] = 0f;
            projectile.ai[1] = 0f;
            projectile.netUpdate = true;
            return;
        }
        projectile.ai[0]++;
        float num950 = 30f;
        if (flag62)
        {
            num950 = 10f;
        }
        if (flag63)
        {
            num950 = 5f;
        }
        if (!(projectile.ai[0] >= num950))
        {
            return;
        }
        Vector2 vector153 = projectile.DirectionTo(Main.npc[num949].Center);
        if (vector153.HasNaNs())
        {
            vector153 = Vector2.UnitY;
        }
        float num951 = vector153.ToRotation();
        int num952 = ((vector153.X > 0f) ? 1 : (-1));
        if (flag62)
        {
            projectile.direction = num952;
            projectile.ai[0] = -20f;
            projectile.ai[1] = num951 + (float)num952 * (float)Math.PI / 6f;
            projectile.netUpdate = true;
            if (projectile.owner == Main.myPlayer)
            {
                Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center.X, projectile.Center.Y, vector153.X, vector153.Y, 642, projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[1], projectile.whoAmI);
            }
        }
        if (!flag63)
        {
            return;
        }
        projectile.direction = num952;
        projectile.ai[0] = -20f;
        projectile.netUpdate = true;
        if (projectile.owner != Main.myPlayer)
        {
            return;
        }
        NPC nPC15 = Main.npc[num949];
        Vector2 vector154 = nPC15.position + nPC15.Size * Utils.RandomVector2(Main.rand, 0f, 1f) - projectile.Center;
        for (int num953 = 0; num953 < 3; num953++)
        {
            Vector2 other = projectile.Center + vector154;
            Vector2 vector155 = nPC15.velocity * 30f;
            other += vector155;
            float num954 = MathHelper.Lerp(0.1f, 0.75f, Utils.GetLerpValue(800f, 200f, projectile.Distance(other)));
            if (num953 > 0)
            {
                other = projectile.Center + vector154.RotatedByRandom(0.7853981852531433) * (Main.rand.NextFloat() * num954 + 0.5f);
            }
            float x13 = Main.rgbToHsl(new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB)).X;
            Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), other.X, other.Y, 0f, 0f, 644, projectile.damage, projectile.knockBack, projectile.owner, x13, projectile.whoAmI);
        }
    }
    public static void AI_124(Projectile projectile)
    {
        bool flag64 = projectile.type == 650;
        Player player11 = Main.player[projectile.owner];
        if (player11.dead)
        {
            projectile.Kill();
            return;
        }
        if (projectile.type == 650 && player11.suspiciouslookingTentacle)
        {
            projectile.timeLeft = 2;
        }
        if (projectile.type == 882 && player11.petFlagEyeOfCthulhuPet)
        {
            projectile.timeLeft = 2;
        }
        if (projectile.type == 888 && player11.petFlagTwinsPet)
        {
            projectile.timeLeft = 2;
        }
        if (projectile.type == 895 && player11.petFlagFairyQueenPet)
        {
            projectile.timeLeft = 2;
        }
        if (projectile.type == 896 && player11.petFlagPumpkingPet)
        {
            projectile.timeLeft = 2;
        }
        if (projectile.type == 898 && player11.petFlagIceQueenPet)
        {
            projectile.timeLeft = 2;
        }
        if (projectile.type == 957 && player11.petFlagGlommerPet)
        {
            projectile.timeLeft = 2;
        }
        projectile.direction = (projectile.spriteDirection = player11.direction);
        if (projectile.type == 650)
        {
            Vector3 v3_ = new Vector3(0.5f, 0.9f, 1f) * 2f;
            DelegateMethods.v3_1 = v3_;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * 6f, 20f, DelegateMethods.CastLightOpen);
            Utils.PlotTileLine(projectile.Left, projectile.Right, 20f, DelegateMethods.CastLightOpen);
            Utils.PlotTileLine(player11.Center, player11.Center + player11.velocity * 6f, 40f, DelegateMethods.CastLightOpen);
            Utils.PlotTileLine(player11.Left, player11.Right, 40f, DelegateMethods.CastLightOpen);
        }
        if (projectile.type == 895)
        {
            Vector3 vector156 = new Vector3(1f, 0.6f, 1f) * 1.5f;
            DelegateMethods.v3_1 = vector156 * 0.75f;
            Utils.PlotTileLine(player11.Center, player11.Center + player11.velocity * 6f, 40f, DelegateMethods.CastLightOpen);
            Utils.PlotTileLine(player11.Left, player11.Right, 40f, DelegateMethods.CastLightOpen);
            DelegateMethods.v3_1 = vector156 * 1.5f;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * 6f, 30f, DelegateMethods.CastLightOpen);
            Utils.PlotTileLine(projectile.Left, projectile.Right, 20f, DelegateMethods.CastLightOpen);
        }
        if (projectile.type == 896)
        {
            Vector3 vector157 = new Vector3(1f, 0.7f, 0.05f) * 1.5f;
            DelegateMethods.v3_1 = vector157 * 0.75f;
            Utils.PlotTileLine(player11.Center, player11.Center + player11.velocity * 6f, 40f, DelegateMethods.CastLightOpen);
            Utils.PlotTileLine(player11.Left, player11.Right, 40f, DelegateMethods.CastLightOpen);
            DelegateMethods.v3_1 = vector157 * 1.5f;
            Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * 6f, 30f, DelegateMethods.CastLightOpen);
            Utils.PlotTileLine(projectile.Left, projectile.Right, 20f, DelegateMethods.CastLightOpen);
        }
        float num955 = 30f;
        float y11 = -20f;
        int num956 = player11.direction;
        if (projectile.type == 882 && player11.ownedProjectileCounts[650] > 0)
        {
            num956 *= -1;
        }
        if (projectile.type == 888)
        {
            num955 = 30f;
            y11 = -50f;
            if (player11.ownedProjectileCounts[650] > 0)
            {
                y11 = -70f;
            }
        }
        if (projectile.type == 895)
        {
            num955 = -36f;
            y11 = -50f;
        }
        if (projectile.type == 896)
        {
            num955 = 30f;
            y11 = -60f;
            if (player11.ownedProjectileCounts[888] > 0)
            {
                num955 = -30f;
            }
        }
        if (projectile.type == 898)
        {
            num955 = -30f;
            y11 = -50f;
            if (player11.ownedProjectileCounts[895] > 0)
            {
                num955 = 30f;
            }
        }
        if (projectile.type == 957)
        {
            num955 = -40f;
            y11 = -40f;
            if (player11.ownedProjectileCounts[895] > 0)
            {
                num955 = 40f;
            }
        }
        Vector2 vector158 = new Vector2((float)num956 * num955, y11);
        Vector2 vector159 = player11.MountedCenter + vector158;
        float num957 = Vector2.Distance(projectile.Center, vector159);
        if (num957 > 1000f)
        {
            projectile.Center = player11.Center + vector158;
        }
        Vector2 vector160 = vector159 - projectile.Center;
        float num958 = 4f;
        if (num957 < num958)
        {
            projectile.velocity *= 0.25f;
        }
        if (vector160 != Vector2.Zero)
        {
            if (vector160.Length() < num958)
            {
                projectile.velocity = vector160;
            }
            else
            {
                projectile.velocity = vector160 * 0.1f;
            }
        }
        if (num957 > 50f && (projectile.type == 895 || projectile.type == 898 || projectile.type == 957))
        {
            projectile.direction = (projectile.spriteDirection = 1);
            if (projectile.velocity.X < 0f)
            {
                projectile.direction = (projectile.spriteDirection = -1);
            }
        }
        if (projectile.velocity.Length() > 6f)
        {
            if (projectile.type == 650)
            {
                float num959 = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                if (Math.Abs(projectile.rotation - num959) >= (float)Math.PI)
                {
                    if (num959 < projectile.rotation)
                    {
                        projectile.rotation -= (float)Math.PI * 2f;
                    }
                    else
                    {
                        projectile.rotation += (float)Math.PI * 2f;
                    }
                }
                float num960 = 12f;
                projectile.rotation = (projectile.rotation * (num960 - 1f) + num959) / num960;
                if (++projectile.frameCounter >= 4)
                {
                    projectile.frameCounter = 0;
                    if (++projectile.frame >= Main.projFrames[projectile.type])
                    {
                        projectile.frame = 0;
                    }
                }
            }
            else if (projectile.type == 882)
            {
                projectile.rotation = projectile.velocity.X * 0.125f;
                if (++projectile.frameCounter >= 3)
                {
                    projectile.frameCounter = 0;
                    if (projectile.frame < 6)
                    {
                        projectile.frame = 6;
                    }
                    else
                    {
                        projectile.frame++;
                        if (projectile.frame > 15)
                        {
                            projectile.frame = 10;
                        }
                    }
                }
            }
            else if (projectile.type == 888)
            {
                float num961 = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                if (Math.Abs(projectile.rotation - num961) >= (float)Math.PI)
                {
                    if (num961 < projectile.rotation)
                    {
                        projectile.rotation -= (float)Math.PI * 2f;
                    }
                    else
                    {
                        projectile.rotation += (float)Math.PI * 2f;
                    }
                }
                float num962 = 12f;
                projectile.rotation = (projectile.rotation * (num962 - 1f) + num961) / num962;
                if (++projectile.frameCounter >= 3)
                {
                    projectile.frameCounter = 0;
                    if (projectile.frame < 6)
                    {
                        projectile.frame = 6;
                    }
                    else
                    {
                        projectile.frame++;
                        if (projectile.frame > 17)
                        {
                            projectile.frame = 6;
                        }
                    }
                }
            }
            else if (projectile.type == 895)
            {
                float num963 = projectile.velocity.X * 0.1f;
                if (Math.Abs(projectile.rotation - num963) >= (float)Math.PI)
                {
                    if (num963 < projectile.rotation)
                    {
                        projectile.rotation -= (float)Math.PI * 2f;
                    }
                    else
                    {
                        projectile.rotation += (float)Math.PI * 2f;
                    }
                }
                float num964 = 12f;
                projectile.rotation = (projectile.rotation * (num964 - 1f) + num963) / num964;
                if (++projectile.frameCounter >= 3)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                    if (projectile.frame >= Main.projFrames[projectile.type])
                    {
                        projectile.frame = 0;
                    }
                }
            }
            else if (projectile.type == 896)
            {
                float num966 = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                if (Math.Abs(projectile.rotation - num966) >= (float)Math.PI)
                {
                    if (num966 < projectile.rotation)
                    {
                        projectile.rotation -= (float)Math.PI * 2f;
                    }
                    else
                    {
                        projectile.rotation += (float)Math.PI * 2f;
                    }
                }
                float num967 = 12f;
                projectile.rotation = (projectile.rotation * (num967 - 1f) + num966) / num967;
                if (++projectile.frameCounter >= 4)
                {
                    projectile.frameCounter = 0;
                    if (projectile.frame < 8)
                    {
                        projectile.frame = 8;
                    }
                    else
                    {
                        projectile.frame++;
                        if (projectile.frame >= Main.projFrames[projectile.type])
                        {
                            projectile.frame = 8;
                        }
                    }
                }
            }
            else if (projectile.type == 898)
            {
                float num968 = projectile.velocity.X * 0.1f;
                if (Math.Abs(projectile.rotation - num968) >= (float)Math.PI)
                {
                    if (num968 < projectile.rotation)
                    {
                        projectile.rotation -= (float)Math.PI * 2f;
                    }
                    else
                    {
                        projectile.rotation += (float)Math.PI * 2f;
                    }
                }
                float num969 = 12f;
                projectile.rotation = (projectile.rotation * (num969 - 1f) + num968) / num969;
                if (++projectile.frameCounter >= 3)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                    if (projectile.frame >= Main.projFrames[projectile.type])
                    {
                        projectile.frame = 0;
                    }
                }
            }
            else if (projectile.type == 957)
            {
                float num971 = projectile.velocity.X * 0.05f;
                if (Math.Abs(projectile.rotation - num971) >= (float)Math.PI)
                {
                    if (num971 < projectile.rotation)
                    {
                        projectile.rotation -= (float)Math.PI * 2f;
                    }
                    else
                    {
                        projectile.rotation += (float)Math.PI * 2f;
                    }
                }
                float num972 = 12f;
                projectile.rotation = (projectile.rotation * (num972 - 1f) + num971) / num972;
                if (++projectile.frameCounter >= 4)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                    if (projectile.frame >= Main.projFrames[projectile.type] * 2)
                    {
                        projectile.frame = 0;
                    }
                }
            }
        }
        else if (projectile.type == 650)
        {
            if (projectile.rotation > (float)Math.PI)
            {
                projectile.rotation -= (float)Math.PI * 2f;
            }
            if (projectile.rotation > -0.005f && projectile.rotation < 0.005f)
            {
                projectile.rotation = 0f;
            }
            else
            {
                projectile.rotation *= 0.96f;
            }
            if (++projectile.frameCounter >= 6)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }
        else if (projectile.type == 882)
        {
            projectile.rotation = projectile.velocity.X * 0.125f;
            if (++projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame == 6 || projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }
        else if (projectile.type == 888)
        {
            if (projectile.rotation > (float)Math.PI)
            {
                projectile.rotation -= (float)Math.PI * 2f;
            }
            if (projectile.rotation > -0.005f && projectile.rotation < 0.005f)
            {
                projectile.rotation = 0f;
            }
            else
            {
                projectile.rotation *= 0.96f;
            }
            if (++projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame == 6 || projectile.frame >= 18)
                {
                    projectile.frame = 0;
                }
            }
        }
        else if (projectile.type == 895)
        {
            if (projectile.rotation > (float)Math.PI)
            {
                projectile.rotation -= (float)Math.PI * 2f;
            }
            if (projectile.rotation > -0.005f && projectile.rotation < 0.005f)
            {
                projectile.rotation = 0f;
            }
            else
            {
                projectile.rotation *= 0.96f;
            }
            if (++projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }
        else if (projectile.type == 896)
        {
            if (projectile.rotation > (float)Math.PI)
            {
                projectile.rotation -= (float)Math.PI * 2f;
            }
            if (projectile.rotation > -0.005f && projectile.rotation < 0.005f)
            {
                projectile.rotation = 0f;
            }
            else
            {
                projectile.rotation *= 0.96f;
            }
            if (++projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame == 8 || projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                    if (Main.rand.Next(15) == 0)
                    {
                        projectile.frame = 8;
                    }
                }
            }
        }
        else if (projectile.type == 898)
        {
            if (projectile.rotation > (float)Math.PI)
            {
                projectile.rotation -= (float)Math.PI * 2f;
            }
            if (projectile.rotation > -0.005f && projectile.rotation < 0.005f)
            {
                projectile.rotation = 0f;
            }
            else
            {
                projectile.rotation *= 0.96f;
            }
            if (++projectile.frameCounter >= 5)
            {
                projectile.frameCounter = 0;
                projectile.frame++;
                if (projectile.frame >= Main.projFrames[projectile.type])
                {
                    projectile.frame = 0;
                }
            }
        }
        else if (projectile.type == 957)
        {
            int num973 = Main.projFrames[projectile.type];
            if (projectile.rotation > (float)Math.PI)
            {
                projectile.rotation -= (float)Math.PI * 2f;
            }
            if (projectile.rotation > -0.005f && projectile.rotation < 0.005f)
            {
                projectile.rotation = 0f;
            }
            else
            {
                projectile.rotation *= 0.96f;
            }
            if (projectile.velocity.Length() <= 0.01f)
            {
                bool flag65 = true;
                int num974 = (int)projectile.Center.X / 16;
                int num975 = (int)projectile.Center.Y / 16;
                int num976 = 4;
                for (int num977 = 0; num977 < num976 + 1; num977++)
                {
                    if (num974 < 0 || num974 >= Main.maxTilesX || num975 < 0 || num975 >= Main.maxTilesY)
                    {
                        flag65 = false;
                        break;
                    }
                    bool flag66 = WorldGen.SolidTileAllowBottomSlope(num974, num975);
                    if ((num977 == num976 && !flag66) || (num977 < num976 && flag66))
                    {
                        flag65 = false;
                        break;
                    }
                    num975++;
                }
                if (flag65)
                {
                    projectile.localAI[0]--;
                    if (projectile.localAI[0] <= 0f)
                    {
                        projectile.localAI[0] = 0f;
                        if (projectile.frame < num973 * 2)
                        {
                            projectile.frame = num973 * 2;
                        }
                        int num978 = 3;
                        if (projectile.frame <= 30 && projectile.frame <= 33)
                        {
                            num978 = 2;
                        }
                        if (++projectile.frameCounter >= num978)
                        {
                            projectile.frameCounter = 0;
                            projectile.frame++;
                            if (projectile.frame >= num973 * 3)
                            {
                                projectile.localAI[0] = 200 + Main.rand.Next(150);
                                projectile.frame = 0;
                            }
                            if (projectile.frame == 32)
                            {
                                SoundEngine.PlaySound(SoundID.GlommerBounce, projectile.Bottom);
                            }
                        }
                    }
                    else if (++projectile.frameCounter >= 4)
                    {
                        projectile.frameCounter = 0;
                        projectile.frame++;
                        if (projectile.frame >= num973 * 2)
                        {
                            projectile.frame = 0;
                        }
                    }
                }
                else
                {
                    projectile.localAI[0] = 300f;
                    if (++projectile.frameCounter >= 4)
                    {
                        projectile.frameCounter = 0;
                        projectile.frame++;
                        if (projectile.frame >= num973 * 2)
                        {
                            projectile.frame = 0;
                        }
                    }
                }
            }
            else
            {
                projectile.localAI[0] = 300f;
                if (++projectile.frameCounter >= 4)
                {
                    projectile.frameCounter = 0;
                    projectile.frame++;
                    if (projectile.frame >= num973 * 2)
                    {
                        projectile.frame = 0;
                    }
                }
            }
        }
        if (flag64 && projectile.ai[0] > 0f && (projectile.ai[0] += 1f) >= 60f)
        {
            projectile.ai[0] = 0f;
            projectile.ai[1] = 0f;
        }
        if (flag64 && Main.rand.Next(15) == 0)
        {
            int num979 = -1;
            int num980 = -1;
            float num981 = -1f;
            int num982 = 17;
            if ((projectile.Center - player11.Center).Length() < (float)Main.screenWidth)
            {
                int num983 = (int)projectile.Center.X / 16;
                int num984 = (int)projectile.Center.Y / 16;
                num983 = (int)MathHelper.Clamp(num983, num982 + 1, Main.maxTilesX - num982 - 1);
                num984 = (int)MathHelper.Clamp(num984, num982 + 1, Main.maxTilesY - num982 - 1);
                for (int num985 = num983 - num982; num985 <= num983 + num982; num985++)
                {
                    for (int num986 = num984 - num982; num986 <= num984 + num982; num986++)
                    {
                        int num987 = Main.rand.Next(8);
                        if (num987 < 4 && new Vector2(num983 - num985, num984 - num986).Length() < (float)num982 && Main.tile[num985, num986] != null && Main.tile[num985, num986].active() && Main.IsTileSpelunkable(Main.tile[num985, num986]))
                        {
                            float num988 = projectile.Distance(new Vector2(num985 * 16 + 8, num986 * 16 + 8));
                            if (num988 < num981 || num981 == -1f)
                            {
                                num981 = num988;
                                num979 = num985;
                                num980 = num986;
                                projectile.ai[0] = 1f;
                                projectile.ai[1] = projectile.AngleTo(new Vector2(num985 * 16 + 8, num986 * 16 + 8));
                            }
                        }
                    }
                }
            }
        }
        if (!flag64)
        {
            return;
        }
        float f3 = projectile.localAI[0] % ((float)Math.PI * 2f) - (float)Math.PI;
        float num990 = (float)Math.IEEERemainder(projectile.localAI[1], 1.0);
        if (num990 < 0f)
        {
            num990 += 1f;
        }
        float num991 = (float)Math.Floor(projectile.localAI[1]);
        float max = 0.999f;
        float num992 = 0f;
        int num993 = 0;
        float amount2 = 0.1f;
        bool flag67 = player11.velocity.Length() > 3f;
        int num994 = -1;
        int num995 = -1;
        float num996 = 300f;
        float num997 = 500f;
        for (int num998 = 0; num998 < 200; num998++)
        {
            NPC nPC16 = Main.npc[num998];
            if (!nPC16.active || !nPC16.chaseable || nPC16.dontTakeDamage || nPC16.immortal)
            {
                continue;
            }
            float num999 = projectile.Distance(nPC16.Center);
            if (nPC16.friendly || nPC16.lifeMax <= 5)
            {
                if (num999 < num996 && !flag67)
                {
                    num996 = num999;
                    num995 = num998;
                }
            }
            else if (num999 < num997)
            {
                num997 = num999;
                num994 = num998;
            }
        }
        if (flag67)
        {
            num992 = projectile.AngleTo(projectile.Center + player11.velocity);
            num993 = 1;
            num990 = MathHelper.Clamp(num990 + 0.05f, 0f, max);
            num991 += (float)Math.Sign(-10f - num991);
        }
        else if (num994 != -1)
        {
            num992 = projectile.AngleTo(Main.npc[num994].Center);
            num993 = 2;
            num990 = MathHelper.Clamp(num990 + 0.05f, 0f, max);
            num991 += (float)Math.Sign(-12f - num991);
        }
        else if (num995 != -1)
        {
            num992 = projectile.AngleTo(Main.npc[num995].Center);
            num993 = 3;
            num990 = MathHelper.Clamp(num990 + 0.05f, 0f, max);
            num991 += (float)Math.Sign(6f - num991);
        }
        else if (projectile.ai[0] > 0f)
        {
            num992 = projectile.ai[1];
            num990 = MathHelper.Clamp(num990 + (float)Math.Sign(0.75f - num990) * 0.05f, 0f, max);
            num993 = 4;
            num991 += (float)Math.Sign(10f - num991);
        }
        else
        {
            num992 = ((player11.direction == 1) ? 0f : 3.1416028f);
            num990 = MathHelper.Clamp(num990 + (float)Math.Sign(0.75f - num990) * 0.05f, 0f, max);
            num991 += (float)Math.Sign(0f - num991);
            amount2 = 0.12f;
        }
        Vector2 value19 = num992.ToRotationVector2();
        num992 = Vector2.Lerp(f3.ToRotationVector2(), value19, amount2).ToRotation();
        projectile.localAI[0] = num992 + (float)num993 * ((float)Math.PI * 2f) + (float)Math.PI;
        projectile.localAI[1] = num991 + num990;
    }
    public static void AI_125(Projectile projectile)
    {
        Player player12 = Main.player[projectile.owner];
        if (Main.myPlayer == projectile.owner)
        {
            if (projectile.localAI[1] > 0f)
            {
                projectile.localAI[1]--;
            }
            if (player12.noItems || player12.CCed || player12.dead)
            {
                projectile.Kill();
            }
            else if (Main.mouseRight && Main.mouseRightRelease)
            {
                projectile.Kill();
                player12.mouseInterface = true;
                Main.blockMouse = true;
            }
            else if (!player12.channel)
            {
                if (projectile.localAI[0] == 0f)
                {
                    projectile.localAI[0] = 1f;
                }
                projectile.Kill();
            }
            else if (projectile.localAI[1] == 0f)
            {
                Vector2 vector161 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
                if (player12.gravDir == -1f)
                {
                    vector161.Y = (float)(Main.screenHeight - Main.mouseY) + Main.screenPosition.Y;
                }
                if (vector161 != projectile.Center)
                {
                    projectile.netUpdate = true;
                    projectile.Center = vector161;
                    projectile.localAI[1] = 1f;
                }
                if (projectile.ai[0] == 0f && projectile.ai[1] == 0f)
                {
                    projectile.ai[0] = (int)projectile.Center.X / 16;
                    projectile.ai[1] = (int)projectile.Center.Y / 16;
                    projectile.netUpdate = true;
                    projectile.velocity = Vector2.Zero;
                }
            }
            projectile.velocity = Vector2.Zero;
            Point point4 = new Vector2(projectile.ai[0], projectile.ai[1]).ToPoint();
            Point point5 = projectile.Center.ToTileCoordinates();
            int num1001 = Math.Abs(point4.X - point5.X);
            int num1002 = Math.Abs(point4.Y - point5.Y);
            int num1003 = Math.Sign(point5.X - point4.X);
            int num1004 = Math.Sign(point5.Y - point4.Y);
            Point point6 = default(Point);
            bool flag68 = false;
            bool flag69 = player12.direction == 1;
            int num1005;
            int num1006;
            int num1007;
            if (flag69)
            {
                point6.X = point4.X;
                num1005 = point4.Y;
                num1006 = point5.Y;
                num1007 = num1004;
            }
            else
            {
                point6.Y = point4.Y;
                num1005 = point4.X;
                num1006 = point5.X;
                num1007 = num1003;
            }
            for (int num1008 = num1005; num1008 != num1006; num1008 += num1007)
            {
                if (flag68)
                {
                    break;
                }
                if (flag69)
                {
                    point6.Y = num1008;
                }
                else
                {
                    point6.X = num1008;
                }
                if (WorldGen.InWorld(point6.X, point6.Y, 1))
                {
                    ITile tile3 = Main.tile[point6.X, point6.Y];
                }
            }
            if (flag69)
            {
                point6.Y = point5.Y;
                num1005 = point4.X;
                num1006 = point5.X;
                num1007 = num1003;
            }
            else
            {
                point6.X = point5.X;
                num1005 = point4.Y;
                num1006 = point5.Y;
                num1007 = num1004;
            }
            for (int num1009 = num1005; num1009 != num1006; num1009 += num1007)
            {
                if (flag68)
                {
                    break;
                }
                if (!flag69)
                {
                    point6.Y = num1009;
                }
                else
                {
                    point6.X = num1009;
                }
                if (WorldGen.InWorld(point6.X, point6.Y, 1))
                {
                    ITile tile3 = Main.tile[point6.X, point6.Y];
                }
            }
        }
        int num1010 = Math.Sign(player12.velocity.X);
        if (num1010 != 0)
        {
            player12.ChangeDir(num1010);
        }
        player12.heldProj = projectile.whoAmI;
        player12.SetDummyItemTime(2);
        player12.itemRotation = 0f;
    }
    public static void AI_126(Projectile projectile)
    {
        int num1011 = Math.Sign(projectile.velocity.Y);
        int num1012 = ((num1011 != -1) ? 1 : 0);
        if (projectile.ai[0] == 0f)
        {
            if (!Collision.SolidCollision(projectile.position + new Vector2(0f, (num1011 == -1) ? (projectile.height - 48) : 0), projectile.width, 48) && !Collision.WetCollision(projectile.position + new Vector2(0f, (num1011 == -1) ? (projectile.height - 20) : 0), projectile.width, 20))
            {
                projectile.velocity = new Vector2(0f, (float)Math.Sign(projectile.velocity.Y) * 0.001f);
                projectile.ai[0] = 1f;
                projectile.ai[1] = 0f;
                projectile.timeLeft = 60;
            }
            projectile.ai[1]++;
            if (projectile.ai[1] >= 60f)
            {
                projectile.Kill();
            }
        }
        if (projectile.ai[0] != 1f)
        {
            return;
        }
        projectile.velocity = new Vector2(0f, (float)Math.Sign(projectile.velocity.Y) * 0.001f);
        if (num1011 != 0)
        {
            int num1015 = 16;
            int num1016 = 320;
            if (projectile.type == 670)
            {
                num1016 -= (int)Math.Abs(projectile.localAI[1]) * 64;
            }
            for (; num1015 < num1016 && !Collision.SolidCollision(projectile.position + new Vector2(0f, (num1011 == -1) ? (projectile.height - num1015 - 16) : 0), projectile.width, num1015 + 16); num1015 += 16)
            {
            }
            if (num1011 == -1)
            {
                projectile.position.Y += projectile.height;
                projectile.height = num1015;
                projectile.position.Y -= num1015;
            }
            else
            {
                projectile.height = num1015;
            }
        }
        projectile.ai[1]++;
        if (projectile.type == 670 && projectile.owner == Main.myPlayer && projectile.ai[1] == 12f && projectile.localAI[1] < 3f && projectile.localAI[1] > -3f)
        {
            if (projectile.localAI[1] == 0f)
            {
                int num1017 = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Bottom + new Vector2(-50f, -10f), -Vector2.UnitY, projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
                Main.projectile[num1017].localAI[1] = projectile.localAI[1] - 1f;
                num1017 = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Bottom + new Vector2(50f, -10f), -Vector2.UnitY, projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
                Main.projectile[num1017].localAI[1] = projectile.localAI[1] + 1f;
            }
            else
            {
                int num1018 = Math.Sign(projectile.localAI[1]);
                int num1019 = Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Bottom + new Vector2(50 * num1018, -10f), -Vector2.UnitY, projectile.type, projectile.damage, projectile.knockBack, projectile.owner);
                Main.projectile[num1019].localAI[1] = projectile.localAI[1] + (float)num1018;
            }
        }
        if (projectile.ai[1] >= 60f)
        {
            projectile.Kill();
        }
        if (projectile.localAI[0] == 0f)
        {
            projectile.localAI[0] = 1f;
        }
        int num1026 = (int)(projectile.ai[1] / 60f * (float)projectile.height) * 3;
        if (num1026 > projectile.height)
        {
            num1026 = projectile.height;
        }
        Vector2 vector162 = projectile.position + ((num1011 == -1) ? new Vector2(0f, projectile.height - num1026) : Vector2.Zero);
        Vector2 vector163 = projectile.position + ((num1011 == -1) ? new Vector2(0f, projectile.height) : Vector2.Zero);
    }
    public static void AI_127(Projectile projectile)
    {
        float num1032 = 900f;
        if (projectile.type == 657)
        {
            num1032 = 300f;
        }
        if (projectile.soundDelay == 0)
        {
            projectile.soundDelay = -1;
            //SoundEngine.PlaySound(SoundID.Item82, projectile.Center);
        }
        projectile.ai[0]++;
        if (projectile.ai[0] >= num1032)
        {
            projectile.Kill();
        }
        if (projectile.type == 656 && projectile.localAI[0] >= 30f)
        {
            projectile.damage = 0;
            if (projectile.ai[0] < num1032 - 120f)
            {
                float num1033 = projectile.ai[0] % 60f;
                projectile.ai[0] = num1032 - 120f + num1033;
                projectile.netUpdate = true;
            }
        }
        float num1034 = 15f;
        float num1035 = 15f;
        Point point7 = projectile.Center.ToTileCoordinates();
        Collision.ExpandVertically(point7.X, point7.Y, out var topY, out var bottomY, (int)num1034, (int)num1035);
        topY++;
        bottomY--;
        Vector2 value20 = new Vector2(point7.X, topY) * 16f + new Vector2(8f);
        Vector2 value21 = new Vector2(point7.X, bottomY) * 16f + new Vector2(8f);
        Vector2 vector164 = Vector2.Lerp(value20, value21, 0.5f);
        Vector2 vector165 = new Vector2(0f, value21.Y - value20.Y);
        vector165.X = vector165.Y * 0.2f;
        projectile.width = (int)(vector165.X * 0.65f);
        projectile.height = (int)vector165.Y;
        projectile.Center = vector164;
        if (projectile.type == 656 && projectile.owner == Main.myPlayer)
        {
            bool flag70 = false;
            Vector2 center20 = Main.player[projectile.owner].Center;
            Vector2 top = Main.player[projectile.owner].Top;
            for (float num1036 = 0f; num1036 < 1f; num1036 += 0.05f)
            {
                Vector2 position = Vector2.Lerp(value20, value21, num1036);
                if (Collision.CanHitLine(position, 0, 0, center20, 0, 0) || Collision.CanHitLine(position, 0, 0, top, 0, 0))
                {
                    flag70 = true;
                    break;
                }
            }
            if (!flag70 && projectile.ai[0] < num1032 - 120f)
            {
                float num1037 = projectile.ai[0] % 60f;
                projectile.ai[0] = num1032 - 120f + num1037;
                projectile.netUpdate = true;
            }
        }
        if (!(projectile.ai[0] < num1032 - 120f))
        {
            return;
        }
    }
    public static void AI_128(Projectile projectile)
    {
        Color newColor4 = new Color(255, 255, 255);
        if (projectile.soundDelay == 0)
        {
            projectile.soundDelay = -1;
            SoundEngine.PlaySound(SoundID.Item60, projectile.Center);
        }
        if (projectile.localAI[0] == 0f)
        {
            projectile.localAI[0] = 0.8f;
            projectile.direction = 1;
            Point point8 = projectile.Center.ToTileCoordinates();
            projectile.Center = new Vector2(point8.X * 16 + 8, point8.Y * 16 + 8);
        }
        projectile.rotation = projectile.localAI[1] / 40f * ((float)Math.PI * 2f) * (float)projectile.direction;
        if (projectile.localAI[1] < 33f)
        {
            if (projectile.alpha > 0)
            {
                projectile.alpha -= 8;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
        }
        if (projectile.localAI[1] > 103f)
        {
            if (projectile.alpha < 255)
            {
                projectile.alpha += 16;
            }
            if (projectile.alpha > 255)
            {
                projectile.alpha = 255;
            }
        }
        if (projectile.alpha == 0)
        {
            Lighting.AddLight(projectile.Center, newColor4.ToVector3() * 0.5f);
        }
        if (projectile.localAI[1] < 33f || projectile.localAI[1] > 87f)
        {
            projectile.scale = projectile.Opacity / 2f * projectile.localAI[0];
        }
        projectile.velocity = Vector2.Zero;
        projectile.localAI[1]++;
        if (projectile.localAI[1] == 60f && projectile.owner == Main.myPlayer)
        {
            int num1042 = 30;
            if (Main.expertMode)
            {
                num1042 = 22;
            }
            Projectile.NewProjectile(projectile.GetProjectileSource_FromThis(), projectile.Center, Vector2.Zero, 657, num1042, 3f, projectile.owner);
        }
        if (projectile.localAI[1] >= 120f)
        {
            projectile.Kill();
        }
    }
    public static void AI_129(Projectile projectile)
    {
        float num1043 = 10f;
        float num1044 = 5f;
        float num1045 = 40f;
        int num1046 = 300;
        int num1047 = 180;
        if (projectile.type == 659)
        {
            num1046 = 420;
            num1047 = 240;
            num1043 = 3f;
            num1044 = 7.5f;
            num1045 = 1f;
            if (projectile.localAI[0] > 0f)
            {
                projectile.localAI[0]--;
            }
            if (projectile.localAI[0] == 0f && projectile.ai[0] < 0f && projectile.owner == Main.myPlayer)
            {
                projectile.localAI[0] = 5f;
                for (int num1048 = 0; num1048 < 200; num1048++)
                {
                    NPC nPC17 = Main.npc[num1048];
                    if (nPC17.CanBeChasedBy(projectile))
                    {
                        bool flag71 = projectile.ai[0] < 0f || Main.npc[(int)projectile.ai[0]].Distance(projectile.Center) > nPC17.Distance(projectile.Center);
                        if ((flag71 & (nPC17.Distance(projectile.Center) < 400f)) && (Collision.CanHitLine(projectile.Center, 0, 0, nPC17.Center, 0, 0) || Collision.CanHitLine(projectile.Center, 0, 0, nPC17.Top, 0, 0)))
                        {
                            projectile.ai[0] = num1048;
                        }
                    }
                }
                if (projectile.ai[0] >= 0f)
                {
                    projectile.timeLeft = num1046;
                    projectile.netUpdate = true;
                }
            }
            if (projectile.timeLeft > 30 && projectile.alpha > 0)
            {
                projectile.alpha -= 12;
            }
            if (projectile.timeLeft > 30 && projectile.alpha < 128 && Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
            {
                projectile.alpha = 128;
            }
            if (projectile.alpha < 0)
            {
                projectile.alpha = 0;
            }
            if (++projectile.frameCounter > 4)
            {
                projectile.frameCounter = 0;
                if (++projectile.frame >= 4)
                {
                    projectile.frame = 0;
                }
            }
            projectile.ai[1]++;
        }
        if (projectile.timeLeft > 2 && Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
        {
            projectile.timeLeft = 2;
        }
        int num1052 = (int)projectile.ai[0];
        if (num1052 >= 0 && Main.npc[num1052].active)
        {
            if (projectile.Distance(Main.npc[num1052].Center) > num1045)
            {
                Vector2 vector174 = projectile.DirectionTo(Main.npc[num1052].Center);
                if (vector174.HasNaNs())
                {
                    vector174 = Vector2.UnitY;
                }
                projectile.velocity = (projectile.velocity * (num1043 - 1f) + vector174 * num1044) / num1043;
            }
            return;
        }
        if (projectile.ai[0] == -1f && projectile.timeLeft > 5)
        {
            projectile.timeLeft = 5;
        }
        if (projectile.ai[0] == -2f && projectile.timeLeft > num1047)
        {
            projectile.timeLeft = num1047;
        }
        if (projectile.ai[0] >= 0f)
        {
            projectile.ai[0] = -1f;
            projectile.netUpdate = true;
        }
    }
    public static void AI_132(Projectile projectile)
    {
        if (projectile.localAI[1] == 0f)
        {
            if (projectile.localAI[0] == 0f)
            {
                SoundEngine.PlayTrackedSound(SoundID.DD2_DefeatScene, projectile.Center);
            }
            if (projectile.localAI[0] == 105f)
            {
                for (int num1053 = 0; num1053 < 20; num1053++)
                {
                    float num1054 = (float)num1053 / 20f;
                    Vector2 vector175 = new Vector2(Main.rand.NextFloat() * 10f, 0f).RotatedBy(num1054 * -(float)Math.PI + Main.rand.NextFloat() * 0.1f - 0.05f);
                    Gore gore2 = Gore.NewGoreDirect(projectile.Center + vector175 * 3f, vector175, Utils.SelectRandom<int>(Main.rand, 1027, 1028, 1029, 1030));
                    if (gore2.velocity.Y > 0f)
                    {
                        Gore gore = gore2;
                        gore.velocity *= -0.5f;
                    }
                    if (gore2.velocity.Y < -5f)
                    {
                        gore2.velocity.Y *= 0.8f;
                    }
                    gore2.velocity.Y *= 1.1f;
                    gore2.velocity.X *= 0.88f;
                }
            }
            if (!Main.dedServ)
            {
                if (!Filters.Scene["CrystalDestructionVortex"].IsActive())
                {
                    Filters.Scene.Activate("CrystalDestructionVortex", default(Vector2));
                }
                if (!Filters.Scene["CrystalDestructionColor"].IsActive())
                {
                    Filters.Scene.Activate("CrystalDestructionColor", default(Vector2));
                }
                float num1055 = Math.Min(1f, projectile.localAI[0] / 120f);
                Filters.Scene["CrystalDestructionColor"].GetShader().UseIntensity(num1055);
                Filters.Scene["CrystalDestructionVortex"].GetShader().UseIntensity(num1055 * 2f).UseProgress(0f)
                    .UseTargetPosition(projectile.Center);
            }
            if (projectile.localAI[0] == 120f)
            {
                projectile.localAI[0] = 0f;
                projectile.localAI[1]++;
            }
        }
        else if (projectile.localAI[1] == 1f)
        {
            if (!Main.dedServ)
            {
                float num1056 = projectile.localAI[0] / 300f;
                float num1057 = Math.Min(1f, projectile.localAI[0] / 150f);
                projectile.velocity.Y = num1057 * -0.25f;
                if (!Filters.Scene["CrystalDestructionVortex"].IsActive())
                {
                    Filters.Scene.Activate("CrystalDestructionVortex", default(Vector2));
                }
                if (!Filters.Scene["CrystalDestructionColor"].IsActive())
                {
                    Filters.Scene.Activate("CrystalDestructionColor", default(Vector2));
                }
                num1057 = 1f;
                Filters.Scene["CrystalDestructionColor"].GetShader().UseIntensity(num1057);
                Filters.Scene["CrystalDestructionVortex"].GetShader().UseIntensity(num1057 * 2f).UseProgress(0f)
                    .UseTargetPosition(projectile.Center);
            }
            if (projectile.localAI[0] == 300f)
            {
                projectile.localAI[0] = 0f;
                projectile.localAI[1]++;
            }
        }
        else if (projectile.localAI[1] == 2f)
        {
            float num1058 = projectile.localAI[0] / 300f;
            if (Main.netMode != 2)
            {
                Filters.Scene["CrystalDestructionVortex"].GetShader().UseIntensity(2f).UseProgress(num1058 * 30f);
            }
            projectile.velocity.Y -= 1f;
            if (projectile.localAI[0] == 60f)
            {
                projectile.localAI[0] = 0f;
                projectile.localAI[1]++;
            }
        }
        else if (projectile.localAI[1] == 3f)
        {
            if (!Main.dedServ)
            {
                Filters.Scene.Deactivate("CrystalDestructionVortex");
                Filters.Scene.Deactivate("CrystalDestructionColor");
            }
            projectile.Kill();
        }
        projectile.localAI[0]++;
    }
    public static void AI_133(Projectile projectile)
    {
        if (projectile.type == 673)
        {
            if (projectile.ai[0] == 70f)
            {
                SoundEngine.PlayTrackedSound(SoundID.DD2_SkeletonSummoned, projectile.Center);
            }
            projectile.ai[0]++;
            float opacity = 0f;
            if (projectile.ai[0] < 20f)
            {
                opacity = Utils.GetLerpValue(0f, 20f, projectile.ai[0], clamped: true);
            }
            else if (projectile.ai[0] < 60f)
            {
                opacity = 1f;
            }
            else if (projectile.ai[0] < 80f)
            {
                opacity = Utils.GetLerpValue(80f, 60f, projectile.ai[0], clamped: true);
            }
            else
            {
                projectile.Kill();
            }
            projectile.Opacity = opacity;
            _ = projectile.owner;
            _ = Main.myPlayer;
        }
        if (projectile.type != 674)
        {
            return;
        }
        if (projectile.ai[0] == 0f)
        {
            SoundEngine.PlayTrackedSound(SoundID.DD2_DarkMageHealImpact, projectile.Center);
        }
        projectile.ai[0]++;
        if (!(projectile.ai[0] >= 40f))
        {
            return;
        }
        for (int num1061 = 0; num1061 < 200; num1061++)
        {
            NPC nPC18 = Main.npc[num1061];
            if (nPC18.active && nPC18.damage >= 1 && nPC18.lifeMax >= 30 && !(projectile.Distance(nPC18.Center) > 1000f) && nPC18.type != 564 && nPC18.type != 565)
            {
                int num1062 = 500;
                int num1063 = nPC18.lifeMax - nPC18.life;
                if (num1062 > num1063)
                {
                    num1062 = num1063;
                }
                if (num1062 > 0)
                {
                    NPC nPC13 = nPC18;
                    nPC13.life += num1062;
                    nPC18.HealEffect(num1062);
                    nPC18.netUpdate = true;
                }
            }
        }
        projectile.Kill();
    }
}