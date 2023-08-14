# hexfall-clone

A hexagonal match-3 puzzle game. 

Clone of the Hexfall game. My demo project for Vertigo Games.

## Play

Download the APK from the releases page: https://github.com/starikcetin/hexfall-clone/releases

Tap to select a group of 3 hexagons. Swipe to rotate them. Match the same colors.

## References

Reference page for terminology and formulas: https://www.redblobgames.com/grids/hexagons/

## Technical

Assets/Plugins contains a symlink `Eflatun.UnityCommon` that points to `submodules/Eflatun.UnityCommon/Assets/Eflatun.UnityCommon`. It might get broken after a repo clone due to the way git handles NTFS symlinks. If it is broken, simply remove the dummy file in place of the symlink and recreate the NTFS symlink.

## 3rd Party

* [JohannesMP.SceneReference](https://gist.github.com/JohannesMP/ec7d3f0bcf167dab3d0d3bb480e0e07b)
* [DOTween](http://dotween.demigiant.com/)
* [LeanTouch](https://assetstore.unity.com/packages/tools/input-management/lean-touch-30111)
* [LeanTransition](https://assetstore.unity.com/packages/tools/animation/lean-transition-144107)
* [NuGet](https://assetstore.unity.com/packages/tools/utilities/nuget-for-unity-104640)
* [morelinq.3.1.1](https://www.nuget.org/packages/MoreLinq)

# License

MIT License. Refer to the [LICENSE](LICENSE) file.

Copyright (c) 2019 [S. Tarık Çetin](https://github.com/starikcetin)
