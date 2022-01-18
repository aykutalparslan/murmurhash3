/*
 * Copyright(c) 2022 Aykut Alparslan KOC <aykutalparslan@msn.com>
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

// Ported from the original C++ implementation located at:
//		* https://github.com/aappleby/smhasher/blob/master/src/
//		* MurmurHash3.cpp
// original-header 
// MurmurHash3 was written by Austin Appleby, and is placed in the public
// domain. The author hereby disclaims copyright to this source code.
// original-header

using System;
using System.Runtime.InteropServices;

namespace MurmurHash;
public static class MurmurHashExtensions
{
	private static uint rotl32(uint x, byte r)
	{
		return (x << r) | (x >> (32 - r));
	}
	private static ulong rotl64(ulong x, byte r)
	{
		return (x << r) | (x >> (64 - r));
	}
	private static uint fmix32(uint h)
	{
		h ^= h >> 16;
		h *= 0x85ebca6b;
		h ^= h >> 13;
		h *= 0xc2b2ae35;
		h ^= h >> 16;

		return h;
	}
	private static ulong fmix64(ulong k)
	{
		k ^= k >> 33;
		k *= 0xff51afd7ed558ccd;
		k ^= k >> 33;
		k *= 0xc4ceb9fe1a85ec53;
		k ^= k >> 33;

		return k;
	}
	public static int GetMurmur32BitsX86(this byte[] data, uint seed = 0)
	{
		return GetMurmurHash32BitsX86(data, seed);
	}
	public static int GetMurmur32BitsX86(this Span<byte> data, uint seed = 0)
	{
		return GetMurmurHash32BitsX86(data, seed);
	}
	public static int GetMurmur32BitsX86(this ReadOnlySpan<byte> data, uint seed = 0)
    {
		return GetMurmurHash32BitsX86(data,seed);
	}
	private static int GetMurmurHash32BitsX86(ReadOnlySpan<byte> data, uint seed)
	{
		int nblocks = data.Length / 4;

		uint h1 = seed;
		uint k1 = 0;
		const uint c1 = 0xcc9e2d51;
		const uint c2 = 0x1b873593;

		ReadOnlySpan<uint> blocks = MemoryMarshal.Cast<byte, uint>(data);

		for (int i = 0; i < nblocks; i++)
		{
			k1 = blocks[i];

			k1 *= c1;
			k1 = rotl32(k1, 15);
			k1 *= c2;

			h1 ^= k1;
			h1 = rotl32(h1, 13);
			h1 = h1 * 5 + 0xe6546b64;
		}


		k1 = 0;

		switch (data.Length % 4)
		{
			case 3:
				k1 ^= ((uint)data[nblocks * 4 + 2]) << 16;
				goto case 2;
			case 2:
				k1 ^= ((uint)data[nblocks * 4 + 1]) << 8;
				goto case 1;
			case 1:
				k1 ^= ((uint)data[nblocks * 4]);
				k1 *= c1; k1 = rotl32(k1, 15); k1 *= c2; h1 ^= k1;
				break;
		};

		h1 ^= (uint)data.Length;
		h1 = fmix32(h1);

		return unchecked((int)h1);
	}
	public static uint[] GetMurmur128BitsX86(this byte[] data, uint seed = 0)
	{
		return GetMurmurHash128BitsX86(data, seed);
	}
	public static uint[] GetMurmur128BitsX86(this Span<byte> data, uint seed = 0)
	{
		return GetMurmurHash128BitsX86(data, seed);
	}
	public static uint[] GetMurmur128BitsX86(this ReadOnlySpan<byte> data, uint seed = 0)
    {
		return GetMurmurHash128BitsX86(data, seed);
    }
	private static uint[] GetMurmurHash128BitsX86(ReadOnlySpan<byte> data, uint seed)
	{
		int nblocks = data.Length / 16;

		uint h1 = seed;
		uint h2 = seed;
		uint h3 = seed;
		uint h4 = seed;

		uint c1 = 0x239b961b;
		uint c2 = 0xab0e9789;
		uint c3 = 0x38b34ae5;
		uint c4 = 0xa1e38b93;

		uint k1 = 0;
		uint k2 = 0;
		uint k3 = 0;
		uint k4 = 0;

		ReadOnlySpan<uint> blocks = MemoryMarshal.Cast<byte, uint>(data);

		for (int i = 0; i < nblocks; i++)
		{
			k1 = blocks[i * 4];
			k2 = blocks[i * 4 + 1];
			k3 = blocks[i * 4 + 2];
			k4 = blocks[i * 4 + 3];

			k1 *= c1; k1 = rotl32(k1, 15); k1 *= c2; h1 ^= k1;

			h1 = rotl32(h1, 19); h1 += h2; h1 = h1 * 5 + 0x561ccd1b;

			k2 *= c2; k2 = rotl32(k2, 16); k2 *= c3; h2 ^= k2;

			h2 = rotl32(h2, 17); h2 += h3; h2 = h2 * 5 + 0x0bcaa747;

			k3 *= c3; k3 = rotl32(k3, 17); k3 *= c4; h3 ^= k3;

			h3 = rotl32(h3, 15); h3 += h4; h3 = h3 * 5 + 0x96cd1c35;

			k4 *= c4; k4 = rotl32(k4, 18); k4 *= c1; h4 ^= k4;

			h4 = rotl32(h4, 13); h4 += h1; h4 = h4 * 5 + 0x32ac3b17;
		}

		k1 = 0;
		k2 = 0;
		k3 = 0;
		k4 = 0;

		switch (data.Length % 16)
		{
			case 15:
				k4 ^= (uint)data[nblocks * 16 + 14] << 16;
				goto case 14;
			case 14:
				k4 ^= (uint)data[nblocks * 16 + 13] << 8;
				goto case 13;
			case 13:
				k4 ^= (uint)data[nblocks * 16 + 12] << 0;
				k4 *= c4; k4 = rotl32(k4, 18); k4 *= c1; h4 ^= k4;
				goto case 12;
			case 12:
				k3 ^= (uint)data[nblocks * 16 + 11] << 24;
				goto case 11;
			case 11:
				k3 ^= (uint)data[nblocks * 16 + 10] << 16;
				goto case 10;
			case 10:
				k3 ^= (uint)data[nblocks * 16 + 9] << 8;
				goto case 9;
			case 9:
				k3 ^= (uint)data[nblocks * 16 + 8] << 0;
				k3 *= c3; k3 = rotl32(k3, 17); k3 *= c4; h3 ^= k3;
				goto case 8;
			case 8:
				k2 ^= (uint)data[nblocks * 16 + 7] << 24;
				goto case 7;
			case 7:
				k2 ^= (uint)data[nblocks * 16 + 6] << 16;
				goto case 6;
			case 6:
				k2 ^= (uint)data[nblocks * 16 + 5] << 8;
				goto case 5;
			case 5:
				k2 ^= (uint)data[nblocks * 16 + 4] << 0;
				k2 *= c2; k2 = rotl32(k2, 16); k2 *= c3; h2 ^= k2;
				goto case 4;
			case 4:
				k1 ^= (uint)data[nblocks * 16 + 3] << 24;
				goto case 3;
			case 3:
				k1 ^= (uint)data[nblocks * 16 + 2] << 16;
				goto case 2;
			case 2:
				k1 ^= (uint)data[nblocks * 16 + 1] << 8;
				goto case 1;
			case 1:
				k1 ^= (uint)data[nblocks * 16 + 0] << 0;
				k1 *= c1; k1 = rotl32(k1, 15); k1 *= c2; h1 ^= k1;
				break;
		};

		h1 ^= (uint)data.Length; h2 ^= (uint)data.Length;
		h3 ^= (uint)data.Length; h4 ^= (uint)data.Length;

		h1 += h2; h1 += h3; h1 += h4;
		h2 += h1; h3 += h1; h4 += h1;

		h1 = fmix32(h1);
		h2 = fmix32(h2);
		h3 = fmix32(h3);
		h4 = fmix32(h4);

		h1 += h2; h1 += h3; h1 += h4;
		h2 += h1; h3 += h1; h4 += h1;
		return new[] { h1, h2, h3, h4 };
	}
	public static ulong[] GetMurmur128BitsX64(this byte[] data, uint seed = 0)
	{
		return GetMurmurHash128BitsX64(data, seed);
	}
	public static ulong[] GetMurmur128BitsX64(this Span<byte> data, uint seed = 0)
	{
		return GetMurmurHash128BitsX64(data, seed);
	}
	public static ulong[] GetMurmur128BitsX64(this ReadOnlySpan<byte> data, uint seed = 0)
    {
		return GetMurmurHash128BitsX64(data, seed);
    }
	private static ulong[] GetMurmurHash128BitsX64(ReadOnlySpan<byte> data, uint seed)
	{
		int nblocks = data.Length / 16;

		ulong h1 = seed;
		ulong h2 = seed;

		ulong c1 = 0x87c37b91114253d5;
		ulong c2 = 0x4cf5ad432745937f;

		ulong k1 = 0;
		ulong k2 = 0;

		ReadOnlySpan<ulong> blocks = MemoryMarshal.Cast<byte, ulong>(data);

		for (int i = 0; i < nblocks; i++)
		{
			k1 = blocks[i * 2];
			k2 = blocks[i * 2 + 1];

			k1 *= c1; k1 = rotl64(k1, 31); k1 *= c2; h1 ^= k1;

			h1 = rotl64(h1, 27); h1 += h2; h1 = h1 * 5 + 0x52dce729;

			k2 *= c2; k2 = rotl64(k2, 33); k2 *= c1; h2 ^= k2;

			h2 = rotl64(h2, 31); h2 += h1; h2 = h2 * 5 + 0x38495ab5;
		}

		k1 = 0;
		k2 = 0;

		switch (data.Length % 16)
		{
			case 15:
				k2 ^= ((ulong)data[nblocks * 16 + 14]) << 48;
				goto case 14;
			case 14:
				k2 ^= ((ulong)data[nblocks * 16 + 13]) << 40;
				goto case 13;
			case 13:
				k2 ^= ((ulong)data[nblocks * 16 + 12]) << 32;
				goto case 12;
			case 12:
				k2 ^= ((ulong)data[nblocks * 16 + 11]) << 24;
				goto case 11;
			case 11:
				k2 ^= ((ulong)data[nblocks * 16 + 10]) << 16;
				goto case 10;
			case 10:
				k2 ^= ((ulong)data[nblocks * 16 + 9]) << 8;
				goto case 9;
			case 9:
				k2 ^= ((ulong)data[nblocks * 16 + 8]) << 0;
				k2 *= c2; k2 = rotl64(k2, 33); k2 *= c1; h2 ^= k2;
				goto case 8;
			case 8:
				k1 ^= ((ulong)data[nblocks * 16 + 7]) << 56;
				goto case 7;
			case 7:
				k1 ^= ((ulong)data[nblocks * 16 + 6]) << 48;
				goto case 6;
			case 6:
				k1 ^= ((ulong)data[nblocks * 16 + 5]) << 40;
				goto case 5;
			case 5:
				k1 ^= ((ulong)data[nblocks * 16 + 4]) << 32;
				goto case 4;
			case 4:
				k1 ^= ((ulong)data[nblocks * 16 + 3]) << 24;
				goto case 3;
			case 3:
				k1 ^= ((ulong)data[nblocks * 16 + 2]) << 16;
				goto case 2;
			case 2:
				k1 ^= ((ulong)data[nblocks * 16 + 1]) << 8;
				goto case 1;
			case 1:
				k1 ^= ((ulong)data[nblocks * 16]) << 0;
				k1 *= c1; k1 = rotl64(k1, 31); k1 *= c2; h1 ^= k1;
				break;
		};

		h1 ^= (uint)data.Length; h2 ^= (uint)data.Length;

		h1 += h2;
		h2 += h1;

		h1 = fmix64(h1);
		h2 = fmix64(h2);

		h1 += h2;
		h2 += h1;
		return new[] { h1, h2 };
	}
}


