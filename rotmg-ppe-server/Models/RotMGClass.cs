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
using static rotmg_ppe_server.models.ItemCategory;
namespace rotmg_ppe_server.models;

public enum RotMGClass: int
{
    [JsonProperty(PropertyName = "rouge")] 
    Rouge = Dagger | Cloak | LeatherArmor | Ring,

    [JsonProperty(PropertyName = "archer")]
    Archer = Bow | Quiver | LeatherArmor | Ring,

    [JsonProperty(PropertyName = "wizard")]
    Wizard = Staff | Spell | Robe | Ring,

    [JsonProperty(PropertyName = "priest")]
    Priest = Wand | Tome | Robe | Ring,

    [JsonProperty(PropertyName = "warrior")]
    Warrior = Sword | Helm | HeavyArmor | Ring,

    [JsonProperty(PropertyName = "knight")]
    Knight = Sword | HeavyArmor | Shield | Ring,

    [JsonProperty(PropertyName = "paladin")]
    Paladin = Sword | HeavyArmor | Seal | Ring,

    [JsonProperty(PropertyName = "assassin")]
    Assassin = Dagger | LeatherArmor | Poison | Ring,

    [JsonProperty(PropertyName = "necromancer")]
    Necromancer = Staff | Robe | Skull | Ring,

    [JsonProperty(PropertyName = "huntress")]
    Huntress = Bow | Trap | LeatherArmor | Ring,

    [JsonProperty(PropertyName = "mystic")]
    Mystic = Staff | Orb | Robe | Ring,

    [JsonProperty(PropertyName = "trickster")]
    Trickster = Dagger | Prism | LeatherArmor | Ring,

    [JsonProperty(PropertyName = "sorcerer")]
    Sorcerer = Wand | Scepter | Robe | Ring,

    [JsonProperty(PropertyName = "ninja")] Ninja = Katana | Star | LeatherArmor | Ring,

    [JsonProperty(PropertyName = "samurai")]
    Samurai = Katana | Wakizashi | HeavyArmor | Ring,

    [JsonProperty(PropertyName = "bard")] Bard = Bow | Lute | Robe | Ring,

    [JsonProperty(PropertyName = "summoner")]
    Summoner = Wand | Mace | Robe | Ring,

    [JsonProperty(PropertyName = "kensei")]
    Kensei = Katana | Sheath | HeavyArmor | Ring
}