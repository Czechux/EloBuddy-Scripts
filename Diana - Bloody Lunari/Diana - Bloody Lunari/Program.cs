using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using static SharpDX.Color;

namespace Diana___Bloody_Lunari
{
    internal class Program

    {
     //   private static Menu HorizonU, ActivatorM;
        private static Menu StartMenu, ComboMenu, DrawingsMenu, AHarrasM, ActivatorMenu, HarrasMenu, LCMenu, AntiSpellMenu, LastHitM, KSMenu;
        public static Spell.Skillshot _Q;
        public static Spell.Active _W;
        public static Spell.Active _E;
        public static Spell.Targeted _R;
        public static Spell.Targeted _Ignite;
        private static Spell.Targeted _RM;


        private static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (!_Player.ChampionName.Contains("Diana"))
            {
                return;
            }

            Chat.Print("Diana - Bloody Lunari Loaded!", System.Drawing.Color.Crimson); /*Color.Crimson);*/
            Chat.Print("Good luck and have fun, summoner.", System.Drawing.Color.DarkViolet); /*Color.DarkViolet);*/

            _Q = new Spell.Skillshot(SpellSlot.Q, 900, SkillShotType.Circular, 250, 1400, 200);
            _W = new Spell.Active(SpellSlot.W, 200);
            _E = new Spell.Active(SpellSlot.E, 450);
            _R = new Spell.Targeted(SpellSlot.R, 825);
            _Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);
            //Diana 
            StartMenu = MainMenu.AddMenu("Diana", "Diana");
            ComboMenu = StartMenu.AddSubMenu("Combo Settings", "Combo Settings");
            HarrasMenu = StartMenu.AddSubMenu("Harras Settings", "Harras Settings");
            AHarrasM = StartMenu.AddSubMenu("AutoHarras Settings", "AutoHarras Settings");
            LastHitM = StartMenu.AddSubMenu("Last Hit Settings", "Last Hit Settings");
            LCMenu = StartMenu.AddSubMenu("LaneClear Settings", "LaneClear Settings");
            KSMenu = StartMenu.AddSubMenu("KillSteall Settings", "KillSteall Settings");
            AntiSpellMenu = StartMenu.AddSubMenu("AntiSpell Settings", "AntiSpell Settings");
            ActivatorMenu = StartMenu.AddSubMenu("Activator Settings", "Activator Settings");
            DrawingsMenu = StartMenu.AddSubMenu("Drawings Settings", "Drawings Settings");

            // Horizon Utility
      //      HorizonU = MainMenu.AddMenu("Horizon Utility", "Horizon Utility");
       //     ActivatorM = HorizonU.AddSubMenu("Activator", "Activator");


            ActivatorMenu.AddGroupLabel("Activator Settings");
            ActivatorMenu.AddLabel("Use Summoner Spell");
            ActivatorMenu.Add("IGNI", new CheckBox("- Use Ignite if enemy is killable"));



            StartMenu.AddGroupLabel("Diana - Bloody Lunari");
            StartMenu.AddSeparator(2);
            StartMenu.AddGroupLabel("Made By");
            StartMenu.AddLabel("- Horizon");
            StartMenu.AddLabel("- Radi");

           // HorizonU.AddLabel("Welcomy to Utlility Beta made by Horizon!");


            ComboMenu.AddGroupLabel("Combo Settings");

            ComboMenu.AddLabel("Q Spell Settings");
            ComboMenu.Add("UseQ", new CheckBox("Use [Q]"));

            ComboMenu.AddSeparator(0);
            ComboMenu.AddLabel("W Spell Settings");
            ComboMenu.Add("UseW", new CheckBox("Use [W]"));

            ComboMenu.AddSeparator(0);
            ComboMenu.AddLabel("E Spell Settings");
            ComboMenu.Add("UseE", new CheckBox("Use [E] (when enemy out of AA range)"));

            ComboMenu.AddSeparator(0);
            ComboMenu.AddLabel("R Spell Settings");
            ComboMenu.Add("UseR", new CheckBox("Use [R] on Combo"));
            ComboMenu.AddSeparator(1);
            ComboMenu.Add("CR", new CheckBox("Use Second Ult in combo if enemy is killable"));
            ComboMenu.Add("Misaya", new KeyBind("Use Misaya Combo (R->Q->R)", false, KeyBind.BindTypes.HoldActive, 'T'));

            ComboMenu.AddSeparator(0);
            ComboMenu.AddLabel("R Spell Settings - tick only one option");
            ComboMenu.Add("Combos", new ComboBox("Use R", 0, "Only with Q Mark", "Always"));
            //   ComboMenu.Add("RONLY", new CheckBox("Use [R] (only when target got Q mark)"));
            // ComboMenu.Add("RNO", new CheckBox("Use [R] (always) ", false));

