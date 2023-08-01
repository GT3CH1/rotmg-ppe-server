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

namespace rotmg_ppe_server.models;
public enum ItemCategory : int
{
    Sword = 1, // 1
    Dagger = 1 << 1, // 2
    Katana = 1 << 2, // 4
    Bow = 1 << 3, // 8
    Wand = 1 << 4, // 16
    Staff = 1 << 5, // 32
    Cloak = 1 << 6, // 64
    Quiver = 1 << 7, // 128
    Spell = 1 << 8, // 256
    Tome = 1 << 9, // 512
    Helm = 1 << 10, // 1024
    Shield = 1 << 11, // 2048
    Seal = 1 << 12, // 4096
    Poison = 1 << 13, // 8192
    Skull = 1 << 14, // 16384
    Trap = 1 << 15, // 32768
    Orb = 1 << 16, // 65536
    Prism = 1 << 17, // 131072
    Scepter = 1 << 18, // 262144
    Star = 1 << 19, // 524288
    Wakizashi = 1 << 20, // 1048576
    Lute = 1 << 21, // 2097152
    Mace = 1 << 22, // 4194304
    Sheath = 1 << 23, // 8388608
    LeatherArmor = 1 << 24, // 16777216
    Robe = 1 << 25, // 33554432
    HeavyArmor = 1 << 26, // 67108864
    Ring = 1 << 27, // 134217728
    UT = 1 << 28, // 268435456
    ST = 1 << 29 // 536870912
}