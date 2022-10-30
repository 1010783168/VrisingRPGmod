using Il2CppSystem.IO;
using ProjectM;
using RPGMods.Commands;
using RPGMods.Utils;
using System.Collections.Generic;
using System.Text.Json;
using Unity.Entities;
using Wetstone.API;

namespace RPGMods.Systems
{
    public static class RebirthSystem
    {
        private static EntityManager entityManager = VWorld.Server.EntityManager;
        //最大转生等级
        public static int MaxRebirthLevel = 50;
        
        public static void Rebirth(Context ctx,int level = 1)
        {
            ulong platformId = ctx.Event.User.PlatformId;
            Database.player_experience[platformId] = 0;
            
            if (Database.rebirths.ContainsKey(platformId))
            {
                Database.rebirths[platformId] += level;
            }
            else
            {
                Database.rebirths.Add(platformId, level);
            }
            SaveRebirths();
            ExperienceSystem.SetLevel(ctx.Event.SenderCharacterEntity, ctx.Event.SenderUserEntity, platformId);
        }



        public static void Rebirth(ulong platformId , Entity P,Entity U)
        {
            Database.player_experience[platformId] = 0;

            if (Database.rebirths.ContainsKey(platformId))
            {
                Database.rebirths[platformId] += 1;
            }
            else
            {
                Database.rebirths.Add(platformId, 1);
            }
            SaveRebirths();
            ExperienceSystem.SetLevel(P, U, platformId);
        }


        public static List<ModifyUnitStatBuff_DOTS> GetBonusStats(int level)
        {
            List<ModifyUnitStatBuff_DOTS> list = new List<ModifyUnitStatBuff_DOTS>();
            ModifyUnitStatBuff_DOTS item = default(ModifyUnitStatBuff_DOTS);
            //最大生命
            item = default(ModifyUnitStatBuff_DOTS);
            item.StatType = UnitStatType.MaxHealth;
            item.Value = (float)level * 100;
            item.ModificationType = (ModificationType)3;
            item.Id = new ModificationId(0);
            list.Add(item);
            //法术伤害
            item = default(ModifyUnitStatBuff_DOTS);
            item.StatType = UnitStatType.SpellPower;
            item.Value = (float)level * 100;
            item.ModificationType = (ModificationType)3;
            item.Id = new ModificationId(0);
            list.Add(item);
            //物理伤害
            item = default(ModifyUnitStatBuff_DOTS);
            item.StatType = UnitStatType.PhysicalPower;
            item.Value = (float)level * 100;
            item.ModificationType = (ModificationType)3;
            item.Id = new ModificationId(0);
            list.Add(item);
            //物理吸血
            item = default(ModifyUnitStatBuff_DOTS);
            item.StatType = UnitStatType.PhysicalLifeLeech;
            item.Value = (float)((double)(float)level * 0.1);
            item.ModificationType = (ModificationType)3;
            item.Id = new ModificationId(0);
            list.Add(item);
            //法术吸血
            item = default(ModifyUnitStatBuff_DOTS);
            item.StatType = UnitStatType.SpellLifeLeech;
            item.Value = (float)((double)(float)level * 0.1);
            item.ModificationType = (ModificationType)3;
            item.Id = new ModificationId(0);
            list.Add(item);
            //物理暴击率
            item = default(ModifyUnitStatBuff_DOTS);
            item.StatType = UnitStatType.PhysicalCriticalStrikeChance;
            item.Value = (float)((double)(float)level * 0.1);
            item.ModificationType = (ModificationType)3;
            item.Id = new ModificationId(0);
            list.Add(item);
            //物理爆伤
            item = default(ModifyUnitStatBuff_DOTS);
            item.StatType = UnitStatType.PhysicalCriticalStrikeDamage;
            item.Value = (float)((double)(float)level * 0.1);
            item.ModificationType = (ModificationType)3;
            item.Id = new ModificationId(0);
            list.Add(item);
            //法术暴率
            item = default(ModifyUnitStatBuff_DOTS);
            item.StatType = UnitStatType.SpellCriticalStrikeChance;
            item.Value = (float)((double)(float)level * 0.1);
            item.ModificationType = (ModificationType)3;
            item.Id = new ModificationId(0);
            list.Add(item);
            //法术暴率
            item = default(ModifyUnitStatBuff_DOTS);
            item.StatType = UnitStatType.SpellCriticalStrikeDamage;
            item.Value = (float)((double)(float)level * 0.1);
            item.ModificationType = (ModificationType)3;
            item.Id = new ModificationId(0);
            list.Add(item);
            //攻击速度
            item = default(ModifyUnitStatBuff_DOTS);
            item.StatType = UnitStatType.PrimaryAttackSpeed;
            item.Value = (float)((double)(float)level * 0.1);
            item.ModificationType = (ModificationType)3;
            item.Id = new ModificationId(0);
            list.Add(item);

            return list;
        }
        public static int getLevel(ulong steamId)
        {
            int num;
            if(Database.rebirths.ContainsKey(steamId))
            {
                return Database.rebirths.TryGetValue(steamId, out num) ? num : 0;
            }
            return 0;
        }

        public static void SaveRebirths() => File.WriteAllText("BepInEx/config/RPGMods/Saves/rebirths.json", JsonSerializer.Serialize<Dictionary<ulong, int>>(Database.rebirths, Database.JSON_options));

        public static void LoadRebirths()
        {


            if (!File.Exists("BepInEx/config/RPGMods/Saves/rebirths.json"))
            {
                var stream = File.Create("BepInEx/config/RPGMods/Saves/rebirths.json");
                stream.Dispose();
            }
            string json = File.ReadAllText("BepInEx/config/RPGMods/Saves/rebirths.json");
            try
            {
                Database.rebirths = JsonSerializer.Deserialize<Dictionary<ulong, int>>(json, (JsonSerializerOptions)null);
                Plugin.Logger.LogWarning("rebirths DB Populated.");
            }
            catch
            {
                Database.rebirths = new Dictionary<ulong, int>();
                Plugin.Logger.LogWarning("rebirths DB Created.");
            }

        }
    }
}