            HarrasMenu.AddGroupLabel("Harras Settings");

            HarrasMenu.AddLabel("Q Spell Settings");
            HarrasMenu.Add("UseQH", new CheckBox("Use [Q] for Harras"));

            HarrasMenu.AddSeparator(0);
            HarrasMenu.AddLabel("W Spell Settings");
            HarrasMenu.Add("UseWH", new CheckBox("Use [W] for Harras"));

            HarrasMenu.AddSeparator(0);
            HarrasMenu.AddLabel("E Spell Settings");
            HarrasMenu.Add("UseEH", new CheckBox("Use [E] for Harras"));

            AHarrasM.AddGroupLabel("Auto Harras Settings");

            AHarrasM.Add("AHQ", new CheckBox("Use Auto Harras"));

            AHarrasM.AddLabel("Q Spell Settings");
            AHarrasM.Add("QAO", new CheckBox("Use [Q] for Auto Harras"));

            AHarrasM.AddSeparator(0);
            AHarrasM.AddLabel("Mana settings");
            AHarrasM.Add("AHQM", new Slider("Minimum mana percentage for use [Q] in Auto Harras (%{0})", 50, 1));

            LastHitM.AddGroupLabel("Last Hit Settings");

            LastHitM.AddLabel("Q Spell Settings");
            LastHitM.Add("Qlh", new CheckBox("Use Q to Last Hit"));

            LastHitM.AddSeparator(0);
            LastHitM.AddLabel("W Spell Settings");
            LastHitM.Add("Elh", new CheckBox("Use W to Last Hit"));

            LastHitM.AddSeparator(0);
            LastHitM.AddLabel("Mana settings");
            LastHitM.Add("manalh", new Slider("Minimum mana percentage for use [Q] [W] in Auto Harras (%{0})", 50, 1));

            LCMenu.AddGroupLabel("Lane Clear Settings");

            LCMenu.Add("LCQ", new CheckBox("Use [Q] for Lane Clear"));
            LCMenu.Add("LCQM", new Slider("Minimum mana percentage for use [Q] in Lane Clear (%{0})", 50, 1));
            LCMenu.AddSeparator(1);
            LCMenu.Add("LCW", new CheckBox("Use [W] for Lane Clear"));
            LCMenu.Add("LCWM", new Slider("Minimum mana percentage for use [W] in Lane Clear (%{0})", 50, 1));
            LCMenu.AddSeparator(2);
            LCMenu.Add("JGCQ", new CheckBox("Use [Q] for Jungle clear"));
            LCMenu.Add("JGCQM", new Slider("Minimum mana percentage for use [Q] in Jungle Clear (%{0})", 50, 1));
            LCMenu.Add("JGCW", new CheckBox("Use [W] for Jungle clear"));
            LCMenu.Add("JGCWM", new Slider("Minimum mana percentage for use [W] in Jungle Clear (%{0})", 50, 1));

            KSMenu.AddGroupLabel("KillSteal Settings");

            KSMenu.AddLabel("Q Spell Settings");
            KSMenu.Add("KSQ", new CheckBox(" - KillSteal with Q"));

            KSMenu.AddSeparator(0);
            KSMenu.AddLabel("W Spell Settings");
            KSMenu.Add("KSW", new CheckBox(" - KillSteal with W"));

            KSMenu.AddSeparator(0);
            KSMenu.AddLabel("R Spell Settings");
            KSMenu.Add("KSR", new CheckBox(" - KillSteal with R"));

            AntiSpellMenu.AddGroupLabel("Anti Spell Settings");

            AntiSpellMenu.AddLabel("Use shield when playing against Morgana or Lux");

            AntiSpellMenu.Add("ASLux", new CheckBox("- Anti Lux Passive"));
            AntiSpellMenu.AddSeparator(0);
            AntiSpellMenu.Add("ASMorgana", new CheckBox("- Anti Morgana"));
            AntiSpellMenu.AddLabel("Mana settings");
            AntiSpellMenu.Add("ASPM", new Slider("Minimum mana percentage for Shield (%{0})", 80, 1));

            ActivatorMenu.AddGroupLabel("Activator Settings");
            ActivatorMenu.AddLabel("Use Summoner Spell");
            ActivatorMenu.Add("IGNI", new CheckBox("- Use Ignite if enemy is killable"));

