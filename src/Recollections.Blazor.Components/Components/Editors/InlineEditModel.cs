﻿using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Recollections.Components.Editors
{
    public class InlineEditModel<T> : ComponentBase
    {
        private T originalValue;

        [Parameter]
        protected T Value { get; set; }

        [Parameter]
        protected Action<T> ValueChanged { get; set; }

        [Parameter]
        protected string PlaceHolder { get; set; }

        protected bool IsEditMode { get; set; }

        protected void OnEdit()
        {
            IsEditMode = true;
            originalValue = Value;
        }

        protected Task OnSaveValueAsync()
        {
            IsEditMode = false;
            return OnValueChangedAsync();
        }

        protected Task OnResetAsync()
        {
            IsEditMode = false;
            Value = originalValue;
            return Task.CompletedTask;
        }

        protected virtual Task OnValueChangedAsync()
        {
            ValueChanged?.Invoke(Value);
            return Task.CompletedTask;
        }

        protected string GetModeCssClass() 
            => IsEditMode ? String.Empty : "inline-editor-viewmode";
    }
}
