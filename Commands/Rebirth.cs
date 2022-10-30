using RPGMods.Systems;
using RPGMods.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Wetstone.API;
using ProjectM.Network;
using Il2CppSystem;

namespace RPGMods.Commands
{   //r set 0 pn 1 level 2       // r force pn 
    //[top] reset                X      0         1           2
    [Command("r" ,Usage = "r <reset>|<set> <player_name> <level>", Description = "重置等级加强你的属性进行重生.")]
    public static class Rebirth
    {
        public static void Initialize(Context ctx)
        {
 
            if (ctx.Args.Length == 0)
            {

                long platformId = (long)ctx.Event.User.PlatformId;
                int level1 = ExperienceSystem.getLevel((ulong)platformId);
                int level2 = RebirthSystem.getLevel((ulong)platformId);


 
                if (level1 >= ExperienceSystem.MaxLevel - 1)
                {
                    if (level2 < RebirthSystem.MaxRebirthLevel)
                    {
                        RebirthSystem.Rebirth(ctx);
                        ctx.Event.User.SendSystemMessage("<color=#a8323e>- - - 你 刚 刚 转 生 了 - - -</color>");
                        ctx.Event.User.SendSystemMessage("<color=#a8323e>- - - - - - - 转生属性加成 - - - - - - - - </color>");
                        ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 生命值<color=#00FF00>+0,5%</color></color>");
                        ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 物理攻击 <color=#00FF00>+0,5%</color></color>");
                        ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 法术攻击 <color=#00FF00>+0,5</color></color>");
                        ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 物理吸血 <color=#00FF00>+0,3%</color>");
                        ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 法术吸血 <color=#00FF00>+10</color>");
                        ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 攻击速度 <color=#00FF00>+10</color>");
                        ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 物理暴击率<color=#00FF00>+10</color>");
                        ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 法术暴击率<color=#00FF00>+10</color>");
                        ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 物理暴击<color=#00FF00>+10</color>");
                        ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 法术暴击<color=#00FF00>+10</color>");
                        ctx.Event.User.SendSystemMessage("<color=#a8323e>- - - - - - - - - - - - - - - - - - - - - -</color>");
                        ctx.Event.User.SendSystemMessage(string.Format("<color=#a8323e>转生等级: </color><color=#808080>{0}</color> -> <color=#00FF00>{1}</color>", (object)level2, (object)(level2 + 1)));
                    }
                    else
                        ctx.Event.User.SendSystemMessage(string.Format("<color=#a8323e>转生等级: </color><color=#ff0000>{0}</color> (<color=#ff0000>MAX</color>)", (object)level2));
                }
                else if (level2 < RebirthSystem.MaxRebirthLevel)
                {
                    ctx.Event.User.SendSystemMessage(string.Format("<color=#a8323e>当前转生等级: </color><color=#34ebe5>{0}</color>", (object)level2));
                    ctx.Event.User.SendSystemMessage(string.Format("<color=#a8323e>需要 LV <color=#34ebe5>{0}</color> 才能转生</color>", (object)ExperienceSystem.MaxLevel));
                    ctx.Event.User.SendSystemMessage("<color=#a8323e>- - - - - - -  每次转生加成 - - - - - - -</color>");
                    ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 生命值<color=#00FF00>+0,5%</color></color>");
                    ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 物理攻击 <color=#00FF00>+0,5%</color></color>");
                    ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 法术攻击 <color=#00FF00>+0,5</color></color>");
                    ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 技能吸血 <color=#00FF00>+0,3%</color>");
                    ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 法术吸血<color=#00FF00>+10</color>");
                    ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 攻击速度<color=#00FF00>+10</color>");
                    ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 物理暴击率<color=#00FF00>+10</color>");
                    ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 法术暴击率<color=#00FF00>+10</color>");
                    ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 物理暴击<color=#00FF00>+10</color>");
                    ctx.Event.User.SendSystemMessage("<color=#a8323e> <color=#00FF00>↗</color> 法术暴击<color=#00FF00>+10</color>");
                    ctx.Event.User.SendSystemMessage("<color=#a8323e>- - - - - - - - - - - - - - - - - - - - - -</color>");
                }
                else
                    ctx.Event.User.SendSystemMessage(string.Format("<color=#a8323e>最大转生等级: </color><color=#34ebe5>{0}</color> (<color=#ffffff>MAX</color>)", (object)level2));
            }
            else
            {
                if (ctx.Args.Length < 2)
                {
                    Output.InvalidArguments(ctx);
                    return;
                }
                    bool isAllowed = ctx.Event.User.IsAdmin || PermissionSystem.PermissionCheck(ctx.Event.User.PlatformId, "mastery_args");
                if (!isAllowed) return;
                string PlayerName = ctx.Args[1].ToLower();
                if (!Helper.FindPlayer(PlayerName, false, out var playerEntity, out var userEntity))
                {
                    Output.CustomErrorMessage(ctx, "未找到玩家.");
                    return;
                }
                ulong SteamID = Plugin.Server.EntityManager.GetComponentData<User>(userEntity).PlatformId;

                if (ctx.Args[0].ToLower().Equals("set"))
                {
                    if(Database.rebirths.ContainsKey(SteamID))
                    {
                        bool levelOK = float.TryParse(ctx.Args[2], out var level);
                        if(levelOK && level >= 0)
                        {
                            Database.rebirths[SteamID] = (int)level;
                            ctx.Event.User.SendSystemMessage("<color=#a8323e>已成功将</color>" + "<color=#a8323e>" + PlayerName.ToString() + "</color>" + "的等级设置为" + "<color=#a8323e>" + level.ToString() + "</color>");
                        }
                        else
                        {
                            Output.InvalidArguments(ctx);
                        }
                    }
                    else
                    {
                        bool levelOK = float.TryParse(ctx.Args[2], out var level);
                        if (levelOK && level >= 0)
                        {
                            Database.rebirths.Add(SteamID, (int)level);
                        }
                    }
                    RebirthSystem.SaveRebirths();
                    return;
                }
                if(ctx.Args[0].ToLower().Equals("force"))
                {
                    if(ctx.Args.Length == 2)
                    {
                        int Alevel1 = ExperienceSystem.getLevel((ulong)SteamID);
                        int Alevel2 = RebirthSystem.getLevel((ulong)SteamID);



                        if (Alevel1 >= ExperienceSystem.MaxLevel - 1)
                        {
                            if (Alevel2 < RebirthSystem.MaxRebirthLevel)
                            {
                                RebirthSystem.Rebirth(SteamID, playerEntity,userEntity);
                                ctx.Event.User.SendSystemMessage("<color=#a8323e>已成功将</color>" + "<color=#a8323e>" + PlayerName.ToString() + "</color>" + "<color=#a8323e>转生</color>");
                                ExperienceSystem.SaveEXPData();
                            }
                            
                        }
                        else
                        {
                            ctx.Event.User.SendSystemMessage("<color=#a8323e>" + PlayerName.ToString() + "</color>" + "<color=#a8323e>的等级不足转生</color>");
                            RebirthSystem.SaveRebirths();
                            return;
                        }
                        RebirthSystem.SaveRebirths();

                    }
                    else
                    {
                        RebirthSystem.SaveRebirths();
                        Output.InvalidArguments(ctx);
                        return;
                    }
                    
                    return;
                }
                //if (!(ctx.Args[0].ToLower() == "top"))
                //   return;
                //if (Database.rebirths.Count > 0)
                //{
                //  ctx.Event.User.SendSystemMessage("<color=#ffffff>==========<color=#34ebe5>TOP RESETS</color>==========</color>");
                //  List<KeyValuePair<ulong, int>> list = Database.rebirths.OrderByDescending<KeyValuePair<ulong, int>, int>((Func<KeyValuePair<ulong, int>, int>)(x => x.Value)).ToList<KeyValuePair<ulong, int>>();
                //   for (int index = 0; index < 10 && list.Count > index; ++index)
                //   {
                //      KeyValuePair<ulong, int> keyValuePair = list[index];
                //      string nameFromSteamId = Helper.GetNameFromSteamID(keyValuePair.Key);
                //      int num = keyValuePair.Value;
                //      ctx.Event.User.SendSystemMessage(string.Format("<color=#34ebe5>#{0}°</color><color=#808080> {1} </color>- <color=#34ebe5>{2} Reset(s)</color>", (object)(index + 1), (object)nameFromSteamId, (object)num));
                //  }
                //}
                //else
                //Output.CustomErrorMessage(ctx, "Ninguém resetou ainda!");
            }
        }
    }
}