            DrawingsMenu.AddGroupLabel("Drawing Settings");
            DrawingsMenu.AddLabel("Tick for enable/disable spell drawings");
            DrawingsMenu.Add("DQ", new CheckBox("- Draw [Q] range"));
            DrawingsMenu.AddSeparator(0);
            DrawingsMenu.Add("DW", new CheckBox("- Draw [W] range"));
            DrawingsMenu.AddSeparator(0);
            DrawingsMenu.Add("DE", new CheckBox("- Draw [E] range"));
            DrawingsMenu.AddSeparator(0);
            DrawingsMenu.Add("DR", new CheckBox("- Draw [R] range"));
            DrawingsMenu.AddSeparator(0);
            DrawingsMenu.Add("DM", new CheckBox("- Draw [R] Mode"));


            Game.OnUpdate += Game_OnUpdate;
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);

            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Harras();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                LaneClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear))
            {
                JungleClear();
            }
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                LastHit();
            }
            AHarra();
            KillSteal();
            Activator();
            AntiSpell();
            if (ComboMenu["Misaya"].Cast<KeyBind>().CurrentValue)
            {
                Misaya();
            }

        }

        private static void Game_OnTick(EventArgs args)
        {
        }

        public static void Misaya()
        {
            var UR = ComboMenu["UseR"].Cast<CheckBox>().CurrentValue;
            var UQ = ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue;
            var targetR = TargetSelector.GetTarget(_R.Range, DamageType.Physical);
            var targetQ = TargetSelector.GetTarget(_Q.Range, DamageType.Physical);
            if (targetR == null)
            {
                return;
            }
            if (targetQ == null)
            {
                return;
            }
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            var rDelay = _R.CastDelay = 500;
            if (UR && UQ)
            {
                if (targetQ.IsInRange(_Player, _Q.Range) && targetQ.IsValidTarget() && _Q.IsReady())
                {
                    _Q.Cast(targetQ.ServerPosition);
                }
                if (targetR.IsInRange(_Player, _R.Range) && targetR.IsValidTarget() && _R.IsReady() && !_Q.IsOnCooldown)
                {
                    _R.Cast(targetR);
                }
                if (targetR.IsInRange(_Player, _R.Range) && targetR.IsValidTarget() && _R.IsReady())
                {
                    _R.Cast(targetR);
                }

            }
        }


        private static void Drawing_OnDraw(EventArgs args)
        {
            var RQ = ComboMenu["Combos"].Cast<ComboBox>().SelectedIndex == 0;
            var RA = ComboMenu["Combos"].Cast<ComboBox>().SelectedIndex == 1;
            var DM = DrawingsMenu["DM"].Cast<CheckBox>().CurrentValue;
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Physical);
            if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue && _Q.IsLearned)
                
            {
                if (!_Q.IsReady())
                    Circle.Draw(Red, _Q.Range, _Player);

                else if (_Q.IsReady())
                {
                    Circle.Draw(Cyan, _Q.Range, _Player);
                }


            }
            if (DrawingsMenu["DW"].Cast<CheckBox>().CurrentValue && _W.IsLearned)
            {
                if (!_W.IsReady())
                    Circle.Draw(Red, _W.Range, _Player);

                else if (_W.IsReady())
                {
                    Circle.Draw(Cyan, _W.Range, _Player);
                }
            }
            if (DrawingsMenu["DE"].Cast<CheckBox>().CurrentValue && _E.IsLearned)
            {
                if (!_E.IsReady())
                    Circle.Draw(Red, _E.Range, _Player);

                else if (_E.IsReady())
                {
                    Circle.Draw(Red, _E.Range, _Player);
                }
            }
            if (DrawingsMenu["DR"].Cast<CheckBox>().CurrentValue && _R.IsLearned)
            {
                if (!_R.IsReady())
                    Circle.Draw(Red, _R.Range, _Player);

                else if (_R.IsReady())
                {
                    Circle.Draw(Cyan, _R.Range, _Player);
                }
            }

            if (RQ && DM)
            {
                Drawing.DrawText(Drawing.WorldToScreen(_Player.Position).X - 50,
                Drawing.WorldToScreen(_Player.Position).Y + 10,
                System.Drawing.Color.White,
                "     R Mode: Mark ");

            }
            if (RA && DM)
            {
                Drawing.DrawText(Drawing.WorldToScreen(_Player.Position).X - 50,
                 Drawing.WorldToScreen(_Player.Position).Y + 10,
                 System.Drawing.Color.White,
                 "     R Mode: Always ");
            }
        }
        public static void AHarra()
        {
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }

            {
                if (_Player.ManaPercent > AHarrasM["AHQM"].Cast<Slider>().CurrentValue &&
                    AHarrasM["AHQ"].Cast<CheckBox>().CurrentValue && AHarrasM["QAO"].Cast<CheckBox>().CurrentValue)
                {
                    var Qpred = _Q.GetPrediction(target);
                    if (Qpred.HitChance >= HitChance.High && target.IsValidTarget(_Q.Range))
                    {
                        if (target.IsInRange(_Player, _Q.Range) && _Q.IsReady())
                        {
                            _Q.Cast(target);
                        }
                    }
                }
            }
        }

        public static void AntiSpell()
        {
            var ManaAutoS = AntiSpellMenu["ASPM"].Cast<Slider>().CurrentValue;

            if (ManaAutoS < _Player.ManaPercent && _W.IsReady())
            {
                if (AntiSpellMenu["ASLux"].Cast<CheckBox>().CurrentValue)
                {
                    if (_Player.HasBuff("LuxLightBindingMis"))


                    {
                        _W.Cast();
                    }
                }
            }
            else if (ManaAutoS < _Player.ManaPercent && _W.IsReady())
            {
                if (AntiSpellMenu["ASMorgana"].Cast<CheckBox>().CurrentValue)
                {
                    if (_Player.HasBuff("Dark Binding"))
                    {
                        _W.Cast();
                    }
                }
            }
        }

        private static void LaneClear()
        {
            if (LCMenu["LCQ"].Cast<CheckBox>().CurrentValue)
            {
                if (!_Q.IsReady())
                {
                    return;
                }
                _Q.CastOnBestFarmPosition(2, 50);
            }

            if (_Player.ManaPercent > LCMenu["LCWM"].Cast<Slider>().CurrentValue && LCMenu["LCW"].Cast<CheckBox>().CurrentValue)
            {
                if (!_W.IsReady())
                {
                    return;
                }
                var minionsList = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget()).Where(minion => minion.IsInRange(Player.Instance, _W.Range)).ToList();

                if (minionsList.Count >= 3)
                {
                    _W.Cast();
                }
            }
        }

        private static void JungleClear()
        {
            var MHR = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(a => a.Distance(Player.Instance) <= _Q.Range).OrderBy(a => a.Health).FirstOrDefault();
            if (MHR != null)
            {
                if (_Player.ManaPercent > LCMenu["JGCQM"].Cast<Slider>().CurrentValue && LCMenu["JGCQ"].Cast<CheckBox>().CurrentValue && MHR.IsValidTarget(_Q.Range))
                {
                    _Q.Cast(MHR);
                }
            }

            if (_Player.ManaPercent > LCMenu["JGCQM"].Cast<Slider>().CurrentValue && LCMenu["JGCW"].Cast<CheckBox>().CurrentValue && MHR.IsValidTarget(_W.Range))
            {
                _W.Cast();
            }
        }
        private static void Combo()
        {
            var RQ = ComboMenu["Combos"].Cast<ComboBox>().SelectedIndex == 0;
            var RA = ComboMenu["Combos"].Cast<ComboBox>().SelectedIndex == 1;
            var CR = ComboMenu["CR"].Cast<CheckBox>().CurrentValue;
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            var targetR = TargetSelector.GetTarget(_R.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }
            if (targetR == null)
            {
                return;
            }
            if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue)
            {
                var Qpred = _Q.GetPrediction(target);
                var wheretocastt = _Player.Position.Extend(Player.Instance, Qpred.CastPosition.Distance(Player.Instance) + 125).To3DWorld();
                if (Qpred.HitChance >= HitChance.High && target.IsValidTarget(_Q.Range))
                {
                    if (!target.IsInRange(_Player, _Q.Range) && _Q.IsReady())
                    {
                        return;
                    }
                }
                {
                    _Q.CastStartToEnd(wheretocastt, Qpred.CastPosition);
                }
            }

            if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue && RQ)
            {
                if (target.HasBuff("DianaMoonlight") && target.IsInRange(_Player, _R.Range))
                {
                    _R.Cast(target);
                }
            }

            if (RA && ComboMenu["UseR"].Cast<CheckBox>().CurrentValue)
            {
                if (target.IsInRange(_Player, _R.Range) && target.IsValidTarget() && _R.IsReady())
                {
                    _R.Cast(target);
                }
            }
            if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
            {
                if (_E.IsInRange(target) && !Player.Instance.IsInAutoAttackRange(target))
                {
                    _E.Cast();
                }
            }

            if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue)
            {
                if (!target.IsInRange(_Player, _W.Range) && _W.IsReady())
                {
                    return;
                }
                {
                    _W.Cast();
                }
            }

            if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue && CR)
            {
                if (target.IsInRange(_Player, _R.Range) && _R.IsReady() && targetR.Health + targetR.AttackShield < _Player.GetSpellDamage(targetR, SpellSlot.R))
                {
                    _R.Cast(targetR);
                }
            }
        }


        private static void LastHit()
        {
            var MHR = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(a => a.Distance(Player.Instance) <= _Q.Range).OrderBy(a => a.Health).FirstOrDefault();
            if (MHR != null)
            {
                if (LastHitM["Qlh"].Cast<CheckBox>().CurrentValue && _Q.IsReady() && Player.Instance.ManaPercent > LastHitM["manalh"].Cast<Slider>().CurrentValue && MHR.IsValidTarget(_Q.Range) &&
                    Player.Instance.GetSpellDamage(MHR, SpellSlot.Q) >= MHR.TotalShieldHealth())

                {
                    _Q.Cast(MHR);
                }

                if (LastHitM["Elh"].Cast<CheckBox>().CurrentValue && _W.IsReady() && Player.Instance.GetSpellDamage(MHR, SpellSlot.W) >= MHR.TotalShieldHealth() &&
                    Player.Instance.ManaPercent > LastHitM["manalh"].Cast<Slider>().CurrentValue)
                {
                    _W.Cast();
                }
            }
        }

        private static void Harras()
        {
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }

            if (HarrasMenu["UseQH"].Cast<CheckBox>().CurrentValue)
            {
                var Qpred = _Q.GetPrediction(target);
                if (Qpred.HitChance >= HitChance.High && target.IsValidTarget(_Q.Range))
                {
                    if (!target.IsInRange(_Player, _Q.Range) && _Q.IsReady())
                    {
                        return;
                    }
                }
                {
                    _Q.Cast(target);
                }
            }
            if (HarrasMenu["UseEH"].Cast<CheckBox>().CurrentValue)
            {
                if (!target.IsInRange(_Player, _E.Range) && _E.IsReady())
                {
                    return;
                }
                {
                    _E.Cast();
                }
            }
            if (HarrasMenu["UseWH"].Cast<CheckBox>().CurrentValue)
            {
                if (!target.IsInRange(_Player, _W.Range) && _W.IsReady())
                {
                    return;
                }
                {
                    _W.Cast();
                }
            }
        }



        public static void KillSteal()
        {
            var targetQ = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            var targetW = TargetSelector.GetTarget(_W.Range, DamageType.Magical);
            var targetR = TargetSelector.GetTarget(_R.Range, DamageType.Magical);
            if (targetQ == null)
            {
                return;
            }
            if (targetW == null)
            {
                return;
            }
            if (targetR == null)
            {
                return;
            }
            if (KSMenu["KSQ"].Cast<CheckBox>().CurrentValue)
            {
                var Qpred = _Q.GetPrediction(targetQ);
                if (Qpred.HitChance >= HitChance.High && targetQ.IsValidTarget(_Q.Range))
                {
                    if (targetQ.Health + targetQ.AttackShield < _Player.GetSpellDamage(targetQ, SpellSlot.Q))
                    {
                        if (!targetQ.IsInRange(_Player, _Q.Range) && _Q.IsReady())
                        {
                            _Q.Cast(targetQ);
                        }
                    }
                }
                return;
            }

            if (KSMenu["KSW"].Cast<CheckBox>().CurrentValue)
            {
                if (targetW.Health + targetW.AttackShield < _Player.GetSpellDamage(targetW, SpellSlot.W))
                {
                    if (!targetW.IsInRange(_Player, _W.Range) && _W.IsReady())
                    {
                        return;
                    }
                }
                {
                    _W.Cast();
                }
            }

            if (KSMenu["KSR"].Cast<CheckBox>().CurrentValue)
            {
                if (targetR.Health + targetR.AttackShield < _Player.GetSpellDamage(targetR, SpellSlot.R))
                {
                    if (!targetR.IsInRange(_Player, _R.Range) && _R.IsReady())
                    {
                        return;
                    }
                }
                {
                    _R.Cast(targetR);
                }
            }




        }
        // end region of Diana

        //Utility region
        public static void Activator()
        {
            var target = TargetSelector.GetTarget(_Ignite.Range, DamageType.True);
            if (target == null)
            {
                return;
            }
            if (ActivatorMenu["IGNI"].Cast<CheckBox>().CurrentValue && _Ignite.IsReady() && target.IsValidTarget())

            {
                if (target.Health + target.AttackShield <
                    _Player.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite))
                {
                    _Ignite.Cast(target);
                }
            }
        }
    }
}