using System;

namespace Polaris.Lib.Data
{
    /*
        Hoo boy, this is 468 bytes!
        TODO: Map out this struct.
        Most of this is WRONG!!! Needs serious investigation.
        
        [K873] What I've currently mapped out
          24  -> Start
          38  -> Quest Name/Type Index?
          100 -> Bitfield 1
          102 -> Estimated Play Time
          103 -> Party Type
          104 -> Difficulties Available
          105 -> Difficulties Completed
          108 -> Starting Level
          120 -> Item Data 1?
          12C -> Item Data 2?
    */

    [Flags]
    public enum QuestBitfield1 : ushort
    {
        MatterObjectiveQuest = 0x0001,
        ClientOrderOnQuest = 0x0008,
        NewQuest = 0x0100,
        ClientOrder = 0x0800,
        UnknownLevel = 0x1000
    }

    public enum PartyType
    {
        SoloQuest,
        SinglePartyQuest,
        MultiPartyQuest,
    }

    public enum EstimatedTime
    {
        Short = 1,
        Medium,
        Long
    }

    [Flags]
    public enum Difficulties
    {
        Normal = 0x01,
        hard = 0x02,
        VeryHard = 0x04,
        SuperHard = 0x08,
        ExtraHard = 0x10,
        Dummy1 = 0x20,
        Dummy2 = 0x40,
        Dummy3 = 0x80,
    }

    public class QuestList
    {
    }
}
