﻿using Microsoft.AspNetCore.Components;
using Neptuo;
using Neptuo.Recollections.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Recollections.Sharing.Components
{
    public partial class ShareButton
    {
        private IApi api;

        [Inject]
        protected Api Api { get; set; }

        [Parameter]
        public string EntryId { get; set; }

        [Parameter]
        public string StoryId { get; set; }

        protected bool AreItemsLoading { get; set; }
        protected Modal Modal { get; set; }
        protected List<ShareModel> Items { get; set; }

        protected ShareModel NewShare { get; } = new ShareModel();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            if (!String.IsNullOrEmpty(EntryId))
                api = new EntryApi(Api, EntryId);
            else if (!String.IsNullOrEmpty(StoryId))
                api = new StoryApi(Api, StoryId);
            else
                throw Ensure.Exception.InvalidOperation("One of 'entryId' and 'storyId' must be provided.");
        }

        private async Task LoadAsync()
        {
            AreItemsLoading = true;
            Items = await api.GetListAsync();
            AreItemsLoading = false;
            StateHasChanged();
        }

        protected void OnShow()
        { 
            Modal.Show();
            _ = LoadAsync();
        }

        protected async Task OnAddAsync()
        {
            if (String.IsNullOrEmpty(NewShare.UserName) || String.IsNullOrWhiteSpace(NewShare.UserName))
                NewShare.UserName = null;

            await api.CreateAsync(NewShare);
            await LoadAsync();

            NewShare.UserName = null;
            NewShare.Permission = Permission.Read;
        }

        protected async Task OnDeleteAsync(ShareModel model)
        {
            await api.DeleteAsync(model);
            await LoadAsync();
        }

        interface IApi
        {
            Task<List<ShareModel>> GetListAsync();
            Task CreateAsync(ShareModel model);
            Task DeleteAsync(ShareModel model);
        }

        class EntryApi : IApi
        {
            private readonly Api api;
            private readonly string entryId;

            public EntryApi(Api api, string entryId)
            {
                Ensure.NotNull(api, "api");
                Ensure.NotNull(entryId, "entryId");
                this.api = api;
                this.entryId = entryId;
            }

            public Task CreateAsync(ShareModel model)
                => api.CreateEntryAsync(entryId, model);

            public Task DeleteAsync(ShareModel model)
                => api.DeleteEntryAsync(entryId, model);

            public Task<List<ShareModel>> GetListAsync()
                => api.GetEntryListAsync(entryId);
        }

        class StoryApi : IApi
        {
            private readonly Api api;
            private readonly string storyId;

            public StoryApi(Api api, string storyId)
            {
                Ensure.NotNull(api, "api");
                Ensure.NotNull(storyId, "storyId");
                this.api = api;
                this.storyId = storyId;
            }

            public Task CreateAsync(ShareModel model)
                => api.CreateStoryAsync(storyId, model);

            public Task DeleteAsync(ShareModel model)
                => api.DeleteStoryAsync(storyId, model);

            public Task<List<ShareModel>> GetListAsync()
                => api.GetStoryListAsync(storyId);
        }
    }
}
