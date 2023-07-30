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
using System.Runtime.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace rotmg_ppe_server.models;

[JsonObject(MemberSerialization.OptIn)]
[PrimaryKey("ItemId")]
public class Item
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [JsonProperty(PropertyName = "id")]
    public int ItemId { get; set; }

    public virtual List<Player>? Players { get; }

    [JsonProperty(PropertyName = "name")] public string Name { get; set; }

    [JsonProperty(PropertyName = "worth")] public int? Worth { get; set; } = 0;


    [JsonProperty(PropertyName = "itemType")]
    public int ItemType { get; set; } = 0;

    public bool Soulbound() => (ItemType & (int)ItemCategory.UT) != 0;

    public bool ValidForClass(RotMGClass rotMgClass)
    {
        return (ItemType | (int)rotMgClass) != 0;
    }


    public Item(string name)
    {
        Name = name;
    }

    public Item(string name, int worth)
    {
        Name = name;
        Worth = worth;
    }

    public Item()
    {
    }

    public void SetCategory(ItemCategory category)
    {
        ItemType = ItemType | (int)category;
    }

    public bool IsWeapon()
    {
        return (ItemType >> 5 | 0x1F) != 0;
    }

    public bool IsRing() => IsOfType(ItemCategory.Ring) || (IsOfType(ItemCategory.Ring) && IsOfType(ItemCategory.UT));
    public bool isUT() => IsOfType(ItemCategory.UT);

    public bool IsOfType(ItemCategory category)
    {
        return (ItemType & (int)category) != 0;
    }

    public bool IsArmor()
    {
        // get bits 24-26
        return (ItemType >> 24 | 0x7) != 0;
    }

    public bool IsAbility()
    {
        return (ItemType >> 6 & 0x3FFFF) != 0;
    }
}