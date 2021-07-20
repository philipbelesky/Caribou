<div align="center">

![Caribou Logo](/assets/logo_logo.png)

# Caribou

[![Build Action](https://github.com/philipbelesky/Caribou/workflows/Build%20Grasshopper%20Plugin/badge.svg)](https://github.com/philipbelesky/Caribou/actions/workflows/dotnet-grasshopper.yml)
[![Test Action](https://github.com/philipbelesky/Caribou/workflows/Test%20Grasshopper%20Plugin/badge.svg)](https://github.com/philipbelesky/Caribou/actions/workflows/dotnet-tests.yml)
[![Maintainability](https://api.codeclimate.com/v1/badges/20e0e2fd92a1951ccb20/maintainability)](https://codeclimate.com/github/philipbelesky/Caribou/maintainability)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/39c17c1e89d74fccbece8013b74cb7b6)](https://www.codacy.com/gh/philipbelesky/Caribou/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=philipbelesky/Caribou&amp;utm_campaign=Badge_Grade)

</div>

Caribou is a [Grasshopper plugin](https://www.grasshopper3d.com/) for parsing downloaded Open Street Map data into Rhino geometry.

Caribou is currently in an beta state. Feedback, issues, and pull-requests are encouraged.

## Features

- ✅ Windows and MacOS are both fully supported
- ✅ Very fast parsing of even very large files
- ✅ Data-rich GUI interface provided for understanding and filtering OSM metadata
- ✅ Parsing is performed asynchronously so Grasshopper remains responsive
- ✅ Parse multiple OSM files simultaneously with de-duplication of geometry
- ✅ Allows for querying for arbitrary data outside of the primary OSM features/sub-features taxonomy
- ✅ Outputs are tree-formatted and organised per data-type to allow for downstream filtering, tagging, baking, etc

## Roadmap

- 🕘 Documentation and examples
- 🕘 Further speed optimisations
- 🕘 Component to help construct queries for arbitrary Metadata
- 🕘 Parsing of `<relation>` type data
- 🕘 Integration with Rhino's `EarthAnchorPoint`

## Setup and Use

![Image of the definition setup](/assets/demo-v0.7.png)

- Plugin installation
  1. For now, releases are available by searching for *Caribou* in the [Rhino package manager](https://www.rhino3d.com/features/package-manager/) only.
- Data gathering
  1. Go to [https://www.openstreetmap.org](openstreetmap.org)
  2. Locate the general area you wish to model and hit `export`, then `manually select an area`
  3. Click the `OVERPASS API` link to download the `xml` file
- Grasshopper setup
  1. Place Caribou's `Extract Nodes` or `Extract Ways` component (or both)
  2. Place a standard Grasshopper `File Path` component, reference your `xml` file(s), and connect the outputs to the `OSM File` input parameter
  3. Place Caribou's `Specify Features` component.
  4. Click the button at the bottom of the `Specify Features` component and select the types of features you want to extract.
  5. Connect the `OSM Features` output to the `OSM Features` input.

See `examples/Simple.ghx` for a definition the contains a completed example of the above steps as well as components to provide a Legend and categorised baking.

## Recognition

Thanks to:

- Timothy Logan, author of [Elk](https://github.com/logant/Elk), for LatLon conversion math and for an example of feature-picker form.
- Dimitrie Stefanescu and the authors of the [GrasshopperAsyncComponent](https://github.com/specklesystems/GrasshopperAsyncComponent) repo.
- Povl Filip Sonne-Frederiksen, author of the [Yak package manager action](https://github.com/pfmephisto/rhino-yak).
