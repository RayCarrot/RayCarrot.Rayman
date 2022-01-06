## RayCarrot.Rayman

***NOTE: This library is archived and no longer maintained. As of 2022 it has been replaced by the [BinarySerializer](https://github.com/BinarySerializer) libraries. It is highly recommended using those instead as the code here may be outdated.***

---

RayCarrot.Rayman is a helper library for managing file and configuration data from the Rayman games.

### UbiIni Handlers
Allows reading and writing to the data available in the common Rayman ubi.ini configuration files.

### Binary serializers/deserializers
Allows reading and writing to supported binary files. Below is a list of the currently supported files.

#### Rayman 1
* Archives (.dat)
* Rayman 1 save files on PC (.sav)
* Rayman 1 config files on PC (.cfg)
* Rayman Designer, By his Fans & 60 Levels save files (.sct)

#### OpenSpace
* Archives (.cnt)
* Graphic files (.gf)
* Rayman 2 save files on PC (.sav)
* Rayman M/Arena save files on PC (.sav)
* Rayman 3 config files on PC (.cfg)
* Rayman 3 save files on PC (.sav)

#### UbiArt
* Archives (.ipk)
* Texture header (.tex)
* Xbox 360 textures (.dds)
* Localization files (.loc, .loc8)
* Rayman Origins save files on PC
* Rayman Jungle Run save files on PC (.dat)
* Rayman Legends save files on PC
* Rayman Fiesta Run save files on PC Windows 10 Edition (.dat) - (WIP)

### Other
* Rayman 1 PS1 save password encoder/decoder

## Dependencies
RayCarrot.Rayman uses these main dependencies:

- [RayCarrot.Binary](https://github.com/RayCarrot/RayCarrot.Binary)
- [RayCarrot.IO](https://github.com/RayCarrot/RayCarrot.IO)
- [ini-parser](https://github.com/rickyah/ini-parser)
- [DotNetZip](https://github.com/haf/DotNetZip.Semverd)

## Licence

[MIT License (MIT)](./LICENSE)
