using RPGMods.Systems;
using RPGMods.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Wetstone.API;
using ProjectM.Network;
using Il2CppSystem;


namespace RPGMods.Commands
{
    [Command("rh,rhinfo" + ", Usage = " < top > ", Description = "重置等级加强你的属性进行重生")]
     public static class Rhinfo
    {

        public static void Initialize(Context ctx)
        {
            if (!(ctx.Args[0].ToLower() == "top"))
                  return;
                if (Database.rebirths.Count > 0)
                {
                  ctx.Event.User.SendSystemMessage("<color=#ffffff>==========<color=#34ebe5>转生排行榜</color>==========</color>");
                  List<KeyValuePair<ulong, int>> list = Database.rebirths.OrderByDescending<KeyValuePair<ulong, int>, int>((Func<KeyValuePair<ulong, int>, int>)(x => x.Value)).ToList<KeyValuePair<ulong, int>>();
                   for (int index = 0; index < 10 && list.Count > index; ++index)
                   {
                      KeyValuePair<ulong, int> keyValuePair = list[index];
                      string nameFromSteamId = Helper.GetNameFromSteamID(keyValuePair.Key);
                      int num = keyValuePair.Value;
                      ctx.Event.User.SendSystemMessage(string.Format("<color=#34ebe5>#{0}°</color><color=#808080> {1} </color>- <color=#34ebe5>{2} Reset(s)</color>", (object)(index + 1), (object)nameFromSteamId, (object)num));
                  }
                }
                else
                Output.CustomErrorMessage(ctx, "Ninguém resetou ainda!");
    }
}
