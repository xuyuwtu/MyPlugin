Imports System.IO
Imports System.Timers
Imports System.Runtime.CompilerServices
Imports Microsoft.Xna.Framework

Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Imports Terraria
Imports TerrariaApi.Server

Imports TShockAPI

#Const TrVer = 36
Namespace NPCStrengthen
    <ApiVersion(2, 1)>
    Public Class NPCStrengthen
        Inherits TerrariaPlugin
#Region "插件信息"
        Public Overrides ReadOnly Property Author As String = "yu"
        Public Overrides ReadOnly Property Name As String = "VBY.NPCStrengthen"
        Public Overrides ReadOnly Property Description As String = "改变NPC生成后的血量"
#End Region
#Region "全局属性"
        <JsonProperty> Private 正常运行 As Boolean = True
        <JsonProperty> Private IsRegister As Boolean = False
        <JsonProperty> Private 公共属性 As 强化信息

        Private NPChandler As New HookHandler(Of NpcSpawnEventArgs)(AddressOf OnNpcSpawn)
        Private SetDefaultsByIdhandler As New [On].Terraria.NPC.hook_SetDefaults(AddressOf OnPreSetDefaultsById)
#End Region
#Region "只读属性"
        Private ReadOnly CommandArray As Command() = {New Command("vby.npcstrengthen.admin", AddressOf Cmd, "getnpctype")}
        <JsonProperty> Private ReadOnly 公共排除列表 As New List(Of Integer)

        <JsonProperty> Private ReadOnly 配置文件路径 As String = Path.Combine(TShock.SavePath, "定义怪物血量.json")

        <JsonProperty> Private ReadOnly 强化NPC列表 As New Dictionary(Of Integer, 强化信息)
        Private ReadOnly 活动强化NPC列表 As New List(Of 强化NPC)
        Private ReadOnly 活动强化NPC检测 As New Timer(5000)

        <JsonProperty> Private ReadOnly 怪物组列表 As New Dictionary(Of Integer, String)
        <JsonProperty> Private ReadOnly 怪物组信息列表 As New Dictionary(Of String, 组强化信息)
        Private ReadOnly 活动怪物组信息列表 As New Dictionary(Of String, 强化组NPC列表)
        Private ReadOnly 活动怪物组检测 As New Timer(5000)
        <JsonProperty> Private ReadOnly 属性组 As New Dictionary(Of String, 属性信息)
        <JsonProperty> Private ReadOnly 射弹组 As New Dictionary(Of String, 射弹信息())
