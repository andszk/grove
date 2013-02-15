﻿namespace Grove.Core.Modifiers
{
  using Infrastructure;
  using Messages;
  using Targeting;

  public class LevelLifetime : Lifetime, IReceive<LevelChanged>
  {
    private readonly int? _maxLevel;
    private readonly int _minLevel;
    private Card _modifierTarget;

    public LevelLifetime(int minLevel, int? maxLevel)
    {
      _minLevel = minLevel;
      _maxLevel = maxLevel;      
    }

    public override void Initialize(Modifier modifier, Game game)
    {
      base.Initialize(modifier, game);
      _modifierTarget = modifier.Target.Card();
    }

    public void Receive(LevelChanged message)
    {
      if (message.Card != _modifierTarget)
        return;

      if (_modifierTarget.Level < _minLevel ||
        _modifierTarget.Level > _maxLevel)
      {
        End();
      }
    }
  }
}