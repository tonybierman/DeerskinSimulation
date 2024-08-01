namespace DeerskinSimulation.Models
{
    using System;

    [Flags]
    public enum Terrains
    {
        Option1 = 1 << 0,  // 1
        Option2 = 1 << 1,  // 2
        Option3 = 1 << 2,  // 4
        Option4 = 1 << 3,  // 8
        Option5 = 1 << 4,  // 16
        Option6 = 1 << 5,  // 32
        Option7 = 1 << 6,  // 64
        Option8 = 1 << 7,  // 128
        Option9 = 1 << 8,  // 256
        Option10 = 1 << 9,  // 512
        Option11 = 1 << 10, // 1024
        Option12 = 1 << 11, // 2048
        Option13 = 1 << 12, // 4096
        Option14 = 1 << 13, // 8192
        Option15 = 1 << 14, // 16384
        Option16 = 1 << 15, // 32768
        Option17 = 1 << 16, // 65536
        Option18 = 1 << 17, // 131072
        Option19 = 1 << 18, // 262144
        Option20 = 1 << 19, // 524288
        Option21 = 1 << 20, // 1048576
        Option22 = 1 << 21, // 2097152
        Option23 = 1 << 22, // 4194304
        Option24 = 1 << 23, // 8388608
        Option25 = 1 << 24, // 16777216
        Option26 = 1 << 25, // 33554432
        Option27 = 1 << 26, // 67108864
        Option28 = 1 << 27, // 134217728
        Option29 = 1 << 28, // 268435456
        Option30 = 1 << 29, // 536870912
        Option31 = 1 << 30, // 1073741824
        Option32 = 1 << 31, // 2147483648
    }
}