#End Region
#Region "初始化和销毁函数"
        Public Sub New(game As Main)
            MyBase.New(game)
            If Not File.Exists(配置文件路径) Then
                Dim DefaultConfig As String = <string>{"属性组":{"普通":{"血量倍数":3,"生成间隔":5,"锁定范围":100,"射弹组":"普通","射弹个数":2,"锁定个数":2},"单长枪":{"血量倍数":3,"生成间隔":5,"锁定范围":100,"射弹组":"单长枪","射弹个数":-1,"锁定个数":-1},"2倍血":{"血量倍数":2,"生成间隔":5,"锁定范围":100,"射弹组":"单寒霜","射弹个数":-1,"锁定个数":-1}},"射弹组":{"普通":[{"ID":348,"伤害":20,"速度":3,"备注":"寒霜波"},{"ID":467,"伤害":20,"速度":3.5,"备注":"火球"},{"ID":257,"伤害":20,"速度":4,"备注":"寒霜光束"},{"ID":919,"伤害":20,"速度":4.5,"使用AI":true,"备注":"空灵长枪"}],"单长枪":[{"ID":919,"伤害":20,"速度":4.5,"使用AI":true,"备注":"空灵长枪"}],"单寒霜":[{"ID":257,"伤害":20,"速度":5,"备注":"寒霜光束"}]},"公共":{"强化列表":["4","50","35","36","87","266","222","113","114","125","126","127","245","247","248","262","327","345","370","395","440","454","459","491","657"],"属性组":"普通"},"单独":[{"NPCID":[128,129,130,131,396,397],"属性组":"单长枪"},{"NPCID":[636],"属性信息":{"血量倍数":5,"生成间隔":1,"锁定范围":100,"射弹信息":[{"ID":467,"伤害":20,"速度":3.5,"备注":"火球"}],"射弹个数":-1,"锁定个数":-1}}],"怪物组":{"毁灭者":{"ID列表":[134,135,136],"属性组":"单长枪","间隔":0.1},"世吞":{"ID列表":[13,14,15],"属性组":"单长枪","间隔":1}}}</string>.Value
                File.WriteAllText(配置文件路径, JObject.Parse(DefaultConfig).ToString())
            End If
        End Sub
        Public Overrides Sub Initialize()
            Commands.ChatCommands.AddRange(CommandArray)
            AddHandler TShockAPI.Hooks.GeneralHooks.ReloadEvent, AddressOf OnReload
            AddHandler 活动强化NPC检测.Elapsed, Sub()
                                              活动强化NPC列表.FindAll(Function(x) (Not x.NPC.active) OrElse (Not x.ID = x.NPC.netID)).ForEach(Sub(x) 活动强化NPC列表.Remove(x))
                                              If 活动强化NPC列表.Count = 0 Then 活动强化NPC检测.Stop()
                                          End Sub
            AddHandler 活动怪物组检测.Elapsed, Sub()
                                            活动怪物组信息列表.ForEach(Sub(x) x.Value.NPC列表.FindAll(Function(y) Not y.NPC.active).ForEach(Sub(y) x.Value.NPC列表.Remove(y)))
                                            活动怪物组信息列表.FindAllKey(Function(x) x.Value.NPC列表.Count = 0).ForEach(Sub(x) 活动怪物组信息列表.Remove(x))
                                            If 活动怪物组信息列表.Count = 0 Then 活动强化NPC检测.Stop()
                                        End Sub
            Dim str = ReadConfig()
            If Not 正常运行 Then
                Console.ForegroundColor = ConsoleColor.Red
                Console.WriteLine($"[{Name}]读取配置文件错误")
                Console.WriteLine(str)
                Console.ResetColor()
                Console.ReadKey()
            End If
        End Sub
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                CommandArray.ForEach(Sub(x) Commands.ChatCommands.Remove(x))
                RemoveHandler TShockAPI.Hooks.GeneralHooks.ReloadEvent, AddressOf OnReload
                注册事件(False)
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region
#Region "事件处理"
        Private Sub OnReload(args As TShockAPI.Hooks.ReloadEventArgs)
            Dim sender = If(args?.Player, TSPlayer.Server)
            If File.Exists(配置文件路径) Then
                Dim str = ReadConfig()
                If 正常运行 Then
                    sender.SendSuccessMessage($"[{Name}]重读配置完成")
                Else
                    If sender.Index = -1 Then
                        sender.SendErrorMessage($"[{Name}]重读配置文件出错")
                        sender.SendErrorMessage(str)
                    Else
                        sender.SendErrorMessage($"[{Name}]重读配置文件出错,详情查看日志")
                    End If
                    TShock.Log.Error(str)
                End If
            Else
                sender.SendErrorMessage($"[{Name}]配置文件不存在,保持不变")
            End If
        End Sub
        Private Sub PublicOnNpcSpawn(args As NpcSpawnEventArgs)
            Dim npc = Main.npc(args.NpcId)
            If Not 公共排除列表.Contains(npc.type) Then
                If 强化NPC列表.ContainsKey(npc.type) Then
                    If Not 活动强化NPC检测.Enabled Then 活动强化NPC检测.Start()
                    Dim 强化信息 = 强化NPC列表(npc.type)
                    With npc
                        If 强化信息.属性.血量倍数 <> 1.0F Then
                            .lifeMax = CInt(.lifeMax * 强化信息.属性.血量倍数)
                            .life = .lifeMax
                        End If
                        If 强化信息.射弹 IsNot Nothing AndAlso Not 强化信息.射弹.Length = 0 Then 活动强化NPC列表.Add(New 强化NPC(args.NpcId, Main.npc(args.NpcId), 强化信息))
                    End With
                ElseIf 怪物组列表.ContainsKey(Main.npc(args.NpcId).type) Then
                    If Not 活动怪物组检测.Enabled Then 活动怪物组检测.Start()
                    Dim 组名 = 怪物组列表(Main.npc(args.NpcId).type)
                    Dim 强化信息 = 怪物组信息列表(组名)
                    With npc
                        If 强化信息.属性.血量倍数 <> 1.0F Then
                            .lifeMax = CInt(.lifeMax * 强化信息.属性.血量倍数)
                            .life = .lifeMax
                        End If
                    End With
                    Dim value As 强化组NPC列表 = Nothing
                    If 活动怪物组信息列表.TryGetValue(组名, value) Then
                        value.Add(New 强化NPC(args.NpcId, Main.npc(args.NpcId), 强化信息, False))
                    Else
                        活动怪物组信息列表.Add(组名, New 强化组NPC列表(New 强化NPC(args.NpcId, Main.npc(args.NpcId), 强化信息, False), 强化信息.间隔))
                    End If
                Else
                    With npc
                        .lifeMax = CInt(.lifeMax * If(公共属性?.属性.血量倍数, 1.0F))
                        .life = .lifeMax
                    End With
                End If
                npc.StrikeNPC(1, 0, 0)
                TSPlayer.All.SendData(PacketTypes.NpcUpdate,, args.NpcId, 255, 7)
            End If
        End Sub
        Private Sub OnNpcSpawn(args As NpcSpawnEventArgs)
            If 强化NPC列表.ContainsKey(Main.npc(args.NpcId).type) Then
                If Not 活动强化NPC检测.Enabled Then 活动强化NPC检测.Start()
                Dim 强化信息 = 强化NPC列表(Main.npc(args.NpcId).type)
                With Main.npc(args.NpcId)
                    If 强化信息.属性.血量倍数 <> 1.0F Then
                        .lifeMax = CInt(.lifeMax * 强化信息.属性.血量倍数)
                        .life = .lifeMax
                        .StrikeNPC(1, 0, 0)
                        TSPlayer.All.SendData(PacketTypes.NpcUpdate,, args.NpcId, 255, 7)
                    End If
                    If (强化信息.射弹 IsNot Nothing) AndAlso (Not 强化信息.射弹.Length = 0) Then 活动强化NPC列表.Add(New 强化NPC(args.NpcId, Main.npc(args.NpcId), 强化信息))
                End With
            End If
            If 怪物组列表.ContainsKey(Main.npc(args.NpcId).type) Then
                If Not 活动怪物组检测.Enabled Then 活动怪物组检测.Start()
                Dim 组名 = 怪物组列表(Main.npc(args.NpcId).type)
                Dim 强化信息 = 怪物组信息列表(组名)
                With Main.npc(args.NpcId)
                    If 强化信息.属性.血量倍数 <> 1.0F Then
                        .lifeMax = CInt(.lifeMax * 强化信息.属性.血量倍数)
                        .life = .lifeMax
                        .StrikeNPC(1, 0, 0)
                        TSPlayer.All.SendData(PacketTypes.NpcUpdate,, args.NpcId, 255, 7)
                    End If
                End With
                Dim value As 强化组NPC列表 = Nothing
                If 活动怪物组信息列表.TryGetValue(组名, value) Then
                    value.Add(New 强化NPC(args.NpcId, Main.npc(args.NpcId), 强化信息, False))
                Else
                    活动怪物组信息列表.Add(组名, New 强化组NPC列表(New 强化NPC(args.NpcId, Main.npc(args.NpcId), 强化信息, False), 强化信息.间隔))
                End If
            End If
        End Sub
        Private Sub PublicOnPreSetDefaultsById(orig As [On].Terraria.NPC.orig_SetDefaults, self As NPC, Type As Integer, spawnparams As NPCSpawnParams)
            If Not 公共排除列表.Contains(Type) Then
                spawnparams = New NPCSpawnParams()
                If 强化NPC列表.ContainsKey(Type) Then
                    spawnparams.strengthMultiplierOverride = 强化NPC列表(Type).属性.强化系数
                ElseIf 怪物组列表.ContainsKey(Type) Then
                    spawnparams.strengthMultiplierOverride = 怪物组信息列表(怪物组列表(Type)).属性.强化系数
                Else
                    spawnparams.strengthMultiplierOverride = If(公共属性?.属性?.强化系数, 1.0F)
                End If
            End If
            orig(self, Type, spawnparams)
        End Sub
        Private Sub OnPreSetDefaultsById(orig As [On].Terraria.NPC.orig_SetDefaults, self As NPC, Type As Integer, spawnparams As NPCSpawnParams)
            If 强化NPC列表.ContainsKey(Type) Then
                spawnparams.strengthMultiplierOverride = 强化NPC列表(Type).属性.强化系数
            ElseIf 怪物组列表.ContainsKey(Type) Then
                spawnparams.strengthMultiplierOverride = 怪物组信息列表(怪物组列表(Type)).属性.强化系数
            End If
            orig(self, Type, spawnparams)
        End Sub
