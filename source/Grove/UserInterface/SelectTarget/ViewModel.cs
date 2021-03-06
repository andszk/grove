﻿namespace Grove.UserInterface.SelectTarget
{
  using System;
  using System.Collections.Generic;
  using Caliburn.Micro;
  using Infrastructure;
  using Messages;

  public class ViewModel : ViewModelBase, IReceive<SelectionChanged>
  {
    private readonly bool _canCancel;
    private readonly BindableCollection<ITarget> _selection = new BindableCollection<ITarget>();
    private readonly Action<ITarget> _targetSelected;
    private readonly Action<ITarget> _targetUnselected;
    private readonly object _triggerMessage;
    private readonly int? _x;

    public ViewModel(SelectTargetParameters p)
    {
      TargetValidator = p.Validator;
      _canCancel = p.CanCancel;
      _targetSelected = p.TargetSelected ?? DefaultTargetSelected;
      _targetUnselected = p.TargetUnselected ?? DefaultTargetUnselected;
      _triggerMessage = p.TriggerMessage;
      _x = p.X;

      Instructions = p.Instructions ?? GetDefaultInstructions();
    }

    private bool CanAutoComplete
    {
      get
      {
        return TargetValidator.MaxCount != null &&
          _selection.Count == TargetValidator.MaxCount.GetValue(_x);
      }
    }

    public string Text { get { return TargetValidator.GetMessage(_selection.Count + 1, _x); } }
    public string Instructions { get; private set; }
    private bool IsDone { get { return _selection.Count >= TargetValidator.MinCount.GetValue(_x); } }
    public IList<ITarget> Selection { get { return _selection; } }
    public TargetValidator TargetValidator { get; private set; }
    public bool WasCanceled { get; private set; }

    [Updates("Text")]
    public virtual void Receive(SelectionChanged message)
    {
      if (TargetValidator.MaxCount != null &&
        _selection.Count >= TargetValidator.MaxCount.GetValue(_x))
        return;

      if (_selection.Contains(message.Selection))
      {
        _targetUnselected(message.Selection);
        _selection.Remove(message.Selection);
        return;
      }

      if (TargetValidator.HasValidZone(message.Selection) == false)
        return;

      if (TargetValidator.IsTargetValid(message.Selection, _triggerMessage) == false)
        return;

      _targetSelected(message.Selection);
      _selection.Add(message.Selection);

      if (CanAutoComplete)
        Done();
    }

    private string GetDefaultInstructions()
    {
      if (TargetValidator.MaxCount == null ||
        TargetValidator.MinCount.GetValue(_x) < TargetValidator.MaxCount.GetValue(_x))
      {
        if (_canCancel)
          return "(Press Enter when done. Press Esc to cancel.)";

        return "(Press Enter when done.)";
      }

      if (_canCancel)
        return "(Press Esc to cancel.)";

      return null;
    }

    private void DefaultTargetSelected(ITarget target)
    {
      Publisher.Publish(new TargetSelected { Target = target });
    }

    private void DefaultTargetUnselected(ITarget target)
    {
      Publisher.Publish(new TargetUnselected { Target = target });
    }

    public void Cancel()
    {
      if (!_canCancel)
        return;

      _selection.Clear();
      WasCanceled = true;
      this.Close();
    }

    public void Done()
    {
      if (!IsDone)
        return;

      this.Close();
    }

    public interface IFactory
    {
      ViewModel Create(SelectTargetParameters p);
    }
  }
}