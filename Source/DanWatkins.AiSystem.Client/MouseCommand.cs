﻿using Eto.Forms;
using System;

namespace DanWatkins.AiSystem.Client
{
    public class MouseCommand
    {
        public event EventHandler<MouseEventArgs> Executed;

        public void Execute(object sender, MouseEventArgs eventArgs)
        {
            Executed?.Invoke(sender, eventArgs);
        }
    }
}