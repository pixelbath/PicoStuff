# PicoStuff
Feeling Pico, might delete later idk.

## What It Is
Basically, it's a chunk parser/extractor for `pico8.dat`. I suppose this readme will also contain any information I've discovered about the contents or format.

## What It Isn't
It's not [picotool](https://github.com/dansanderson/picotool), which is aimed at tools for Pico-8 cartridges.

## What Sucks About It
This is what you get when you mix whisky and code. That said, this is more of a to-do list:

* Unknown `CFIL` entries - there's "." which is fun but easily handled. Others seem to have unicode filenames?
* I _think_ `CFIL` entries with 0 size specified are folder names. File creation seems to bear this out, but...weird. I'm already creating paths automatically, but maybe I'm not supposed to.
* `CBMP` looks fairly simple, but still unknown format.
* I  have some documentation on `CPAL`, but didn't implement it.

# pico8.dat chunk types
* **CPOD** - I guess these are "pods". They look like generic container headers.
* **CBMP** - Bitmaps. Looks like these may also contain `CPAL`
* **CFIL** - File. After `CFIL`, next 32-bit uint is the chunk size, next 64 bytes are the filename (empty space zero-filled).