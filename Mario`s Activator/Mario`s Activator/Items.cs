﻿using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK;

namespace Mario_s_Activator
{
    public class MItem
    {
        public MItem(int _itemId, float _range, bool _requiresTarget, bool _afterAA = false)
        {
            ItemID = _itemId;
            Range = _range;
            RequireTarget = _requiresTarget;
            AfterAA = _afterAA;
        }

        public int ItemID;
        public float Range;
        public bool RequireTarget;
        public bool AfterAA;

    }

    internal class Offensive : Helpers
    {
        public static List<MItem> OffensiveItems = new List<MItem>
        {
            //Bilgewater Cutlass
            new MItem(3144, 380,  true),
            //Blade of the ruined king
            new MItem(3153, 550,  true),
            //Tiamat
            new MItem(3077, 380,  false, true),
            //Hydra
            new MItem(3074, 380,  false, true),
            //Titanic
            new MItem(3053, Player.Instance.GetAutoAttackRange(),  false, true),
            //Youmuus
            new MItem(3142, 0,  false),
            //Hextech
            new MItem(3146, 0,  true),
            //Manamune
            new MItem(3004, 0,  false),
            //Frost Queens Claim
            new MItem(3092, 4500,  false),
        };

        public static void Cast()
        {
            foreach (var off in OffensiveItems)
            {
                var item = new Item(off.ItemID, off.Range);
                var target = TargetSelector.GetTarget(item.Range, DamageType.Mixed);

                if (target != null && item.IsOwned() && item.IsReady() && target.IsValidTarget())
                {
                    if (off.AfterAA)
                    {
                        if (Activator.CanPost)
                        {
                            if (off.RequireTarget)
                            {
                                Chat.Print("Casting an off item");
                                item.Cast(target);
                            }
                            else
                            {
                                Chat.Print("Casting an off item");
                                item.Cast();
                            }
                        }
                    }
                    else
                    {
                        if (off.RequireTarget)
                        {
                            Chat.Print("Casting an off item");
                            item.Cast(target);
                        }
                        else
                        {
                            Chat.Print("Casting an off item");
                            item.Cast();
                        }
                    }
                }
            }
        }
    }

    internal class Defensive : Helpers
    {
        public static List<MItem> DefensiveItems = new List<MItem>
        {
            //Zhonyas
            new MItem(3157, 0,  false),
            //Seraph
            new MItem(3040, 0,  false),
            //FaceOfTheMountain
            new MItem(3401, 0,  false),
            //Talisman
            new MItem(3069, 0,  false),
            //Solari
            new MItem(3190, 0,  false),
            //Randuins
            new MItem(3143, 480,  false),
            //Ohmwrecker
            new MItem(3056, 0,  false),
        };

        public static void Cast(Obj_AI_Base target, int percent, int count = 1)
        {
            foreach (var def in DefensiveItems)
            {
                var item = new Item(def.ItemID, def.Range);

                switch (def.ItemID)
                {
                    case 3056:
                        if (target.IsReceivingTurretAA() && item.IsReady() && item.IsOwned())
                        {
                            item.Cast();
                        }
                        return;
                    case 3143:
                        if (Player.Instance.CountEnemiesInRange(item.Range) >= count && item.IsReady() && item.IsOwned())
                        {
                            item.Cast();
                        }
                        return;
                }

                if (item.IsReady() && item.IsOwned() && target != null && target.IsInDanger(percent))
                {
                    if (def.RequireTarget)
                    {
                        Chat.Print("Casting an def item");
                        item.Cast(target);
                    }
                    else
                    {
                        Chat.Print("Casting an def item");
                        item.Cast();
                    }
                }
            }
        }
    }

    internal class Cleansers : Helpers
    {
        public static List<MItem> CleansersItems = new List<MItem>
        {
            //Mikael
            new MItem(3222, 750,  true),
            //Dervish Blade
            new MItem(3137, 0,  false),
            //Mercurial Scimitar
            new MItem(3139, 0,  false),
            //Quicksilver
            new MItem(3140, 0,  false),
        };

        public static void Cast(Obj_AI_Base target)
        {
            if (target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Snare) ||
                target.HasBuffOfType(BuffType.Blind))
            {
                foreach (var i in CleansersItems)
                {
                    var item = new Item(i.ItemID, i.Range);

                    if (item.IsReady() && item.IsOwned())
                    {
                        if (i.RequireTarget)
                        {
                            if (target.IsValidTarget(item.Range))
                            {
                                Chat.Print("Casting a cleanse item");
                                item.Cast(target);
                            }
                        }
                        else
                        {
                            Chat.Print("Casting a cleanse item");
                            item.Cast();
                        }
                    }
                }
            }
        }
    }

    internal class Consumables : Helpers
    {
        public static List<MItem> ComsumableItems = new List<MItem>
        {
            //HP Pot
            new MItem(2003, 0, false),
            //BISCOITO OU BOLOCHA
            new MItem(2010, 0, false),
            //Hunter`s pot
            new MItem(2032, 0, false),
            //Corrupt pot
            new MItem(2033, 0, false),
            //Refill pot
            new MItem(2031, 0, false),
            //Tank elixir
            new MItem(2138, 0, false),
            //AD Elixir
            new MItem(2140, 0, false),
            //AP Elixir
            new MItem(2139, 0, false),
        };

        public static void Cast(int percent)
        {
            foreach (var con in ComsumableItems)
            {
                var item = new Item(con.ItemID, con.Range);

                if (item.IsReady() && item.IsOwned() && Player.Instance.HealthPercent <= percent)
                {
                    switch (item.Id)
                    {
                        case ItemId.Elixir_of_Iron:
                            if (Player.Instance.CountAlliesInRange(800) >= 1 &&
                                Player.Instance.CountEnemiesInRange(800) >= 1 && item.IsReady() && item.IsOwned())
                            {
                                item.Cast();
                            }
                            return;
                        case ItemId.Elixir_of_Wrath:
                            if (Player.Instance.CountEnemiesInRange(Player.Instance.GetAutoAttackRange()) >= 1 &&
                                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && item.IsReady() &&
                                item.IsOwned())
                            {
                                item.Cast();
                            }
                            return;
                        case ItemId.Elixir_of_Sorcery:
                            if (Player.Instance.CountEnemiesInRange(Player.Instance.GetAutoAttackRange()) >= 1 &&
                                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && item.IsReady() &&
                                item.IsOwned())
                            {
                                item.Cast();
                            }
                            return;
                    }

                    Chat.Print("Casting an cons item");
                    item.Cast();
                }
            }
        }
    }
}
