赛车游戏框架，不仅仅是赛车类型游戏，其它类型游戏也可以拓展使用。
本框架的特点：
1.提供策划excel to protobuff for c# 的Mac和PC端Shell脚本工具，通过protobuff高效读取策划表。
2.有很好的Resource资源重用结构。
3.2D地图的手机适配，地图的运动逻辑设置，障碍车或NPC的刷兵机制实现。
4.战斗组件消息和广播消息框架，避免使用unity低效的sendMessage API,和提供unity没有的跨游戏对象广播机制。
5.基于物理碰撞触发战斗技能系统，技能特效和技能模板重用机制，也可以拓展为用空间分割定位攻击和受击者。
6.UI管理结构，能高效重用UI,和调整UI层级。
7.安全的异步事件结构（避免使用异步事件管理器模式），能有效的实现单位表现，技能模板，或UI动画效果。
PS: 这些机制其它类型的游戏也可以通用。
