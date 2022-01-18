## MurmurHash3 Implementation in C#

Adds extension methods for MurmurHash3
- GetMurmur32BitsX86
- GetMurmur128BitsX86
- GetMurmur128BitsX64

to the following types:
- byte[]
- Span<byte>
- ReadOnlySpan<byte>

  
**Can be used as follows:**
```
using MurmurHash;
  
byte[] data = Encoding.UTF8.GetBytes("Hello MurmurHash3");
int hash = data.GetMurmur32BitsX86();
```
  
  
***seed*** is an optional parameter
```
using MurmurHash;
  
byte[] data = Encoding.UTF8.GetBytes("Hello MurmurHash3");
int hash = data.GetMurmur32BitsX86(**12345**);
```
  
  
For the **128 bit** versions you can use following snippets to convert the return values to byte[] respectively
```
using MurmurHash;
using System.Runtime.InteropServices;
  
byte[] data = Encoding.UTF8.GetBytes("Hello MurmurHash3");

byte[] hash128x86 = MemoryMarshal.Cast<uint, byte>(data.GetMurmur128BitsX86()).ToArray();
  
byte[] hash128x64 = MemoryMarshal.Cast<ulong, byte>(data.GetMurmur128BitsX64()).ToArray();
  
```
