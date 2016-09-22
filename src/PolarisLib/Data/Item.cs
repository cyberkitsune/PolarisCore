using System;

namespace Polaris.Lib.Data
{
	/*
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct PSO2ItemConsumable
	{
		long guid;
		int ID;
		int subID;
		short unused1;
		short quantity;
		fixed int unused2[9];
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public unsafe struct PSO2ItemWeapon
	{
		long guid;
		int ID;
		int subID;
		byte flags;
		byte element;
		byte force;
		byte grind;
		byte grindPercent;
		byte unknown1;
		short unknown2;
		fixed short affixes[8];
		int potential;
		byte extend;
		byte unknown3;
		short unknown4;
		int unknown5;
		int unknown6;
	}
	*/

	public enum ItemType
	{
		Consumable,
		Weapon,
		Costume,
		Unit,
		Room
	}

	[Flags]
	public enum ItemFlags
	{
		Locked = 0x01,
		BoundToOwner = 0x02
	}

	public enum ItemElement
	{
		None,
		Fire,
		Ice,
		Lightning,
		Wind,
		Light,
		Dark
	}

	public class Item
	{
	}
}