#End Region
#Region "命令处理"
        Sub Cmd(args As CommandArgs)
            With args
                If .Parameters.Count > 0 AndAlso Not File.Exists(Path.Combine(TShock.SavePath, .Parameters(0))) Then
                    Dim savepath = Path.Combine(TShock.SavePath, .Parameters(0))
                    File.WriteAllText(savepath, JsonConvert.SerializeObject(Me, Formatting.Indented))
                    .Player.SendInfoMessage($"[{Name}]数据已写入 {savepath}")
                Else
                    .Player.SendInfoMessage(JsonConvert.SerializeObject(Me, Formatting.Indented))
                End If
            End With
        End Sub
#End Region
#Region "辅助函数"
        Private Function ReadConfig() As String
            Try
                公共排除列表.Clear()
                强化NPC列表.Clear()
                活动强化NPC列表.ForEach(Sub(x) x.间隔事件.Stop())
                活动强化NPC列表.Clear()
                怪物组列表.Clear()
                怪物组信息列表.Clear()
                活动怪物组信息列表.ForEach(Sub(x) x.Value.NPC列表.ForEach(Sub(y) y.间隔事件.Stop()))
                活动怪物组信息列表.ForEach(Sub(x) x.Value.NPC列表.Clear())
                活动怪物组信息列表.Clear()
                属性组.Clear()
                射弹组.Clear()
                注册事件(False)
                Dim 配置文件 As JObject = JObject.Parse(File.ReadAllText(配置文件路径))
                If 配置文件("属性组") IsNot Nothing Then CType(配置文件("属性组"), JObject).Properties().ForEach(Sub(x) 属性组.Add(x.Name, JsonConvert.DeserializeObject(Of 属性信息)(x.Value.ToString())))
                If 配置文件("射弹组") IsNot Nothing Then CType(配置文件("射弹组"), JObject).Properties().ForEach(Sub(x) 射弹组.Add(x.Name, JsonConvert.DeserializeObject(Of 射弹信息())(x.Value.ToString())))
                Dim 公共 = 配置文件("公共")
                Dim 公共配置 = JsonConvert.DeserializeObject(Of 公共)(配置文件("公共").ToString())
                If 公共 IsNot Nothing AndAlso 公共配置 IsNot Nothing AndAlso 公共配置.开启 Then
                    公共排除列表.AddRange(公共配置.排除列表)
                    公共属性 = New 强化信息(0, If(公共配置.属性组 = String.Empty, 公共配置.属性信息, 属性组(公共配置.属性组)), 射弹组)
                    NPChandler = New HookHandler(Of NpcSpawnEventArgs)(AddressOf PublicOnNpcSpawn)
                    SetDefaultsByIdhandler = AddressOf PublicOnPreSetDefaultsById
                Else
                    NPChandler = New HookHandler(Of NpcSpawnEventArgs)(AddressOf OnNpcSpawn)
                    SetDefaultsByIdhandler = AddressOf OnPreSetDefaultsById
                End If
                Dim 单独配置 = 配置文件("单独")
                If 单独配置 IsNot Nothing Then JsonConvert.DeserializeObject(Of 单独())(单独配置.ToString()).ForEach(Sub(x) x.NPCID.ForEach(Sub(y) 强化NPC列表.Add(y, New 强化信息(y, If(x.属性组 = String.Empty, x.属性信息, 属性组(x.属性组)), 射弹组))))
                Dim 怪物组配置 = 配置文件("怪物组")
                If 怪物组配置 IsNot Nothing Then CType(怪物组配置, JObject).Properties().ForEach(Sub(x)
                                                                                           Dim 怪物组信息 = JsonConvert.DeserializeObject(Of 怪物组信息)(x.Value.ToString())
                                                                                           怪物组信息列表.Add(x.Name, New 组强化信息(0, If(怪物组信息.属性组 = String.Empty, 怪物组信息.属性信息, 属性组(怪物组信息.属性组)), 射弹组, 怪物组信息.间隔))
                                                                                           怪物组信息.ID列表.ForEach(Sub(y) 怪物组列表.Add(y, x.Name))
                                                                                       End Sub)
                正常运行 = True
                If Not 注册事件() Then 注册事件(True)
            Catch ex As Exception
                正常运行 = False
                If 注册事件() Then 注册事件(False)
                Return ex.ToString()
            End Try
            Return "正常"
        End Function
        Private Function 注册事件() As Boolean
            Return IsRegister
        End Function
        Private Sub 注册事件(value As Boolean)
            If value Then
                If Not IsRegister Then
                    ServerApi.Hooks.NpcSpawn.Register(Me, NPChandler)
                    AddHandler [On].Terraria.NPC.SetDefaults, SetDefaultsByIdhandler
                End If
            Else
                If IsRegister Then
                    ServerApi.Hooks.NpcSpawn.Deregister(Me, NPChandler)
                    RemoveHandler [On].Terraria.NPC.SetDefaults, SetDefaultsByIdhandler
                End If
            End If
            IsRegister = value
        End Sub
