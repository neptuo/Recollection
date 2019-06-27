﻿using Microsoft.AspNetCore.Components;
using Neptuo.Recollections.Accounts.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Recollections.Entries.Pages
{
    public class ImageDetailModel : ComponentBase
    {
        [Inject]
        protected Navigator Navigator { get; set; }

        [Inject]
        protected Api Api { get; set; }

        [Inject]
        protected UiOptions UiOptions { get; set; }

        [Parameter]
        protected string EntryId { get; set; }

        [Parameter]
        protected string ImageId { get; set; }

        [CascadingParameter]
        protected UserStateModel UserState { get; set; }

        protected ImageModel Model { get; set; }

        protected async override Task OnInitAsync()
        {
            await base.OnInitAsync();
            await UserState.EnsureAuthenticatedAsync();

            Model = await Api.GetImageAsync(EntryId, ImageId);
        }

        protected async Task SaveNameAsync(string value)
        {
            Model.Name = value;
            await SaveAsync();
        }

        protected async Task SaveDescriptionAsync(string value)
        {
            Model.Description = value;
            await SaveAsync();
        }

        protected async Task SaveWhenAsync(DateTime value)
        {
            Model.When = value;
            await SaveAsync();
        }

        private Task SaveAsync() => Api.UpdateImageAsync(EntryId, Model);

        protected async Task DeleteAsync()
        {
            if (await Navigator.AskAsync($"Do you really want to delete this image?"))
            {
                await Api.DeleteImageAsync(EntryId, ImageId);
                Navigator.OpenEntryDetail(EntryId);
            }
        }
    }
}