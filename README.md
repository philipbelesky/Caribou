<div align="center">

![Caribou Logo](/assets/logo_logo.png)

# Caribou

[![Build Action](https://github.com/philipbelesky/Caribou/workflows/Build%20Grasshopper%20Plugin/badge.svg)](https://github.com/philipbelesky/Caribou/actions/workflows/dotnet-grasshopper.yml)
[![Test Action](https://github.com/philipbelesky/Caribou/workflows/Test%20Grasshopper%20Plugin/badge.svg)](https://github.com/philipbelesky/Caribou/actions/workflows/dotnet-tests.yml)
[![Maintainability](https://api.codeclimate.com/v1/badges/20e0e2fd92a1951ccb20/maintainability)](https://codeclimate.com/github/philipbelesky/Caribou/maintainability)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/39c17c1e89d74fccbece8013b74cb7b6)](https://www.codacy.com/gh/philipbelesky/Caribou/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=philipbelesky/Caribou&amp;utm_campaign=Badge_Grade)

</div>

Caribou is a [Grasshopper plugin](https://www.grasshopper3d.com/) for parsing downloaded Open Street Map data into Rhino geometry. Caribou is currently in an beta state, but core functionality should be stable.

### Installation

Caribou is available to download via the [Rhino Package Manager](https://www.rhino3d.com/features/package-manager/) (search *"Caribou"*) or on [Food4Rhino](https://www.food4rhino.com/en/app/caribou?lang=en).

### Documentation & Support

Caribou's documentation [lives on this website](https://caribou.philipbelesky.com) and on [YouTube](https://www.youtube.com/user/philipbelesky).

Support can be requested, or feedback provided, by [opening a discussion on GitHub](https://github.com/philipbelesky/Caribou/discussions). Issues and pull-requests are encouraged.

### Features

- ✅ Windows and MacOS are both fully supported
- ✅ Very fast parsing of even very large files
- ✅ Data-rich GUI interface provided for understanding and filtering OSM metadata
- ✅ Parsing is performed asynchronously so Grasshopper remains responsive
- ✅ Parse multiple OSM files simultaneously with de-duplication of geometry
- ✅ Allows for querying for arbitrary data outside of the primary OSM features/sub-features taxonomy
- ✅ Outputs are tree-formatted and organised per data-type to allow for downstream filtering, tagging, baking, etc

### Roadmap

- 🕘 Further speed optimisations
- 🕘 Component to help construct queries for arbitrary metadata
- 🕘 Parsing of `<relation>` type data
- 🕘 Integration with Rhino's `EarthAnchorPoint`
- 🕘 Customisable projection methods

### Changelog

See [CHANGELOG.md](https://github.com/philipbelesky/Caribou/blob/main/CHANGELOG.md).

### Recognition

Thanks to:

- Timothy Logan, author of [Elk](https://github.com/logant/Elk), for LatLon conversion math and for an example of feature-picker form.
- Dimitrie Stefanescu and the authors of the [GrasshopperAsyncComponent](https://github.com/specklesystems/GrasshopperAsyncComponent) repo.
- Povl Filip Sonne-Frederiksen, author of the [Yak package manager action](https://github.com/pfmephisto/rhino-yak).
