using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using UnityEngine;
using TownOfHost;

namespace TownOfHost
{
    public abstract class OptionItem
    {
        #region static
        public static IReadOnlyList<OptionItem> AllOptions => _allOptions;
        private static List<OptionItem> _allOptions = new();
        public static int CurrentPreset { get; set; }
        #endregion

        // 必須情報 (コンストラクタで必ず設定させる必要がある値)
        public int Id { get; }
        public string Name { get; }
        public int DefaultValue { get; }
        public TabGroup Tab { get; }
        public bool IsSingleValue { get; }

        // 任意情報 (空・nullを許容する または、ほとんど初期値で問題ない値)
        public Color NameColor { get; protected set; }
        public OptionFormat ValueFormat { get; protected set; }
        public CustomGameMode GameMode { get; protected set; }
        public bool IsHeader { get; protected set; }
        public bool IsHidden { get; protected set; }
        public Dictionary<string, string> ReplacementDictionary
        {
            get => _replacementDictionary;
            set
            {
                if (value == null) _replacementDictionary?.Clear();
                else _replacementDictionary = value;
            }
        }
        private Dictionary<string, string> _replacementDictionary;

        // 設定値情報 (オプションの値に関わる情報)
        public int CurrentValue
        {
            get => GetValue();
            set => SetValue(value);
        }

        // 親子情報
        public OptionItem Parent { get; private set; }
        public List<OptionItem> Children;

        // 内部情報 (外部から参照することを想定していない情報)
        public ConfigEntry<int> CurrentEntry =>
            IsSingleValue ? singleEntry : AllConfigEntries[CurrentPreset];
        private ConfigEntry<int>[] AllConfigEntries;
        private ConfigEntry<int> singleEntry;


        public OptionBehaviour OptionBehaviour;
    }

    public enum TabGroup
    {
        MainSettings,
        ImpostorRoles,
        CrewmateRoles,
        NeutralRoles,
        Addons
    }
    public enum OptionFormat
    {
        None,
        Players,
        Seconds,
        Percent,
        Times,
        Multiplier,
        Votes,
        Pieces,
    }
}