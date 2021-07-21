---
layout: default
---

## Overview

Caribou is a [Grasshopper](https://www.grasshopper3d.com) plugin for parsing downloaded [Open Street Map](https://www.openstreetmap.org) data into Rhino geometry. Caribou is currently in an beta state, but core functionality should be stable.

## Features

- ✅ Windows and MacOS are both fully supported on Rhino 6 and Rhino 7
- ✅ Very fast parsing of even very large files
- ✅ Data-rich GUI interface provided for understanding and filtering OSM metadata
- ✅ Parsing is performed asynchronously so Grasshopper remains responsive
- ✅ Parse multiple OSM files simultaneously with de-duplication of geometry
- ✅ Allows for querying for arbitrary data outside of the primary OSM features/sub-features taxonomy
- ✅ Outputs are tree-formatted and organised per data-type to allow for downstream filtering, tagging, baking, etc

## Installation

Caribou is available to download via the [Rhino Package Manager](https://www.rhino3d.com/features/package-manager/) (search *"Caribou"*) or on [Food4Rhino](https://www.food4rhino.com/en/app/caribou?lang=en). If installing via the Package Manager, please ensure you fully quit/restart Rhinoceros after installing.

## Setup and Use

### Downloading Open Street Map data

1. Go to [https://www.openstreetmap.org](https://www.openstreetmap.org)
2. Locate the general area you wish to model and hit **export**, then **manually select an area**
3. Click the **OVERPASS API** link to download the `xml` file

Note that Caribou can parse and combine multiple `xml` downloads. If you are constrained by the maximum export area, you can use multiple crops/downloads to increase coverage of your site. Ensure there are clear overlaps between your crops - Caribou will handle de-duplication of any overlapping geometry.

### Parsing the data into Grasshopper Geometry

Caribou's `Extract Nodes` and `Extract Ways` components each process two distinct types of Open Street Map data.

1. Nodes become `Points` in Grasshopper/Rhino and usually correspond to precise spatial markers; e.g. an ATM location, a traffic light, an address, or a tram stop.
2. Ways become `Polylines` in Grasshopper/Rhino and usually correspond to areas or routes; e.g. a road, a bus route, a coastline, or a park.

Caribou also provides an `Extract Buildings` component that handles converting `Way` geometry into 3D shapes if that `Way` is marked as a `building` and if metadata regarding that building's height is present.

Regardless of the type of `Extract` component you are using, the workflow is the same.

1. Place Caribou's `Extract Nodes` or `Extract Ways` or `Extract Buildings` component(s)
2. Place a standard Grasshopper `File Path` component, reference your `xml` file(s), and connect the outputs to the `OSM File` input parameter
3. Place Caribou's `Specify Features` component.
4. Click the button at the bottom of the `Specify Features` component and select the types of features you want to extract.
5. Connect the `OSM Features` output from `Specify Features` to the `OSM Features` input of your `Extract` component(s).
6. Done!

### Non-Geometry Outputs

Each `Extract` component also has two non-geometry outputs.

*Tags* list all the different pieces of metadata attached to a geometric output. For example, a `Way` representing a building might have tags of:

```
addr: housenumber=158
addr: street=Cuba Street
addr: suburb=Te Aro
building-apartments
building:levels=8
name=Cubana Apartments
ref: linz: address id=stack(20 61327-2061340)
````

*Report* lists information related to the parsing process itself:

```
Office
building=office
35 found
233,0,10
Building
building::office
```

This data is provided as:

1. The specific type of information (Office)
2. The 'raw' query string
3. The number of items found
4. A suggested color for this layer (colors are developed to maximise perceptual difference)
5. The 'parent' type of the information
6. A layer path string (to aid baking)

### Filtering Open Street Map Data

TODO

### Baking and Labeling Geometry

You can download this definition to see examples of how to:

1. Color the geometry in Grasshopper according to it's metadata data
2. Display a legend in Rhino with the above color codes.
3. Bake out the geometry to individually-labelled layers
  - This requires the use of the [*human* plugin](https://discourse.mcneel.com/c/grasshopper/human/88) which is available for Rhino 6/7 on Windows/Mac.

## Support and Source

Support can be requested, or feedback provided, by [opening a discussion on GitHub](https://github.com/philipbelesky/Caribou/discussions). Issues and pull-requests are encouraged.

Caribou's source code [is available on GitHub](https://github.com/philipbelesky/Caribou/) under [the LGPL v3.0 license](https://github.com/philipbelesky/Caribou/blob/develop/LICENSE.md).
