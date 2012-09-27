# SimilarImageDotNet

A library and a command line tool that allow hashing and comparing images to get a similitude coeficient.
It is written on .NET 4.0, so you can run on Windows directly with the .NET Framework installed or on
Linux and Mac using Mono.

The method used is described here: https://github.com/soywiz/SimilarImageDotNet/blob/master/abstract.txt

## How to use on large image sets to detect similar images

Similar images/slightly altered usually share exactly first characters of the hash, that way you can create
an indexed structure (maybe a table/collection in a SQL/NOSQL database) with an index in the hash, so you can
search hashes starting with your hash. Then you can compute the distance between your hash and the stored hash
and using a threshold you can determine if you have a similar image. Then you can compare two images pixel
per pixel to get a more accurate comparison.

The idea is to generate the hash of an image using this executable tool and then compare two hashes
in your own language. Hashes are encoded as hexadecimal nibbles.

You can check SimilarImage.CompareHashes for a reference implementation of the comparison algorithm.

## Similar libraries/tools:

* http://libpuzzle.pureftpd.org/project/libpuzzle (LibPuzzle)
* http://appsrv.cse.cuhk.edu.hk/~jkzhu/felib.html (FELib)
* http://phash.org/ (pHash)

## Command line help

```
SimilarImageDotNet - 1.0 - Hash and compare images for similarities - soywiz - Copyright ©  2012

Commands:
   -C <FileName1, FileName2> - Compares two images and get a coefficient of similarity (0.0=Completely distinct, 1.0=Equal)
   -CH <Hash1, Hash2> - Compares two images and get a coefficient of similarity (0.0=Completely distinct, 1.0=Equal)
   -H <FileName> - Generates a hash
   -? - Shows this help
   -L [1, 2, 3, 4, 5, 6] - Set hash levels (default = 4)

Examples:
   SimilarImageDotNet.exe -C file1.png file2.png
   SimilarImageDotNet.exe -CH HASH1 HASH2
   SimilarImageDotNet.exe -H file.png
```

## Usage example

### Get a hash level 3 of an image
```bash
SimilarImageDotNet.exe -L 3 -H Samples\test1_a.png
000B000000F00F7F0984000000000000F0000000D60000F0D6F000F3F6F00B0A5B0E0F7F3F6F0000A000
```

### Compare two images using a hash level 3
```bash
SimilarImageDotNet.exe -L 3 -C Samples\test1_b.png Samples\test1_c.png
0.949219
```

### Compare two hashes
```bash
SimilarImageDotNet.exe -CH 000B000000F00F7F0984000000000000F0000000D60000F0D6F000F3F6F00B0A5B0E0F7F3F6F0000A000 000B000000F00F7F0984000000000000F0000000D0000000D0F000F3F2F00B0A5B0E0F7F3D6F0000A000
0.960938
```