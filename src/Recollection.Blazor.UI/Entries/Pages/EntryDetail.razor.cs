﻿using Microsoft.AspNetCore.Components;
using Neptuo.Recollection.Accounts.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Recollection.Entries.Pages
{ 
    public class EntryDetailModel : ComponentBase
    {
        [Inject]
        protected Api Api { get; set; }

        [Inject]
        protected Navigator Navigator { get; set; }

        [CascadingParameter]
        protected UserStateModel UserState { get; set; }

        [Parameter]
        protected string EntryId { get; set; }

        protected EntryModel Model { get; set; }

        protected async override Task OnInitAsync()
        {
            await base.OnInitAsync();
            await UserState.EnsureAuthenticated();

            Model = await Api.GetDetailAsync(EntryId);
        }

        protected async Task SaveTitleAsync(string value)
        {
            Model.Title = value;
            await SaveAsync();
        } 

        protected async Task SaveTextAsync(string value)
        {
            Model.Text = value;
            await SaveAsync();
        }

        protected async Task SaveWhenAsync(DateTime value)
        {
            Model.When = value;
            await SaveAsync();
        }

        private Task SaveAsync() => Api.UpdateAsync(Model);

        public async Task DeleteAsync()
        {
            if (await Navigator.AskAsync($"Do you really want to delete entry '{Model.Title}'?"))
            {
                await Api.DeleteAsync(Model.Id);
                Navigator.OpenTimeline();
            }
        }
    }
}
