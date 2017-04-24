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


namespace Evelynn___Bloody_Shadow
{
    class Program
    {

        private static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

        private static Menu StartMenu, ComboMenu, DrawingsMenu, AHarrasM, LastHitM, LCMenu, HarrasMenu, FlyMenu, ActivatorMenu, KSMenu;
        public static Spell.Active _Q;
        public static Spell.Active _W;
        public static Spell.Targeted _E;
        public static Spell.Skillshot _R;
        public static Spell.Targeted _Ignite;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (!_Player.ChampionName.Contains("Evelynn"))
            {
                return;
            }
            Chat.Print("Bloody Evelynn - Loaded", System.Drawing.Color.Crimson);


            _Q = new Spell.Active(SpellSlot.Q, 500);
            _W = new Spell.Active(SpellSlot.W);
            _E = new Spell.Targeted(SpellSlot.E, 225);
            _R = new Spell.Skillshot(SpellSlot.R, 625, SkillShotType.Circular, 250, int.MaxValue, 300);
            _Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);


            StartMenu = MainMenu.AddMenu("Bloody Evelynn", "Bloody Evelynn");
            ComboMenu = StartMenu.AddSubMenu("Combo Settings", "Combo Settings");
            HarrasMenu = StartMenu.AddSubMenu("Harras Settings", "Harras Settings");
            AHarrasM = StartMenu.AddSubMenu("AutoHarras Settings", "AutoHarras Settings");
            LastHitM = StartMenu.AddSubMenu("Last Hit Settings", "Last Hit Settings");
            LCMenu = StartMenu.AddSubMenu("Clear Settings", "Clear Settings");
            FlyMenu = StartMenu.AddSubMenu("Flee Settings", "Flee Settings");
            KSMenu = StartMenu.AddSubMenu("KillSteal Settings", "KillSteal Settings");
            ActivatorMenu = StartMenu.AddSubMenu("Activator Settings", "Activator Settings");
            DrawingsMenu = StartMenu.AddSubMenu("Drawing Settings", "Drawing Settings");

            StartMenu.AddGroupLabel("Evelynn - Bloody Shadow");
            StartMenu.AddSeparator(2);
            StartMenu.AddGroupLabel("Made By");
            StartMenu.AddLabel("- Horizon");
            StartMenu.AddLabel("- Radi");

            ComboMenu.AddGroupLabel("Combo Settings");
            ComboMenu.Add("Combos", new ComboBox("Combo Logic", 0, "Q->E->R", "E->Q->R", "R->Q->E", "E->R->Q", "Q->R->E"));
            ComboMenu.AddSeparator(0);

            ComboMenu.AddLabel("Q Spell Settings");
            ComboMenu.Add("UseQ", new CheckBox("Use [Q]"));


            ComboMenu.AddSeparator(0);
            ComboMenu.AddLabel("W Spell Settings");
            ComboMenu.Add("UseW", new CheckBox("Use [W] in Combo"));
            ComboMenu.Add("WRange", new Slider("Minimum range for W", 525, 1, 800));
            ComboMenu.Add("Wn", new Slider("Minimum enemy in range for W", 1, 0, 5));

            ComboMenu.AddSeparator(0);
            ComboMenu.AddLabel("E Spell Settings");
            ComboMenu.Add("UseE", new CheckBox("Use [E] "));


            ComboMenu.AddSeparator(0);
            ComboMenu.AddLabel("R Spell Settings");
            ComboMenu.Add("UseR", new CheckBox("Use [R] in Combo"));
            ComboMenu.Add("Rn", new Slider("Minimum enemy for R", 1, 0, 5));
            ComboMenu.AddSeparator(0);

            HarrasMenu.AddGroupLabel("Harras Settings");
            HarrasMenu.AddLabel("Q Spell Settings");
            HarrasMenu.Add("UseQH", new CheckBox("Use [Q] for harras"));

            HarrasMenu.AddSeparator(0);
            HarrasMenu.AddLabel("E Spell Settings");
            HarrasMenu.Add("UseEH", new CheckBox("Use [E] for harras"));

