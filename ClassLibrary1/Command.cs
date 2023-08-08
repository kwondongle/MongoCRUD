﻿using System;
using System.Windows.Input;

namespace Mongo.Common
{
    public class Command : ICommand
    {
        private Action<object> _executeMethod;

        public Command(Action<object> executeMethod)
        {
            _executeMethod = executeMethod;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _executeMethod(parameter);
        }
    }
}