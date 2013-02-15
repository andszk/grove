﻿namespace Grove.Cards
{
  using System.Collections.Generic;
  using Core;
  using Core.Ai.TargetingRules;
  using Core.Ai.TimingRules;
  using Core.Costs;
  using Core.Dsl;
  using Core.Effects;
  using Core.Modifiers;

  public class ManaLeech : CardsSource
  {
    public override IEnumerable<CardFactory> GetCards()
    {
      yield return Card
        .Named("Mana Leech")
        .ManaCost("{2}{B}")
        .Type("Creature Leech")
        .Text(
          "You may choose not to untap Mana Leech during your untap step.{EOL}{T}: Tap target land. It doesn't untap during its controller's untap step for as long as Mana Leech remains tapped.")
        .Power(1)
        .Toughness(1)
        .MayChooseNotToUntapDuringUntap()
        .ActivatedAbility(p =>
          {
            p.Text =
              "{T}: Tap target land. It doesn't untap during its controller's untap step for as long as Mana Leech remains tapped.";
            p.Cost = new Tap();

            p.Effect = () => new CompoundEffect(
              new TapTarget(),
              new ApplyModifiersToTargets(() =>
                {
                  var modifier = new AddStaticAbility(Static.DoesNotUntap);

                  modifier.AddLifetime(new PermanentGetsUntapedLifetime(
                    l => l.Modifier.Source));

                  return modifier;
                }));

            p.TargetSelector.AddEffect(trg => trg.Is.Card(c => c.Is().Land).On.Battlefield());
            p.TimingRule(new Turn(passive: true));
            p.TimingRule(new Steps(Step.Upkeep));
            p.TargetingRule(new TapLands());
          });
    }
  }
}