            AHarrasM.AddGroupLabel("AutoHarras Settings");
            AHarrasM.AddLabel("Q Spell Settings");
            AHarrasM.Add("AHQ", new CheckBox("Use AutoHarras", false));
            AHarrasM.Add("QAO", new CheckBox("Use [Q] for AutoHarras"));
            AHarrasM.Add("AHQM", new Slider("Minimum mana percentage for use[Q] in AutoHarras(%{ 0})", 65, 1));

            LastHitM.AddGroupLabel("Last Hit Settings");
            LastHitM.AddLabel("Q Spell Settings");
            LastHitM.Add("Qlh", new CheckBox("Use Q to Last hit"));
            LastHitM.Add("manalh", new Slider("Minimum mana percentage for use [Q] in Last Hit(%{ 0})", 40, 1));

            KSMenu.AddGroupLabel("KillSteal Settings");
            KSMenu.AddLabel("Q Spell Settings");
            KSMenu.Add("KSQ", new CheckBox(" - KillSteal with Q"));
            KSMenu.AddSeparator(0);
            KSMenu.AddLabel("E Spell Settings");
            KSMenu.Add("KSE", new CheckBox(" - KillSteal with E"));
            KSMenu.AddSeparator(0);
            KSMenu.AddLabel("R Spell Settings");
            KSMenu.Add("KSR", new CheckBox(" - KillSteal with R", false));

            LCMenu.AddGroupLabel("LaneClear Settings");
            LCMenu.Add("LCQ", new CheckBox("Use [Q] for Lane Clear"));
            LCMenu.Add("LCQM", new Slider("Minimum mana percentage for use [Q] in Lane Clear (%{0})", 30, 1));
            LCMenu.AddSeparator(1);
            LCMenu.Add("LCW", new CheckBox("Use [E] for Lane Clear"));
            LCMenu.Add("LCWM", new Slider("Minimum mana percentage for use [E] in Lane Clear (%{0})", 30, 1));
            LCMenu.AddSeparator(2);
            LCMenu.Add("JGCQ", new CheckBox("Use [Q] for Jungle clear"));
            LCMenu.Add("JGCQM", new Slider("Minimum mana percentage for use [Q] in Jungle Clear (%{0})", 30, 1));
            LCMenu.Add("JGCW", new CheckBox("Use [E] for Jungle clear"));
            LCMenu.Add("JGCWM", new Slider("Minimum mana percentage for use [E] in Jungle Clear (%{0})", 30, 1));

            FlyMenu.AddGroupLabel("Flee Settings");
            FlyMenu.AddLabel("Tick for enable/disable skill usage in flee");
            FlyMenu.AddSeparator(0);
            FlyMenu.AddLabel("Q Spell Settings");
            FlyMenu.Add("UseQf", new CheckBox("Use Q to Flee"));

            ActivatorMenu.AddGroupLabel("Activator Settings");
            ActivatorMenu.AddLabel("Use Summoner Spell");
            ActivatorMenu.Add("IGNI", new CheckBox("- Use Ignite if enemy is killable"));