#End Region
    End Class
#Region "辅助类或结构"
    Public Class 强化NPC
        Public Index As Integer
        Public ID As Integer
        Public NPC As NPC
        Public 范围 As Short
        Public 射弹 As 射弹信息()
        Public 间隔 As Double
        Public 间隔事件 As Timer
        Public 射弹个数, 锁定个数 As Short
        Public Sub New(Index As Integer, NPC As NPC, value As 强化信息, Optional 开启事件 As Boolean = True)
            With Me
                .Index = Index
                .NPC = NPC
                .ID = value.ID
                .射弹 = value.射弹
                .范围 = value.属性.锁定范围
                .间隔 = value.属性.生成间隔
                .间隔事件 = New Timer(1000 * 间隔)
                AddHandler 间隔事件.Elapsed, AddressOf 发射射弹
                If 开启事件 Then .间隔事件.Start()
                .射弹个数 = If(value.属性.射弹个数 = 0S OrElse value.属性.射弹个数 < -1S, -1S, value.属性.射弹个数)
                .锁定个数 = If(value.属性.锁定个数 = 0S OrElse value.属性.锁定个数 < -1S, -1S, value.属性.锁定个数)
            End With
        End Sub
        Public Sub 发射射弹()
            If Not Me.NPC.active Then Me.间隔事件.Stop() : Return
            Dim SendPlayerList As New List(Of TSPlayer)(4)
            For i = 0 To TShock.Config.Settings.MaxSlots - 1
                Dim ply = TShock.Players(i)
                If ply Is Nothing Then Continue For
                If ply.Active AndAlso (Not ply.Dead) AndAlso 在范围内(ply, Me.NPC, 范围) Then SendPlayerList.Add(ply)
                If Not 锁定个数 = -1S AndAlso SendPlayerList.Count = 锁定个数 Then Exit For
            Next
            Dim SendProjectileList As New List(Of 射弹信息)
            Dim LifePercentage = Me.NPC.life / Me.NPC.lifeMax
            For Each i In 射弹
                'TSPlayer.All.SendInfoMessage($"血量:{LifePercentage} 大于始:{LifePercentage >= i.血量条件.始值} 小于终:{LifePercentage <= i.血量条件.终值}")
                If LifePercentage >= i.血量条件.始值 / 100 AndAlso LifePercentage <= i.血量条件.终值 / 100 Then SendProjectileList.Add(i)
            Next
            If 射弹个数 = -1S Then
                For Each proj As 射弹信息 In SendProjectileList
                    发射射弹2(proj, SendPlayerList)
                Next
            Else
                Dim rdm = New Random()
                If SendProjectileList.Count <> 0 Then
                    For i = 1S To 射弹个数
                        Dim proj = SendProjectileList(rdm.Next(0, SendProjectileList.Count - 1))
                        发射射弹2(proj, SendPlayerList)
                    Next
                End If
            End If
        End Sub
        Private Sub 发射射弹2(proj As 射弹信息, SendPlayerList As List(Of TSPlayer))
            Dim StartVector2 As Vector2
            Dim VelocityVecotr2 As Vector2
            For Each ply In SendPlayerList
                With proj
                    Select Case .起始点
                        Case 1
                            StartVector2 = ply.TPlayer.Center
                        Case Else
                            StartVector2 = Me.NPC.Center
                    End Select
                    StartVector2 += .起始偏移
                    Select Case .目标点
                        Case 1
                            VelocityVecotr2 = .目标偏移
                        Case Else
                            VelocityVecotr2 = 计算目标点(New Vector2(ply.TPlayer.Center.X - Me.NPC.Center.X, ply.TPlayer.Center.Y - Me.NPC.Center.Y), .速度)
                    End Select
                    If .使用AI Then
                        Dim targetVector2 As Vector2
                        If .仍然偏移 Then
                            targetVector2 = .目标偏移
                        Else
                            targetVector2 = Vector2.Zero
                        End If
                        ply.SendData(PacketTypes.ProjectileNew,, Projectile.NewProjectile(Projectile.GetNoneSource(), StartVector2, targetVector2, .ID, .伤害, 0,, VelocityVecotr2.ToRotation(), .AI(1)))
                    Else
                        ply.SendData(PacketTypes.ProjectileNew,, Projectile.NewProjectile(Projectile.GetNoneSource(), StartVector2, VelocityVecotr2, .ID, .伤害, 0,, .AI(0), .AI(1)))
                    End If
                End With
            Next
        End Sub
        Function 在范围内(玩家 As TSPlayer, 当前强化NPC As NPC, 锁定范围 As Integer) As Boolean
            Return Math.Abs((当前强化NPC.Center.X - 玩家.TPlayer.Center.X) / 16) < 锁定范围 AndAlso Math.Abs((当前强化NPC.Center.Y - 玩家.TPlayer.Center.Y) / 16) < 锁定范围
        End Function
        Function 计算目标点(目标点 As Vector2, 最大值 As Single) As Vector2
            Dim 目标X As Single, 目标Y As Single
            Dim 目标点绝对值 As New Vector2(Math.Abs(目标点.X)， Math.Abs(目标点.Y))
            If 目标点绝对值.X <> 最大值 OrElse 目标点绝对值.Y <> 最大值 Then
                If 目标点绝对值.X > 目标点绝对值.Y Then
                    Dim 比例 As Single = 最大值 / Math.Abs(目标点.X)
                    目标X = If(目标点.X > 0, 最大值, -最大值)
                    目标Y = 目标点.Y * 比例
                Else
                    Dim 比例 As Single = 最大值 / Math.Abs(目标点.Y)
                    目标Y = If(目标点.Y > 0, 最大值, -最大值)
                    目标X = 目标点.X * 比例
                End If
            End If
            Return New Vector2(目标X, 目标Y)
        End Function
    End Class
    Public Class 强化信息
        Public ID As Integer
        Public 属性 As 属性信息
        Public Property 射弹 As 射弹信息()
            Get
                Return 属性.射弹信息
            End Get
            Set(value As 射弹信息())
                属性.射弹信息 = value
            End Set
        End Property
        Public Sub New(ID As Integer, 属性 As 属性信息, 射弹 As Dictionary(Of String, 射弹信息()))
            With Me
                .ID = ID
                .属性 = 属性
                .属性.射弹信息 = If(属性.射弹组 = String.Empty, 属性.射弹信息, 射弹(属性.射弹组))
            End With
        End Sub
    End Class
    Public Class 组强化信息
        Inherits 强化信息
        Public 间隔 As Double
        Public 组ID As Integer()

        Public Sub New(ID As Integer, 属性 As 属性信息, 射弹 As Dictionary(Of String, 射弹信息()), 间隔 As Double)
            MyBase.New(ID, 属性, 射弹)
            Me.间隔 = 间隔
        End Sub
    End Class
    Public Class 强化组NPC列表
        Public NPC列表 As New List(Of 强化NPC)
        Public 间隔 As Double
        Public 间隔事件 As Timer
        Public rdm As Random
        Public Sub New(NPC As 强化NPC, 间隔 As Double)
            With Me
                .间隔 = 间隔
                .间隔事件 = New Timer(间隔 * 1000)
                AddHandler .间隔事件.Elapsed, AddressOf RandomSend
                .间隔事件.Start()
                .NPC列表.Add(NPC)
                .rdm = New Random
            End With
        End Sub
        Public Sub Add(NPC As 强化NPC)
            NPC列表.Add(NPC)
        End Sub
        Public Sub RandomSend()
            NPC列表(rdm.Next(0, NPC列表.Count - 1)).发射射弹()
            If NPC列表.Count = 0 Then 间隔事件.Stop()
        End Sub
    End Class
    'Public Class Pr
    '    Implements DataStructures.IEntitySource
    'End Class
    Module ExtModule
        <Extension>
        Function FindAllKey(Of TKey, TValue)(args As Dictionary(Of TKey, TValue), predicate As Predicate(Of KeyValuePair(Of TKey, TValue))) As List(Of TKey)
            Dim list As New List(Of TKey)
            For Each i In args
                If predicate(i) Then list.Add(i.Key)
            Next
            Return list
        End Function

    End Module
#End Region
End Namespace
