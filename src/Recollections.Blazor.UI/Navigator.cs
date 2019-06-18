﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Recollection
{
    public class Navigator
    {
        private readonly IUriHelper uri;
        private readonly IJSRuntime jsRuntime;

        public Navigator(IUriHelper uri, IJSRuntime jsRuntime)
        {
            Ensure.NotNull(uri, "uri");
            Ensure.NotNull(jsRuntime, "jsRuntime");
            this.uri = uri;
            this.jsRuntime = jsRuntime;
        }

        public Task<bool> AskAsync(string message)
            => jsRuntime.InvokeAsync<bool>("window.confirm", message);

        public string UrlTimeline()
            => "/";

        public void OpenTimeline()
            => uri.NavigateTo(UrlTimeline());

        public string UrlEntryDetail(string entryId)
            => $"/entry/{entryId}";

        public void OpenEntryDetail(string entryId)
            => uri.NavigateTo(UrlEntryDetail(entryId));
    }
}