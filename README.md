# KAT2WAV
KAT is a sound bank file found in Treyarch ports of Tony Hawk's Pro Skater games. On Xbox it was disguised as XSB and has minor differences there.

### Description
Treyarch is a company known for porting Tony Hawk's Pro Skater titles (1 and 2) on Dreamcast and Original Xbox. Level sounds on Dreamcast are stored in KAT containers. On Xbox it is slightly different and wears XSB extension, however it is not the common XSB, but a variation of KAT container. This tool supports both KAT and XSB containers found in Treyarch console ports.

The goal was to get the high quality sounds from THPS1 title, since there were no other sources or tools to convert it. 

### Supported games
* Mat Hoffman's Pro BMX (Dreamcast)
* Tony Hawk's Pro Skater (Dreamcast)
* Tony Hawk's Pro SKater 2x (Xbox)

Even though THPS2 on Dreamcast was ported by Treyarch as well, this tool won't work with it. THPS2 uses 4 bit ADCM compression instead of 8 bit, hence if you'll try to convert it, you'll only end up into bunch of distorted WAvs. However, you can get all the sounds from other ports, including PC and Xbox.

### Usage
You can either use it as GUI tool or command line tool.

GUI:
1. Run the exe
2. Pick the KAT/XSB file
3. Wait for "Done" message
4. Check for the new folder, called as the container, but without extension (i.e. "roswell.kat" -> "roswell" folder)

CMD:
1. Drag-drop KAT/XSB files on the tool icon or use as ```kat2wav C:\kats\file1.kat C:\kats\file2.kat```

2015, 2021, DCxDemo*.