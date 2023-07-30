// Copyright (c) 2022. Gavin Pease and contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
// following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial
// portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT
// OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System.ComponentModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace rotmg_ppe_server.models;

public enum RotMGClass
{
    
    [EnumMember(Value="rouge")]
    [JsonProperty(PropertyName = "rouge")] 
    Rouge,
    [EnumMember(Value="archer")]
    [JsonProperty(PropertyName = "archer")]
    Archer,
    [EnumMember(Value="wizard")]
    [JsonProperty(PropertyName = "wizard")]
    Wizard,
    [EnumMember(Value="priest")]
    [JsonProperty(PropertyName = "priest")]
    Priest,
    [EnumMember(Value="warrior")]
    [JsonProperty(PropertyName = "warrior")]
    Warrior,
    [EnumMember(Value="knight")]
    [JsonProperty(PropertyName = "knight")]
    Knight,
    [EnumMember(Value="paladin")]
    [JsonProperty(PropertyName = "paladin")]
    Paladin,
    [EnumMember(Value="assassin")]
    [JsonProperty(PropertyName = "assassin")]
    Assassin,
    [EnumMember(Value="necromancer")]
    [JsonProperty(PropertyName = "necromancer")]
    Necromancer,
    [EnumMember(Value="huntress")]
    [JsonProperty(PropertyName = "huntress")]
    Huntress,
    [EnumMember(Value="mystic")]
    [JsonProperty(PropertyName = "mystic")]
    Mystic,
    [EnumMember(Value="trickster")]
    [JsonProperty(PropertyName = "trickster")]
    Trickster,
    [EnumMember(Value="sorcerer")]
    [JsonProperty(PropertyName = "sorcerer")]
    Sorcerer,
    [EnumMember(Value="ninja")]
    [JsonProperty(PropertyName = "ninja")] 
    Ninja,
    [EnumMember(Value="samurai")]
    [JsonProperty(PropertyName = "samurai")]
    Samurai,
    [EnumMember(Value="bard")]
    [JsonProperty(PropertyName = "bard")]
    Bard,
    [EnumMember(Value="summoner")]
    [JsonProperty(PropertyName = "summoner")]
    Summoner,
    [EnumMember(Value="kensei")]
    [JsonProperty(PropertyName = "kensei")]
    Kensei
    
}