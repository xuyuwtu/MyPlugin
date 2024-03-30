Imports Microsoft.Xna.Framework

Namespace NPCStrengthen
    Public Class 属性信息
        Public 血量倍数 As Single = 3.0F
        Public 强化系数 As Single = 1.0F
        Public 生成间隔 As Single = 5.0F
        Public 锁定范围 As Short = 50
        Public 射弹组 As String = String.Empty
        Public 射弹信息 As 射弹信息()
        Public 射弹个数 As Short = -1
        Public 锁定个数 As Short = -1
    End Class
    Public Class 射弹信息
        Public ID As Short = 0
        Public 伤害 As Integer = 30
        Public 速度 As Single = 10.0F
        Public 使用AI As Boolean = False
        Public 仍然偏移 As Boolean = False
        Public 起始点 As Byte = 0
        Public 起始偏移 As Vector2 = Vector2.Zero
        Public 目标点 As Byte = 0
        Public 目标偏移 As Vector2 = Vector2.Zero
        Public AI As Single() = New Single(1) {0, 0}
        Public 血量条件 As New 血量条件
    End Class
    Public Class 公共
        Public 开启 As Boolean = False
        Public 排除列表 As Integer()
        Public 属性组 As String = String.Empty
        Public 属性信息 As 属性信息
    End Class
    Public Class 单独
        Public NPCID As Integer()
        Public 属性组 As String = String.Empty
        Public 属性信息 As 属性信息
    End Class
    Public Class 怪物组信息
        Public ID列表 As Integer()
        Public 属性组 As String = String.Empty
        Public 属性信息 As 属性信息
        Public 间隔 As Double
    End Class
    Public Class 血量条件
        Public 始值 As Double = 0
        Public 终值 As Double = 100
    End Class
End Namespace