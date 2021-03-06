﻿using Neptuo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neptuo.Recollections.Entries.Events
{
    public class StoryEntriesChanged
    {
        public string StoryId { get; }
        public string ChapterId { get; }
        public string EntryId { get; }

        public StoryEntriesChanged(string storyId, string chapterId, string entryId)
        {
            Ensure.NotNull(storyId, "storyId");
            Ensure.NotNull(entryId, "entryId");
            StoryId = storyId;
            ChapterId = chapterId;
            EntryId = entryId;
        }
    }
}
