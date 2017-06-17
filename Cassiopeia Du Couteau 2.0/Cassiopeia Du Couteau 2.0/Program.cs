using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Linq;
using Color = SharpDX.Color;


namespace Cassiopeia_Du_Couteau_2
{
    static class Program
    {
        private static AIHeroClient _Player
        {
            get { return ObjectManager.Player; }
        }

       // public static ColorBGRA Red { get; private set; }
     //   public static ColorBGRA Cyan { get; private set; }
        public static Spell.Skillshot _Q;
        public static Spell.Skillshot _W;
        public static Spell.Targeted _E;
        public static Spell.Skillshot _R;
        public static Spell.Targeted _Ignite;
        public static Item Seraph;
        public static Item Zhonia;

        private static Menu StartMenu, ComboMenu, LastHitM, DebugC, DrawingsMenu, ClearMenu, UtilityMenu;

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        public static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (!_Player.ChampionName.Contains("Cassiopeia"))
            {
                return;
            }
            Chat.Print("Cassiopeia Du Couteau by Horizon - Loaded", System.Drawing.Color.Crimson);
            _Q = new Spell.Skillshot(SpellSlot.Q, 750, SkillShotType.Circular, 400, int.MaxValue, 130);
            _W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Circular, 250, 250, 160);
            _E = new Spell.Targeted(SpellSlot.E, 700);
            _R = new Spell.Skillshot(SpellSlot.R, 800, SkillShotType.Cone, 250, 250, 80);
            Zhonia = new Item((int)ItemId.Zhonyas_Hourglass);
            Seraph = new Item((int)ItemId.Seraphs_Embrace);
            _Ignite = new Spell.Targeted(ObjectManager.Player.GetSpellSlotFromName("summonerdot"), 600);
            StartMenu = MainMenu.AddMenu("Cassiopeia", "Cassiopeia");
            ComboMenu = StartMenu.AddSubMenu("General/Combo Settings", "General/Combo Settings");
            ClearMenu = StartMenu.AddSubMenu("Clearing Menu", "Clearing Menu");
            DrawingsMenu = StartMenu.AddSubMenu("Drawing Settings", "Drawing Settings");
            DebugC = StartMenu.AddSubMenu("Debug", "Debug");
            ComboMenu.AddGroupLabel("Cassiopeia Du Couteau by Horizon");
            ComboMenu.Add("PredHit", new ComboBox("Prediction", 0, "HitChance :: High", "HitChance :: Medium", "HitChance :: Low"));
            ComboMenu.Add("DrawStatus", new CheckBox("Draw Current Orbwalker mode ? [BETA]"));
            ComboMenu.AddLabel("If you wanna use drawing orbwalker mode you need 16:9 resoultion,");
            ComboMenu.AddLabel("In future i will add customizable position.");
            ComboMenu.Add("DisableAA", new CheckBox("Disable AA in Combo for faster Kite", false));
            ComboMenu.AddLabel("Q Spell Settings");
            ComboMenu.Add("UseQ", new CheckBox("Use [Q]"));
            ComboMenu.Add("UseQH", new CheckBox("Use [Q] in Harass"));
            ComboMenu.Add("UseS", new CheckBox("Use [Q] Mana Saver?", false));
            ComboMenu.Add("UseQI", new CheckBox("Use always [Q] if enemy is immobile?"));
            ComboMenu.Add("UseQ2", new CheckBox("Try to hit =< 2 champions if can ?"));
            ComboMenu.Add("UseQPok", new CheckBox("Use always [Q] if enemy is killable by Poison?"));
            ComboMenu.Add("QComboDash", new CheckBox("Always use [Q] on Dash end position?"));
            ComboMenu.AddLabel("W Spell Settings");
            ComboMenu.Add("UseW", new CheckBox("Use [W]"));
            ComboMenu.Add("UseWH", new CheckBox("Use [W] in Harass", false));
            ComboMenu.Add("UseW2", new CheckBox("Try to hit =< 2 champions if can ?"));
            ComboMenu.AddLabel("E Spell Settings");
            ComboMenu.Add("UseE", new CheckBox("Use [E]"));
            ComboMenu.Add("UseEH", new CheckBox("Use [E] in Harass"));
            ComboMenu.Add("UseES", new CheckBox("Use [E] casting speedup ? (animation cancel)"));
            ComboMenu.Add("UseEK", new CheckBox("Use [E] always if enemy is killable?"));
            ComboMenu.AddLabel("R Spell Settings");
            ComboMenu.Add("UseR", new CheckBox("Use [R]"));
            ComboMenu.Add("UseRFace", new CheckBox("Use [R] only on facing enemy ?"));
            ComboMenu.Add("RGapClose", new CheckBox("Try use [R] for Gapclosing enemy ?", false));
            ComboMenu.Add("Rint", new CheckBox("Try use [R] for interrupt enemy ?"));
            ComboMenu.Add("UseRG", new CheckBox("Use [R] use minimum enemys for R ?"));
            ComboMenu.Add("UseRGs", new Slider("Minimum people for R", 1, 1, 5));
            ComboMenu.AddLabel("Other options");
            ComboMenu.Add("Ignite", new CheckBox("Use Summoner Ignite if target is killable ?"));
            ComboMenu.Add("Zhonya", new CheckBox("Use Zhonya if dangerous ?"));
            ComboMenu.Add("ZhonyaHP", new Slider("Zhonya Health for use?  %", 25));
            ComboMenu.AddSeparator((int)10);
            ComboMenu.Add("Seraph", new CheckBox("Use Seraph"));
            ComboMenu.Add("SeraphHP", new Slider("Seraph's Health for use? %", 35));
            ComboMenu.Add("SeraphMana", new Slider("Minimum Mana for Seraph's use? %", 40));
            ComboMenu.AddSeparator((int)10);
            ComboMenu.AddLabel("Skin Changer");
            var skin = ComboMenu.Add("SkinMisc", new Slider("Skin changer", 0, 0, 9));
            ComboMenu.AddLabel("For now, you shouldn't use this one, sometimes making R glitched");
       //     ClearMenu.Add("EMode", new ComboBox("Clear E mode", 0, "Always", "Poisoned"));
            ClearMenu.Add("UseQCL", new CheckBox("Use [Q] in clear ?"));
            ClearMenu.Add("UseWCL", new CheckBox("Use [W] in clear ?", false));
            ClearMenu.Add("UseECL", new CheckBox("Use [E] in clear ?"));
            ClearMenu.AddSeparator(1);
            ClearMenu.Add("UseQLH", new CheckBox("Use [Q] in LastHit ?", false));
            ClearMenu.Add("UseWLH", new CheckBox("Use [W] in LastHit ?", false));
            ClearMenu.Add("UseELH", new CheckBox("Use [E] in LastHit ?"));
            ClearMenu.Add("ClearMana", new Slider("Minimum mana for clear %", 50));

