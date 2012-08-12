I wrote this program because synchronizing mp3 files from my music collection to my Sansa Fuze mp3 player with sd slot (seen as 2 different storage units from the PC) was a pain to deal with.
This tool will synchronize your music collection between your PC hard drive and your player internal memory + SD card.
Kinda like a crude Robocopy from a source to 2 destinations.

Principle:
A hash is computed for each file on both PC and player:

    Each file that's present on the player but not on the PC will be moved from the player to a chosen directory on the PC.
    Each file on the PC source that's not on the player will be copied.


Known limitations:

    10Mo of available space will be kept on the player, and 10Mo on the SD card (overstuffed devices seldom works well).
    First hash compute is really long, but wait, it will be cached for future use, so the second run will be way quicker.
    Files will be copied within their parent directory, but no further: A file c:\foo\bar\baz\quux.mp3 on the PC will be copied as baz\quux.mp3 on the player.
    A modified mp3 tag on the source will be considered as a different file (hash based comparison) => hence the file will be copied all over again.
    For 2 discs albums, a duplicated folder.jpg file will only be copied once.
    Only works in MSC USB mode (I guess this tool is pretty useless in MTP mode, isn't it?).
    Only works in console mode (but a GUI will be easy to shape and to interface with the core dll).


Command line:

Mp3Sync.exe -s SourcePath -d1 DestinationPath1 -d2 DestinationPath2 [-dbin DestinationBin] [-ext ExtensionString] [-auto AutoSync]

        SourcePath: Path of the source to synchronize with, like "c:\music"
        DestinationPath1: Path of the destination 1 (player music dir), like "H:\Music"
        DestinationPath2 (optional): Path of the destination 2 (player sd card music dir), like "I:\Music"
        DestinationBin (optional): Were to put files found on the player, but not in the source, like "c:\MusicDeletedFromPlayer" - If not provided, extra or out of date files will be deleted.
        ExtensionString (optional): Pipe separated extensions of the files to synchronize - If not provided, "".mp3|.jpg"" will be used.
	AutoSync in split folders (optional): name of a file to duplicate if a folder happens to be duplicated. If not provided ""folder.jpg"" will be used.

- or -

Mp3Sync.exe Inputs.xml

with an input file like:

<?xml version="1.0" encoding="utf-8"?>
<Mp3SyncSettings>
  <Source>c:\Music</Source>
  <Dests>
    <string>d:\audio</string>
    <string>e:\audio</string>
  </Dests>
  <DestBin>c:\MusicRemovedFromPlayer</DestBin>
  <StExt>.mp3|.jpg</StExt>
  <AutoSync>folder.jpg</AutoSync>
</Mp3SyncSettings>


Note: This tool might be used as a generic synchroniser, but there are tools really more suited than this one if it's a one to one synchronization.
This program is distributed under the GNU General Public License. 