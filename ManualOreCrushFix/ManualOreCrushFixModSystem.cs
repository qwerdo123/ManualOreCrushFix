using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using HarmonyLib;
using ImmersiveOreCrush;
using System.Security.Cryptography.X509Certificates;

namespace ManualOreCrushFix
{
    [Harmony]
    public class ManualOreCrushFixModSystem : ModSystem
    {
        public static ICoreAPI api;
        public Harmony harmony;
        public override void Start(ICoreAPI api)
        {
            ManualOreCrushFixModSystem.api = api;
            if (!Harmony.HasAnyPatches("ManualOreCrushFix"))
            {
                harmony = new Harmony("ManualOreCrushFix");
                harmony.PatchAll();
            }

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ImmersiveOreCrush.ImmersiveOreCrush), "GetNuggetCountBasedOnQuality")]
        public static bool GetNuggetCount(ref int __result, string orePath, ImmersiveOreCrush.ImmersiveOreCrush __instance)
        {
            var config = ImmersiveOreCrush.ImmersiveOreCrush.config;
            api.Logger.Notification("start of patch");

            bool flag = orePath == "stone-chalk" || orePath == "stone-limestone" || orePath == "stone-marble";
            bool flagGoldTin = orePath.Contains("gold") || orePath.Contains("cassiterite") || orePath.Contains("franckeite") || orePath.Contains("teallite");
            bool flagSilver = orePath.Contains("silver") || orePath.Contains("freibergite");
            int result;

            if (flag)
            {
                result = 1;
            }
            else
            {
                bool flag2 = orePath.Contains("poor");
                if (flag2)
                {
                    if (flagGoldTin)
                    {
                        result = config.StandardPoorNuggetCount - 2;
                    }
                    else if (flagSilver)
                    {
                        result = config.StandardPoorNuggetCount - 1;
                    }
                    else
                    {
                        result = config.StandardPoorNuggetCount;
                    }
                }
                else
                {
                    bool flag3 = orePath.Contains("medium");
                    if (flag3)
                    {
                        if (flagGoldTin)
                        {
                            result = config.StandardMediumNuggetCount - 2;
                        }
                        else if (flagSilver)
                        {
                            result = config.StandardMediumNuggetCount - 1;
                        }
                        else
                        {
                            result = config.StandardMediumNuggetCount;
                        }
                    }
                    else
                    {
                        bool flag4 = orePath.Contains("rich");
                        if (flag4)
                        {
                            if (flagGoldTin)
                            {
                                result = config.StandardRichNuggetCount - 2;
                            }
                            else if (flagSilver)
                            {
                                result = config.StandardRichNuggetCount - 1;
                            }
                            else
                            {
                                result = config.StandardRichNuggetCount;
                            }
                        }
                        else
                        {
                            bool flag5 = orePath.Contains("bountiful");
                            if (flag5)
                            {
                                if (flagGoldTin)
                                {
                                    result = config.StandardBountifulNuggetCount - 3;
                                }
                                else if (flagSilver)
                                {
                                    result = config.StandardBountifulNuggetCount - 2;
                                }
                                else
                                {
                                    result = config.StandardBountifulNuggetCount;
                                }
                            }
                            else
                            {
                                result = 3;
                            }
                        }
                    }
                }
            }
            __result = result;
            return false;
        }
    }
}
