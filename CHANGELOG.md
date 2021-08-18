# Change Log

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.12.0] - 2021-08-18
### Added
- New 'extension' examples file to show how to easily bake/filter/legend the Caribou outputs
- A boundary output for the parsing components to easily see the extents of the OSM files

### Fixed
- A crash that could occur when clicking the header rows of the *Specify Features* pop-up. Thanks Clarke for reporting this!

## [0.11.0] - 2021-08-08
### Added
- Live reporting of progress parsing files (below the component)

### Changed
- Reduced minimum required Rhino 6 version to `6.10` or greater

## [0.10.0] - 2021-07-21
### Added
- A bake-friendly layer name to the `Report` output parameter
- Documentation at [https://caribou.philipbelesky.com](caribou.philipbelesky.com)

## [0.9.1] - 2021-07-20
### Changed
- Removed pre-release marker

## [0.9.0-beta] - 2021-07-20
### Added
- A 'Extract Buildings' parameter that creates 3D geometry for items tagged with heights/levels

### Changed
- Tree branches for output geometry will now always match the structure of the Report branches

## [0.8.0-alpha] - 2021-06-08
### Added
- The `Report` parameter now provides a unique per-branch color for use in Geometry Previews and Legends. Colors generated using the HSLuv color space to enhance contrast.

### Changed
- License changed to LGPL

### Fixed
- The 'Nice Name' items in the MetaData report parameter would fail to correctly label subfeatues
- Alignment issue with checkbox label on macOS

## [0.7.1-alpha] - 2021-06-05
### Changed
- Fixed the Yak publishing GitHub Action

## [0.7.0-alpha] - 2021-06-05

### Changed
- Added selection/expansion manipulation buttons to the feature selection GUI
- Added a button to open the feature selection GUI directly from the component
- Parser components now take file paths, not file contents, as inputs

## [0.6.0-alpha] - 2021-06-01

### Added
- Component and associated GUI form for selecting features

## [0.5.0-alpha] - 2021-05-16

Initial (alpha) release
