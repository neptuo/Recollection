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
        [Parameter]
        protected string EntryId { get; set; }

        [Parameter]
        protected string ImageId { get; set; }

        [CascadingParameter]
        protected UserStateModel UserState { get; set; }

        [Inject]
        protected Api Api { get; set; }

        protected ImageModel Model { get; set; }

        protected async override Task OnInitAsync()
        {
            await base.OnInitAsync();
            await UserState.EnsureAuthenticatedAsync();

            Model = await Api.GetImageAsync(EntryId, ImageId);
        }
    }
}
