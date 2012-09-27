--------------------
 SimilarImageDotNet
--------------------

A library and a command line tool that allow hashing and comparing images to get a similitude coeficient.
The method used is described here: https://github.com/soywiz/SimilarImageDotNet/blob/master/abstract.txt

Similar libraries:
* http://libpuzzle.pureftpd.org/project/libpuzzle (LibPuzzle)
* http://appsrv.cse.cuhk.edu.hk/~jkzhu/felib.html (FELib)
* http://phash.org/ (pHash)

--------------------

SimilarImageDotNet - 1.0 - Hash and compare images for similarities - soywiz - Copyright ©  2012

Commands:
   -C <FileName1, FileName2> - Compares two images and get a coefficient of similarity (0.0=Completely distinct, 1.0=Equal)
   -H <FileName> - Generates a hash
   -? - Shows this help
   -L [1, 2, 3, 4, 5, 6] - Set hash levels (default = 4)

Examples:
   SimilarImageDotNet.exe -C file1.png file2.png
   SimilarImageDotNet.exe -H file.png
   
--------------------

Usage example:

SimilarImageDotNet.exe -L 3 -H Samples\test1_a.png
000B000000F00F7F0984000000000000F0000000D60000F0D6F000F3F6F00B0A5B0E0F7F3F6F0000A000

SimilarImageDotNet.exe -L 3 -C Samples\test1_b.png Samples\test1_c.png
0.949219