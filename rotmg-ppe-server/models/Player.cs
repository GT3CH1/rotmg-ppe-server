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

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace rotmg_ppe_server.models;

[PrimaryKey("PlayerId")]
[JsonObject(MemberSerialization.OptIn)]
public class Player
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty(PropertyName = "id")]
    public int PlayerId { get; set; }

    [JsonProperty(PropertyName = "name")] public string? Name { get; set; } = null!;

    [JsonProperty(PropertyName = "dead")] public bool? IsDead { get; set; } = false;
    [JsonProperty(PropertyName = "upe")] public bool? IsUpe { get; set; } = null!;

    [JsonProperty(PropertyName = "items")] public virtual List<Item>? Items { get; set; }

    [JsonProperty(PropertyName = "worth")] public int? Worth => GetWorth();

    [JsonProperty(PropertyName = "class")] public RotMGClass? CharacterClass { get; set; } = null!;
    public int GetClass() => (int)CharacterClass;

    public Player()
    {
    }

    public bool Dead() => IsDead ?? false;

    public int GetWorth()
    {
        var scalar = 1.0f;
        if (Items == null || Items.Count == 0)
            return 0;
        if (IsUpe.GetValueOrDefault())
            scalar = 1.5f;
        return (int)(Items.Sum(i => i.Worth) * scalar);
    }
    
    public bool ItemValidForClass(Item i)
    {
        return ItemValidForClass(i, CharacterClass.GetValueOrDefault());
    }

    public static bool ItemValidForClass(Item i, RotMGClass c)
    {
        return (i.ItemType & (int)c) != 0;
    }
    
    public static bool ItemValidForClass(ItemCategory i, RotMGClass c)
    {
        return ((int)i & (int)c) != 0;
    }
}