            DrawingsMenu.AddGroupLabel("Drawing Settings");
            DrawingsMenu.AddLabel("Tick for enable/disable spell drawings");
            DrawingsMenu.Add("DQ", new CheckBox("Draw [Q] range"));
            DrawingsMenu.Add("QPred", new CheckBox("Draw [Q] Prediction"));
            DrawingsMenu.AddSeparator(0);
            DrawingsMenu.Add("DW", new CheckBox("Draw [W] range"));
            DrawingsMenu.AddSeparator(0);
            DrawingsMenu.Add("DE", new CheckBox("Draw [E] range"));
            DrawingsMenu.AddSeparator(0);
            DrawingsMenu.Add("DR", new CheckBox("Draw [R] range"));
            DebugC.Add("Debug", new CheckBox("Debug Console+Chat", false));
            DebugC.Add("DrawStatus1", new CheckBox("Debug Curret Orbawlker mode"));

            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Interrupter.OnInterruptableSpell += Interruptererer;
            skin.OnValueChange += Skin_OnValueChange;
            Dash.OnDash += Dash_OnDash;
            Player.SetSkinId(ComboMenu["SkinMisc"].Cast<Slider>().CurrentValue);
        }
        public static bool PoisonWillExpire(this Obj_AI_Base target, float time)
        {
            var buff = target.Buffs.OrderByDescending(x => x.EndTime).FirstOrDefault(x => x.Type == BuffType.Poison && x.IsActive && x.IsValid);
            return buff == null || time > (buff.EndTime - Game.Time) * 1000f;
        }
        private static bool Immobile(Obj_AI_Base unit)
        {
            return unit.HasBuffOfType(BuffType.Charm) || unit.HasBuffOfType(BuffType.Stun) ||
                   unit.HasBuffOfType(BuffType.Knockup) || unit.HasBuffOfType(BuffType.Snare) ||
                   unit.HasBuffOfType(BuffType.Taunt) || unit.HasBuffOfType(BuffType.Suppression) ||
                   unit.HasBuffOfType(BuffType.Polymorph);
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
            Ignite();
            ImmobileQ();
            KillSteal();
            Zhonya();
            SeraphsEmbrace();
        }
        public static void LastHit()
        {
            var MHR = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(a => a.Distance(Player.Instance) <= _Q.Range).OrderBy(a => a.Health).FirstOrDefault();
            if (MHR != null)
            {

                if (ClearMenu["UseELH"].Cast<CheckBox>().CurrentValue && _E.IsReady() && Player.Instance.ManaPercent > ClearMenu["ClearMana"].Cast<Slider>().CurrentValue && MHR.IsValidTarget(_E.Range) &&
                    Player.Instance.GetSpellDamage(MHR, SpellSlot.E) >= MHR.TotalShieldHealth())

                {
                    _E.Cast(MHR);
                }

                if (ClearMenu["UseQLH"].Cast<CheckBox>().CurrentValue && _Q.IsReady() && Player.Instance.ManaPercent > ClearMenu["ClearMana"].Cast<Slider>().CurrentValue && MHR.IsValidTarget(_Q.Range) &&
                    Player.Instance.GetSpellDamage(MHR, SpellSlot.Q) >= MHR.TotalShieldHealth())

                {
                    _Q.Cast(MHR);
                }


                if (ClearMenu["UseWLH"].Cast<CheckBox>().CurrentValue && _W.IsReady() && Player.Instance.GetSpellDamage(MHR, SpellSlot.W) >= MHR.TotalShieldHealth() &&
                    Player.Instance.ManaPercent > ClearMenu["ClearMana"].Cast<Slider>().CurrentValue)
                {
                    _W.Cast(MHR);
                }

            

                
            }
        }

        public static void JungleClear()
        {
            var MHR = EntityManager.MinionsAndMonsters.GetJungleMonsters().Where(a => a.Distance(Player.Instance) <= _Q.Range).OrderBy(a => a.Health).FirstOrDefault();
            if (MHR != null)
            {
                if (_Player.ManaPercent > ClearMenu["ClearMana"].Cast<Slider>().CurrentValue && ClearMenu["UseQCL"].Cast<CheckBox>().CurrentValue && MHR.IsValidTarget(_Q.Range))
                {
                    _Q.Cast(MHR);
                }
            }

            if (_Player.ManaPercent > ClearMenu["ClearMana"].Cast<Slider>().CurrentValue && ClearMenu["UseWCL"].Cast<CheckBox>().CurrentValue && MHR.IsValidTarget(_W.Range))
            {
                _W.Cast(MHR);
            }
            if (_Player.ManaPercent > ClearMenu["ClearMana"].Cast<Slider>().CurrentValue && ClearMenu["UseECL"].Cast<CheckBox>().CurrentValue && MHR.IsValidTarget(_E.Range))
            {
                _E.Cast(MHR);
            }
        }

        public static void LaneClear()

        {
            var MHR = EntityManager.MinionsAndMonsters.GetLaneMinions().Where(a => a.Distance(Player.Instance) <= _Q.Range).OrderBy(a => a.Health).FirstOrDefault();
            if (MHR != null)
                if (ClearMenu["UseQCL"].Cast<CheckBox>().CurrentValue)
            {
                if (!_Q.IsReady()) return;
                {
                    _Q.CastOnBestFarmPosition(1, 50);
                }
            }
            if (ClearMenu["UseWCL"].Cast<CheckBox>().CurrentValue)
            {
                if (!_W.IsReady()) return;
                {
                    _W.CastOnBestFarmPosition(2, 50);
                }

            }

            if (ClearMenu["UseECL"].Cast<CheckBox>().CurrentValue && _E.IsReady() && Player.Instance.ManaPercent > ClearMenu["ClearMana"].Cast<Slider>().CurrentValue && MHR.IsValidTarget(_E.Range))

            {
                _E.Cast(MHR);
            }


        }
        public static void Harass()
        {
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            if (target == null) return;
            if (ComboMenu["UseEH"].Cast<CheckBox>().CurrentValue)
            {
                if (!target.IsInRange(_Player, _E.Range) && !_E.IsReady())
                    return;
                {
                    if (_E.IsReady() && ComboMenu["UseES"].Cast<CheckBox>().CurrentValue)
                    {

                        _E.Cast(target);
                        Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                        if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                        {
                            Chat.Print("Casting E with speedup");
                            Console.WriteLine(Game.Time + "Casting E with Speedup");
                        }

                    }
                    if (_E.IsReady() && !ComboMenu["UseES"].Cast<CheckBox>().CurrentValue)
                    {
                        _E.Cast(target);
                        if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                        {
                            Chat.Print("Casting E with normal");
                            Console.WriteLine(Game.Time + "Casting E with Speedup");
                        }
                    }
                }
            }
            if (ComboMenu["UseWH"].Cast<CheckBox>().CurrentValue)
            {
                if (!_W.IsReady() && _Player.Distance(target) >= 500) return;
                {

                    var Wpred = _W.GetPrediction(target);
                    if (Wpred.HitChance >= HitChance.High && target.IsValidTarget(_W.Range))
                    {
                        if (ComboMenu["UseW2"].Cast<CheckBox>().CurrentValue)
                        {
                            var Enemys = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(_Player.Position, _W.Range));
                            if (Enemys != null)
                            {
                                if (Enemys.Count() >= 2)
                                {
                                    _W.Cast(target.ServerPosition);
                                    if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                    {

                                        Chat.Print("Casting W Found more than >= 2 People ");
                                        Console.WriteLine("Casting W Found more than >= 2 People");
                                    }
                                }
                                else if (Enemys.Count() >= 1)
                                {
                                    _W.Cast(target);
                                    if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                    {

                                        Chat.Print("Casting W Found more than >= 1 People ");
                                        Console.WriteLine("Casting W Found more than >= 1 People");
                                    }
                                }
                            }
                        }
                    }

                }
            }
            if (!ComboMenu["UseW2"].Cast<CheckBox>().CurrentValue)
            {
                _W.Cast(target);
                if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                {

                    Chat.Print("Casting W ");
                    Console.WriteLine("Casting W");
                }
            }

            if (ComboMenu["UseQH"].Cast<CheckBox>().CurrentValue && ComboMenu["UseQ2"].Cast<CheckBox>().CurrentValue)
            {
                if (_Q.IsReady())
                {
                    var canHitMoreThanOneTarget =
                      EntityManager.Heroes.Enemies.OrderByDescending(x => x.CountEnemyChampionsInRange(_Q.Width))
                      .FirstOrDefault(x => x.IsValidTarget(_Q.Range) && x.CountEnemyChampionsInRange(_Q.Width) >= 1);
                    if (canHitMoreThanOneTarget != null)
                    {
                        var getAllTargets = EntityManager.Heroes.Enemies.FindAll(x => x.IsValidTarget() && x.IsInRange(canHitMoreThanOneTarget, _Q.Width));
                        var center = getAllTargets.Aggregate(Vector3.Zero, (current, x) => current + x.ServerPosition) / getAllTargets.Count;
                        if (!center.IsZero)
                        {
                            var Qpred = _Q.GetPrediction(target);
                            if (Qpred.HitChance >= HitChance.High && target.IsValidTarget(_Q.Range))
                            {
                                _Q.Cast(target);
                                if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                {

                                    Chat.Print("FOUND 1 PEOPLE FOR Q ");
                                    Console.WriteLine("FOUND 1 PEOPLE FOR Q");
                                }
                            }

                        }
                    }

                }
            }
            if (ComboMenu["UseQH"].Cast<CheckBox>().CurrentValue)
            {

                if (!target.IsInRange(_Player, _Q.Range))
                    return;
                {
                    if (_Q.IsReady() && ComboMenu["UseS"].Cast<CheckBox>().CurrentValue)
                    {
                        var Qpred = _Q.GetPrediction(target);
                        if (Qpred.HitChance >= HitChance.High && target.IsValidTarget(_Q.Range))
                        {
                            if (!target.PoisonWillExpire(250))
                                return;
                            {
                                _Q.Cast(target.ServerPosition);
                                if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                {

                                    Chat.Print("Casting Q with HIGH pred saver ");
                                    Console.WriteLine("Casting Q with HIGH pred saver ");
                                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                                }
                            }
                        }

                    }

                }
            }
        }
        private static void Zhonya()
        {
            var Zhonya = ComboMenu["Zhonya"].Cast<CheckBox>().CurrentValue;
            var ZhonyaHP = ComboMenu["ZhonyaHP"].Cast<Slider>().CurrentValue;
            if (!Zhonya || !Zhonia.IsReady() || !Zhonia.IsOwned()) return;
            if (_Player.HealthPercent <= ZhonyaHP && _Player.CountEnemyChampionsInRange(500) >= 1)
            {
                Zhonia.Cast();
            }
        }
        private static void SeraphsEmbrace()
        {
            if (Seraph.IsReady() && Seraph.IsOwned())
            {
                var embrace = ComboMenu["Seraph"].Cast<CheckBox>().CurrentValue;
                var shealth = ComboMenu["SeraphHP"].Cast<Slider>().CurrentValue;
                var smana = ComboMenu["SeraphMana"].Cast<Slider>().CurrentValue;
                if (!embrace || !Zhonia.IsReady() || !Zhonia.IsOwned()) return;
                if (_Player.HealthPercent <= shealth && _Player.ManaPercent >= smana && _Player.CountEnemyChampionsInRange(500) >= 1)
                {
                    Seraph.Cast();
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            var Combo = Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
            var LastHit = Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
            var LaneClear = Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
            var Harass = Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);

            if (DrawingsMenu["DQ"].Cast<CheckBox>().CurrentValue && _Q.IsLearned)
            {
                Circle.Draw(_Q.IsReady() ? Color.Lime : Color.Red, _Q.Range, _Player);
            }
            if (DrawingsMenu["DE"].Cast<CheckBox>().CurrentValue && _E.IsLearned)
            {
                Circle.Draw(_E.IsReady() ? Color.Cyan : Color.Red, _E.Range, _Player);
            }
            if (DrawingsMenu["DR"].Cast<CheckBox>().CurrentValue && _R.IsLearned)
            {
                Circle.Draw(_R.IsReady() ? Color.Purple : Color.Red, _R.Range, _Player);
            }
            if (DrawingsMenu["QPred"].Cast<CheckBox>().CurrentValue && _Q.IsLearned)
            {
                if (target == null)
                    return;
                Drawing.DrawCircle(_Q.GetPrediction(target).CastPosition, _Q.Width, System.Drawing.Color.Violet);

            }
            if (ComboMenu["DrawStatus"].Cast<CheckBox>().CurrentValue)

            {
                if (Harass && !Combo && LaneClear && LastHit)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: Harass ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.91f, System.Drawing.Color.White, "[ Orbwalker Mode: Harass ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.93f, System.Drawing.Color.White, "[ Orbwalker Mode: LastHit ]");
                }
                if (Harass && Combo && !LaneClear && LastHit)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: Combo ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.91f, System.Drawing.Color.White, "[ Orbwalker Mode: Harass ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.93f, System.Drawing.Color.White, "[ Orbwalker Mode: LastHit ]");
                }
                if (Harass && Combo && LaneClear && !LastHit)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: Combo ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.91f, System.Drawing.Color.White, "[ Orbwalker Mode: Harass ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.93f, System.Drawing.Color.White, "[ Orbwalker Mode: LaneClear ]");
                }
                if (Harass && Combo && LaneClear && LastHit)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: Combo ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.91f, System.Drawing.Color.White, "[ Orbwalker Mode: Harass ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.93f, System.Drawing.Color.White, "[ Orbwalker Mode: LastHit ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.95f, System.Drawing.Color.White, "[ Orbwalker Mode: LaneClear ]");
                }
                if (Harass && !Combo && !LaneClear && LastHit)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: Harass ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.91f, System.Drawing.Color.White, "[ Orbwalker Mode: LastHit ]");
                }
                if (Harass && Combo && !LaneClear && !LastHit)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: Combo ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.91f, System.Drawing.Color.White, "[ Orbwalker Mode: Harass ]");
                }

                if (Harass && LaneClear && !Combo && !LastHit)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: Harass ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.91f, System.Drawing.Color.White, "[ Orbwalker Mode: LaneClear ]");
                }
                if (Harass && !LaneClear && !Combo && !LastHit)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: Harass ]");
                }
                if (LaneClear && LastHit && !Combo && !Harass)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: LastHit ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.91f, System.Drawing.Color.White, "[ Orbwalker Mode: LaneClear ]");
                }
                if (LaneClear && Combo && !LastHit && !Harass)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: Combo ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.91f, System.Drawing.Color.White, "[ Orbwalker Mode: LaneClear ]");
                }
                if (LaneClear && LastHit && Combo && !Harass)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: Combo ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.91f, System.Drawing.Color.White, "[ Orbwalker Mode: LastHit ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.93f, System.Drawing.Color.White, "[ Orbwalker Mode: LaneClear ]");
                }
                if (LaneClear && !LastHit && !Combo && !Harass)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: LaneClear ]");
                }

                if (LastHit && Combo && !LaneClear && !Harass)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: Combo ]");
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.91f, System.Drawing.Color.White, "[ Orbwalker Mode: LastHit ]");
                }
                if (LastHit && !Combo && !LaneClear && !Harass)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: LastHit ]");
                }

                if (Combo && !LastHit && !LaneClear && !Harass)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: Combo ]");
                }

                else if (!Combo && !LastHit && !LaneClear && !Harass)
                {
                    Drawing.DrawText(Drawing.Width * 0.72f, Drawing.Height * 0.89f, System.Drawing.Color.White, "[ Orbwalker Mode: None ]");
                }
            }
        }
        private static void Combo()
        {

            var HighP = ComboMenu["PredHit"].Cast<ComboBox>().SelectedIndex == 0;
            var MediumP = ComboMenu["PredHit"].Cast<ComboBox>().SelectedIndex == 1;
            var LowP = ComboMenu["PredHit"].Cast<ComboBox>().SelectedIndex == 2;
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            var targetQ2 = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }
            if (ComboMenu["DisableAA"].Cast<CheckBox>().CurrentValue && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Orbwalker.DisableAttacking = true;
            }
            if (!ComboMenu["DisableAA"].Cast<CheckBox>().CurrentValue && Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Orbwalker.DisableAttacking = false;
            }
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Orbwalker.DisableAttacking = false;
            }
            if (HighP)
            {
                if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _E.Range) && !_E.IsReady())
                        return;
                    {
                        if (_E.IsReady() && ComboMenu["UseES"].Cast<CheckBox>().CurrentValue)
                        {

                            _E.Cast(target);
                            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                            if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                            {
                                Chat.Print("Casting E with speedup");
                                Console.WriteLine(Game.Time + "Casting E with Speedup");
                            }

                        }
                        if (_E.IsReady() && !ComboMenu["UseES"].Cast<CheckBox>().CurrentValue)
                        {
                            _E.Cast(target);
                            if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                            {
                                Chat.Print("Casting E with normal");
                                Console.WriteLine(Game.Time + "Casting E with Speedup");
                            }
                        }
                    }
                }

                if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue)
                {
                    if (!_W.IsReady() && _Player.Distance(target) >= 500) return;
                    {

                        var Wpred = _W.GetPrediction(target);
                        if (Wpred.HitChance >= HitChance.High && target.IsValidTarget(_W.Range))
                        {
                            if (ComboMenu["UseW2"].Cast<CheckBox>().CurrentValue)
                            {
                                var Enemys = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(_Player.Position, _W.Range));
                                if (Enemys != null)
                                {
                                    if (Enemys.Count() >= 2)
                                    {
                                        _W.Cast(target.ServerPosition);
                                        if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                        {

                                            Chat.Print("Casting W Found more than >= 2 People ");
                                            Console.WriteLine("Casting W Found more than >= 2 People");
                                        }
                                    }
                                    else if (Enemys.Count() >= 1)
                                    {
                                        _W.Cast(target);
                                        if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                        {

                                            Chat.Print("Casting W Found more than >= 1 People ");
                                            Console.WriteLine("Casting W Found more than >= 1 People");
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                if (!ComboMenu["UseW2"].Cast<CheckBox>().CurrentValue)
                {
                    _W.Cast(target);
                    if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                    {

                        Chat.Print("Casting W ");
                        Console.WriteLine("Casting W");
                    }
                }

                if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue && ComboMenu["UseQ2"].Cast<CheckBox>().CurrentValue)
                {
                    if (_Q.IsReady())
                    {
                        var canHitMoreThanOneTarget =
                          EntityManager.Heroes.Enemies.OrderByDescending(x => x.CountEnemyChampionsInRange(_Q.Width))
                          .FirstOrDefault(x => x.IsValidTarget(_Q.Range) && x.CountEnemyChampionsInRange(_Q.Width) >= 2);
                        if (canHitMoreThanOneTarget != null)
                        {
                            var getAllTargets = EntityManager.Heroes.Enemies.FindAll(x => x.IsValidTarget() && x.IsInRange(canHitMoreThanOneTarget, _Q.Width));
                            var center = getAllTargets.Aggregate(Vector3.Zero, (current, x) => current + x.ServerPosition) / getAllTargets.Count;
                            if (!center.IsZero)
                            {
                                var Qpred = _Q.GetPrediction(target);
                                if (Qpred.HitChance >= HitChance.High && target.IsValidTarget(_Q.Range))
                                {
                                    _Q.Cast(target);
                                    if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                    {

                                        Chat.Print("FOUND MORE THAN 2 PEOPLE FOR Q ");
                                        Console.WriteLine("FOUND MORE THAN 2 PEOPLE FOR Q");
                                    }
                                }

                            }

                        }
                    }

                }

                if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue && ComboMenu["UseQ2"].Cast<CheckBox>().CurrentValue)
                {
                    if (_Q.IsReady())
                    {
                        var canHitMoreThanOneTarget =
                          EntityManager.Heroes.Enemies.OrderByDescending(x => x.CountEnemyChampionsInRange(_Q.Width))
                          .FirstOrDefault(x => x.IsValidTarget(_Q.Range) && x.CountEnemyChampionsInRange(_Q.Width) >= 1);
                        if (canHitMoreThanOneTarget != null)
                        {
                            var getAllTargets = EntityManager.Heroes.Enemies.FindAll(x => x.IsValidTarget() && x.IsInRange(canHitMoreThanOneTarget, _Q.Width));
                            var center = getAllTargets.Aggregate(Vector3.Zero, (current, x) => current + x.ServerPosition) / getAllTargets.Count;
                            if (!center.IsZero)
                            {
                                var Qpred = _Q.GetPrediction(target);
                                if (Qpred.HitChance >= HitChance.High && target.IsValidTarget(_Q.Range))
                                {
                                    _Q.Cast(target);
                                    if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                    {

                                        Chat.Print("FOUND 1 PEOPLE FOR Q ");
                                        Console.WriteLine("FOUND 1 PEOPLE FOR Q");
                                    }
                                }

                            }
                        }

                    }
                }
                    if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue)
                    {

                        if (!target.IsInRange(_Player, _Q.Range))
                            return;
                        {
                            if (_Q.IsReady() && ComboMenu["UseS"].Cast<CheckBox>().CurrentValue)
                            {
                                var Qpred = _Q.GetPrediction(target);
                                if (Qpred.HitChance >= HitChance.High && target.IsValidTarget(_Q.Range))
                                {
                                    if (!target.PoisonWillExpire(250))
                                        return;
                                    {
                                        _Q.Cast(target.ServerPosition);
                                        if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                        {

                                            Chat.Print("Casting Q with HIGH pred saver ");
                                            Console.WriteLine("Casting Q with HIGH pred saver ");
                                            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                                        }
                                    }
                                }

                            }

                        }
                    }

                    if (!ComboMenu["UseS"].Cast<CheckBox>().CurrentValue && ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue && !ComboMenu["UseQ2"].Cast<CheckBox>().CurrentValue)
                    {
                        if (_Q.IsReady())
                        {

                            var Qpred = _Q.GetPrediction(target);
                            if (Qpred.HitChance >= HitChance.High && target.IsValidTarget(_Q.Range))
                            {
                                Core.DelayAction(() => _Q.Cast(target), 100);
                                if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                {

                                    Chat.Print("Casting Q with HIGH pred ");
                                    Console.WriteLine("Casting Q with HIGH pred ");
                                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                                }
                            }
                        }



                    }

                    if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue && ComboMenu["UseRG"].Cast<CheckBox>().CurrentValue && _R.IsReady())
                    {
                    var Enemys = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(_Player.Position, _R.Range - 25));
                    if (Enemys != null)
                    {
                        if (Enemys.Count() >= ComboMenu["UseRGs"].Cast<Slider>().CurrentValue && target.IsFacing(_Player) && ComboMenu["UseRFace"].Cast<CheckBox>().CurrentValue)
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, target);
                            _R.Cast(target);
                        }
                        if (Enemys.Count() >= ComboMenu["UseRGs"].Cast<Slider>().CurrentValue && !ComboMenu["UseRFace"].Cast<CheckBox>().CurrentValue)
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, target);
                            _R.Cast(target);
                        }
                    }

                    }

                    if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue && _R.IsReady())
                    {
                    if (!_R.IsReady()) return;
                        {
                            if (target.IsFacing(_Player) && target.IsInRange(_Player, _R.Range) && ComboMenu["UseRFace"].Cast<CheckBox>().CurrentValue)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, target);
                                _R.Cast(target.ServerPosition);
                            }
                        }
                     if (target.IsInRange(_Player, _R.Range) && !ComboMenu["UseRFace"].Cast<CheckBox>().CurrentValue)
                     {
                        Player.IssueOrder(GameObjectOrder.MoveTo, target);
                        _R.Cast(target.ServerPosition);
                     }

                    }
            }

            if (MediumP)
            {
                if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _E.Range) && !_E.IsReady())
                        return;
                    {
                        if (_E.IsReady() && ComboMenu["UseES"].Cast<CheckBox>().CurrentValue)
                        {

                            _E.Cast(target);
                            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                            if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                            {
                                Chat.Print("Casting E with speedup");
                                Console.WriteLine(Game.Time + "Casting E with Speedup");
                            }

                        }
                        if (_E.IsReady() && !ComboMenu["UseES"].Cast<CheckBox>().CurrentValue)
                        {
                            _E.Cast(target);
                            if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                            {
                                Chat.Print("Casting E with normal");
                                Console.WriteLine(Game.Time + "Casting E with Speedup");
                            }
                        }
                    }
                }

                if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue)
                {
                    if (!_W.IsReady() && _Player.Distance(target) >= 500) return;
                    {

                        var Wpred = _W.GetPrediction(target);
                        if (Wpred.HitChance >= HitChance.Medium && target.IsValidTarget(_W.Range))
                        {
                            if (ComboMenu["UseW2"].Cast<CheckBox>().CurrentValue)
                            {
                                var Enemys = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(_Player.Position, _W.Range));
                                if (Enemys != null)
                                {
                                    if (Enemys.Count() >= 2)
                                    {
                                        _W.Cast(target.ServerPosition);
                                        if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                        {

                                            Chat.Print("Casting W Found more than >= 2 People ");
                                            Console.WriteLine("Casting W Found more than >= 2 People");
                                        }
                                    }
                                    else if (Enemys.Count() >= 1)
                                    {
                                        _W.Cast(target);
                                        if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                        {


                                            Chat.Print("Casting W Found more than >= 1 People ");
                                            Console.WriteLine("Casting W Found more than >= 1 People");
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
                if (!ComboMenu["UseW2"].Cast<CheckBox>().CurrentValue)
                {
                    _W.Cast(target);
                    if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                    {

                        Chat.Print("Casting W ");
                        Console.WriteLine("Casting W");
                    }
                }

                if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue && ComboMenu["UseQ2"].Cast<CheckBox>().CurrentValue)
                {
                    if (_Q.IsReady())
                    {
                        var canHitMoreThanOneTarget =
                          EntityManager.Heroes.Enemies.OrderByDescending(x => x.CountEnemyChampionsInRange(_Q.Width))
                          .FirstOrDefault(x => x.IsValidTarget(_Q.Range) && x.CountEnemyChampionsInRange(_Q.Width) >= 2);
                        if (canHitMoreThanOneTarget != null)
                        {
                            var getAllTargets = EntityManager.Heroes.Enemies.FindAll(x => x.IsValidTarget() && x.IsInRange(canHitMoreThanOneTarget, _Q.Width));
                            var center = getAllTargets.Aggregate(Vector3.Zero, (current, x) => current + x.ServerPosition) / getAllTargets.Count;
                            if (!center.IsZero)
                            {
                                var Qpred = _Q.GetPrediction(target);
                                if (Qpred.HitChance >= HitChance.Medium && target.IsValidTarget(_Q.Range))
                                {
                                    _Q.Cast(target);
                                    if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                    {

                                        Chat.Print("FOUND MORE THAN 2 PEOPLE FOR Q ");
                                        Console.WriteLine("FOUND MORE THAN 2 PEOPLE FOR Q");
                                    }
                                }

                            }


                        }
                    }

                }

                if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue && ComboMenu["UseQ2"].Cast<CheckBox>().CurrentValue)
                {
                    if (_Q.IsReady())
                    {
                        var canHitMoreThanOneTarget =
                          EntityManager.Heroes.Enemies.OrderByDescending(x => x.CountEnemyChampionsInRange(_Q.Width))
                          .FirstOrDefault(x => x.IsValidTarget(_Q.Range) && x.CountEnemyChampionsInRange(_Q.Width) >= 1);
                        if (canHitMoreThanOneTarget != null)
                        {
                            var getAllTargets = EntityManager.Heroes.Enemies.FindAll(x => x.IsValidTarget() && x.IsInRange(canHitMoreThanOneTarget, _Q.Width));
                            var center = getAllTargets.Aggregate(Vector3.Zero, (current, x) => current + x.ServerPosition) / getAllTargets.Count;
                            if (!center.IsZero)
                            {
                                var Qpred = _Q.GetPrediction(target);
                                if (Qpred.HitChance >= HitChance.Medium && target.IsValidTarget(_Q.Range))
                                {
                                    _Q.Cast(target);
                                    if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                    {

                                        Chat.Print("FOUND 1 PEOPLE FOR Q ");
                                        Console.WriteLine("FOUND 1 PEOPLE FOR Q");
                                    }
                                }


                            }
                        }

                    }

                }
                    if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue)
                    {

                        if (!target.IsInRange(_Player, _Q.Range))
                            return;
                        {
                            if (_Q.IsReady() && ComboMenu["UseS"].Cast<CheckBox>().CurrentValue)
                            {
                                var Qpred = _Q.GetPrediction(target);
                                if (Qpred.HitChance >= HitChance.Medium && target.IsValidTarget(_Q.Range))
                                {
                                    if (!target.PoisonWillExpire(250))
                                        return;
                                    {
                                        _Q.Cast(target.ServerPosition);
                                        if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                        {

                                            Chat.Print("Casting Q with HIGH pred saver ");
                                            Console.WriteLine("Casting Q with HIGH pred saver ");
                                            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                                        }
                                    }

                                }


                            }

                        }
                    }

                    if (!ComboMenu["UseS"].Cast<CheckBox>().CurrentValue && ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue && !ComboMenu["UseQ2"].Cast<CheckBox>().CurrentValue)
                    {
                        if (_Q.IsReady())
                        {

                            var Qpred = _Q.GetPrediction(target);
                            if (Qpred.HitChance >= HitChance.Medium && target.IsValidTarget(_Q.Range))
                            {
                                Core.DelayAction(() => _Q.Cast(target), 100);
                                if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                {

                                    Chat.Print("Casting Q with HIGH pred ");
                                    Console.WriteLine("Casting Q with HIGH pred ");
                                    Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                                }
                            }

                        }



                    }

                    if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue && ComboMenu["UseRG"].Cast<CheckBox>().CurrentValue && _R.IsReady())
                    {
                    var Enemys = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(_Player.Position, _R.Range - 25));
                    if (Enemys != null)
                    {
                        if (Enemys.Count() >= ComboMenu["UseRGs"].Cast<Slider>().CurrentValue && target.IsFacing(_Player) && ComboMenu["UseRFace"].Cast<CheckBox>().CurrentValue)
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, target);
                            _R.Cast(target);
                        }
                        if (Enemys.Count() >= ComboMenu["UseRGs"].Cast<Slider>().CurrentValue && !ComboMenu["UseRFace"].Cast<CheckBox>().CurrentValue)
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, target);
                            _R.Cast(target);
                        }
                    }


                    }

                    if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue && _R.IsReady())
                    {
                    if (!_R.IsReady()) return;
                        {
                            if (target.IsFacing(_Player) && target.IsInRange(_Player, _R.Range) && ComboMenu["UseRFace"].Cast<CheckBox>().CurrentValue)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, target);
                                _R.Cast(target.ServerPosition);
                            }
                        }
                     if (target.IsInRange(_Player, _R.Range) && !ComboMenu["UseRFace"].Cast<CheckBox>().CurrentValue)
                     {
                        Player.IssueOrder(GameObjectOrder.MoveTo, target);
                        _R.Cast(target.ServerPosition);
                     }

                    }
            }
                if (LowP)
                {
                if (ComboMenu["UseE"].Cast<CheckBox>().CurrentValue)
                {
                    if (!target.IsInRange(_Player, _E.Range) && !_E.IsReady())
                        return;
                    {
                        if (_E.IsReady() && ComboMenu["UseES"].Cast<CheckBox>().CurrentValue)
                        {

                            _E.Cast(target);
                            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                            if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                            {
                                Chat.Print("Casting E with speedup");
                                Console.WriteLine(Game.Time + "Casting E with Speedup");
                            }


                        }
                        if (_E.IsReady() && !ComboMenu["UseES"].Cast<CheckBox>().CurrentValue)
                        {
                            _E.Cast(target);
                            if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                            {
                                Chat.Print("Casting E with normal");
                                Console.WriteLine(Game.Time + "Casting E with Speedup");
                            }
                        }
                    }
                }

                if (ComboMenu["UseW"].Cast<CheckBox>().CurrentValue)
                {
                    if (!_W.IsReady() && _Player.Distance(target) >= 500) return;
                    {

                        var Wpred = _W.GetPrediction(target);
                        if (Wpred.HitChance >= HitChance.Low && target.IsValidTarget(_W.Range))
                        {
                            if (ComboMenu["UseW2"].Cast<CheckBox>().CurrentValue)
                            {
                                var Enemys = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(_Player.Position, _W.Range));
                                if (Enemys != null)
                                {
                                    if (Enemys.Count() >= 2)
                                    {
                                        _W.Cast(target.ServerPosition);
                                        if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                        {

                                            Chat.Print("Casting W Found more than >= 2 People ");
                                            Console.WriteLine("Casting W Found more than >= 2 People");
                                        }
                                    }
                                    else if (Enemys.Count() >= 1)
                                    {
                                        _W.Cast(target);
                                        if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                        {

                                            Chat.Print("Casting W Found more than >= 1 People ");
                                            Console.WriteLine("Casting W Found more than >= 1 People");
                                        }
                                    }

                                }
                            }
                        }

                    }
                }
                if (!ComboMenu["UseW2"].Cast<CheckBox>().CurrentValue)
                {
                    _W.Cast(target);
                    if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                    {

                        Chat.Print("Casting W ");
                        Console.WriteLine("Casting W");
                    }
                }

                if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue && ComboMenu["UseQ2"].Cast<CheckBox>().CurrentValue)
                {
                    if (_Q.IsReady())
                    {
                        var canHitMoreThanOneTarget =
                          EntityManager.Heroes.Enemies.OrderByDescending(x => x.CountEnemyChampionsInRange(_Q.Width))
                          .FirstOrDefault(x => x.IsValidTarget(_Q.Range) && x.CountEnemyChampionsInRange(_Q.Width) >= 2);
                        if (canHitMoreThanOneTarget != null)
                        {
                            var getAllTargets = EntityManager.Heroes.Enemies.FindAll(x => x.IsValidTarget() && x.IsInRange(canHitMoreThanOneTarget, _Q.Width));
                            var center = getAllTargets.Aggregate(Vector3.Zero, (current, x) => current + x.ServerPosition) / getAllTargets.Count;
                            if (!center.IsZero)
                            {
                                var Qpred = _Q.GetPrediction(target);
                                if (Qpred.HitChance >= HitChance.Low && target.IsValidTarget(_Q.Range))
                                {
                                    _Q.Cast(target);
                                    if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                    {
                                        //radi is gay
                                        Chat.Print("FOUND MORE THAN 2 PEOPLE FOR Q ");
                                        Console.WriteLine("FOUND MORE THAN 2 PEOPLE FOR Q");
                                    }
                                }

                            }

                        }
                    }

                }

                if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue && ComboMenu["UseQ2"].Cast<CheckBox>().CurrentValue)
                {
                    if (_Q.IsReady())
                    {
                        var canHitMoreThanOneTarget =
                          EntityManager.Heroes.Enemies.OrderByDescending(x => x.CountEnemyChampionsInRange(_Q.Width))
                          .FirstOrDefault(x => x.IsValidTarget(_Q.Range) && x.CountEnemyChampionsInRange(_Q.Width) >= 1);
                        if (canHitMoreThanOneTarget != null)
                        {
                            var getAllTargets = EntityManager.Heroes.Enemies.FindAll(x => x.IsValidTarget() && x.IsInRange(canHitMoreThanOneTarget, _Q.Width));
                            var center = getAllTargets.Aggregate(Vector3.Zero, (current, x) => current + x.ServerPosition) / getAllTargets.Count;
                            if (!center.IsZero)
                            {
                                var Qpred = _Q.GetPrediction(target);
                                if (Qpred.HitChance >= HitChance.Low && target.IsValidTarget(_Q.Range))
                                {
                                    _Q.Cast(target);
                                    if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                    {

                                        Chat.Print("FOUND 1 PEOPLE FOR Q ");
                                        Console.WriteLine("FOUND 1 PEOPLE FOR Q");
                                    }
                                }

                            }
                        }

                    }
                }
                if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue)
                {

                    if (!target.IsInRange(_Player, _Q.Range))
                        return;
                    {
                        if (_Q.IsReady() && ComboMenu["UseS"].Cast<CheckBox>().CurrentValue)
                        {
                            var Qpred = _Q.GetPrediction(target);
                            if (Qpred.HitChance >= HitChance.Low && target.IsValidTarget(_Q.Range))
                            {
                                if (!target.PoisonWillExpire(250))
                                    return;
                                {
                                    _Q.Cast(target.ServerPosition);
                                    if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                                    {

                                        Chat.Print("Casting Q with HIGH pred saver ");
                                        Console.WriteLine("Casting Q with HIGH pred saver ");
                                        Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                                    }
                                }
                            }

                        }

                    }
                }

                if (!ComboMenu["UseS"].Cast<CheckBox>().CurrentValue && ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue && !ComboMenu["UseQ2"].Cast<CheckBox>().CurrentValue)
                {
                    if (_Q.IsReady())
                    {

                        var Qpred = _Q.GetPrediction(target);
                        if (Qpred.HitChance >= HitChance.Low && target.IsValidTarget(_Q.Range))
                        {
                            Core.DelayAction(() => _Q.Cast(target), 100);
                            if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                            {

                                Chat.Print("Casting Q with HIGH pred ");
                                Console.WriteLine("Casting Q with HIGH pred ");
                                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
                            }
                        }
                    }



                }

                if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue && ComboMenu["UseRG"].Cast<CheckBox>().CurrentValue && _R.IsReady())
                {
                    var Enemys = EntityManager.Heroes.Enemies.Where(x => x.IsInRange(_Player.Position, _R.Range - 25));
                    if (Enemys != null)
                    {
                        if (Enemys.Count() >= ComboMenu["UseRGs"].Cast<Slider>().CurrentValue && target.IsFacing(_Player) && ComboMenu["UseRFace"].Cast<CheckBox>().CurrentValue)
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, target);
                            _R.Cast(target);
                        }
                        if (Enemys.Count() >= ComboMenu["UseRGs"].Cast<Slider>().CurrentValue && !ComboMenu["UseRFace"].Cast<CheckBox>().CurrentValue)
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, target);
                            _R.Cast(target);
                        }
                    }

                }

                if (ComboMenu["UseR"].Cast<CheckBox>().CurrentValue && _R.IsReady())
                {
                    if (!_R.IsReady()) return;
                    {
                        if (target.IsFacing(_Player) && target.IsInRange(_Player, _R.Range) && ComboMenu["UseRFace"].Cast<CheckBox>().CurrentValue)
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, target);
                            _R.Cast(target.ServerPosition);
                        }
                    }
                    if (target.IsInRange(_Player, _R.Range) && !ComboMenu["UseRFace"].Cast<CheckBox>().CurrentValue)
                    {
                        Player.IssueOrder(GameObjectOrder.MoveTo, target);
                        _R.Cast(target.ServerPosition);
                    }

                }
                }
        }
        private static void Interruptererer(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs args)
        {
            var RintTarget = TargetSelector.GetTarget(_R.Range, DamageType.Magical);
            if (RintTarget == null) return;
            if (_R.IsReady() && sender.IsValidTarget(_R.Range) && ComboMenu["Rint"].Cast<CheckBox>().CurrentValue)
                _R.Cast(RintTarget);

        }
        private static void Gapcloser_OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!ComboMenu["RGapClose"].Cast<CheckBox>().CurrentValue) return;
            if (sender.IsEnemy)
                _R.Cast(e.Start);
        }
        public static void Ignite()
        {
            var target = TargetSelector.GetTarget(_Ignite.Range, DamageType.True);
            if (target == null)
            {
                return;
            }
            if (ComboMenu["Ignite"].Cast<CheckBox>().CurrentValue && !_Ignite.IsReady() && target.IsValidTarget()) return;

            {
                if (target.Health + target.AttackShield <
                    _Player.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite))
                {
                    _Ignite.Cast(target);
                }
            }
        }
        private static void Dash_OnDash(Obj_AI_Base sender, Dash.DashEventArgs e)
        {
            if (!ComboMenu["QComboDash"].Cast<CheckBox>().CurrentValue) return;
            if (!sender.IsEnemy) return;
            if (!_Q.IsReady()) return;
            if (e.EndPos.IsInRange(Player.Instance.Position, _Q.Range))
                _Q.Cast(e.EndPos);
        }
        private static void Skin_OnValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            Player.SetSkinId(sender.CurrentValue);
        }
        public static void KillSteal()
        {
            var targetQ = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            var targetE = TargetSelector.GetTarget(_W.Range, DamageType.Magical);
            if (targetQ == null)
            {
                return;
            }
            if (targetE == null)
            {
                return;
            }
            if (ComboMenu["UseQPok"].Cast<CheckBox>().CurrentValue)
            {
                var Qpred = _Q.GetPrediction(targetQ);
                if (Qpred.HitChance >= HitChance.High && targetQ.IsValidTarget(_Q.Range))
                {
                    if (targetQ.Health + targetQ.AttackShield < _Player.GetSpellDamage(targetQ, SpellSlot.Q))
                    {
                        if (!targetQ.IsInRange(_Player, _Q.Range) && !_Q.IsReady()) return;
                        {
                            _Q.Cast(targetQ);
                        }
                    }
                }
            }

            if (ComboMenu["UseEK"].Cast<CheckBox>().CurrentValue)
            {
                if (targetE.Health + targetE.AttackShield < _Player.GetSpellDamage(targetE, SpellSlot.E))
                {
                    if (!targetE.IsInRange(_Player, _E.Range) && !_E.IsReady()) return;
                    {
                        {
                            _E.Cast(targetE);
                        }
                    }
                }
                
            }
        }
        private static void ImmobileQ()
        {
            var target = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            if (target == null)
            {
                return;
            }
            if (ComboMenu["UseQ"].Cast<CheckBox>().CurrentValue && ComboMenu["UseQI"].Cast<CheckBox>().CurrentValue)

            {
                if (_Q.IsReady())
                {

                    var Qpred = _Q.GetPrediction(target);
                    if (Qpred.HitChance >= HitChance.Immobile && target.IsValidTarget(_Q.Range))
                    {
                        _Q.Cast(target);
                        if (DebugC["Debug"].Cast<CheckBox>().CurrentValue)
                        {

                            Chat.Print("Casting Q for immobile enemy");
                            Console.WriteLine("Casting Q for immobile enemy ");
                        }
                    }
                }
            }

        }

    }    
 
}
