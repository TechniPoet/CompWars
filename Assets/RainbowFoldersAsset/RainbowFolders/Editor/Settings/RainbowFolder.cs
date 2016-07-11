﻿/*
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */

using System;
using UnityEngine;

namespace Borodar.RainbowFolders.Editor.Settings
{
    [Serializable]
    public class RainbowFolder
    {
        [ObsoleteAttribute("Use RainbowFolder.Key and RainbowFolder.Type instead.", false)]
        public string Name;

        public string Key;
        public KeyType Type;

        public Texture2D SmallIcon;
        public Texture2D LargeIcon;

        public enum KeyType
        {
            Name,
            Path
        }
    }
}