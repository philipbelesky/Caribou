import xml.etree.ElementTree as ET
import json

root = ET.parse('./tagfinder_thesaurus.rdf.xmp').getroot()
namespaces = {
    'foaf': "http://xmlns.com/foaf/0.1/",
    'skos': "http://www.w3.org/2004/02/skos/core#",
    'rdf': "http://www.w3.org/1999/02/22-rdf-syntax-ns#",
    'osm': "http://wiki.openstreetmap.org/wiki/",
    'dcterms': "http://purl.org/dc/terms/",
}

defined_features = [
    "aerialway", "aeroway", "amenity", "barrier", "boundary", "building", "craft", "emergency", "geological", "healthcare", "highway", "historic", "landuse", "leisure", "man_made", "military", "natural", "office", "place", "power", "public_transport", "railway", "route", "shop", "sport", "telecom", "tourism", "water", "waterway",
]

# Tags related to defined features; Subfeatures are tags relating to features defined in OSMDefinedFeatures
# Keys relate to arbitrary metadata
data = {
    'subfeature': [], # e.g. <skos:Concept rdf:about="http://wiki.openstreetmap.org/wiki/Tag:shop=computer">
    'tag': [], # e.g. <skos:Concept rdf:about="http://wiki.openstreetmap.org/wiki/Tag:shop=computer">
    'key': []  # e.g. <skos:Concept rdf:about="http://wiki.openstreetmap.org/wiki/Key:meadow">
}

i = 0
for tagRoot in root:
    tagUrl = tagRoot.attrib['{http://www.w3.org/1999/02/22-rdf-syntax-ns#}about']

    if 'Tag:' in tagUrl:
        tagPath = tagUrl.split('Tag:')[1]
        feature = tagPath.split('=')[0]
        subfeature = tagPath.split('=')[1]

        if feature in defined_features:
            tagType = "subfeature"
        else:
            tagType = "tag"


    elif 'Key:' in tagUrl:
        tagType = "key"
        tagPath = tagUrl.split('Key:')[1]
        if tagPath.count(':') == 0:
            feature = tagPath
            subfeature = ''
        else:
            feature = tagPath.split(':')[0]
            subfeature = tagPath.split(':')[1]
    else:
        continue

    description = ""
    descriptionTags = tagRoot.findall('skos:scopeNote', namespaces)
    for tag in descriptionTags:
        if "{http://www.w3.org/XML/1998/namespace}lang" in tag.attrib:
            if tag.attrib["{http://www.w3.org/XML/1998/namespace}lang"] != "en":
                continue

            if hasattr(tag, 'text'):
                description = tag.text

    node_countTag = tagRoot.find('osm:node', namespaces)
    node_count = ''
    if node_countTag is not None:
        node_count = json.loads(node_countTag.text)['count']

    way_countTag = tagRoot.find('osm:way', namespaces)
    way_count = ''
    if way_countTag is not None:
        way_count = json.loads(way_countTag.text)['count']

    relation_countTag = tagRoot.find('osm:relation', namespaces)
    relation_count = ''
    if relation_countTag is not None:
        relation_count = json.loads(relation_countTag.text)['count']

    tag_example = {
        'feature': feature,
        'subfeature': subfeature,
        'description': description,
        'nodes': node_count,
        'ways': way_count,
        'relations': relation_count,
    }
    data[tagType].append(tag_example)

print("Found this many subfeatures:", len(data['subfeature']))
print("Found this many tags:", len(data['tag']))
print("Found this many key:", len(data['key']))

with open('SubFeatureData.json', 'w') as fp:
    json.dump(data['subfeature'], fp, indent=4, sort_keys=True)

with open('TagData.json', 'w') as fp:
    json.dump(data['tag'], fp, indent=4, sort_keys=True)

with open('KeyData.json', 'w') as fp:
    json.dump(data['key'], fp, indent=4, sort_keys=True)
