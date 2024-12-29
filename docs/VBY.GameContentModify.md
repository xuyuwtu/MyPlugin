# VBY.GameContentModify 游戏内容修改

- 作者: xuyuwtu
- 出处: [MyPlgin](https://github.com/xuyuwtu/MyPlugin)

- 爆改Terraria的一些东西...



## 指令

| 语法                          |          权限           |         说明         |
|-----------------------------|:---------------------:|:------------------:|
| /gcm         |  gcm.ctl  | 查看该指令的子命令菜单 |
| /gcm show main [-nd] [-code]           |  gcm.ctl   |      查看主配置文件       |
| /gcm show chest           |  gcm.ctl  |      查看箱子配置文件       |
| /gcm set <属性> <值> | gcm.ctl |      设置主配置文件的设置项       |
| /gcm save | gcm.ctl |      保存配置       |
| /gcm debug | gcm.ctl |      切换调试模式       |
| /reload          | gcm.ctl  |      重载配置文件       |


## 配置
> 主配置路径 Config/VBY.GameContentModify/MainConfig.json
```json5     
{
  "生成": {
    "城镇NPC": {
      "禁止生成的城镇NPC": [],
      "禁止自然生成": false,
      "晚上生成": false,
      "有入侵时生成": false,
      "日食时生成": false,
      "时间速率为0时仍然生成": false,
      "时间速率为0时的生成速率": 1,
      "旅商晚上不离开": false,
      "房屋忽略腐化检测": false,
      "忽略松露人环境检测": false
    },
    "自然生成陨石的几率": 50,
    "克苏鲁之眼": {
      "自然生成击败检测": true,
      "自然生成血量和防御检测": true,
      "自然生成未击败时生成的几率": 3,
      "自然生成已击败时生成的几率": 3,
      "自然生成城镇NPC数量检测检测": true
    },
    "机械Boss": {
      "自然生成的几率": 10,
      "自然生成击败检测": true,
      "自然生成克眼自然生成检测": true,
      "自然生成时三王为或者的关系": false,
      "自然生成时世界上是否有Boss检测": true
    },
    "召唤月亮领主等待时间": 720,
    "柱子死亡后月亮领主等待时间": 3600,
    "禁用世花花苞生成世花已存在检测": false
  },
  "入侵": {
    "未击败时自然生成哥布林入侵的几率": 3,
    "已击败时自然生成哥布林入侵的几率": 30,
    "困难模式且已击败时自然生成哥布林入侵的几率": 60,
    "未击败时自然生成海盗入侵的几率": 30,
    "已击败时自然生成海盗入侵的几率": 60
  },
  "血月": {
    "自然生成几率": 9,
    "十周年世界自然生成几率": 6,
    "自然生成玩家血量检测": true,
    "自然生成克眼检测": true,
    "清除日晷和月晷冷却": true
  },
  "世界": {
    "生长生命果需要的进度ID": [
      7,
      9
    ],
    "非困难模式时启用困难模式更新": false,
    "感染传播距离": 3,
    "不生成落星": false,
    "白天也生成落星": false,
    "时间速率为0时仍然更新": false,
    "时间速率为0时的更新速率": 1,
    "禁止液体更新": false,
    "宝石树可以在地表生长": false
  },
  "球体": {
    "生成Boss需要的数量": 3,
    "心脏掉落物品": [
      [
        {
          "type": 800,
          "stack": 1,
          "prefix": -1
        },
        {
          "type": 97,
          "stack": 100,
          "prefix": -1
        }
      ],
      [
        {
          "type": 1256,
          "stack": 1,
          "prefix": -1
        }
      ],
      [
        {
          "type": 802,
          "stack": 1,
          "prefix": -1
        }
      ],
      [
        {
          "type": 3062,
          "stack": 1,
          "prefix": -1
        }
      ],
      [
        {
          "type": 1290,
          "stack": 1,
          "prefix": -1
        }
      ]
    ],
    "暗影珠掉落物品": [
      [
        {
          "type": 96,
          "stack": 1,
          "prefix": -1
        },
        {
          "type": 97,
          "stack": 100,
          "prefix": -1
        }
      ],
      [
        {
          "type": 64,
          "stack": 1,
          "prefix": -1
        }
      ],
      [
        {
          "type": 162,
          "stack": 1,
          "prefix": -1
        }
      ],
      [
        {
          "type": 115,
          "stack": 1,
          "prefix": -1
        }
      ],
      [
        {
          "type": 111,
          "stack": 1,
          "prefix": -1
        }
      ]
    ]
  },
  "晶塔": {
    "危险检测": true,
    "花前神庙墙检测": true,
    "环境要求检测": true,
    "需要的城镇NPC数量": 2,
    "使用时检测距离X": 60,
    "使用时检测距离Y": 60
  },
  "网络消息": {
    "同步所有NPC": false,
    "同步所有物品": false,
    "同步所有射弹": false
  },
  "扩展": {
    "不发送的NetPacket类名称": [],
    "在每天开始的时候生成旅商": false,
    "在每天开始的时候生成旅商的几率": 10,
    "免疫熔岩的NPC": [],
    "免疫射弹的NPC": [],
    "渔夫在海边水里死亡时会生成猪鲨": false,
    "旅商每天刷新": false
  },
  "液体": {
    "破坏信息": {},
    "禁止熔岩破坏草方块": false,
    "启用液体破坏修改": false
  },
  "机械": {
    "300范围内的物品生成数量限制": 3,
    "800范围内的物品生成数量限制": 6,
    "世界范围内的物品生成数量限制": 10,
    "物品生成数量限制使用物品数量": false,
    "生成物品的冷却时间": 600,
    "200范围内的NPC生成数量限制": 3,
    "600范围内的NPC生成数量限制": 6,
    "世界范围内的NPC生成数量限制": 10,
    "生成NPC的冷却时间": 30,
    "巨石雕像冷却时间": 900,
    "飞镖机关冷却时间": 200,
    "尖球机关冷却时间": 300,
    "长矛机关冷却时间": 90,
    "热喷泉冷却时间": 200
  },
  "自然生成日食几率": 20,
  "屏蔽射弹破坏物块ID列表": [
    102
  ],
  "老旧摇摇箱开启后生成的物品ID列表": [],
  "毒气陷阱叠加": false,
  "毒气陷阱生成射弹ID": 1007,
  "毒气陷阱射弹伤害": 10,
  "日晷冷却天数": 8,
  "月晷冷却天数": 8,
  "附魔剑冢掉落物品信息": [
    {
      "RandomValue": 50,
      "Items": [
        {
          "stackEnd": null,
          "type": 4144,
          "stack": 1,
          "prefix": -1
        }
      ],
      "Else": [
        {
          "RandomValue": 1,
          "Items": [
            {
              "stackEnd": null,
              "type": 989,
              "stack": 1,
              "prefix": -1
            }
          ],
          "Else": null,
          "Continute": true
        }
      ],
      "Continute": true
    }
  ],
  "老旧摇摇箱在长者史莱姆解锁后依旧生成": false,
  "禁止蜂王和小蜜蜂伤害其他敌对NPC": false,
  "禁止掉出世界时杀死城镇NPC的圣诞节检查": false,
  "禁止敌怪NPC碰撞伤害城镇NPC": false,
  "禁用微光分解": false,
  "每天可以摇树的数量": 500,
  "摇树不掉落炸弹射弹": false,
  "罐子不掉落炸弹射弹": false,
  "禁止马桶生成射弹": false,
  "开始史莱姆雨的随机数的被除数": 450000.0,
  "史莱姆雨时生成史莱姆王需要击杀的史莱姆数量": 150,
  "史莱姆王被击败后史莱姆雨时生成史莱姆王需要击杀的史莱姆数量": 75,
  "史莱姆雨期间杀死史莱姆王不停止史莱姆雨": false,
  "禁止NPC死亡时生成熔岩": false,
  "禁止敌对雪球生成雪块": false,
  "禁止敌对雪球掉落物品": false,
  "禁止蚁狮的沙球生成沙块": false,
  "蚁狮的沙球会掉落物品": false,
  "禁止墓碑射弹放置墓碑并掉落墓碑物品": false,
  "药草生长改为随机": false,
  "药草随机生长几率": 50,
  "下雨基础随机数": 86000,
  "城镇NPC死亡时掉落墓碑": false,
  "城镇NPC不会淹死": false,
  "向导巫毒娃娃生成血肉墙时可以没有向导": false,
  "NPC不掉落旗帜": false,
  "破坏蜂巢不生成蜜蜂": false,
  "草药盆不生长杂草": false
}
```



## 进度值

| 进度值           | 数值 |
|-----------------|------|
| 无              | 0    |
| 史莱姆王        | 1    |
| 克眼            | 2    |
| 世吞克脑        | 3    |
| 蜂王            | 4    |
| 骷髅王          | 5    |
| 鹿角怪          | 6    |
| 困难模式（肉山）| 7    |
| 史莱姆皇后      | 8    |
| 任意机械BOSS    | 9    |
| 毁灭者          | 10   |
| 双子魔眼        | 11   |
| 机械骷髅王      | 12   |
| 世纪之花        | 13   |
| 石巨人          | 14   |
| 猪鲨            | 15   |
| 光女            | 16   |
| 教徒            | 17   |
| 日耀柱          | 18   |
| 星云柱          | 19   |
| 星璇柱          | 20   |
| 星尘柱          | 21   |
| 月总            | 22   |
| 衰木            | 23   |
| 南瓜王          | 24   |
| 常绿尖叫怪      | 25   |
| 圣诞坦克        | 26   |
| 冰雪女王        | 27   |
| 四柱            | 28   |
| 血月小丑        | 29   |
| 哥布林入侵      | 30   |
| 海盗入侵        | 31   |
| 火星暴乱        | 32   |


## 更新日志

```
1.9
添加:
PlanterBoxNotGrowingWeeds 草药盆不生长杂草
Spawn.TownNPC.HouseIgnoreEvil 房屋忽略腐化检测
Spawn.TownNPC.IgnoreTruffEnvCheck 忽略松露人环境检测
Spawn.TownNPC.DisableSpawnTownNPC 禁止生成的城镇NPC
Spawn.DisablePlanteraBulbSpawnPlanteraExistsCheck 禁用世花花苞生成世花已存在检测
Extension.TravelNPCRefreshOnStartDay 旅商每天刷新
修复：
建表错误导致的破坏方块掉落物异常问题（挖外露宝石掉魔刺BUG、宝石合成的方块挖了不掉落BUG）

羽学的配置文件：配置了老旧摇摇箱的物品

1.8
添加:
Spawn.TownNPC.TravelNPCNotLeavingAtNight 旅商晚上不离开
Spawn.TownNPC.HouseIgnoreEvil 房屋忽略腐化检测
Spawn.TownNPC.IgnoreTruffEnvCheck 忽略松露人环境检测
World.GemTreeCanGrowOverground 宝石树可以在地表生长
NotSpawnBeeWhenKillHive 破坏蜂巢不生成蜜蜂

1.7
NPCNotDropBanner NPC不掉落旗帜
GuideVoodooDollSpawnWOFCanWithoutGuide 向导巫毒娃娃生成血肉墙时可以没有向导
Spawn.TownNPC.DisableSpawn 禁止自然生成城镇NPC(包括老人)
Spawn.MoonLordCountdownOfSummon 召唤月亮领主等待时间
Spawn.MoonLordCountdownOfTowerKilled 柱子死亡后月亮领主等待时间
Extension.IgnoreLavaNPCs 免疫熔岩的NPC
Extension.IgnoreProjectileNPCs 免疫射弹的NPC
World.DisableLiquidUpdate 禁止液体更新（存在BUG）

1.6 fix：
Mech.ItemSpawnLimitOfRange600 600范围内的物品生成数量限制
改为：
Mech.ItemSpawnLimitOfRange800 800范围内的物品生成数量限制

1.6：
添加：
TownNPCDropTombstoneWhenDead 城镇死亡时掉落墓碑
TownNPCDrowningImmunity 城镇NPC不会淹死
Spawn.TownNPC.SpawnStillWhenTimeRateIsZero 时间速率为0时仍然生成
Spawn.TownNPC.SpawnTimeRateWhenTimeRateIsZero 时间速率为0时的生成速率
World.UpdateStillWhenTimeRateIsZero 时间速率为0时仍然更新
World.UpdateTimeRateWhenTimeRateIsZero 时间速率为0时的更新速率
Mech.ItemSpawnLimitOfRange300 300范围内的物品生成数量限制
Mech.ItemSpawnLimitOfRange600 600范围内的物品生成数量限制
Mech.ItemSpawnLimitOfWorld 世界范围内的物品生成数量限制
Mech.ItemSpawnLimitUseStack 物品生成数量限制使用物品数量
Mech.ItemSpawnCoolingTime 生成物品的冷却时间
Mech.NPCSpawnLimitOfRange200 200范围内的NPC生成数量限制
Mech.NPCSpawnLimitOfRange600 600范围内的NPC生成数量限制
Mech.NPCSpawnLimitOfWorld 世界范围内的NPC生成数量限制
Mech.NPCSpawnCoolingTime 生成NPC的冷却时间
Mech.BoulderStatueCoolingTime 巨石雕像冷却时间
Mech.DartTrapCoolingTime 飞镖机关冷却时间 —— [超级/毒液]飞镖/烈焰
Mech.SpikyBallTrapCoolingTime 尖球机关冷却时间
Mech.SpearTrapCoolingTime 长矛机关冷却时间
Mech.GeyserTrapCoolingTime 热喷泉冷却时间

1.5(FIx1):
修复 World.GrowLifeFruitRequireProgressIDs 配置无效且导致无法生成生命果的问题(不写任何值是可以生成的，不过那样也没有了进度限制)

1.5
改名：
World.hardUpdateWorldCheck 困难模式世界更新是否检测困难模式 
->World.EnableHardModeUpdateWhenNotHardMode 非困难模式时启用困难模式更新

更新Common以支持static的ActionHook

修复晶塔修改不生效的问题

新增.命令：
/gcm debug 切换调试模式，
切换后reload时会在控制台显示一些信息

新增.配置：
Liquid.DisableLavaDestoryGrassTile 
禁止熔岩破坏草方块
Liquid.EnableLiquidDeathModify 
启用液体破坏修改 —— 修改可能会导致奇怪问题，
为true时 Liquid.DestoryInfo 才有效，
仅对可被液体浸没的图格生效，固体物块不生效

Liquid.DestoryInfo 破坏信息
格式：
[{
     "图格ID": {
        "子ID": {
            "WaterDeath": true, //水破坏
            "LavaDeath": false //熔岩破坏
        }
    }
}]
当debug开启时，会显示是设置的TileObjectData还是Main.tile{Lava,Water}Death，
对于Main.tile{Lava,Water}Death，子ID无效，但是要写

World.GrowLifeFruitRequireProgressIDs 
生长生命果需要的进度ID —— 使用微光转换的那边的进度ID，全部ID对应的进度都满足时可生成生命果

进度值：
无 0
史莱姆王 1
克眼 2
世吞克脑 3
蜂王 4
骷髅王 5
鹿角怪 6
困难模式（肉山） 7
史莱姆皇后 8
任意机械BOSS 9
毁灭者 10
双子魔眼 11
机械骷髅王 12
世纪之花 13
石巨人 14
猪鲨 15
光女 16
教徒 17
日耀柱 18
星云柱 19
星璇柱 20
星尘柱 21
月总 22
衰木 23
南瓜王 24
常绿尖叫怪 25
圣诞坦克 26
冰雪女王 27
四柱 28
血月小丑 29
哥布林入侵 30
海盗入侵 31
火星暴乱 32

EnchantedSwordInStoneDropItemInfo 
附魔剑冢掉落物品信息 —— 可以稍微修改下附魔剑冢的掉落物
格式：
[
    {
      "RandomValue": 50, //几率 依旧为 1/RandomValue 的几率，不写时为1，必中
      "Items": [ //生成物品信息
        {
          "stackEnd": null, //数量的结尾，会生成 [stack-stackEnd] 之间数量的物品
为null或和stack相同时生成 stack数量的物品，不写为null
          "type": 4144,
          "stack": 1, //数量，不写为1
          "prefix": -1 //前缀，不写为-1
        }
      ],
      "Else": [{ //否则，如果上面的随机没成功，会尝试Else里的，不写为null
        "Items": [{"type": 989}],
        "Else": null,
        "Continute": true
      }],
      "Continute": true //如果有多个信息对象
这个决定是否进行下个对象试掉落
看父对象是否成功(会掉落物品)
如果成功会进入下一个对象尝试掉落，不写为true
    }
  ]

1.4
本插件目标为游戏内容的修改，全部的操作都在服务端，不要问我一些客户端的东西
有几个配置项，西江要说一下
修改：
不再有需要重启服务器才能生效的配置项

添加：
StaticStartRainBaseRandomNum 下雨基础随机数 —— 游戏每秒检查60次，想好改什么数

MemberSetter.json 
用于设置OTAPI一些静态成员，插件重载时会还原值，如果你会看代码的话，可以自己加新的可更改成员，
不局限于默认内容，目前只支持基本数字数据类型、布尔和字符串，及其对应数组

MemberName 用于设置成员名称，简短格式为 "<自定义成员名称>": "<成员完全名称>"，长格式为 "<自定义成员名称>": { "Name": "<成员完全名称>", ...}
MemberValue 用于设置成员值，非数组成员直接 "<自定义成员名称>": 成员值，数组成员为"自定义成员名称": { "<索引>": 成员值, ... }，长格式值名称为Value，MemberName长格式名称为Name
示例
{
    "MemberName": {
        "可掉落图格": {
            "Name": "Terraria.ID.TileID+Sets.Falling",
            "Tip": "可掉落图格，可以用来把沙子一类设置为false来防止掉落，不要给原本不可掉落的设置为true，那些并没有相应弹幕承载"
        },
        "物品免疫熔岩摧毁": "Terraria.ID.ItemID+Sets.IsLavaImmuneRegardlessOfRarity",
        "检测宽度": "Terraria.NPC.sWidth",
        "检测高度": "Terraria.NPC.sHeight"
    },
    "MemberValue": {
        "可掉落图格": {
            "53": {
                "Value": false,
                "Tip": "沙子"
            },
            "112": false
        },
        "物品免疫熔岩摧毁": {
            "1": true
        },
        "检测宽度": 1920,
        "检测高度": 1680
    }
}


【MainConfig.json】
屏蔽射弹破坏物块ID列表
这个配置项目是ServerOnly(仅由服务器产生的射弹)的，玩家的炸弹是客户端自己算的，然后转换为方块破坏发送到服务器(这也是ts物块破坏速度能限制炸弹的原因)，这里有个默认值是机械骷髅王的炸弹，为ftw世界准备的，ts也有一个屏蔽这个炸弹的，不过那是直接把射弹删掉了，其他的非玩家弹幕破坏方块的好像也没几个。

老旧摇摇箱在长者史莱姆解锁后依旧生成
这个配置是为了让世界上一些不可再生的饰品准备的，比如钉鞋什么的只能开箱子，箱子开完就没了，可以把物品ID加到列表(BoundTownSlimeOldSpawnItemIDs)里，并设置这个为true，这样玩家后来解救这个npc的时候就会出列表里的物品

GasTraps系列
类型：娱乐
叠加：如果里面有多个炸弹，会全都爆，伤害会叠加到生成的射弹
射弹Type：不用多说，不过毒气炸弹这个弹幕设置伤害是没用的，所以要用其他弹幕，比如炸药桶的
射弹伤害：伤害

渔夫在海边水里死亡时会生成猪鲨：肉后可用

不发送的NetPacket类名称
用来屏蔽一些包到服务器的，这个包名需要自己看代码，
本是某多世界服务器主城有晶塔，传送后把这个数据带到了其他世界，导致地图错乱而出的，晶塔的类名为NetTeleportPylonModule

可用来拦截的类名有：
NetAmbienceModule【星星夜、血月等事件播报模块】
NetBestiaryModule【怪物图鉴模块】
NetCreativePowerPermissionsModule【创意权限模块】
NetCreativePowersModule【创意力量模块】
NetCreativeUnlocksModule【创意解锁模块】
NetCreativeUnlocksPlayerReportModule【玩家解锁物品信息报告模块】
NetLiquidModule【液体模块】
NetParticlesModule【粒子模块】
NetPingModule【位置数据模块】
NetTeleportPylonModule【晶塔模块】
NetTextModule【聊天模块】

自然生成机械Boss时三王为或者的关系
类型：扩展
一次只能生成三王之一不给力？这个可以让三王都可能来
（来三王的游戏基础条件是要打破一个祭坛的，不要问我把几率改成1为什么不来)

SpawnTravel系列
类型：扩展
总是没旅商？开个让每天开始都可能来

hardUpdateWorldCheck(困难模式世界更新是否检测困难模式)
可以在肉前享受肉后的世界更新：腐化石块蔓延，蔓延距离增加，叶绿生成等

Orb.*DropItems(球体的掉落物品)
列表的第一个子列表是第一次打破必掉的(游戏也这么写的，我改成数组了而已)物品，后来的随机取其一

TeleportPylons.Reach{X,Y}(使用时检测距离Y)
虽然这个能扩展服务器那边的限制，但是客户端有自己的限制(离太远地图上点不了)，所以其实意义不大

NetMessage.SyncAll*
进入世界时同步的内容，为多世界子服所准备，不建议开启SyncAllProjectile，这1000个还是有点多了
产生原因是在其他世界的NPC和物品如果在新世界没有对于索引的对象顶替，那么对于客户端来说依旧可用(比如主城未强制开荒，玩家丢天顶出来然后切服，如果切的服没有任何物品或者刚好那个位置没有物品，那么客户端就可以去捡上个世界丢出的物品，NPC同理)，弹幕多销毁的也快，感觉不用多管。
感觉这样应该交给多世界端处理，而不是新世界全同步


【ItemTrasnfrom.json】
微光转换修改的配置文件，不包括微光分解
默认配置内容为原版微光特殊转换，谨慎删除
TransformInfos：可以写文本形式和json形式

文本形式：

源物品ID- 目标物品ID 进度ID
    "1326 5335 22",
    "779 5134 22",
    "3031-5364 22",【“-”表示互相，比如3031(无底水桶)与5364(无底微光桶)在微光内属于互转】
    "1533 1156 13",
    "1534 1571 13",
    "1535 1569 13",
    "1536 1260 13",
    "1537 1572 13",
    "4714 4607 13"

json形式 
{
 "src": 0,【丢入微光的物品ID】
 "dest": 0,【转换的物品ID】
 "pg": 0【进度ID】
}，

进度ID如下：
0 无
1 史莱姆王
2 克眼
3 世吞、克脑
4 蜂王
5 骷髅王
6 鹿角怪
7 困难模式（肉山）
8 史莱姆皇后
9 任意机械BOSS
10 毁灭者
11 双子眼
12 机械骷髅王
13 世花
14 石巨人
15 猪鲨
16 光女
17 教徒
18 日耀柱
19 星云柱
20 星璇柱
21 星尘柱
22 月总
23 衰木
24 南瓜王
25 常绿尖叫怪
26 圣诞坦克
27 冰雪女王
28 四柱
29 血月小丑
30 哥布林入侵
31 海盗入侵
32 火星暴乱

ClearIDs：
如果不想要一些原版的微光转换，可以把源物品ID写在这，会清除对应的转换


【ChestSpawn.json】
箱子生成的配置文件，默认配置内容为原版的三环境宝箱怪还有一个3金钥匙换普通宝箱怪，谨慎删除
  {
    "NPCType": 475, 【腐化世界生成的NPCID】
    "NpcStack": 1, 【NPC数量】
    "Action": 0,
    "ItemType": 3092,【物品ID】
    "ItemStack": 1, 【物品数量】
    "OnlyEqualItemStack": true,【仅当数量相同时启用】
    "CrimsonNPCType": 475【猩红世界生成的NPC ID】
  },
——————————————————
更新日志1.3
VBY.Common 有所更改，以修复插件热重载无法清理的问题，对于普通用户无差别
修复：
SpawnTravelNPCWhenStartDay(在每天开始的时候生成旅商的几率)使用随机数是SpawnMeteorRandomNum(自然生成陨石的几率)的问题

添加：
DisablePotDropBombProj
罐子不掉落炸弹射弹

DisableTombstoneProjPlaceTombstoneAndDropTombstoneItem
禁止墓碑射弹放置墓碑并掉落墓碑物品

HerbGrowIsRandom
药草生长改为随机,取消所有药草原本的生长和开花要求，
统一设置为随机判定，开花后不会再回到成熟阶段

HerbGrowRandomNumWhenIsRandom 药草随机生长几率
仅当HerbGrowIsRandom为true时有效

更新日志1.2.1
给指令/gcm，增加权限名：gcm.ctl

更新日志1.2
MainConfig.json大部分内容改为中文，移动部分内容
做不到：
落星物品白天不消失（服务器客户端双判）
添加：
World.NoSpawnFallenStar 不生成落星
World.SpawnFallenStarAtDay 白天也生成落星
DisableSpawnLavaWhenNPCDead 禁止NPC死亡时生成熔岩
DisableHostileSnowBallFromGeneratingSnowBlock 禁止敌对雪球生成雪块
DisableHostileSnowBallDropItem 禁止敌对雪球掉落物品
DisableSandBallOfAntlionFromGeneratingSandBlock 禁止蚁狮的沙球生成沙块
SandBallOfAntlionCanDropItem 蚁狮的沙球会掉落物品

移动：
SpawnDukeFishronWhenAnglerDeadAtSeaZoneInWater -> Extension.{name}
NotSendNetPacketIDs -> Extension.{name}
Spawn.SpawnTravelNPCWhenStartDay -> Extension.{name}
Spawn.SpawnTravelNPCWhenStartDayRandomNum -> Extension.{name}
Spawn.Eye{*} -> Spawn.EyeOfCthulhu.$1
Spawn.MechBoss{*} -> Spawn.MechBoss.$1


更新日志1.1
添加：
DisableShimmerDecrafte 禁用微光分解
MaxTreeShakes 每天最大摇树数量
DisableShakeTreeDropBombProj 摇树不掉落炸弹
DisableToiletSpawnProj 禁止马桶生成射弹
MoondialCooldown 日晷冷却天数
SundialCooldown 月晷冷却天数
DividendOfStartSlimeRainRandomNum 开始史莱姆雨的随机数的被除数 设为负数理论上是可以禁掉史莱姆雨，未测试
SpawnSlimeKingWhenSlimeRainNeedKillCount 史莱姆雨时生成史莱姆王需要击杀的史莱姆数量
SpawnSlimeKingWhenSlimeRainAndDownedSlimeKingNeedKillCount 史莱姆王被击败后史莱姆雨时生成史莱姆王需要击杀的史莱姆数量
NoStopSlimeRainAfterKillSlimeKingWhenSlimeRaining 史莱姆雨期间杀死史莱姆王不停止史莱姆雨
World.InfectionTransmissionDistance 感染传播距离

修复概率性事件设置为0时几率为100%的问题

```

## 反馈

- 优先发issued -> 共同维护的插件库：https://github.com/UnrealMultiple/TShockPlugin
- 次优先：TShock官方群：816771079
- 大概率看不到但是也可以：国内社区trhub.cn ，bbstr.net , tr.monika.love
