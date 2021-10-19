# Release Checklist

1. Bump version numbers in:
  1. Assembly / Visual Studio
  2. Yak `manifest.yml`
  3. Definitions, `Caribou, Version=0.13.0.0` string
2. Update CHANGELOG date
3. Publish release on GitHub (triggers Yak release)
4. Check Yak release - [Auth Token may have expired](https://discourse.mcneel.com/t/github-action-to-yak/120815)