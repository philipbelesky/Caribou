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

![A minimum viable definition](/assets/minimum-viable-definition.png)

[Download the minimal example definition](https://raw.githubusercontent.com/philipbelesky/Caribou/main/examples/Caribou%20-%20Simple%20Example.ghx)

### Non-Geometry Outputs

Each `Extract` component also has two non-geometry outputs.

*Tags* list all the different pieces of metadata attached to a geometric output. For example, a `Way` representing a building might have tags of:

```
{1;2}
addr: housenumber=158
addr: street=Cuba Street
addr: suburb=Te Aro
building-apartments
building:levels=8
name=Cubana Apartments
ref: linz: address id=stack(20 61327-2061340)
````

In this case the `1` in the path would refer to the queried feature (e.g. `apartments`) while the `2` would correspond to the geometry outputs (e.g. the second item).

*Report* lists information related to the feature types requested for parsing, e.g.:

```
{3}
Office
building=office
35 found
233,0,10
Building
building::office
An Office Building.
```

The `3` in the path relates to the queried feature, e.g. this is the report for the 3rd feature requested. This data provided corresponds to:

1. The specific type of feature
2. The 'raw' query string
3. The number of items found
4. A suggested color for this layer (colors are developed to maximise perceptual difference)
5. The 'parent' type of the information
6. A layer path string (to aid baking)
7. The description of the type according to the [OSM Wiki](https://wiki.openstreetmap.org/wiki/Main_Page).

### Open Street Map Data Types

![The Selection filtering UI](/assets/selection-filtering.png)

Because of the way that Open Street Maps assigns metadata, the *Select Features* interface has a number of nuances.

All metadata on Open Street Map is in a `Key:Value` tag format where a `Node` or `Way` can have any number of pairs. For example, a tram stop might have a **name** of *Stop 1: Spencer Street* a **network** of *PTV - Metropolitan Trams* and a **railway** of *tram_stop*.

Certain types of pairs are specified as **features** and *subfeatures* which correspond to a broad set of types. E.g. **building**=*church* or **craft**=*jeweller*. These 'defined' features/subfeatures are what are presented within Caribou's *Specify Features* pop-up.

It is important to note that, although the features/subfeatures are presented in a series of lists, they are not mutually-exclusive categories. A piece of geometry might be a **building** feature with the subfeature of *hotel* while also being classified as an **amenity** feature of the subfeature type *cafe*. Many tags only specify a feature without a specific subfeature, e.g. just **building** or **shop**.

When using the feature selection UI, it matters if the top-level feature is checked or not. When searching for **Public Transport** feature types (with it and all child-items selected), Caribou will only output/classify items according to the main feature, e.g. **Public Transport**. If you want to output/classify items according to their subfeatures, you should uncheck the feature and then select all the subfeatures.

### Querying Arbitrary Metadata

If you want to search for a piece of metadata that is not a defined feature/subfeature, you can use a `Panel` component or `Text` parameter and manually-specify a key-value pair to find using the `key=value` format. To search for a number of pairs, the text can be separated with a comma or a new line. For example:

```
network=PTV - Metropolitan Trams
cuisine=mexican
building:levels=4
addr:suburb=Te Aro
```

### Previewing, Baking, Filtering, and Labeling Geometry

You can download [this definition](https://raw.githubusercontent.com/philipbelesky/Caribou/main/examples/Caribou%20-%20Extensions%20Example.ghx) to see examples of how to:

1. Color the geometry in Grasshopper according to it's feature/subfeature types
2. Display a legend in Rhino with the above color codes for features/subfeatures
3. Filter the results of the parsed geometry/data; e.g. to find all buildings with a height above 40m
4. Bake out the geometry to individually-labelled layers
  - This requires the use of the [*human* plugin](https://discourse.mcneel.com/c/grasshopper/human/88) which is available for Rhino 6/7 on Windows/Mac. Opening the definition should prompt you to install it.

*When saving the file you will probably need to remove the `.txt` extension, e.g. save it as `Caribou - Extension Examples.ghx`.*

## Support and Source

Support can be requested, or feedback provided, by [opening a discussion on GitHub](https://github.com/philipbelesky/Caribou/discussions). Issues and pull-requests are encouraged.

Caribou's source code [is available on GitHub](https://github.com/philipbelesky/Caribou/) under [the LGPL v3.0 license](https://github.com/philipbelesky/Caribou/blob/develop/LICENSE.md).