            DrawingsMenu.AddGroupLabel("Drawing Settings");
            DrawingsMenu.AddLabel("Tick for enable/disable Draw");
            DrawingsMenu.Add("DQ", new CheckBox("- Draw [Q] range"));
            DrawingsMenu.AddSeparator(0);
            DrawingsMenu.Add("DE", new CheckBox("- Draw [E] range"));
            DrawingsMenu.AddSeparator(0);
            DrawingsMenu.Add("DR", new CheckBox("- Draw [R] range"));
            DrawingsMenu.AddSeparator(0);
            DrawingsMenu.Add("DRpred", new CheckBox("- Draw Ultimate prediction"));

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
            if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Harass))
            {
                Harass();
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
            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Flee))
            {
                Flee();
            }
            AHarra();
            KillSteal();
            Activator();
        }

        private static void Game_OnTick(EventArgs args)
        {

        }

        private static void Combo()
        {
            var QER = ComboMenu["Combos"].Cast<ComboBox>().SelectedIndex == 0;
            var EQR = ComboMenu["Combos"].Cast<ComboBox>().SelectedIndex == 1;
            var RQE = ComboMenu["Combos"].Cast<ComboBox>().SelectedIndex == 2;
            var ERQ = ComboMenu["Combos"].Cast<ComboBox>().SelectedIndex == 3;
            var QRE = ComboMenu["Combos"].Cast<ComboBox>().SelectedIndex == 4;
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            if (target == null)
                return;
            var rtarget = TargetSelector.GetTarget(_R.Range, DamageType.Magical);


            if (QER)
            {
                if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue)
                {
                    var enemies = EntityManager.Enemies.Where(x => x.IsInRange(Player.Instance.Position, ComboMenu["WRange"].Cast<Slider>().CurrentValue));
                    if (enemies != null)
                    {
                        if (enemies.Count() >= ComboMenu["Wn"].Cast<Slider>().CurrentValue)
                        {
                            _W.Cast();
                        }
                    }
                }

                if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _Q.Range) && _Q.IsReady())
                        return;
                    {
                        _Q.Cast(target);
                    }
                }


                if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _E.Range) && _E.IsReady())
                        return;
                    {
                        _E.Cast(target);
                    }
                }


                if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue)
                {
                    var Enemys = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(_Player.Position, 625));
                    if (Enemys != null)
                    {
                        if (Enemys.Count() >= ComboMenu["Rn"].Cast<Slider>().CurrentValue)
                        {
                            if (_R.IsReady())
                            {
                                _R.Cast(rtarget);
                            }
                        }

                    }
                }
            }
            if (EQR)
            {

                if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue)
                {
                    var enemies = EntityManager.Enemies.Where(x => x.IsInRange(Player.Instance.Position, ComboMenu["WRange"].Cast<Slider>().CurrentValue));
                    if (enemies != null)
                    {
                        if (enemies.Count() >= ComboMenu["Wn"].Cast<Slider>().CurrentValue)
                        {
                            _W.Cast();
                        }
                    }
                }
                if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _E.Range) && _E.IsReady())
                        return;
                    {
                        _E.Cast(target);
                    }
                }
                if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _Q.Range) && _Q.IsReady())
                        return;
                    {
                        _Q.Cast(target);
                    }
                }
                if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue)
                {
                    var Enemys = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(_Player.Position, 625));
                    if (Enemys != null)
                    {
                        if (Enemys.Count() >= ComboMenu["Rn"].Cast<Slider>().CurrentValue)
                        {
                            if (_R.IsReady())
                            {
                                _R.Cast(rtarget);
                            }
                        }
                    }
                }


            }
            if (RQE)
            {
                if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue)
                {
                    var enemies = EntityManager.Enemies.Where(x => x.IsInRange(Player.Instance.Position, ComboMenu["WRange"].Cast<Slider>().CurrentValue));
                    if (enemies != null)
                    {
                        if (enemies.Count() >= ComboMenu["Wn"].Cast<Slider>().CurrentValue)
                        {
                            _W.Cast();
                        }
                    }
                }
                if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue)
                {
                    var Enemys = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(_Player.Position, 625));
                    if (Enemys != null)
                    {
                        if (Enemys.Count() >= ComboMenu["Rn"].Cast<Slider>().CurrentValue)
                        {
                            if (_R.IsReady())
                            {
                                _R.Cast(rtarget);
                            }
                        }
                    }
                }
                if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _Q.Range) && _Q.IsReady())
                        return;
                    {
                        _Q.Cast(target);
                    }
                }

                if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _E.Range) && _E.IsReady())
                        return;
                    {
                        _E.Cast(target);
                    }
                }
            }

            if (ERQ)
            {
                if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue)
                {
                    var enemies = EntityManager.Enemies.Where(x => x.IsInRange(Player.Instance.Position, ComboMenu["WRange"].Cast<Slider>().CurrentValue));
                    if (enemies != null)
                    {
                        if (enemies.Count() >= ComboMenu["Wn"].Cast<Slider>().CurrentValue)
                        {
                            _W.Cast();
                        }
                    }
                }
                if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _E.Range) && _E.IsReady())
                        return;
                    {
                        _E.Cast(target);
                    }
                }
                if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue)
                {
                    var Enemys = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(_Player.Position, 625));
                    if (Enemys != null)
                    {
                        if (Enemys.Count() >= ComboMenu["Rn"].Cast<Slider>().CurrentValue)
                        {
                            if (_R.IsReady())
                            {
                                _R.Cast(rtarget);
                            }
                        }
                    }
                }
                if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _Q.Range) && _Q.IsReady())
                        return;
                    {
                        _Q.Cast(target);
                    }
                }
            }
            if (QRE)
            {
                if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue)
                {
                    var enemies = EntityManager.Enemies.Where(x => x.IsInRange(Player.Instance.Position, ComboMenu["WRange"].Cast<Slider>().CurrentValue));
                    if (enemies != null)
                    {
                        if (enemies.Count() >= ComboMenu["Wn"].Cast<Slider>().CurrentValue)
                        {
                            _W.Cast();
                        }
                    }
                }
                if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _Q.Range) && _Q.IsReady())
                        return;
                    {
                        _Q.Cast(target);
                    }
                }
                if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue)
                {
                    var Enemys = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(_Player.Position, 625));
                    if (Enemys != null)
                    {
                        if (Enemys.Count() >= ComboMenu["Rn"].Cast<Slider>().CurrentValue)
                        {
                            if (_R.IsReady())
                            {
                                _R.Cast(rtarget);
                            }
                        }
                    }
                }
                if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _E.Range) && _E.IsReady())
                        return;
                    {
                        _E.Cast(target);

                    }
                }
            }
        }

        private static void Harass()
        {
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            if (target == null)
                return;

            if (HarrasMenu["UseQH"].Cast<CheckBox>().CurrentValue)
            {
                if (!target.IsInRange(_Player, _Q.Range) && _Q.IsReady())
                    return;
                {
                    _Q.Cast(target);
                }
            }
            if (HarrasMenu["UseEH"].Cast<CheckBox>().CurrentValue)
            {
                if (!target.IsInRange(_Player, _E.Range) && _E.IsReady())
                    return;
                {
                    _E.Cast(target);
                }
            }
        }

        private static void AHarra()
        {
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            if (target == null)
                return;
            if (_Player.ManaPercent > AHarrasM["AHQM"].Cast<Slider>().CurrentValue &&
                AHarrasM["AHQ"].Cast<CheckBox>().CurrentValue && AHarrasM["QAO"].Cast<CheckBox>().CurrentValue)
            {
                if (!target.IsInRange(_Player, _Q.Range) && _Q.IsReady())
                    return;
                {
                    _Q.Cast(target);
                }
            }
        }

        private static void LastHit()
        {
            var MHR = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(a => a.Distance(Player.Instance) <= _Q.Range).OrderBy(a => a.Health).FirstOrDefault();
            if (MHR != null)


            {
                if (LastHitM["Qlh"].Cast<CheckBox>().CurrentValue && _Q.IsReady() && Player.Instance.ManaPercent > LastHitM["manalh"].Cast<Slider>().CurrentValue && MHR.IsValidTarget(_Q.Range) && Player.Instance.GetSpellDamage(MHR, SpellSlot.Q) >= MHR.TotalShieldHealth())

                {
                    _Q.Cast(MHR);
                }


                if (LastHitM["Elh"].Cast<CheckBox>().CurrentValue && _W.IsReady() && Player.Instance.GetSpellDamage(MHR, SpellSlot.E) >= MHR.TotalShieldHealth() && Player.Instance.ManaPercent > LastHitM["manalh"].Cast<Slider>().CurrentValue)
                {
                    _W.Cast(MHR);
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
                var minionsList = EntityManager.MinionsAndMonsters.EnemyMinions.Where(x => x.IsValidTarget()).Where(minion => minion.IsInRange(Player.Instance, _Q.Range)).ToList();

                if (minionsList.Count >= 2)
                {
                    _Q.Cast();
                }

            }

            if (_Player.ManaPercent > LCMenu["LCWM"].Cast<Slider>().CurrentValue && LCMenu["LCW"].Cast<CheckBox>().CurrentValue)
            {
                if (!_W.IsReady())
                {
                    return;
                }
                var MHR = EntityManager.MinionsAndMonsters.EnemyMinions.Where(a => a.Distance(Player.Instance) <= _E.Range).OrderBy(a => a.Health).FirstOrDefault();
                if (MHR != null)

                {
                    _E.Cast(MHR);
                }
            }
        }

        private static void JungleClear()
        {
            var MHRE = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(a => a.Distance(Player.Instance) <= _E.Range).OrderBy(a => a.Health).FirstOrDefault();
            var MHRQ = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(a => a.Distance(Player.Instance) <= _Q.Range).OrderBy(a => a.Health).FirstOrDefault();
            if (MHRE != null)
                if (MHRQ != null)
                    if (_Player.ManaPercent > LCMenu["JGCQM"].Cast<Slider>().CurrentValue &&
                        LCMenu["JGCQ"].Cast<CheckBox>().CurrentValue && MHRQ.IsValidTarget(_Q.Range))
                    {
                        if (!_Q.IsReady())
                        {
                            return;
                        }
                        {

                            _Q.Cast(MHRQ);

                        }
                    }


            if (_Player.ManaPercent > LCMenu["JGCQM"].Cast<Slider>().CurrentValue && LCMenu["JGCW"].Cast<CheckBox>().CurrentValue && MHRE.IsValidTarget(_E.Range))
            {
                _E.Cast(MHRE);
            }
        }

        private static void Flee()
        {
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            if (target == null)
                return;
            if (FlyMenu["UseQf"].Cast<CheckBox>().CurrentValue)
            {
                if (!target.IsInRange(_Player, _Q.Range) && !_Q.IsReady())
                    return;
                {
                    _Q.Cast(target);
                }
            }

        }

        public static void KillSteal()
        {
            var targetQ = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            var targetE = TargetSelector.GetTarget(_E.Range, DamageType.Magical);
            var targetR = TargetSelector.GetTarget(_R.Range, DamageType.Magical);

            if (targetQ == null)
            {
                return;
            }
            if (targetE == null)
            {
                return;
            }
            if (targetR == null)
            {
                return;
            }
            if (KSMenu["KSQ"].Cast<CheckBox>().CurrentValue)
            {
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

            if (KSMenu["KSE"].Cast<CheckBox>().CurrentValue)
            {
                if (targetE.Health + targetE.AttackShield < _Player.GetSpellDamage(targetE, SpellSlot.E))
                {
                    if (!targetE.IsInRange(_Player, _E.Range) && _E.IsReady())
                    {
                        return;
                    }
                }
                {
                    _E.Cast();
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

        public static void Activator()
        {
            var target = TargetSelector.GetTarget(_Ignite.Range, DamageType.True);
            if (target == null)
                return;
            if (ActivatorMenu["IGNI"].Cast<CheckBox>().CurrentValue && _Ignite.IsReady() && target.IsValidTarget())

            {
                if (target.Health + target.AttackShield <
                    _Player.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite))
                {
                    _Ignite.Cast(target);
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var target = TargetSelector.GetTarget(_R.Range, DamageType.Physical);
            if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue && _Q.IsLearned)
            {
                Circle.Draw(_Q.IsReady() ? Color.White : Color.Red, _Q.Range, _Player);
            }
            if (DrawingsMenu["DE"].Cast<CheckBox>().CurrentValue && _E.IsLearned)
            {
                Circle.Draw(_E.IsReady() ? Color.White : Color.Red, _E.Range, _Player);
            }
            if (DrawingsMenu["DR"].Cast<CheckBox>().CurrentValue && _R.IsLearned)
            {
                Circle.Draw(_R.IsReady() ? Color.White : Color.Red, _R.Range, _Player);
            }
            if (DrawingsMenu["DRpred"].Cast<CheckBox>().CurrentValue && _R.IsLearned)
            {
                if (target == null)
                    return;
                Drawing.DrawCircle(_R.GetPrediction(target).CastPosition, _R.Width, System.Drawing.Color.Violet);

            }
        }

    }

}