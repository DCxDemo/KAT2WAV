### Description
Treyarch is a company known for porting Tony Hawk's Pro Skater titles (1 and 2) on Dreamcast and Original Xbox. Level sounds on Dreamcast are stored in KAT containers. On Xbox it is slightly different and wears XSB extension, however it is not the common XSB, but a variation of KAT container. This tool supports both KAT and XSB containers found in Treyarch console ports. XSB also supports original filenames. It is recommended to use THPS2x XSB files, since it contains both THPS1 and THPS2 samples, however some samples are still THPS1 exculsive and only obtainable from Dreamcast version.

### Supported games
* Tony Hawk's Pro Skater (Dreamcast)
* Tony Hawk's Pro SKater 2x (Xbox)
* Mat Hoffman's Pro BMX (Dreamcast)

Even though THPS2 on Dreamcast was ported by Treyarch as well, this tool won't work with it. THPS2 uses 4 bit ADPCM compression instead of 8 bit, hence if you'll try to convert it, you'll only end up into bunch of distorted WAVs. ADPCM support is unlikely to be implememnted, since you can get all the sounds from other ports in raw formats anyways (PC, Mac, Xbox).

### Usage
You can either use it as a GUI tool or a command line tool.

GUI:
1. Run the exe
2. Pick the KAT/XSB file
3. Wait for "Done" message
4. Check for the new folder, called as the container, but without extension (i.e. "roswell.kat" -> "roswell" folder)

CMD:
1. Drag-drop KAT/XSB files on the tool icon or use via CMD as ```kat2wav_x64 C:\kats\file1.kat C:\kats\file2.kat```

2015, 2021, DCxDemo*.
